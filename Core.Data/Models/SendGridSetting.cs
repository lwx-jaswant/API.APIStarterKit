namespace Core.Data.Models
{
    public class SendGridSetting : EntityBase
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
        public string FromEmail { get; set; }
        public string FromFullName { get; set; }
        public bool IsDefault { get; set; }
    }
}
