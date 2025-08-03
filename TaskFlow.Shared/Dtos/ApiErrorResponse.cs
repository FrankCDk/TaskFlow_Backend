using System.Net;

namespace TaskFlow.Shared.Dtos
{
    public class ApiErrorResponse
    {
        public HttpStatusCode Status { get; set; } = HttpStatusCode.InternalServerError;
        public string Title {  get; set; } = "An unexpected error occurred.";
        public string Detail { get; set; } = "Please try again later or contact support.";
        public Dictionary<string, List<string>>? Errors { get; set; } = null;
    }
}
