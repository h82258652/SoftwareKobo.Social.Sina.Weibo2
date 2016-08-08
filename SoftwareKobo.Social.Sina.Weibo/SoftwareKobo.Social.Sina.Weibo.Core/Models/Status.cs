﻿using Newtonsoft.Json;

namespace SoftwareKobo.Social.Sina.Weibo.Core.Models
{
    [JsonObject]
    public class Status
    {
        /// <summary>
        /// 微博创建时间
        /// </summary>
        [JsonProperty("created_at")]
        public string CreatedAt
        {
            get;
            set;
        }

        /// <summary>
        /// 微博ID
        /// </summary>
        [JsonProperty("id")]
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// 微博MID
        /// </summary>
        [JsonProperty("mid")]
        public long Mid
        {
            get;
            set;
        }

        /// <summary>
        /// 字符串型的微博ID
        /// </summary>
        [JsonProperty("idstr")]
        public string IdStr
        {
            get;
            set;
        }

        /// <summary>
        /// 微博信息内容
        /// </summary>
        [JsonProperty("text")]
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// 微博来源
        /// </summary>
        [JsonProperty("source")]
        public string Source
        {
            get;
            set;
        }

        /// <summary>
        /// 是否已收藏，true：是，false：否
        /// </summary>
        [JsonProperty("favorited")]
        public bool Favorited
        {
            get;
            set;
        }

        /// <summary>
        /// 是否被截断，true：是，false：否
        /// </summary>
        [JsonProperty("truncated")]
        public bool Truncated
        {
            get;
            set;
        }

        /// <summary>
        /// （暂未支持）回复ID
        /// </summary>
        [JsonProperty("in_reply_to_status_id")]
        public string InReplyToStatusId
        {
            get;
            set;
        }

        /// <summary>
        /// （暂未支持）回复人UID
        /// </summary>
        [JsonProperty("in_reply_to_user_id")]
        public string InReplyToUserId
        {
            get;
            set;
        }

        /// <summary>
        /// （暂未支持）回复人昵称
        /// </summary>
        [JsonProperty("in_reply_to_screen_name")]
        public string InReplyToScreenName
        {
            get;
            set;
        }

        /// <summary>
        /// 缩略图片地址，没有时不返回此字段
        /// </summary>
        [JsonProperty("thumbnail_pic")]
        public string ThumbnailPic
        {
            get;
            set;
        }

        /// <summary>
        /// 中等尺寸图片地址，没有时不返回此字段
        /// </summary>
        [JsonProperty("bmiddle_pic")]
        public string BmiddlePic
        {
            get;
            set;
        }

        /// <summary>
        /// 原始图片地址，没有时不返回此字段
        /// </summary>
        [JsonProperty("original_pic")]
        public string OriginalPic
        {
            get;
            set;
        }

        /// <summary>
        /// 地理信息字段
        /// </summary>
        [JsonProperty("geo")]
        public Geo Geo
        {
            get;
            set;
        }

        /// <summary>
        /// 微博作者的用户信息字段
        /// </summary>
        [JsonProperty("user")]
        public User User
        {
            get;
            set;
        }

        /// <summary>
        /// 被转发的原微博信息字段，当该微博为转发微博时返回
        /// </summary>
        [JsonProperty("retweeted_status")]
        public Status RetweetedStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 转发数
        /// </summary>
        [JsonProperty("reposts_count")]
        public int RepostsCount
        {
            get;
            set;
        }

        /// <summary>
        /// 评论数
        /// </summary>
        [JsonProperty("comments_count")]
        public int CommentsCount
        {
            get;
            set;
        }

        /// <summary>
        /// 表态数
        /// </summary>
        [JsonProperty("attitudes_count")]
        public int AttitudesCount
        {
            get;
            set;
        }

        /// <summary>
        /// 暂未支持
        /// </summary>
        [JsonProperty("mlevel")]
        public int Mlevel
        {
            get;
            set;
        }

        /// <summary>
        /// 微博的可见性及指定可见分组信息。该object中type取值，0：普通微博，1：私密微博，3：指定分组微博，4：密友微博；list_id为分组的组号
        /// </summary>
        [JsonProperty("visible")]
        public StatusVisible Visible
        {
            get;
            set;
        }

        /// <summary>
        /// 微博配图ID。多图时返回多图ID，用来拼接图片url。用返回字段thumbnail_pic的地址配上该返回字段的图片ID，即可得到多个图片url。
        /// </summary>
        [JsonProperty("pic_ids")]
        public object PicIds
        {
            get;
            set;
        }

        /// <summary>
        /// 微博流内的推广微博ID
        /// </summary>
        [JsonProperty("ad")]
        public object[] Ad
        {
            get;
            set;
        }
    }
}