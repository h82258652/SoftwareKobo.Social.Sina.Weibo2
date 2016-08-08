using SoftwareKobo.Social.Sina.Weibo.Core;
using SoftwareKobo.Social.Sina.Weibo.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace DesktopDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            WeiboClient client = new WeiboClient();
            await client.AuthorizeAsync("393209958", "3c2387aa56497a4ed187f146afc8cb34", "http://bing.coding.io/");

            //var result = await client.UpdateAsync(ToSend.Text);
            var sss = await client.ShowAsync(5523030798);
            Image.Source = new BitmapImage(new Uri(sss.AvatarHd, UriKind.Absolute));
            MessageBox.Show("finish");
            Debugger.Break();
        }
    }
}