using System;
using System.Globalization;
using System.Web;
using System.Windows.Forms;

namespace SoftwareKobo.Social.Sina.Weibo.Extensions
{
    public partial class AuthorizeDialog : Form
    {
        public AuthorizeDialog()
        {
            InitializeComponent();
        }

        public string AuthorizeCode
        {
            get;
            private set;
        }

        private void WebBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            Console.WriteLine(e.Url);

            var query = HttpUtility.ParseQueryString(e.Url.Query);
            var code = query.Get("code");
            if (code != null)
            {
                AuthorizeCode = code;
                DialogResult = DialogResult.OK;
            }
        }

        private void AuthorizeDialog_Shown(object sender, EventArgs e)
        {
            string language = "";
            var lcid = CultureInfo.CurrentUICulture.LCID;
            if (lcid == 2052// 中国大陆
                || lcid == 4100// 新加坡
                || lcid == 1028// 台湾
                || lcid == 3076// 香港
                || lcid == 5124// 澳门
                )
            {
            }
            else
            {
                language = "en";
            }

            // TODO
            webBrowser.Navigate("https://api.weibo.com/oauth2/authorize");
        }
    }
}