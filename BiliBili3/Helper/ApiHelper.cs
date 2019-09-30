using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiliBili3.Class;
using BiliBili3.Models;
using Newtonsoft.Json;

namespace BiliBili3
{
    public static class ApiHelper
    {
        //九幽反馈
        public const string JyAppkey = @"afaaf76fbe62a275d4dc309d6151d3c3";
        public static readonly ApiKeyInfo AndroidKey = new ApiKeyInfo("1d8b6e7d45233436", "560c52ccd288fed045859ed18bffd973");
        public static readonly ApiKeyInfo AndroidVideoKey = new ApiKeyInfo("iVGUTjsxvpLeuDCf", "aHRmhWMLkdeMuILqORnYZocwMBpMEOdt");
        public static readonly ApiKeyInfo WebVideoKey = new ApiKeyInfo("84956560bc028eb7", "94aba54af9065f71de72f5508f1cd42e");
        public static readonly ApiKeyInfo VideoKey = new ApiKeyInfo("", "1c15888dc316e05a15fdd0a02ed6584f");
        public static readonly ApiKeyInfo IosKey = new ApiKeyInfo("4ebafd7c4951b366", "8cb98205e9b2ad3669aad0fce12a4c13");

        public const string build = "5442100";

        private static string _access_key;
        public static string AccessKey
        {
            get
            {
                if (string.IsNullOrEmpty(_access_key))
                {
                    return SettingHelper.AccessKey;
                }
                else
                {
                    return _access_key;
                }
            }
            set => _access_key = value;
        }

        public static string GetSign(string url, ApiKeyInfo apiKeyInfo = null)
        {
            if (apiKeyInfo == null)
            {
                apiKeyInfo = AndroidKey;
            }
            string result;
            string str = url.Substring(url.IndexOf("?", 4) + 1);
            var list = str.Split('&');
            Array.Sort(list);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendJoin('&', list);
            stringBuilder.Append(apiKeyInfo.Secret);
            result = MD5.GetMd5String(stringBuilder.ToString()).ToLower();
            return result;
        }

        public static string GetSignWithUrl(string url, ApiKeyInfo apiKeyInfo = null)
        {
            return url + "&sign=" + GetSign(url, apiKeyInfo);
        }

        public static long TimeStamp => Convert.ToInt64((DateTime.Now - new DateTime(1970, 1, 1, 8, 0, 0, 0)).TotalSeconds);

        public static long TimeStamp2 => Convert.ToInt64((DateTime.Now - new DateTime(1970, 1, 1, 8, 0, 0, 0)).TotalMilliseconds);

        public static List<EmojiModel> emojis;
        public static List<FaceModel> emoji;
        public static async void SetEmojis()
        {
            try
            {
                string url = "http://api.bilibili.com/x/v2/reply/emojis";
                string results = await WebClientClass.GetResults(new Uri(url));
                FaceModel model = JsonConvert.DeserializeObject<FaceModel>(results);
                emoji = model.data;
                emojis = emoji.SelectMany(x => x.emojis).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public static List<RegionModel> regions;
        public static async Task SetRegionsAsync()
        {
            try
            {
                string url = string.Format("https://app.bilibili.com/x/v2/region/index?appkey={0}&build={2}&mobi_app=android&platform=android&ts={1}", ApiHelper.AndroidKey.Appkey, TimeStamp, ApiHelper.build);
                url = GetSignWithUrl(url);

                string results = await WebClientClass.GetResults(new Uri(url));
                RegionModel model = JsonConvert.DeserializeObject<RegionModel>(results);
                if (model.Code == 0)
                {
                    model.Data.RemoveAll(x => x.Name == "会员购" || x.Name == "游戏中心" || string.IsNullOrEmpty(x.Logo));
                    regions = model.Data;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public static async void SetRegions() => await SetRegionsAsync();

        public static string GetUserId()
        {
            if (IsLogin())
            {
                return SettingHelper.UserID.ToString();
            }
            else
            {
                return "0";
            }
        }

        public static bool IsLogin()
        {
            return !string.IsNullOrEmpty(SettingHelper.AccessKey);
        }
    }

    public class ApiKeyInfo
    {
        public ApiKeyInfo(string key, string secret)
        {
            Appkey = key;
            Secret = secret;
        }
        public string Appkey { get; set; }
        public string Secret { get; set; }
    }

    public class RegionModel
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("data")]
        public List<RegionModel> Data { get; set; }

        [JsonProperty("tid")]
        public int Tid { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("logo")]
        public string Logo { get; set; }
        [JsonProperty("uri")]
        public string Uri { get; set; }
        [JsonProperty("children")]
        public List<RegionModel> Children { get; set; }
    }
}
