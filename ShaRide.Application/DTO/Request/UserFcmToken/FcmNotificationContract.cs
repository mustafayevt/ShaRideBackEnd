using System.Collections.Generic;
using Newtonsoft.Json;
using ShaRide.Application.DTO.Response.Message;

namespace ShaRide.Application.DTO.Request.UserFcmToken
{
    public class FcmNotificationContract
    {
        public FcmNotificationContract()
        {
            
        }
        public Notification notification { get; set; }
        public string priority { get; set; } = "high";
        public Data data { get; set; }
        public List<string> registration_ids { get; set; }
        public class Notification
        {
            public Notification(string body, string title)
            {
                this.body = body;
                this.title = title;
            }

            public Notification()
            {
                
            }

            public string body { get; set; }
            public string title { get; set; }
        }

        public class Data
        {
            public Data()
            {
                
            }

            public string Message { get; set; }
            public string Body { get; set; }
            public string Title { get; set; }
            [JsonProperty("click_action")]
            public string ClickAction { get; set; }
            public string Id { get; set; }
            public string Status { get; set; }
            [JsonProperty("action_in_app")]
            public string ActionInApp { get; set; }

            public IModel Model { get; set; }
        }
    }
}