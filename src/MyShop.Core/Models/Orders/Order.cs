using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.ShoppingCarts;
using MyShop.Core.Models.Users;
using MyShop.Core.Utils;
using MyShop.Core.ValueObjects.Orders;
using MyShop.Core.ValueObjects.Shared;

namespace MyShop.Core.Models.Orders;
public sealed class Order : BaseTimestampEntity
{
    public Email Email { get; private set; } = default!;
    public FirstName FirstName { get; private set; } = default!;
    public LastName LastName { get; private set; } = default!;
    public OrderPhoneNumber PhoneNumber { get; private set; } = default!;
    public StreetName StreetName { get; private set; } = default!;
    public BuildingNumber BuildingNumber { get; private set; } = default!;
    public ApartmentNumber ApartmentNumber { get; private set; } = default!;
    public City City { get; private set; } = default!;
    public ZipCode ZipCode { get; private set; } = default!;
    public Country Country { get; private set; } = default!;
    public OrderStatus Status { get; private set; } = default!;
    public DeliveryMethod DeliveryMethod { get; private set; } = default!;
    public PaymentMethod PaymentMethod { get; private set; } = default!;
    public Guid? PaymentId { get; private set; }
    public Uri? RedirectPaymentUri { get; private set; }
    public User User { get; private set; } = default!;
    public Guid UserId { get; private set; }
    public Invoice? Invoice { get; private set; } = default!;
    public Guid? InvoiceId { get; private set; } = default!;
    public IReadOnlyCollection<OrderProduct> OrderProducts => _orderProducts;
    private readonly List<OrderProduct> _orderProducts = default!;
    public IReadOnlyCollection<OrderStatusHistory> OrderStatusHistories => _orderStatusHistories;
    private readonly List<OrderStatusHistory> _orderStatusHistories = default!;

    private Order() { }

    private Order(
        string checkoutId,
        StreetName streetName,
        BuildingNumber buildingNumber,
        ApartmentNumber apartmentNumber,
        ZipCode zipCode,
        City city,
        Country country,
        DeliveryMethod deliveryMethod,
        PaymentMethod paymentMethod,
        User user
        )
    {
        ArgumentNullException.ThrowIfNull(streetName, nameof(streetName));
        ArgumentNullException.ThrowIfNull(buildingNumber, nameof(buildingNumber));
        ArgumentNullException.ThrowIfNull(apartmentNumber, nameof(apartmentNumber));
        ArgumentNullException.ThrowIfNull(city, nameof(city));
        ArgumentNullException.ThrowIfNull(zipCode, nameof(zipCode));
        ArgumentNullException.ThrowIfNull(country, nameof(country));
        ArgumentNullException.ThrowIfNull(deliveryMethod, nameof(deliveryMethod));
        ArgumentNullException.ThrowIfNull(paymentMethod, nameof(paymentMethod));
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(user.ShoppingCart, nameof(user.ShoppingCart));
        ArgumentNullException.ThrowIfNull(user.ShoppingCart.ShoppingCartItems, nameof(user.ShoppingCart.ShoppingCartItems));

        if (checkoutId != user.ShoppingCart.CheckoutId)
        {
            throw new BadRequestException($"Your {nameof(ShoppingCart).ToTitleCase()} changed after checkout.");
        }

        StreetName = streetName;
        BuildingNumber = buildingNumber;
        ApartmentNumber = apartmentNumber;
        City = city;
        ZipCode = zipCode;
        Country = country;
        DeliveryMethod = deliveryMethod;
        PaymentMethod = paymentMethod;
        Status = OrderStatus.New;

        if (user.ShoppingCart.ShoppingCartItems.Count <= 0)
        {
            throw new BadRequestException($"Your {nameof(ShoppingCart).ToTitleCase()} is empty.");
        }

        _orderStatusHistories = [];
        _orderStatusHistories.Add(new OrderStatusHistory(OrderStatus.New, this));

        _orderProducts = user
            .ShoppingCart
            .ShoppingCartItems
            .Select(item => item.ToOrderProduct(this))
            .ToList();

        user.ShoppingCart.ClearShoppingCart();
    }

    public Order(
        string checkoutId,
        StreetName streetName,
        BuildingNumber buildingNumber,
        ApartmentNumber apartmentNumber,
        City city,
        ZipCode zipCode,
        Country country,
        DeliveryMethod deliveryMethod,
        PaymentMethod paymentMethod,
        Guest guest,
        Email email,
        FirstName firstName,
        LastName lastName,
        OrderPhoneNumber phoneNumber
       ) : this(
           checkoutId,
           streetName,
           buildingNumber,
           apartmentNumber,
           zipCode,
           city,
           country,
           deliveryMethod,
           paymentMethod,
           guest
           )
    {
        ArgumentNullException.ThrowIfNull(email, nameof(email));
        ArgumentNullException.ThrowIfNull(firstName, nameof(firstName));
        ArgumentNullException.ThrowIfNull(lastName, nameof(lastName));
        ArgumentNullException.ThrowIfNull(phoneNumber, nameof(phoneNumber));

        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        User = guest;
        UserId = guest.Id;
    }

