using System.Collections.Generic;

namespace ShaRide.Application.DTO.Request.UserFcmToken
{
    public class FcmNotificationContract
    {
        public Notification notification { get; set; }
        public string priority { get; set; } = "high";
        public Data data { get; set; }
        public List<string> registration_ids { get; set; }
        public class Notification
        {
            public string body { get; set; }
            public string title { get; set; } = "ShaRide";
        }

        public class Data
        {
            public Data(string clickAction, string id, string status)
            {
                click_action = clickAction;
                this.id = id;
                this.status = status;
            }

            public Data()
            {
                
            }

            public string click_action { get; set; }
            public string id { get; set; }
            public string status { get; set; }
        }
    }
}