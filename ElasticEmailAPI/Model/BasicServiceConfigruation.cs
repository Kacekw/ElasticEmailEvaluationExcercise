namespace ElasticEmailAPI.Model
{
    public class BasicServiceConfigruation
    {
        public string ElasticEmailBasePath { get; set; } = "https://api.elasticemail.com/v4";
        public string ApiKeyHeaderParameterName { get; set; } = "X-ElasticEmail-ApiKey";
        public string? ApiKey { get; set; }
    }
}
