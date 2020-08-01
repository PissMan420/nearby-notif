using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
	

namespace nearby_notif
{
    class RobloxLib
    {

        private int UserId { get; set; }
        public string Username { get; set; } 
        // currency values
        public int Rap { get; }
        public int Value { get; } 
        
        public string GetUserThumbAvatar() {
            return HttpGet($"https://www.roblox.com/Thumbs/Avatar.ashx?x=1024&y=1024&userid={UserId}");
        }

        public void SetUsername() {
            string json = HttpGet($"https://api.roblox.com/users/{UserId}");
            var Username_Converted = JsonConvert.DeserializeObject(json);
            
            Username = Username_Converted.Name;
        }

        private string HttpGet(string uri) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream)) {
                return reader.ReadToEnd();
            }
        }
    }
}
