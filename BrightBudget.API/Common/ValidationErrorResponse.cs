using System.Collections.Generic;

namespace BrightBudget.API.Common
{
    public class ValidationErrorResponse
    {
        public bool Success { get; set; } = false;
        public string? Message { get; set; }
        public IDictionary<string, string[]> Errors { get; set; }  = new Dictionary<string, string[]>();     
    }
}
