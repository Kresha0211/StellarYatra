namespace AstroSafar.Models
{
    public class TransactionViewModel
    {
        public int TransactionId { get; set; }
        public string UserName { get; set; }
        public string CourseName { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string RazorpayPaymentId { get; set; }
        public string RazorpayOrderId { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public bool CanRetry { get; set; }
    }
}
