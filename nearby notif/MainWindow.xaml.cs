using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebSocketSharp;
using sxlib;
using nearby_notif;

namespace nearby_notif
{
    /// <summary>C:\Users\ASD\Project\csharp\nearby notif\nearby notif\neaby notif script.lua
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        // websocket server
        WebSocket saveSettingRequest = new WebSocket("ws://localhost:24892/custom?channel=settingSaveRequest");
        WebSocket playerChatSocket = new WebSocket("ws://localhost:24892/custom?channel=ChatSocket");
        public MainWindow() {
            InitializeComponent();
            // connect the websocket servers
            saveSettingRequest.Connect();
            saveSettingRequest.Send("Sucessfully connected to the setting ws server");
            saveSettingRequest.OnMessage += SaveSettingRequest_OnMessage;

            playerChatSocket.Connect();
            playerChatSocket.Send("Sucesssfully connected to the player chat ws server");
            playerChatSocket.OnMessage += PlayerChatSocket_OnMessage;
        }

        private void PlayerChatSocket_OnMessage(object sender, MessageEventArgs e) {
            throw new NotImplementedException();
        }

        private void SaveSettingRequest_OnMessage(object sender, MessageEventArgs e) {
            if (e.Data == "LETTER_ERROR") {
                MessageBox.Show("Error with setting the max distance, you cannot set the maximum distance with letter. You might have entered letters.", "Error with setting the max distance", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
        }

        private void ViewRolimon_Click(object sender, RoutedEventArgs e) => CommonlyUsedFunctions.OpenUrl("https://www.google.com");

        private void MaxDist_LostFocus(object sender, RoutedEventArgs e) {
            saveSettingRequest.Send(MaxDist.Text);
        }
    }
}
