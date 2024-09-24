namespace Teledoc_REST_API.Responses
{
    class InvalidArgumentResponse
    {
        public string Message { get; set; }
        public string[] Arguments { get; set; }
        public InvalidArgumentResponse(string message, string[] arguments)
        {
            Message = message;
            Arguments = arguments;
        }
    }
}
