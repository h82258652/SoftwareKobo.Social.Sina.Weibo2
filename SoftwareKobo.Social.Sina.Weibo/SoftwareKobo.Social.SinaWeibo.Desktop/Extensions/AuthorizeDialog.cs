using System;
using System.Web;
using System.Windows.Forms;

namespace SoftwareKobo.Social.SinaWeibo.Extensions
{
    public partial class AuthorizeDialog : Form
    {
        private readonly Uri _requestUri;

        public AuthorizeDialog(Uri requestUri)
        {
            InitializeComponent();
            _requestUri = requestUri;
        }

        public string AuthorizeCode
        {
            get;
            private set;
        }

        private void AuthorizeDialog_Shown(object sender, EventArgs e)
        {
            webBrowser.Navigate(_requestUri);
        }

        private void WebBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            var query = HttpUtility.ParseQueryString(e.Url.Query);
            var code = query.Get("code");
            if (code != null)
            {
                AuthorizeCode = code;
                DialogResult = DialogResult.OK;
            }
        }
    }
}