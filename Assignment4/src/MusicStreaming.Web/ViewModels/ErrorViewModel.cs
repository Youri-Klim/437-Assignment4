using System;

namespace MusicStreaming.Web.ViewModels
{
    public class ErrorViewModel
    {
        public required string RequestId { get; set; }
        public required string ErrorMessage { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}