namespace AstroSafar.Models
{
    public class RazorpayCallbackModel
    {

        public string razorpay_payment_id { get; set; }
        public string razorpay_order_id { get; set; }
        public string razorpay_signature { get; set; }
        public int EnrollmentId { get; set; }
    }
}
