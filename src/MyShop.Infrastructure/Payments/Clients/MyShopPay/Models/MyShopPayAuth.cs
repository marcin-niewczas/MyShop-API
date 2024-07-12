using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Infrastructure.Payments.Clients.MyShopPay.Models;
internal sealed record MyShopPayAuth
{
    public required string AccessToken { get; init; }
    public required DateTime ExpiryAccessTokenDate { get; init; }
}
