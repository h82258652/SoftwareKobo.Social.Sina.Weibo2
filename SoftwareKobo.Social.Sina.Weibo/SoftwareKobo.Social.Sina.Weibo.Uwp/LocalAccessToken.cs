using System;
using Windows.Storage;

namespace SoftwareKobo.Social.Sina.Weibo
{
    internal static class LocalAccessToken
    {
        internal static DateTime ExpiresAt
        {
            get
            {
                return (DateTime)DataContainer.Values["ExpiresAt"];
            }
            set
            {
                DataContainer.Values["ExpiresAt"] = value;
            }
        }

        internal static bool IsUseable
        {
            get
            {
                if (Value == null)
                {
                    return false;
                }
                if (Uid == null)
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
                return (string)DataContainer.Values["Uid"];
            }
            set
            {
                DataContainer.Values["Uid"] = value;
            }
        }

        internal static string Value
        {
            get
            {
                return (string)DataContainer.Values["AccessToken"];
            }
            set
            {
                DataContainer.Values["AccessToken"] = value;
            }
        }

        private static ApplicationDataContainer DataContainer
        {
            get
            {
                // TODO to const
                return ApplicationData.Current.LocalSettings.CreateContainer("SinaWeibo", ApplicationDataCreateDisposition.Always);
            }
        }
    }
}