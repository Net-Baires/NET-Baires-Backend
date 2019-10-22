namespace NetBaires.Api.Models.ServicesResponse.Badgr
{
    public class Recipient
    {
        public string Identity { get; set; }
        public bool Hashed { get; set; }
        public string Type { get; set; }
        public string PlaintextIdentity { get; set; }
        public string Salt { get; set; }
    }
}