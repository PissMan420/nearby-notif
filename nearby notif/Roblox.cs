using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Windows;



namespace nearby_notif
{
    class RobloxStat
    {
        public int Value { get; }
        public int RecentAvetagePrice { get;  }
        RobloxStat(int Value, int RAP) {
            this.Value = Value;
            this.RecentAvetagePrice = RAP;
        }
    }

    class RobloxPlayer
    {
        public Int64 UserId { get; }
        public string Username { get; }

        RobloxPlayer(int userId, string username) {
            this.UserId = userId;
            this.Username = username;
        }
    }
}
