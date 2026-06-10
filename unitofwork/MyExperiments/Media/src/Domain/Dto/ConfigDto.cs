namespace Domain.Dto
{
    public class ConfigDto
    {
        public int TenantId { get; set; }
        public string ConfigType { get; set; }
        public string ConfigName { get; set; }
        public string Version { get; set; }
        public string Value { get; set; }
        public string Comment { get; set; }
    }
}
