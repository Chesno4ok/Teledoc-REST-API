namespace Teledoc_REST_API.Responses
{
    class TextResponse
    {
        public string Message { get; set; }
        public TextResponse(string message)
        {
            Message = message;
        }
    }
}
