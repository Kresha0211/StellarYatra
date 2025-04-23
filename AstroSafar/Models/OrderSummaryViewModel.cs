namespace AstroSafar.Models
{
    public class OrderSummaryViewModel
    {

        public CheckoutViewModel ShippingDetails { get; set; }
        public List<CartItemViewModel> CartItems { get; set; }
    }

}