namespace Common.Shared
{
    public class EmailSettings
    {
        public string Username { get; set; }
        public string From { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public bool IsHtml { get; set; }
    }
}
