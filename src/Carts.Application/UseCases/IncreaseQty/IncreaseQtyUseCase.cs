﻿using Carts.Application.Common.Responses;
using Carts.Application.ExternalServices.Users;
using Carts.Domain;
using Carts.Domain.ValueObjects;

using Mapster;

namespace Carts.Application.UseCases.IncreaseQty;

internal sealed class IncreaseQtyUseCase : IIncreaseQtyUseCase
{
    private readonly IUserService _userService;
    private readonly ICartRepository _cartRepository;

    private IOutputPort _outputPort = new OutputPort();

    public IncreaseQtyUseCase(IUserService userService,
                              ICartRepository cartRepository)
    {
        _userService = userService;
        _cartRepository = cartRepository;
    }

    public async Task ExecuteAsync(IncreaseQtyRequest request, CancellationToken cancellationToken)
    {
        UserId userId = _userService.GetCurrentUserId();

        if (await _cartRepository.GetAsync(userId, cancellationToken) is Cart cart)
        {
            if (cart.TryGetCartItem(request.SkuId, out _))
            {
                cart.IncreaseQuantity(request.SkuId);
                await _cartRepository.UpdateAsync(cart, cancellationToken);
            }

            _outputPort.Success(cart.Adapt<CartResponse>());
            return;
        }

        _outputPort.NotFound(new ApplicationErrorResponse("NOT_FOUND", "Não é possivel aumentar a quantidade", "O carrinho não existe."));
    }

    public void SetOutputPort(IOutputPort outputPort) => _outputPort = outputPort;
}
