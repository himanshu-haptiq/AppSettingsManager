namespace AppSettingsManager.Models
{
    public class TwilioSettings
    {
        // Property Names should exactly match with the key names inside the appsettings
        public string? AuthToken { get; set; } 
        public string? AccountSid { get; set; }
        public string? Phone1Number { get; set; }
    }
}
