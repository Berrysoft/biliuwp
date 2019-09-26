using System;
using System.Runtime.CompilerServices;
using BiliBili3.Helper;
using Windows.ApplicationModel;
using Windows.Storage;

namespace BiliBili3
{
    public static class SettingHelper
    {
        static ApplicationDataContainer container;

        static SettingHelper()
        {
            container = ApplicationData.Current.LocalSettings;
        }

        private static T GetValue<T>(T def, [CallerMemberName] string key = null)
        {
            if (container.Values[key] != null)
            {
                return (T)container.Values[key];
            }
            else
            {
                return def;
            }
        }

        private static T GetOrSetDefault<T>(T def, [CallerMemberName] string key = null)
        {
            if (container.Values[key] != null)
            {
                return (T)container.Values[key];
            }
            else
            {
                SetValue(def, key);
                return def;
            }
        }

        private static T GetOrSetDefault<T>(Func<T> getDefault, [CallerMemberName] string key = null)
        {
            if (container.Values[key] != null)
            {
                return (T)container.Values[key];
            }
            else
            {
                var def = getDefault();
                SetValue(def, key);
                return def;
            }
        }

        private static void SetValue<T>(T value, [CallerMemberName] string key = null)
        {
            container.Values[key] = value;
        }

        #region  外观和常规
        public static string Theme
        {
            get => GetValue("Pink");
            set => SetValue(value);
        }

        public static int Rigth
        {
            get => GetValue(1);
            set => SetValue(value);
        }

        public static bool CustomBG
        {
            get => GetOrSetDefault(false);
            set => SetValue(value);
        }

        public static string BGPath
        {
            get => GetOrSetDefault(string.Empty);
            set => SetValue(value);
        }

        public static int BGStretch
        {
            get => GetOrSetDefault(0);
            set => SetValue(value);
        }

        public static int BGVer
        {
            get => GetOrSetDefault(1);
            set => SetValue(value);
        }

        public static int BGOpacity
        {
            get => GetOrSetDefault(10);
            set => SetValue(value);
        }

        public static int FrostedGlass
        {
            get => GetOrSetDefault(0);
            set => SetValue(value);
        }

        public static int BGMaxWidth
        {
            get => GetOrSetDefault(0);
            set => SetValue(value);
        }

        public static int BGMaxHeight
        {
            get => GetOrSetDefault(0);
            set => SetValue(value);
        }

        public static int _BGHor
        {
            get => GetOrSetDefault(1);
            set => SetValue(value);
        }

        public static bool HideStatus
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }

