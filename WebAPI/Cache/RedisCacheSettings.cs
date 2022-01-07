namespace WebAPI.Cache
{
    public class RedisCacheSettings
    {
        public bool Enabled { get; set; }
        /// <summary>
        /// ciag polaczenia do db redis
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
