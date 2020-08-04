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
        WebSocket playerChatSocket = new WebSocket("ws://localhost:24892/custom?channel=ChatSocket");
        // websocket serrver that is for the connection between the get_user_Value functions it is also used for receiveing message 
        WebSocket PlayerDataSocket = new WebSocket("ws://localhost:24892/custom?channel=PlayerDataSocket");
        public Uri AvatarThumbnail { get; set; }

        public MainWindow() {
            InitializeComponent();
            // connect the websocket servers
            saveSettingRequest.ConnectAsync();
            saveSettingRequest.OnError += SaveSettingRequest_OnError;
            saveSettingRequest.OnMessage += SaveSettingRequest_OnMessage;

            playerChatSocket.ConnectAsync();
            playerChatSocket.OnMessage += PlayerChatSocket_OnMessage;

            PlayerDataSocket.ConnectAsync();
            PlayerDataSocket.OnMessage += PlayerDataSocket_OnMessage;

        }

        private void SaveSettingRequest_OnError(object sender, ErrorEventArgs e) => MessageBox.Show($"Error with conneting to the websocket server :\n{e.Message}\nMaybe synapse isn't  opened loaded or you have't  turned on the websocket in the theme.json of synapse?", "Error with connecting to the websocket server.");

        string rolimon_url;
        private void PlayerDataSocket_OnMessage(object sender, MessageEventArgs e) {
            SpeakerData currentSpeakerData = JsonConvert.DeserializeObject<SpeakerData>(e.Data);
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = currentSpeakerData.AvatarThumb;
            bitmap.EndInit();

            void Action() {
                /*PlayerImage.Dispatcher.Invoke(callback: () => */
                //PlayerImage.Source = bitmap;
                /*PlayerName.Dispatcher.Invoke(() => */
                PlayerName.Content = currentSpeakerData.Username;
                /*rap.Dispatcher.Invoke(() => */
                rap.Content = currentSpeakerData.RecentAveragePrice.ToString();
                /*value.Dispatcher.Invoke(() => */
                value.Content = currentSpeakerData.Value.ToString();
                /*PlayerMessage.Dispatcher.Invoke(() =>.*/
                PlayerMessage.Text = currentSpeakerData.Message;

            }
            Application.Current.Dispatcher.Invoke(Action);
            rolimon_url = "https://www.rolimons.com/player/" + currentSpeakerData.UserId;
        }

        private void SaveSettingRequest_OnOpen(object sender, EventArgs e) => saveSettingRequest.Send("Sucessfully connected to the setting ws server");

        private void PlayerChatSocket_OnMessage(object sender, MessageEventArgs e) {
            throw new NotImplementedException();
        }

        private void SaveSettingRequest_OnMessage(object sender, MessageEventArgs e) {
            if (e.Data == "LETTER_ERROR")
                MessageBox.Show("Error with setting the max distance, you cannot set the maximum distance with letter. You might have entered letters.", "Error with setting the max distance", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void ViewRolimon_Click(object sender, RoutedEventArgs e) => CommonlyUsedFunctions.OpenUrl(rolimon_url);

        private void MaxDist_LostFocus(object sender, RoutedEventArgs e) {
            saveSettingRequest.Send(MaxDist.Text);
        }

        private void HighlightPlayer_Click(object sender, RoutedEventArgs e) {

        }
    }
}
