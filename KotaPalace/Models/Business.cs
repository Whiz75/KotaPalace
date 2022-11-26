namespace KotaPalace.Models
{
    public class Business
    {
        public int Id { get; set; } 
        public string BusinessName { get; set; }
        public string BusinessAddress { get; set; }
        public string BusinessDescription { get; set; }
        public string BusinessPhoneNumber { get; set; }
        public string BusinessEmail { get; set; }
        public string ImgUrl { get; set; }
        public string OwnerId { get; set; } 
        public string BusinessMFClose { get; set; }
        public string BusinessSatClose { get; set; }
        public string BusinessMFOpen { get; set; }
        public string BusinessSatOpen { get; set; }
        public string BusinessSunClose { get; set; }
        public string BusinessSunOpen { get; set; }
        public string OnlineStatus { get; set; }
        public string Coordinates { get; set; }
    }
}
