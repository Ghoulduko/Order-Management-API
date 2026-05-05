using Order.Core.Enums;

namespace Order.Application.Dtos.Cart;

public class UpdateCartItemQuantityDto
{
    public int CartItemId { get; set; }
    public required UpdateCartItemQuantity QuantityAction { get; set; }
}