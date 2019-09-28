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

        public static int BGHor
        {
            get => GetOrSetDefault(1, "_BGHor");
            set => SetValue(value, "_BGHor");
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
        public static double NewDMSize
        {
            get => GetOrSetDefault(1.0);
            set => SetValue(value);
        }

        public static double NewDMTran
        {
            get => GetOrSetDefault(1.0);
            set => SetValue(value);
        }

        public static bool DMStatus
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }

        public static string Guanjianzi
        {
            get => GetValue("Guanjianzi");
            set => SetValue(value);
        }

        public static string Yonghu
        {
            get => GetValue("Yonghu");
            set => SetValue(value);
        }

        public static int DMNumber
        {
            get => GetOrSetDefault(0);
            set => SetValue(value);
        }

        public static bool DMBorder
        {
            get => GetValue(true);
            set => SetValue(value);
        }

        public static bool MergeDanmu
        {
            get => GetValue(false);
            set => SetValue(value);
        }

        public static bool BoldDanmu
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }

        public static string DanmuFont
        {
            get => GetOrSetDefault(string.Empty);
            set => SetValue(value);
        }

        public static bool DanmuNotSubtitle
        {
            get => GetValue(false);
            set => SetValue(value);
        }

        public static double DMSize
        {
            get => GetOrSetDefault(22.0);
            set => SetValue(value);
        }

        public static int DMFont
        {
            get => GetOrSetDefault(0);
            set => SetValue(value);
        }

        public static int DMStyle
        {
            get => GetOrSetDefault(0);
            set => SetValue(value);
        }

        public static double DMSpeed
        {
            get => GetOrSetDefault(12.0);
            set => SetValue(value);
        }

        public static double DMTran
        {
            get => GetOrSetDefault(100.0);
            set => SetValue(value);
        }

        public static bool DMVisTop
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }

        public static bool DMVisBottom
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }

        public static bool DMVisRoll
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }

        public static string DMZZ
        {
            get => GetOrSetDefault(string.Empty);
            set => SetValue(value);
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
        public static bool DTCT
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }

        public static bool DT
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }

        public static bool FJ
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }
        #endregion

        #region 黑科技
        public static bool PlayerMode
        {
            get => GetOrSetDefault(false);
            set => SetValue(value);
        }

        public static bool UseHK
        {
            get => GetOrSetDefault(false);
            set => SetValue(value);
        }

        public static bool UseTW
        {
            get => GetOrSetDefault(false);
            set => SetValue(value);
        }

        public static bool UseCN
        {
            get => GetOrSetDefault(false);
            set => SetValue(value);
        }

        public static bool UseVIP
        {
            get => GetOrSetDefault(false);
            set => SetValue(value);
        }

        public static bool UseOtherSite
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }
        #endregion

        #region 用户信息
        public static string AccessKey
        {
            get => GetValue(string.Empty, "Access_key");
            set => SetValue(value, "Access_key");
        }

        public static string BiliplusCookie
        {
            get => GetValue(string.Empty);
            set => SetValue(value);
        }

        public static string RefreshToken
        {
            get => GetValue(string.Empty, "Refresh_Token");
            set => SetValue(value, "Refresh_Token");
        }

        public static DateTime LoginExpires
        {
            get => GetValue(DateTime.Now);
            set => SetValue(value);
        }

        public static long UserID
        {
            get => GetValue(0L);
            set => SetValue(value);
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
        public static double LDMSize
        {
            get => GetOrSetDefault(22.0);
            set => SetValue(value);
        }

        public static int LDMFont
        {
            get => GetOrSetDefault(0);
            set => SetValue(value);
        }

        public static double LDMSpeed
        {
            get => GetOrSetDefault(100.0);
            set => SetValue(value);
        }

        public static double NewLDMSpeed
        {
            get => GetOrSetDefault(12.0);
            set => SetValue(value);
        }

        public static double LDMTran
        {
            get => GetOrSetDefault(1.0);
            set => SetValue(value);
        }

        public static bool LDMGift
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }

        public static bool LAutoGetAward
        {
            get => GetOrSetDefault(false);
            set => SetValue(value);
        }

        public static bool LReceiveGiftMsg
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }

        public static bool LReceiveWelcomeMsg
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }

        public static bool LReceiveSysMsg
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }

        public static double NewLDMSize
        {
            get => GetOrSetDefault(1.0);
            set => SetValue(value);
        }

        public static int LVolume
        {
            get => GetOrSetDefault(100);
            set => SetValue(value);
        }

        public static int LClear
        {
            get => GetOrSetDefault(50);
            set => SetValue(value);
        }

        public static int LDelay
        {
            get => GetOrSetDefault(200);
            set => SetValue(value);
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
            get => GetOrSetDefault(25.0);
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
