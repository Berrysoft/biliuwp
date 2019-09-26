using System;
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

        #region  外观和常规
        public static string Get_Theme()
        {
            if (container.Values["Theme"] != null)
            {
                return (string)container.Values["Theme"];
            }
            else
            {
                return "Pink";
            }
        }

        public static void Set_Theme(string value)
        {
            container.Values["Theme"] = value;
        }

        public static int Get_Rigth()
        {
            if (container.Values["Rigth"] != null)
            {
                return (int)container.Values["Rigth"];
            }
            else
            {
                return 1;
            }
        }



        public static void Set_Rigth(int value)
        {
            container.Values["Rigth"] = value;
        }


        public static void Set_CustomBG(bool value)
        {
            container.Values["CustomBG"] = value;
        }

        public static bool Get_CustomBG()
        {
            if (container.Values["CustomBG"] != null)
            {
                return (bool)container.Values["CustomBG"];
            }
            else
            {
                Set_CustomBG(false);
                return false;
            }
        }

        public static void Set_BGPath(string value)
        {
            container.Values["BGPath"] = value;
        }

        public static string Get_BGPath()
        {
            if (container.Values["BGPath"] != null)
            {
                return container.Values["BGPath"].ToString();
            }
            else
            {
                Set_BGPath("");
                return "";
            }
        }


        public static void Set_BGStretch(int value)
        {
            container.Values["BGStretch"] = value;
        }

        public static int Get_BGStretch()
        {
            if (container.Values["BGStretch"] != null)
            {
                return (int)container.Values["BGStretch"];
            }
            else
            {
                Set_BGStretch(0);
                return 0;
            }
        }



        public static void Set_BGVer(int value)
        {
            container.Values["BGVer"] = value;
        }

        public static int Get_BGVer()
        {
            if (container.Values["BGVer"] != null)
            {
                return (int)container.Values["BGVer"];
            }
            else
            {
                Set_BGVer(1);
                return 1;
            }
        }

        public static void Set_BGOpacity(int value)
        {
            container.Values["BGOpacity"] = value;
        }

        public static int Get_BGOpacity()
        {
            if (container.Values["BGOpacity"] != null)
            {
                return (int)container.Values["BGOpacity"];
            }
            else
            {
                Set_BGOpacity(10);
                return 10;
            }
        }

        public static void Set_FrostedGlass(int value)
        {
            container.Values["FrostedGlass"] = value;
        }

        public static int Get_FrostedGlass()
        {
            if (container.Values["FrostedGlass"] != null)
            {
                return (int)container.Values["FrostedGlass"];
            }
            else
            {
                Set_FrostedGlass(0);
                return 0;
            }
        }

        public static void Set_BGMaxWidth(int value)
        {
            container.Values["BGMaxWidth"] = value;
        }

        public static int Get_BGMaxWidth()
        {
            if (container.Values["BGMaxWidth"] != null)
            {
                return (int)container.Values["BGMaxWidth"];
            }
            else
            {
                Set_BGMaxWidth(0);
                return 0;
            }
        }

        public static void Set_BGMaxHeight(int value)
        {
            container.Values["BGMaxHeight"] = value;
        }

        public static int Get_BGMaxHeight()
        {
            if (container.Values["BGMaxHeight"] != null)
            {
                return (int)container.Values["BGMaxHeight"];
            }
            else
            {
                Set_BGMaxHeight(0);
                return 0;
            }
        }



        public static void Set_BGHor(int value)
        {
            container.Values["_BGHor"] = value;
        }

        public static int Get__BGHor()
        {
            if (container.Values["_BGHor"] != null)
            {
                return (int)container.Values["_BGHor"];
            }
            else
            {
                Set_BGHor(1);
                return 1;
            }
        }


        public static void Set_HideStatus(bool value)
        {
            container.Values["HideStatus"] = value;
        }

        public static bool Get_HideStatus()
        {
            if (container.Values["HideStatus"] != null)
            {
                return (bool)container.Values["HideStatus"];
            }
            else
            {
                Set_HideStatus(true);
                return true;
            }
        }


        public static void Set_LoadSplash(bool value)
        {
            container.Values["LoadSplash"] = value;
        }

        public static bool Get_LoadSplash()
        {
            if (container.Values["LoadSplash"] != null)
            {
                return (bool)container.Values["LoadSplash"];
            }
            else
            {
                Set_LoadSplash(true);
                return true;
            }
        }


        public static void Set_HideAD(bool value)
        {
            container.Values["HideAD"] = value;
        }

        public static bool Get_HideAD()
        {
            if (container.Values["HideAD"] != null)
            {
                return (bool)container.Values["HideAD"];
            }
            else
            {
                Set_HideAD(false);
                return false;
            }
        }


        public static void Set_MouseBack(bool value)
        {
            container.Values["MouseBack"] = value;
        }

        public static bool Get_MouseBack()
        {
            if (container.Values["MouseBack"] != null)
            {
                return (bool)container.Values["MouseBack"];
            }
            else
            {
                Set_HideAD(true);
                return true;
            }
        }

        //sw_RefreshButton
        public static void Set_RefreshButton(bool value)
        {
            container.Values["RefreshButton"] = value;
        }

        public static bool Get_RefreshButton()
        {
            if (container.Values["RefreshButton"] != null)
            {
                return (bool)container.Values["RefreshButton"];
            }
            else
            {
                Set_RefreshButton(true);
                return true;
            }
        }

        public static bool Get_First()
        {
            if (container.Values["First" + GetVersion()] != null)
            {
                return (bool)container.Values["First" + GetVersion()];
            }
            else
            {
                Set_First(true);
                return true;
            }
        }

        public static void Set_First(bool value)
        {
            container.Values["First" + GetVersion()] = value;
        }

        #endregion

        #region 播放器
        public static double Get_Volume()
        {
            if (container.Values["Volume"] != null)
            {
                return Convert.ToDouble(container.Values["Volume"]);
            }
            else
            {
                container.Values["Volume"] = 1;
                return 1;
            }
        }

        public static void Set_Volume(double value)
        {
            container.Values["Volume"] = value;
        }


        public static double Get_Light()
        {
            if (container.Values["Light"] != null)
            {
                return Convert.ToDouble(container.Values["Light"]);
            }
            else
            {
                container.Values["Light"] = 0;
                return 0;
            }
        }

        public static void Set_Light(double value)
        {
            container.Values["Light"] = value;
        }


        public static int Get_BanPlayer()
        {
            if (container.Values["BanPlayer"] != null)
            {
                return Convert.ToInt32(container.Values["BanPlayer"]);
            }
            else
            {
                container.Values["BanPlayer"] = 2;
                return 2;
            }
        }

        public static void Set_BanPlayer(int value)
        {
            container.Values["BanPlayer"] = value;
        }

        public static int Get_PlayQualit()
        {
            if (container.Values["PlayQualit"] != null)
            {
                var p = (int)container.Values["PlayQualit"];

                return p;
            }
            else
            {
                container.Values["PlayQualit"] = 3;
                return 3;
            }
        }

        public static void Set_PlayQualit(int value)
        {
            container.Values["PlayQualit"] = value;
        }

        public static int Get_VideoType()
        {
            if (container.Values["VideoType"] != null)
            {
                //return (int)container.Values["VideoType"];
                return 0;
            }
            else
            {
                container.Values["VideoType"] = 0;
                return 0;
            }
        }

        public static void Set_VideoType(int value)
        {
            container.Values["VideoType"] = value;
        }

        public static void Set_ForceAudio(bool value)
        {
            container.Values["ForceAudio"] = value;
        }

        public static bool Get_ForceAudio()
        {
            if (container.Values["ForceAudio"] != null)
            {
                return (bool)container.Values["ForceAudio"];
            }
            else
            {
                Set_ForceAudio(true);
                return true;
            }
        }

        public static void Set_ForceVideo(bool value)
        {
            container.Values["ForceVideo"] = value;
        }

        public static bool Get_ForceVideo()
        {
            if (container.Values["ForceVideo"] != null)
            {
                return (bool)container.Values["ForceVideo"];
            }
            else
            {
                Set_ForceVideo(true);
                return true;
            }
        }


        public static int Get_Playback()
        {
            if (container.Values["Playback"] != null)
            {
                return (int)container.Values["Playback"];
            }
            else
            {
                container.Values["Playback"] = 0;
                return 0;
            }
        }

        public static void Set_Playback(int value)
        {
            container.Values["Playback"] = value;
        }


        public static bool Get_FFmpeg()
        {
            if (container.Values["FFmpeg"] != null)
            {
                return (bool)container.Values["FFmpeg"];
            }
            else
            {
                Set_FFmpeg(false);
                return false;
            }
        }

        public static void Set_FFmpeg(bool value)
        {
            container.Values["FFmpeg"] = value;
        }


        public static bool Get_UseH5()
        {
            if (container.Values["UseH5"] != null)
            {
                return (bool)container.Values["UseH5"];
            }
            else
            {
                Set_UseH5(false);
                return false;
            }
        }

        public static void Set_UseH5(bool value)
        {
            container.Values["UseH5"] = value;
        }



        public static int Get_ClearLiveComment()
        {
            if (container.Values["ClearLiveComment"] != null)
            {
                return (int)container.Values["ClearLiveComment"];
            }
            else
            {
                container.Values["ClearLiveComment"] = 1;
                return 1;
            }
        }

        public static void Set_ClearLiveComment(int value)
        {
            container.Values["ClearLiveComment"] = value;
        }


        public static bool Get_Use4GPlay()
        {
            if (container.Values["Use4GPlay"] != null)
            {
                return (bool)container.Values["Use4GPlay"];
            }
            else
            {
                Set_Use4GPlay(true);
                return true;
            }
        }

        public static void Set_Use4GPlay(bool value)
        {
            container.Values["Use4GPlay"] = value;
        }

        public static bool Get_BackPlay()
        {
            if (container.Values["BackPlay"] != null)
            {
                return (bool)container.Values["BackPlay"];
            }
            else
            {
                Set_BackPlay(true);
                return true;
            }
        }

        public static void Set_BackPlay(bool value)
        {
            container.Values["BackPlay"] = value;
        }

        public static bool Get_QZHP()
        {
            if (container.Values["QZHP"] != null)
            {
                return (bool)container.Values["QZHP"];
            }
            else
            {
                Set_QZHP(true);
                return true;
            }
        }

        public static void Set_QZHP(bool value)
        {
            container.Values["QZHP"] = value;
        }

        public static bool Get_AutoFull()
        {
            if (container.Values["AutoFull"] != null)
            {
                return (bool)container.Values["AutoFull"];
            }
            else
            {
                Set_AutoFull(false);
                return false;
            }
        }


        public static void Set_AutoFull(bool value)
        {
            container.Values["AutoFull"] = value;
        }


        public static bool Get_HideCursor()
        {
            if (container.Values["HideCursor"] != null)
            {
                return (bool)container.Values["HideCursor"];
            }
            else
            {
                Set_HideCursor(true);
                return true;
            }
        }


        public static void Set_HideCursor(bool value)
        {
            container.Values["HideCursor"] = value;
        }


        public static bool Get_NewFeed()
        {
            if (container.Values["NewFeed"] != null)
            {
                return (bool)container.Values["NewFeed"];
            }
            else
            {
                Set_NewFeed(false);
                return false;
            }
        }


        public static void Set_NewFeed(bool value)
        {
            container.Values["NewFeed"] = value;
        }


        public static bool Get_NewWindow()
        {
            if (container.Values["NewWindow"] != null)
            {
                return (bool)container.Values["NewWindow"];
            }
            else
            {
                Set_NewWindow(false);
                return false;
            }
        }


        public static void Set_NewWindow(bool value)
        {
            container.Values["NewWindow"] = value;
        }


        public static int Get_NewQuality()
        {
            if (container.Values["NewQuality"] != null)
            {
                return (int)container.Values["NewQuality"];
            }
            else
            {
                container.Values["NewQuality"] = 64;
                return 64;
            }
        }

        public static void Set_NewQuality(int value)
        {
            container.Values["NewQuality"] = value;
        }

        public static bool Get_UseDASH()
        {
            if (container.Values["UseDASH"] != null)
            {
                return (bool)container.Values["UseDASH"];
            }
            else
            {

                //系统版本大于1809启用
                var version = SystemHelper.GetSystemBuild();
                Set_UseDASH(version >= 17763);
                return version >= 17763;
            }
        }


        public static void Set_UseDASH(bool value)
        {
            container.Values["UseDASH"] = value;
        }


        public static bool Get_DASHUseHEVC()
        {
            if (container.Values["DASHUseHEVC"] != null)
            {
                return (bool)container.Values["DASHUseHEVC"];
            }
            else
            {
                Set_DASHUseHEVC(false);
                return false;
            }
        }


        public static void Set_DASHUseHEVC(bool value)
        {
            container.Values["DASHUseHEVC"] = value;
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

        public static int Get_DownQualit()
        {

            if (container.Values["DownQualit"] != null)
            {
                int p = (int)container.Values["DownQualit"];
                return p;
            }
            else
            {
                container.Values["DownQualit"] = 3;
                return 3;
            }
        }

        public static void Set_DownQualit(int value)
        {

            container.Values["DownQualit"] = value;
        }






        public static int Get_DownMode()
        {

            if (container.Values["DownMode"] != null)
            {
                return (int)container.Values["DownMode"];
            }
            else
            {
                container.Values["DownMode"] = 1;
                return 1;
            }
        }

        public static void Set_DownMode(int value)
        {

            container.Values["DownMode"] = value;
        }



        public static void Set_CustomDownPath(bool value)
        {

            container.Values["CustomDownPath"] = value;
        }

        public static bool Get_CustomDownPath()
        {

            if (container.Values["CustomDownPath"] != null)
            {
                return (bool)container.Values["CustomDownPath"];
            }
            else
            {
                Set_CustomDownPath(false);
                return false;
            }
        }

        public static void Set_DownPath(string value)
        {

            container.Values["DownPath"] = value;
        }

        public static string Get_DownPath()
        {

            if (container.Values["DownPath"] != null)
            {
                return (string)container.Values["DownPath"];
            }
            else
            {
                Set_DownPath("系统视频库");
                return "系统视频库";
            }
        }





        public static bool Get_Use4GDown()
        {

            if (container.Values["Use4GDown"] != null)
            {
                return (bool)container.Values["Use4GDown"];
            }
            else
            {
                Set_Use4GDown(false);
                return false;
            }
        }

        public static void Set_Use4GDown(bool value)
        {

            container.Values["Use4GDown"] = value;
        }



        public static bool Get_ToMp4()
        {

            if (container.Values["ToMp4"] != null)
            {
                return (bool)container.Values["ToMp4"];
            }
            else
            {
                Set_ToMp4(false);
                return false;
            }
        }

        public static void Set_ToMp4(bool value)
        {

            container.Values["ToMp4"] = value;
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

        static PackageId pack = (Package.Current).Id;
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

        public static bool Get_PriorityBiliPlus()
        {

            if (container.Values["PriorityBiliPlus"] != null)
            {
                return (bool)container.Values["PriorityBiliPlus"];
            }
            else
            {
                Set_PriorityBiliPlus(false);
                return false;
            }
        }

        public static void Set_PriorityBiliPlus(bool value)
        {

            container.Values["PriorityBiliPlus"] = value;
        }


        #region 字幕设置
        public static double Get_SubtitleSize()
        {

            if (container.Values["SubtitleSize"] != null)
            {
                return Convert.ToDouble(container.Values["SubtitleSize"]);
            }
            else
            {
                container.Values["SubtitleSize"] = 25;
                return 25;
            }
        }

        public static void Set_SubtitleSize(double value)
        {

            container.Values["SubtitleSize"] = value;
        }

        public static string Get_SubtitleFontFamily()
        {

            if (container.Values["SubtitleFontFamily"] != null)
            {
                return container.Values["SubtitleFontFamily"].ToString();
            }
            else
            {
                container.Values["SubtitleFontFamily"] = "Segoe UI";
                return "Segoe UI";

            }
        }

        public static void Set_SubtitleFontFamily(string value)
        {

            container.Values["SubtitleFontFamily"] = value;
        }

        public static double Get_SubtitleBgTran()
        {

            if (container.Values["SubtitleBgTran"] != null)
            {
                return Convert.ToDouble(container.Values["SubtitleBgTran"]);
            }
            else
            {
                container.Values["SubtitleBgTran"] = 0.5;
                return 0.5;

            }
        }

        public static void Set_SubtitleBgTran(double value)
        {

            container.Values["SubtitleBgTran"] = value;
        }


        public static string Get_SubtitleColor()
        {

            if (container.Values["SubtitleColor"] != null)
            {
                return container.Values["SubtitleColor"].ToString();
            }
            else
            {
                container.Values["SubtitleColor"] = "#ffffffff";
                return "#ffffffff";

            }
        }

        public static void Set_SubtitleColor(string value)
        {

            container.Values["SubtitleColor"] = value;
        }
        #endregion

    }
}