        public static bool LoadSplash
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }

        public static bool MouseBack
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }

        //sw_RefreshButton
        public static bool RefreshButton
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }

        public static bool First
        {
            get => GetOrSetDefault(true, "First" + GetVersion());
            set => SetValue(value, "First" + GetVersion());
        }

        #endregion

        #region 播放器
        public static double Volume
        {
            get => GetOrSetDefault(1.0);
            set => SetValue(value);
        }

        public static double Light
        {
            get => GetOrSetDefault(0.0);
            set => SetValue(value);
        }

        public static int BanPlayer
        {
            get => GetOrSetDefault(2);
            set => SetValue(value);
        }

        public static int PlayQualit
        {
            get => GetOrSetDefault(3);
            set => SetValue(value);
        }

        public static int VideoType
        {
            get => GetOrSetDefault(0);
            set => SetValue(value);
        }

        public static bool ForceAudio
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }

        public static bool ForceVideo
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }

        public static int Playback
        {
            get => GetOrSetDefault(0);
            set => SetValue(value);
        }

        public static bool FFmpeg
        {
            get => GetOrSetDefault(false);
            set => SetValue(value);
        }

        public static bool UseH5
        {
            get => GetOrSetDefault(false);
            set => SetValue(value);
        }

        public static int ClearLiveComment
        {
            get => GetOrSetDefault(1);
            set => SetValue(value);
        }

        public static bool Use4GPlay
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }

        public static bool BackPlay
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }

        public static bool QZHP
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }

        public static bool AutoFull
        {
            get => GetOrSetDefault(false);
            set => SetValue(value);
        }

        public static bool HideCursor
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }

        public static bool NewFeed
        {
            get => GetOrSetDefault(false);
            set => SetValue(value);
        }

        public static bool NewWindow
        {
            get => GetOrSetDefault(false);
            set => SetValue(value);
        }

        public static int NewQuality
        {
            get => GetOrSetDefault(64);
            set => SetValue(value);
        }

        public static bool UseDASH
        {
            get => GetOrSetDefault(() => SystemHelper.GetSystemBuild() >= 17763);
            set => SetValue(value);
        }

        public static bool DASHUseHEVC
        {
            get => GetOrSetDefault(false);
            set => SetValue(value);
        }
        #endregion

        #region 弹幕设置

        public static double Get_NewDMSize()
        {
            if (container.Values["NewDMSize"] != null)
            {
                return Convert.ToDouble(container.Values["NewDMSize"]);
            }
            else
            {
                container.Values["NewDMSize"] = 1.0;
                return 1.0;
            }
        }

        public static void Set_NewDMSize(double value)
        {
            container.Values["NewDMSize"] = value;
        }

        public static double Get_NewDMTran()
        {
            if (container.Values["NewDMTran"] != null)
            {
                return Convert.ToDouble(container.Values["NewDMTran"]);
            }
            else
            {
                container.Values["NewDMTran"] = 1.0;
                return 1.0;
            }
        }

        public static void Set_DMStatus(bool value)
        {
            container.Values["DMStatus"] = value;
        }

        public static bool Get_DMStatus()
        {
            if (container.Values["DMStatus"] != null)
            {
                return Convert.ToBoolean(container.Values["DMStatus"]);
            }
            else
            {
                container.Values["DMStatus"] = true;
                return true;
            }
        }

        public static void Set_NewDMTran(double value)
        {
            container.Values["NewDMTran"] = value;
        }


        public static void Set_Guanjianzi(string value)
        {
            container.Values["Guanjianzi"] = value;
        }

        public static string Get_Guanjianzi()
        {
            if (container.Values["Guanjianzi"] != null)
            {
                return (string)container.Values["Guanjianzi"];
            }
            else
            {
                return "Guanjianzi";
            }
        }

        public static void Set_Yonghu(string value)
        {
            container.Values["Yonghu"] = value;
        }

        public static string Get_Yonghu()
        {
            if (container.Values["Yonghu"] != null)
            {
                return (string)container.Values["Yonghu"];
            }
            else
            {
                return "Yonghu";
            }
        }




        public static int Get_DMNumber()
        {
            if (container.Values["DMNumber"] != null)
            {
                return Convert.ToInt32(container.Values["DMNumber"]);
            }
            else
            {
                container.Values["DMNumber"] = 0;
                return 0;
            }
        }

        public static void Set_DMNumber(int value)
        {
            container.Values["DMNumber"] = value;
        }


        //Get_DMBorder
        public static bool Get_DMBorder()
        {
            if (container.Values["DMBorder"] != null)
            {
                return (bool)container.Values["DMBorder"];
            }
            else
            {
                return true;
            }
        }

        public static void Set_DMBorder(bool value)
        {
            container.Values["DMBorder"] = value;
        }

        public static bool Get_MergeDanmu()
        {
            if (container.Values["MergeDanmu"] != null)
            {
                return (bool)container.Values["MergeDanmu"];
            }
            else
            {
                return false;
            }
        }

        public static void Set_MergeDanmu(bool value)
        {
            container.Values["MergeDanmu"] = value;
        }

        public static bool Get_BoldDanmu()
        {
            if (container.Values["BoldDanmu"] != null)
            {
                return (bool)container.Values["BoldDanmu"];
            }
            else
            {
                Set_BoldDanmu(true);
                return true;
            }
        }

        public static void Set_BoldDanmu(bool value)
        {
            container.Values["BoldDanmu"] = value;
        }


        public static string Get_DanmuFont()
        {

            if (container.Values["DanmuFont"] != null)
            {
                return (string)container.Values["DanmuFont"];
            }
            else
            {
                Set_DanmuFont("");
                return "";
            }
        }

        public static void Set_DanmuFont(string value)
        {

            container.Values["DanmuFont"] = value;
        }


        //DanmuNotSubtitle
        public static bool Get_DanmuNotSubtitle()
        {

            if (container.Values["DanmuNotSubtitle"] != null)
            {
                return (bool)container.Values["DanmuNotSubtitle"];
            }
            else
            {
                return false;
            }
        }

        public static void Set_DanmuNotSubtitle(bool value)
        {

            container.Values["DanmuNotSubtitle"] = value;
        }


        public static double Get_DMSize()
        {

            if (container.Values["DMSize"] != null)
            {
                return Convert.ToDouble(container.Values["DMSize"]);
            }
            else
            {
                container.Values["DMSize"] = 22;
                return 22;
            }
        }

        public static void Set_DMSize(double value)
        {

            container.Values["DMSize"] = value;
        }


        public static int Get_DMFont()
        {

            if (container.Values["DMFont"] != null)
            {
                return (int)container.Values["DMFont"];
            }
            else
            {
                container.Values["DMFont"] = 0;
                return 0;
            }
        }

        public static void Set_DMFont(int value)
        {

            container.Values["DMFont"] = value;
        }


        public static int Get_DMStyle()
        {

            if (container.Values["DMStyle"] != null)
            {
                return (int)container.Values["DMStyle"];
            }
            else
            {
                container.Values["DMStyle"] = 0;
                return 0;
            }
        }

        public static void Set_DMStyle(int value)
        {

            container.Values["DMStyle"] = value;
        }



        public static double Get_DMSpeed()
        {

            if (container.Values["DMSpeed"] != null)
            {

                return Convert.ToDouble(container.Values["DMSpeed"]);
            }
            else
            {

                container.Values["DMSpeed"] = 12;
                return 12;



            }
        }

        public static void Set_DMSpeed(double value)
        {

            container.Values["DMSpeed"] = value;
        }

        public static double Get_DMTran()
        {

            if (container.Values["DMTran"] != null)
            {
                return Convert.ToDouble(container.Values["DMTran"]);
            }
            else
            {

                container.Values["DMTran"] = 100;
                return 100;



            }
        }

        public static void Set_DMTran(double value)
        {

            container.Values["DMTran"] = value;
        }



        public static bool Get_DMVisTop()
        {

            if (container.Values["DMVisTop"] != null)
            {
                return (bool)container.Values["DMVisTop"];
            }
            else
            {
                Set_DMVisTop(true);
                return true;
            }
        }

        public static void Set_DMVisTop(bool value)
        {

            container.Values["DMVisTop"] = value;
        }

        public static bool Get_DMVisBottom()
        {

            if (container.Values["DMVisBottom"] != null)
            {
                return (bool)container.Values["DMVisBottom"];
            }
            else
            {
                Set_DMVisBottom(true);
                return true;
            }
        }

        public static void Set_DMVisBottom(bool value)
        {

            container.Values["DMVisBottom"] = value;
        }

        public static bool Get_DMVisRoll()
        {

            if (container.Values["DMVisRoll"] != null)
            {
                return (bool)container.Values["DMVisRoll"];
            }
            else
            {
                Set_DMVisRoll(true);
                return true;
            }
        }

        public static void Set_DMVisRoll(bool value)
        {

            container.Values["DMVisRoll"] = value;
        }

        public static string Get_DMZZ()
        {

            if (container.Values["DMZZ"] != null)
            {
                return (string)container.Values["DMZZ"];
            }
            else
            {
                Set_DMZZ("");
                return "";
            }
        }

        public static void Set_DMZZ(string value)
        {

            container.Values["DMZZ"] = value;
        }







        #endregion


        #region 下载
        public static int DownQualit
        {
            get => GetOrSetDefault(3);
            set => SetValue(value);
        }

        public static int DownMode
        {
            get => GetOrSetDefault(1);
            set => SetValue(value);
        }

        public static bool CustomDownPath
        {
            get => GetOrSetDefault(false);
            set => SetValue(value);
        }

        public static string DownPath
        {
            get => GetOrSetDefault("系统视频库");
            set => SetValue(value);
        }

        public static bool Use4GDown
        {
            get => GetOrSetDefault(false);
            set => SetValue(value);
        }

        public static bool ToMp4
        {
            get => GetOrSetDefault(false);
            set => SetValue(value);
        }

        #endregion


        #region 通知


        public static bool Get_DTCT()
        {

            if (container.Values["DTCT"] != null)
            {
                return (bool)container.Values["DTCT"];
            }
            else
            {
                Set_DTCT(true);
                return true;
            }
        }
        public static void Set_DTCT(bool value)
        {

            container.Values["DTCT"] = value;
        }



        public static bool Get_DT()
        {

            if (container.Values["DT"] != null)
            {
                return (bool)container.Values["DT"];
            }
            else
            {
                Set_DT(true);
                return true;
            }
        }

        public static void Set_DT(bool value)
        {

            container.Values["DT"] = value;
        }

        public static bool Get_FJ()
        {

            if (container.Values["FJ"] != null)
            {
                return (bool)container.Values["FJ"];
            }
            else
            {
                Set_FJ(true);
                return true;
            }
        }

        public static void Set_FJ(bool value)
        {

            container.Values["FJ"] = value;
        }

        public static string Get_TsDt()
        {

            if (container.Values["TsDt"] != null)
            {
                return (string)container.Values["TsDt"];
            }
            else
            {
                Set_TsDt("");
                return "";
            }
        }

        public static void Set_TsDt(string value)
        {

            container.Values["TsDt"] = value;
        }


        #endregion

        #region 黑科技

        public static bool Get_PlayerMode()
        {

            if (container.Values["PlayerMode"] != null)
            {
                return (bool)container.Values["PlayerMode"];
            }
            else
            {
                Set_PlayerMode(false);
                return false;
            }
        }

        public static void Set_PlayerMode(bool value)
        {

            container.Values["PlayerMode"] = value;
        }



        public static bool Get_UseHK()
        {

            if (container.Values["UseHK"] != null)
            {
                return (bool)container.Values["UseHK"];
            }
            else
            {
                Set_UseHK(false);
                return false;
            }
        }

        public static void Set_UseHK(bool value)
        {

            container.Values["UseHK"] = value;
        }


        public static bool Get_UseTW()
        {

            if (container.Values["UseTW"] != null)
            {
                return (bool)container.Values["UseTW"];
            }
            else
            {
                Set_UseTW(false);
                return false;
            }
        }

        public static void Set_UseTW(bool value)
        {

            container.Values["UseTW"] = value;
        }



        public static bool Get_UseCN()
        {

            if (container.Values["UseCN"] != null)
            {
                return (bool)container.Values["UseCN"];
            }
            else
            {
                Set_UseCN(false);
                return false;
            }
        }

        public static void Set_UseCN(bool value)
        {

            container.Values["UseCN"] = value;
        }


        public static bool Get_UseVIP()
        {

            if (container.Values["UseVIP"] != null)
            {
                return (bool)container.Values["UseVIP"];
            }
            else
            {
                Set_UseVIP(false);
                return false;
            }
        }

        public static void Set_UseVIP(bool value)
        {

            container.Values["UseVIP"] = value;
        }


        public static bool Get_UseOtherSite()
        {

            if (container.Values["UseOtherSite"] != null)
            {
                return (bool)container.Values["UseOtherSite"];
            }
            else
            {
                Set_UseOtherSite(true);
                return true;
            }
        }

        public static void Set_UseOtherSite(bool value)
        {

            container.Values["UseOtherSite"] = value;
        }
        #endregion

        #region 用户信息

        public static string Get_UserName()
        {

            if (container.Values["UserName"] != null)
            {
                return (string)container.Values["UserName"];
            }
            else
            {
                return "";
            }
        }
        public static void Set_UserName(string value)
        {

            container.Values["UserName"] = value;
        }
        public static string Get_Password()
        {

            if (container.Values["Password"] != null)
            {
                return (string)container.Values["Password"];
            }
            else
            {
                return "";
            }
        }
        public static void Set_Password(string value)
        {

            container.Values["Password"] = value;
        }

        public static string Get_Access_key()
        {

            if (container.Values["Access_key"] != null)
            {
                return (string)container.Values["Access_key"];
            }
            else
            {
                return "";
            }
        }
        public static void Set_Access_key(string value)
        {

            container.Values["Access_key"] = value;
        }

        public static string Get_BiliplusCookie()
        {

            if (container.Values["BiliplusCookie"] != null)
            {
                return (string)container.Values["BiliplusCookie"];
            }
            else
            {
                return "";
            }
        }
        public static void Set_BiliplusCookie(string value)
        {

            container.Values["BiliplusCookie"] = value;
        }


        public static string Get_Refresh_Token()
        {

            if (container.Values["Refresh_Token"] != null)
            {
                return (string)container.Values["Refresh_Token"];
            }
            else
            {
                return "";
            }
        }
        public static void Set_Refresh_Token(string value)
        {

            container.Values["Refresh_Token"] = value;
        }

        public static DateTime Get_LoginExpires()
        {

            if (container.Values["LoginExpires"] != null)
            {
                return Convert.ToDateTime(container.Values["LoginExpires"]);
            }
            else
            {
                return DateTime.Now;
            }
        }
        public static void Set_LoginExpires(DateTime value)
        {

            container.Values["LoginExpires"] = value.ToString();
        }

        public static long Get_UserID()
        {

            if (container.Values["UserID"] != null)
            {
                return Convert.ToInt64(container.Values["UserID"]);
            }
            else
            {
                return 0;
            }
        }
        public static void Set_UserID(long value)
        {

            container.Values["UserID"] = value;
        }

        #endregion

        #region 系统方法

        static readonly PackageId pack = (Package.Current).Id;
        public static string GetVersion()
        {
            return string.Format("{0}.{1}.{2}.{3}", pack.Version.Major, pack.Version.Minor, pack.Version.Build, pack.Version.Revision);
        }

        #endregion


        #region 直播弹幕
        public static double Get_LDMSize()
        {

            if (container.Values["LDMSize"] != null)
            {
                return Convert.ToDouble(container.Values["LDMSize"]);
            }
            else
            {
                container.Values["LDMSize"] = 22;
                return 22;
            }
        }

        public static void Set_LDMSize(double value)
        {

            container.Values["LDMSize"] = value;
        }


        public static int Get_LDMFont()
        {

            if (container.Values["LDMFont"] != null)
            {
                return (int)container.Values["LDMFont"];
            }
            else
            {
                container.Values["LDMFont"] = 0;
                return 0;
            }
        }

        public static void Set_LDMFont(int value)
        {

            container.Values["LDMFont"] = value;
        }

        public static double Get_LDMSpeed()
        {

            if (container.Values["LDMSpeed"] != null)
            {
                return Convert.ToDouble(container.Values["LDMSpeed"]);
            }
            else
            {

                container.Values["LDMSpeed"] = 100;
                return 100;



            }
        }

        public static void Set_LDMSpeed(double value)
        {

            container.Values["LDMSpeed"] = value;
        }


        public static double Get_NewLDMSpeed()
        {

            if (container.Values["NewLDMSpeed"] != null)
            {
                return Convert.ToDouble(container.Values["NewLDMSpeed"]);
            }
            else
            {

                container.Values["NewLDMSpeed"] = 12;
                return 12;



            }
        }

        public static void Set_NewLDMSpeed(double value)
        {

            container.Values["NewLDMSpeed"] = value;
        }


        public static double Get_LDMTran()
        {

            if (container.Values["LDMTran"] != null)
            {
                double d = Convert.ToDouble(container.Values["LDMTran"]);
                return d;
            }
            else
            {

                container.Values["LDMTran"] = 1;
                return 1;



            }
        }

        public static void Set_LDMTran(double value)
        {

            container.Values["LDMTran"] = value;
        }


        public static bool Get_LDMGift()
        {

            if (container.Values["LDMGift"] != null)
            {
                return (bool)container.Values["LDMGift"];
            }
            else
            {
                Set_LDMGift(true);
                return true;
            }
        }

        public static void Set_LDMGift(bool value)
        {

            container.Values["LDMGift"] = value;
        }


        public static bool Get_LAutoGetAward()
        {

            if (container.Values["LAutoGetAward"] != null)
            {
                return (bool)container.Values["LAutoGetAward"];
            }
            else
            {
                Set_LAutoGetAward(false);
                return false;
            }
        }

        public static void Set_LAutoGetAward(bool value)
        {

            container.Values["LAutoGetAward"] = value;
        }

        public static bool Get_LReceiveGiftMsg()
        {

            if (container.Values["LReceiveGiftMsg"] != null)
            {
                return (bool)container.Values["LReceiveGiftMsg"];
            }
            else
            {
                Set_LReceiveGiftMsg(true);
                return true;
            }
        }

        public static void Set_LReceiveGiftMsg(bool value)
        {

            container.Values["LReceiveGiftMsg"] = value;
        }

        public static bool Get_LReceiveWelcomeMsg()
        {

            if (container.Values["LReceiveWelcomeMsg"] != null)
            {
                return (bool)container.Values["LReceiveWelcomeMsg"];
            }
            else
            {
                Set_LReceiveWelcomeMsg(true);
                return true;
            }
        }

        public static void Set_LReceiveWelcomeMsg(bool value)
        {

            container.Values["LReceiveWelcomeMsg"] = value;
        }

        public static bool Get_LReceiveSysMsg()
        {

            if (container.Values["LReceiveSysMsg"] != null)
            {
                return (bool)container.Values["LReceiveSysMsg"];
            }
            else
            {
                Set_LReceiveSysMsg(true);
                return true;
            }
        }

        public static void Set_LReceiveSysMsg(bool value)
        {

            container.Values["LReceiveSysMsg"] = value;
        }

        public static double Get_NewLDMSize()
        {

            if (container.Values["NewLDMSize"] != null)
            {
                return Convert.ToDouble(container.Values["NewLDMSize"]);
            }
            else
            {
                container.Values["NewLDMSize"] = 1.0;
                return 1.0;
            }
        }

        public static void Set_NewLDMSize(double value)
        {

            container.Values["NewLDMSize"] = value;
        }

        public static int Get_LVolume()
        {

            if (container.Values["LVolume"] != null)
            {
                return (int)container.Values["LVolume"];
            }
            else
            {
                container.Values["LVolume"] = 100;
                return 100;
            }
        }

        public static void Set_LVolume(int value)
        {

            container.Values["LVolume"] = value;
        }

        public static int Get_LClear()
        {

            if (container.Values["LClear"] != null)
            {
                return (int)container.Values["LClear"];
            }
            else
            {
                container.Values["LClear"] = 50;
                return 50;
            }
        }

        public static void Set_LClear(int value)
        {

            container.Values["LClear"] = value;
        }

        public static int Get_LDelay()
        {

            if (container.Values["LDelay"] != null)
            {
                return (int)container.Values["LDelay"];
            }
            else
            {
                container.Values["LDelay"] = 200;
                return 200;
            }
        }

        public static void Set_LDelay(int value)
        {

            container.Values["LDelay"] = value;
        }

        #endregion

        public static bool PriorityBiliPlus
        {
            get => GetOrSetDefault(false);
            set => SetValue(value);
        }

        #region 字幕设置
        public static double SubtitleSize
        {
            get => GetOrSetDefault(25);
            set => SetValue(value);
        }

        public static string SubtitleFontFamily
        {
            get => GetOrSetDefault("Microsoft YaHei UI");
            set => SetValue(value);
        }

        public static double SubtitleBgTran
        {
            get => GetOrSetDefault(0.5);
            set => SetValue(value);
        }

        public static string SubtitleColor
        {
            get => GetOrSetDefault("#ffffffff");
            set => SetValue(value);
        }
        #endregion
    }
}
