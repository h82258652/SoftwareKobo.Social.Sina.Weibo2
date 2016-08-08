using Newtonsoft.Json;
using SoftwareKobo.Social.Sina.Weibo.Core;
using SoftwareKobo.Social.Sina.Weibo.Extensions;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DesktopDemo
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var client = new WeiboClient();
                await client.AuthorizeAsync("393209958", "3c2387aa56497a4ed187f146afc8cb34", "http://bing.coding.io/");
                var user = await client.ShowAsync(long.Parse(client.Uid));
                Image.Source = new BitmapImage(new Uri(user.AvatarHd, UriKind.Absolute));
                UserTextBox.Text = JsonConvert.SerializeObject(user, Formatting.Indented);
                MessageBox.Show("finish");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
    }
}