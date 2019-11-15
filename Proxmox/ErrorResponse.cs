namespace Rundeck
{
    public class ErrorResponse
    {
        public bool Error { get; set; }
        public int Apiversion { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
    }
}