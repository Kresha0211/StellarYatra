namespace AstroSafar.Models
{
    public class RazorpaySuccessViewModel
    {
        public string PaymentId { get; set; }
        public string OrderId { get; set; }
        public string Signature { get; set; }
    }

}