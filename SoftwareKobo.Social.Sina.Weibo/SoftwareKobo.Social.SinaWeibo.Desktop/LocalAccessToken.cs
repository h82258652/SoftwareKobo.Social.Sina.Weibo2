using System;
using SoftwareKobo.Social.SinaWeibo.Properties;

namespace SoftwareKobo.Social.SinaWeibo
{
    internal static class LocalAccessToken
    {
        internal static DateTime ExpiresAt
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
                if (string.IsNullOrEmpty(Settings.Default.AccessToken))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(Settings.Default.Uid))
                {
                    return false;
                }
                return ExpiresAt > DateTime.Now;
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
    }
}