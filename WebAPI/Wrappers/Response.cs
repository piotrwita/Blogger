namespace WebAPI.Wrappers
{

    /// <summary>
    /// Klasa generyczna - można wykorzystać do przesyłania rezultatu niezależnie od zwracanego typu
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T> : Response
    {
        public T Data { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public Response()
        {
        }

        public Response(T data)
        {
            Data =
                data ?? throw new ArgumentException(nameof(data));

            Succeeded = true;
        }
    }

    /// <summary>
    /// Przechwuje informację czy sukces i ew komunikat błędu
    /// </summary>
    public class Response
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }

        public Response()
        {
        }

        public Response(bool succeeded, string message)
        {
            Succeeded = succeeded;
            Message = message;
        }
    }
}
