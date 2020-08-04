using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using WebSocketSharp;
namespace nearby_notif
{
    /// <summary>C:\Users\ASD\Project\csharp\nearby notif\nearby notif\neaby notif script.lua
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // websocket server
        WebSocket saveSettingRequest = new WebSocket("ws://localhost:24892/custom?channel=settingSaveRequest");
        // websocket serrver that is for the connection between the get_user_Value functions it is also used for receiveing message 
        WebSocket PlayerDataSocket = new WebSocket("ws://localhost:24892/custom?channel=PlayerDataSocket");
        WebSocket ExecuteSocket = new WebSocket("ws://localhost:24892/execute");
        public Uri AvatarThumbnail { get; set; }

        public MainWindow() {
            InitializeComponent();
            // connect the websocket servers
            saveSettingRequest.ConnectAsync();
            saveSettingRequest.OnError += SaveSettingRequest_OnError;
            saveSettingRequest.OnMessage += SaveSettingRequest_OnMessage;

            PlayerDataSocket.ConnectAsync();
            PlayerDataSocket.OnMessage += PlayerDataSocket_OnMessage;

            ExecuteSocket.ConnectAsync();
        }

        private void SaveSettingRequest_OnError(object sender, ErrorEventArgs e) => MessageBox.Show($"Error with conneting to the websocket server :\n{e.Message}\nMaybe synapse isn't  opened loaded or you have't  turned on the websocket in the theme.json of synapse?", "Error with connecting to the websocket server.");

        string rolimon_url;
        string PlayerToRender;
        private void PlayerDataSocket_OnMessage(object sender, MessageEventArgs e) {
            SpeakerData currentSpeakerData = JsonConvert.DeserializeObject<SpeakerData>(e.Data);
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = currentSpeakerData.AvatarThumb;
            bitmap.EndInit();
            void Action() {
                //PlayerImage.Source = bitmap;
                PlayerName.Content = currentSpeakerData.Username;
                if (currentSpeakerData.RecentAveragePrice == -1  && currentSpeakerData.Value == -1) {
                    rap.Content = $"The RAP isn't availaible due to {currentSpeakerData.Username} inventory being private";
                    value.Content = $"The value isn't availaible due to {currentSpeakerData.Username} inventory being private";
                } else {
                    rap.Content = currentSpeakerData.RecentAveragePrice.ToString();
                    value.Content = currentSpeakerData.Value.ToString();
                }
                PlayerMessage.Text = currentSpeakerData.Message;

            }
            Application.Current.Dispatcher.Invoke(Action);
            PlayerToRender = currentSpeakerData.Username;
            rolimon_url = "https://www.rolimons.com/player/" + currentSpeakerData.UserId;
        }

        private void SaveSettingRequest_OnOpen(object sender, EventArgs e) => saveSettingRequest.Send("Sucessfully connected to the setting ws server");

        private void SaveSettingRequest_OnMessage(object sender, MessageEventArgs e) {
            if (e.Data == "LETTER_ERROR")
                MessageBox.Show("Error with setting the max distance, you cannot set the maximum distance with letter. You might have entered letters.", "Error with setting the max distance", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void ViewRolimon_Click(object sender, RoutedEventArgs e) => CommonlyUsedFunctions.OpenUrl(rolimon_url);

        private void MaxDist_LostFocus(object sender, RoutedEventArgs e) {
            saveSettingRequest.Send(MaxDist.Text);
        }

        private void HighlightPlayer_Click_1(object sender, RoutedEventArgs e) {
            if (PlayerToRender == "" || PlayerToRender == null) {
                MessageBox.Show("There isn't any player to highlight.", "No player to highliht", MessageBoxButton.OK, MessageBoxImage.Warning);
            } else {
                ExecuteSocket.Send($"local EzESP = loadstring(game:HttpGetAsync('https://pastebin.com/raw/r5xK0qCP'))() local Tracer = EzESP.Tracer(game.Players[\"{PlayerToRender}\"].Character.PrimaryPart, Color3.fromRGB(255, 211, 36), 1) wait(15) Tracer()");
            }

        }
    }
}
