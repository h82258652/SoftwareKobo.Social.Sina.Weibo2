using System;
using System.Windows;
using SoftwareKobo.Social.SinaWeibo;

namespace DesktopDemo
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var client = new SinaWeiboClient("393209958", "3c2387aa56497a4ed187f146afc8cb34", "http://bing.coding.io/");

                var status = await client.ShareAsync("发送测试微博");
                if (status.ErrorCode == 0)
                {
                    MessageBox.Show("发送成功");
                }
                else
                {
                    MessageBox.Show("发送失败，错误码：" + status.ErrorCode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
    }
}