namespace Elmah.Io.AspNetCore.Serilog.AspNetCore60.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}