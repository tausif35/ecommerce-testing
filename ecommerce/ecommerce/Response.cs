using System.Net;


namespace ecommerce
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }
}
