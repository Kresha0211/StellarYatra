namespace AstroSafar.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }  // Add this

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
