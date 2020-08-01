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
    class RobloxStat
    {
        // currency values
        public int Rap { get; }
        public int Value { get; }

        
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

    class RobloxUser
    {
        public Int64 UserId { get; set; }
        public string Username { get; set; }
    }
}
