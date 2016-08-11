﻿using SoftwareKobo.Social.Sina.Weibo.Properties;
using System;

namespace SoftwareKobo.Social.Sina.Weibo
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
                if (Settings.Default.AccessToken == null)
                {
                    return false;
                }
                if (Settings.Default.Uid == null)
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