using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Infrastructure.Payments.Clients.MyShopPay.Models;
public sealed record MyShopPayCreatePaymentRequestModel(
    decimal Price,
    Uri ContinueUri
    );
