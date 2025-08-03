using System.Net;

namespace TaskFlow.Shared.Dtos
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public string Message { get; set; } = "Success";
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T? Data { get; set; }
    }

}
