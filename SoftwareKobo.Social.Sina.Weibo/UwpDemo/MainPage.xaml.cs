using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using SoftwareKobo.Social.SinaWeibo;

namespace UwpDemo
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var client = new SinaWeiboClient("393209958", "3c2387aa56497a4ed187f146afc8cb34",
                    "http://bing.coding.io/");

                var status = await client.ShareAsync("发送测试微博");
                if (status.ErrorCode == 0)
                {
                    await new MessageDialog("发送成功").ShowAsync();
                }
                else
                {
                    await new MessageDialog("发送失败，错误码：" + status.ErrorCode).ShowAsync();
                }
            }
            catch (UserCancelAuthorizeException)
            {
                await new MessageDialog("你取消了授权").ShowAsync();
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message + Environment.NewLine + ex.StackTrace).ShowAsync();
            }
        }
    }
}