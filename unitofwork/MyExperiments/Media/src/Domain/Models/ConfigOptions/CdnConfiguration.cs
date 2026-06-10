namespace Domain.Models.ConfigOptions
{
    public class CdnConfiguration
    {
        public string ImageCdnUri { get; set; }
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string SubscriptionId { get; set; }
        public string ResourceGroupName { get; set; }
        public string ProfileName { get; set; }
        public string EndpointName { get; set; }
    }
}
