using System;
using System.Diagnostics;

namespace BiliBili3.Helper
{
    /// <summary>
    /// 日志记录
    /// </summary>
    public static class LogHelper
    {
        public static void WriteLog(Exception exception)
        {
            try
            {
                if (IsNetworkError(exception))
                {
                    return;
                }
                Debug.WriteLine(exception);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public static bool IsNetworkError(Exception ex)
        {
            if (ex.HResult == -2147012867 || ex.HResult == -2147012889)
            {
                MessageCenter.SendNetworkError(ex.Message);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
