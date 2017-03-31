using System;
using Windows.Storage;

namespace SoftwareKobo.Social.SinaWeibo
{
    internal static class LocalAccessToken
    {
        internal static DateTimeOffset ExpiresAt
        {
            get
            {
                if (DataContainer.Values.ContainsKey("ExpiresAt"))
                {
                    return (DateTimeOffset)DataContainer.Values["ExpiresAt"];
                }
                else
                {
                    return DateTimeOffset.MinValue;
                }
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
                return ExpiresAt > DateTimeOffset.Now;
            }
        }

        internal static string Uid
        {
            get
            {
                if (DataContainer.Values.ContainsKey("Uid"))
                {
                    return (string)DataContainer.Values["Uid"];
                }
                else
                {
                    return null;
                }
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
                if (DataContainer.Values.ContainsKey("AccessToken"))
                {
                    return (string)DataContainer.Values["AccessToken"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                DataContainer.Values["AccessToken"] = value;
            }
        }

        private static ApplicationDataContainer DataContainer => ApplicationData.Current.LocalSettings.CreateContainer(Constants.LocalAccessTokenDataContainerName, ApplicationDataCreateDisposition.Always);
    }
}