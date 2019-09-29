using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace BackTask
{
    static class ApiHelper
    {
        //九幽反馈
        public const string JyAppkey = @"1afd8ae4b933daa51a39573a5719bba5";
        public const string JySecret = @"d9e7262e70801e795c18dc20e0972df6";

        public const string _appSecret_Wp = "ba3a4e554e9a6e15dc4d1d70c2b154e3";//Wp
        public const string _appSecret_IOS = "8cb98205e9b2ad3669aad0fce12a4c13";//Ios
        public const string _appSecret_Android = "ea85624dfcf12d7cc7b2b3a94fac1f2c";//Android
        public const string _appSecret_DONTNOT = "2ad42749773c441109bdc0191257a664";//Android

        public const string _appKey = "422fd9d7289a1dd9";//Wp
        public const string _appKey_IOS = "4ebafd7c4951b366";
        public const string _appKey_Android = "c1b107428d337928";
        public const string _appkey_DONTNOT = "85eb6835b0a1034e";//e5b8ba95cab6104100be35739304c23a
        //85eb6835b0a1034e,2ad42749773c441109bdc0191257a664
        public static string access_key = string.Empty;

        public static string GetSign(string url)
        {
            string result;
            string str = url.Substring(url.IndexOf("?", 4) + 1);
            List<string> list = str.Split('&').ToList();
            list.Sort();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string str1 in list)
            {
                stringBuilder.Append((stringBuilder.Length > 0 ? "&" : string.Empty));
                stringBuilder.Append(str1);
            }
            stringBuilder.Append(_appSecret_Wp);
            result = GetMd5String(stringBuilder.ToString()).ToLower();
            return result;
        }
        public static string GetSign_Android(string url)
        {
            string result;
            string str = url.Substring(url.IndexOf("?", 4) + 1);
            List<string> list = str.Split('&').ToList();
            list.Sort();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string str1 in list)
            {
                stringBuilder.Append((stringBuilder.Length > 0 ? "&" : string.Empty));
                stringBuilder.Append(str1);
            }
            stringBuilder.Append(_appSecret_Android);
            result = GetMd5String(stringBuilder.ToString()).ToLower();
            return result;
        }

        public static long GetTimeSpen()
        {
            return Convert.ToInt64((DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
        }

        public static string GetMd5String(string result)
        {
            //可以选择MD5 Sha1 Sha256 Sha384 Sha512
            string strAlgName = HashAlgorithmNames.Md5;

            // 创建一个 HashAlgorithmProvider 对象
            HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm(strAlgName);

            // 创建一个可重用的CryptographicHash对象           
            CryptographicHash objHash = objAlgProv.CreateHash();

            IBuffer buffMsg1 = CryptographicBuffer.ConvertStringToBinary(result, BinaryStringEncoding.Utf16BE);
            objHash.Append(buffMsg1);
            IBuffer buffHash1 = objHash.GetValueAndReset();
            string strHash1 = CryptographicBuffer.EncodeToHexString(buffHash1);
            return strHash1;
        }
    }
}
