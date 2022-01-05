namespace WebAPI.Wrappers
{
    public class AuthSuccessResponse
    { 
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
