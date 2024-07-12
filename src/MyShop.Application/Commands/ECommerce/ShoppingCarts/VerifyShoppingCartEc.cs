﻿using MyShop.Application.Dtos.ECommerce.ShoppingCarts;
using MyShop.Application.Responses;

namespace MyShop.Application.Commands.ECommerce.ShoppingCarts;
public sealed record VerifyShoppingCartEc
    : ICommand<ApiResponse<ShoppingCartDetailEcDto>>;