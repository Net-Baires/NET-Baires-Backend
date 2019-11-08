namespace NetBaires.Api.Models.ServicesResponse
{
    public class MeetupSelf
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string bio { get; set; }
        public string status { get; set; }
        public long joined { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string localized_country_name { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public Photo photo { get; set; }
        public bool is_pro_admin { get; set; }
    }
}
