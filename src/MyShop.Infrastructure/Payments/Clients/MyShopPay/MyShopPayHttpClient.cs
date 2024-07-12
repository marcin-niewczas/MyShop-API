using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using MyShop.Infrastructure.Options;
using MyShop.Infrastructure.Payments.Clients.MyShopPay.Models;
using MyShop.Infrastructure.Payments.Exceptions;
using Polly;
using Polly.Extensions.Http;
using System.Net;
using System.Net.Http.Json;

namespace MyShop.Infrastructure.Payments.Clients.MyShopPay;
internal sealed class MyShopPayHttpClient : IMyShopPayHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly TimeProvider _timeProvider;
    private readonly MyShopPayOptions _myShopPayOptions;
    private MyShopPayAuth _auth = default!;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public MyShopPayHttpClient(
        HttpClient httpClient,
        IOptions<MyShopPayOptions> options,
        TimeProvider timeProvider
        )
    {
        _httpClient = httpClient;
        _timeProvider = timeProvider;
        _myShopPayOptions = options.Value;
        _httpClient.BaseAddress = _myShopPayOptions.Uri;
    }

    public async Task<MyShopPayCreatePaymentResponse> CreatePaymentAsync(
        MyShopPayCreatePaymentRequestModel model,
        CancellationToken cancellationToken = default
        )
    {
        await AuthAsync(cancellationToken);

        using var response = await GetAuthRetryPolicy()
            .ExecuteAsync(() =>
            {
                _httpClient.DefaultRequestHeaders.Authorization
                    = new("Bearer", _auth.AccessToken);
                return _httpClient.PostAsJsonAsync("payments", model, cancellationToken);
            });

        if (response.IsSuccessStatusCode)
        {
            var content = await response
                .Content
                .ReadFromJsonAsync<MyShopPayCreatePaymentResponse>(cancellationToken)
                    ?? throw new MyShopPayClientException($"Content is null after deserialized data."); ;

            return content;
        }

        throw new MyShopPayClientException(
                    $"Status Code = {response.StatusCode} | Content = {await response.Content.ReadAsStringAsync(cancellationToken)}"
                    );
    }

    public async Task<MyShopPayPayment> GetPaymentAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default
        )
    {
        await AuthAsync(cancellationToken);

        using var response = await GetAuthRetryPolicy()
            .ExecuteAsync(() =>
            {
                _httpClient.DefaultRequestHeaders.Authorization
                    = new(JwtBearerDefaults.AuthenticationScheme, _auth.AccessToken);
                return _httpClient.GetAsync($"payments/{paymentId}", cancellationToken);
            });

        if (response.IsSuccessStatusCode)
        {
            var content = await response
                .Content
                .ReadFromJsonAsync<MyShopPayPayment>(cancellationToken)
                    ?? throw new MyShopPayClientException($"Content is null after deserialized data."); ;

            return content;
        }

        throw new MyShopPayClientException(
                    $"Status Code = {response.StatusCode} | Content = {await response.Content.ReadAsStringAsync(cancellationToken)}"
                    );
    }

    private async Task AuthAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            if (_auth is null || _auth.ExpiryAccessTokenDate <= _timeProvider.GetUtcNow().DateTime)
            {
                using var response = await GetRetryPolicy().ExecuteAsync(() => _httpClient
                    .PostAsJsonAsync("users/sign-in", new MyShopPaySignIn(_myShopPayOptions.Username, _myShopPayOptions.Password), cancellationToken));

                if (response.IsSuccessStatusCode)
                {
                    _auth = await response
                        .Content
                        .ReadFromJsonAsync<MyShopPayAuth>(cancellationToken: cancellationToken)
                        ?? throw new MyShopPayClientException("Content is null after deserialized data.");

                    return;
                }

                throw new MyShopPayClientException(
                    $"Status Code = {response.StatusCode} | Content = {await response.Content.ReadAsStringAsync(cancellationToken)}"
                    );
            }
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private AsyncPolicy<HttpResponseMessage> GetAuthRetryPolicy()
        => HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == HttpStatusCode.Unauthorized)
            .RetryAsync(4, async (response, retryAttempt, context) =>
            {
                if (response.Result.StatusCode is HttpStatusCode.Unauthorized)
                {
                    await AuthAsync();
                }

                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            });

    private static AsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        => HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(4, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}
