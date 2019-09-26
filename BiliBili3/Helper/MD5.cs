using System.Text;
using SystemMd5 = System.Security.Cryptography.MD5;

namespace BiliBili3.Class
{
    public static class MD5
    {
        public static string GetMd5String(string source)
        {
            using (var md = SystemMd5.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(source);
                byte[] buffer2 = md.ComputeHash(bytes);
                StringBuilder builder = new StringBuilder();
                foreach (byte num in buffer2)
                {
                    builder.Append(num.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
