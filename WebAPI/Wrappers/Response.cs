namespace WebAPI.Wrappers
{

    /// <summary>
    /// Klasa generyczna - można wykorzystać do przesyłania rezultatu niezależnie od zwracanego typu
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T>
    {
        public T Data { get; set; }
        public bool Succeeded { get; set; } 
        public string Message { get; set; }
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
}