    public Order(
        string checkoutId,
        StreetName streetName,
        BuildingNumber buildingNumber,
        ApartmentNumber apartmentNumber,
        City city,
        ZipCode zipCode,
        Country country,
        DeliveryMethod deliveryMethod,
        PaymentMethod paymentMethod,
        RegisteredUser registeredUser,
        string? phoneNumber
       ) : this(
           checkoutId,
           streetName,
           buildingNumber,
           apartmentNumber,
           zipCode,
           city,
           country,
           deliveryMethod,
           paymentMethod,
           registeredUser
           )
    {
        if (phoneNumber is null && registeredUser.PhoneNumber is null)
        {
            throw new BadRequestException($"The {nameof(RegisteredUser.PhoneNumber)} is required.");
        }

        Email = registeredUser.Email;
        FirstName = registeredUser.FirstName;
        LastName = registeredUser.LastName;

        if (phoneNumber is null && registeredUser.PhoneNumber is null)
        {
            throw new BadRequestException($"The {nameof(PhoneNumber)} is required.");
        }

        PhoneNumber = phoneNumber ?? registeredUser.PhoneNumber!;
        User = registeredUser;
        UserId = registeredUser.Id;
    }

    public void Update(
        StreetName streetName,
        BuildingNumber buildingNumber,
        ApartmentNumber apartmentNumber,
        City city,
        ZipCode zipCode,
        Country country,
        Email email,
        FirstName firstName,
        LastName lastName,
        OrderPhoneNumber phoneNumber,
        OrderStatus orderStatus,
        Func<OrderStatusHistory, Task>? afterChangeStatus = null
        )
    {
        ArgumentNullException.ThrowIfNull(streetName, nameof(streetName));
        ArgumentNullException.ThrowIfNull(buildingNumber, nameof(buildingNumber));
        ArgumentNullException.ThrowIfNull(apartmentNumber, nameof(apartmentNumber));
        ArgumentNullException.ThrowIfNull(city, nameof(city));
        ArgumentNullException.ThrowIfNull(zipCode, nameof(zipCode));
        ArgumentNullException.ThrowIfNull(country, nameof(country));
        ArgumentNullException.ThrowIfNull(email, nameof(email));
        ArgumentNullException.ThrowIfNull(firstName, nameof(firstName));
        ArgumentNullException.ThrowIfNull(lastName, nameof(lastName));
        ArgumentNullException.ThrowIfNull(phoneNumber, nameof(phoneNumber));
        ArgumentNullException.ThrowIfNull(orderStatus, nameof(orderStatus));

        if (StreetName == streetName &&
            BuildingNumber == buildingNumber &&
            ApartmentNumber == apartmentNumber &&
            City == city &&
            ZipCode == zipCode &&
            Country == country &&
            Email == email &&
            FirstName == firstName &&
            LastName == lastName &&
            PhoneNumber == phoneNumber &&
            Status == orderStatus)
        {
            throw new BadRequestException($"Nothing change in {nameof(Order)}.");
        }

        StreetName = streetName;
        BuildingNumber = buildingNumber;
        ApartmentNumber = apartmentNumber;
        City = city;
        ZipCode = zipCode;
        Country = country;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;

        if (Status != orderStatus)
        {
            var orderStatusHistory = SetStatus(orderStatus);

            if (afterChangeStatus is not null)
            {
                afterChangeStatus(orderStatusHistory);
            }
        }
    }

    public void SetPaymentDetails(
        Guid paymentId,
        Uri redirectPaymentUri,
        Func<OrderStatusHistory, Task>? after = null
        )
    {
        PaymentId = paymentId;
        RedirectPaymentUri = redirectPaymentUri;

        var orderStatusHistory = SetStatus(OrderStatus.WaitingForPayment);

        if (after is not null)
        {
            after(orderStatusHistory);
        }
    }

    public OrderStatusHistory UpdateAsPaymentFailed(
        Func<OrderStatusHistory, Task>? after = null
        )
    {
        RedirectPaymentUri = null;

        var orderStatusHistory = SetStatus(OrderStatus.PaymentFailed);

        _orderStatusHistories.Add(orderStatusHistory);

        if (after is not null)
        {
            after(orderStatusHistory);
        }

        return orderStatusHistory;
    }

    public void UpdateAsPaid(
        Func<OrderStatusHistory, Task>? after = null
        )
    {
        RedirectPaymentUri = null;

        var orderStatusHistory = SetStatus(OrderStatus.PaymentReceived);

        _orderStatusHistories.Add(orderStatusHistory);

        if (after is not null)
        {
            after(orderStatusHistory);
        }
    }

    public void CancelOrder(
        Func<OrderStatusHistory, Task>? after = null
        )
    {
        RedirectPaymentUri = null;

        var orderStatusHistory = SetStatus(OrderStatus.Canceled);

        if (after is not null)
        {
            after(orderStatusHistory);
        }
    }

    private OrderStatusHistory SetStatus(
        OrderStatus status
        )
    {
        if (Status == status)
        {
            throw new ArgumentException($"The {nameof(Order)} with '{Id}' currently has a {status.Value} status.");
        }

        if (_orderStatusHistories is null)
        {
            throw new InvalidOperationException($"{nameof(OrderStatusHistories)} must be included.");
        }

        var availableStatus = OrderStatus.GetAvailableOrderStatusToUpdate();

        if (!availableStatus.Contains(status.Value))
        {
            throw new BadRequestException(
                $"The {nameof(Status)} must in [ {string.Join(", ", availableStatus)} ] for update {nameof(Order)} {Id}."
                );
        }

        var orderStatusHistory = new OrderStatusHistory(
            status,
            this
            );

        Status = status;
        _orderStatusHistories.Add(orderStatusHistory);

        return orderStatusHistory;
    }
}
