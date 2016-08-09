using SoftwareKobo.Social.Sina.Weibo.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareKobo.Social.Sina.Weibo
{
    internal static class LocalAccessToken
    {
        internal static string Value
        {
            get
            {
                return Settings.Default.AccessToken;
            }
            set
            {
                Settings.Default.AccessToken = value;
                Settings.Default.Save();
            }
        }

        internal static string Uid
        {
            get
            {
                return Settings.Default.Uid;
            }
            set
            {
                Settings.Default.Uid = value;
                Settings.Default.Save();
            }
        }

        internal static long ExpiresAt
        {
            get
            {
                return Settings.Default.ExpiresAt;
            }
            set
            {
                Settings.Default.ExpiresAt = value;
                Settings.Default.Save();
            }
        }

        internal static bool IsUseable
        {
            get
            {
            }
        }
    }
}