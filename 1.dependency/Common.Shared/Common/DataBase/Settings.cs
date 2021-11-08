namespace Common.Shared
{
    public class Settings
    {
        public string AzureWebJobsStorage { get; set; }
        public string AzureWebJobsDashboard { get; set; }
        public string AzureUserStorage { get; set; }
        public string AzurePasswordStorage { get; set; }
        public EmailSettings EmailSettings { get; set; }
        public JwtSettings JwtSettings { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public UploadFile UploadFile { get; set; }
    }
}
