using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite;
using Windows.Storage;

namespace BiliBili3
{
    public static class SqlHelper
    {
        /// <summary>
        /// 数据库文件所在路径
        /// </summary>
        public static readonly string DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "RRMJData.db");

        public static SQLiteConnection GetDbConnection()
        {
            // 连接数据库，如果数据库文件不存在则创建一个空数据库。
            var conn = new SQLiteConnection(DbPath);

            conn.CreateTable<HistoryClass>();
            conn.CreateTable<ViewPostHelperClass>();
            conn.CreateTable<DownloadGuidClass>();
            return conn;
        }

        #region viewHistory
        public static List<HistoryClass> GetHistoryList(int mode)
        {
            List<HistoryClass> my = new List<HistoryClass>();
            using (var conn = GetDbConnection())
            {
                TableQuery<HistoryClass> dbPerson = null;
                if (mode == 0)
                {
                    dbPerson = conn.Table<HistoryClass>().OrderByDescending(x => x.LookTime).Take(30);
                }
                if (mode == 1)
                {
                    dbPerson = conn.Table<HistoryClass>().OrderByDescending(x => x.LookTime).Take(50);
                }
                if (mode == 2)
                {
                    dbPerson = conn.Table<HistoryClass>().OrderByDescending(x => x.LookTime);
                }
                foreach (HistoryClass item in dbPerson)
                {
                    my.Add(item);
                }
                return my;
            }
        }
        public static bool AddCommicHistory(HistoryClass mo)
        {
            using (var conn = GetDbConnection())
            {
                // 受影响行数。
                var count = conn.Insert(mo);
                return count == 1;
            }
        }
        public static bool GetComicIsOnHistory(string aid)
        {
            using (var conn = GetDbConnection())
            {
                // 受影响行数。
                var m = from p in conn.Table<HistoryClass>()
                        where p.Aid == aid
                        select p;
                return m.Any();
            }
        }
        public static bool UpdateComicHistory(HistoryClass mo)
        {
            using (var conn = GetDbConnection())
            {
                var count = conn.Execute("UPDATE HistoryClass SET lookTime=?,title=?,up=?,image=? WHERE _aid=?;", DateTime.Now.ToLocalTime(), mo.Title, mo.Up, mo.Image, mo.Aid);
                return count == 1;
            }
        }
        public static void ClearHistory()
        {
            using (var conn = GetDbConnection())
            {
                conn.Execute("DELETE FROM HistoryClass");
            }
        }
        #endregion

        #region 观看进度
        public static ViewPostHelperClass GettViewPost(string id)
        {
            using (var conn = GetDbConnection())
            {
                //var dbPerson = conn.Table<CommicHistoryClass>();
                TableQuery<ViewPostHelperClass> t = conn.Table<ViewPostHelperClass>();
                var q = from s in t.AsParallel<ViewPostHelperClass>()
                        where s.EpId == id
                        select s;
                // 绑定
                return q.FirstOrDefault();
            }
        }
        public static bool AddViewPost(ViewPostHelperClass mo)
        {
            using (var conn = GetDbConnection())
            {
                // 受影响行数。
                var count = conn.Insert(mo);
                return count == 1;
            }
        }
        public static bool GetPostIsViewPost(string Id)
        {
            using (var conn = GetDbConnection())
            {
                // 受影响行数。
                var m = from p in conn.Table<ViewPostHelperClass>()
                        where p.EpId == Id
                        select p;
                return m.Any();
            }
        }
        public static bool UpdateViewPost(ViewPostHelperClass mo)
        {
            using (var conn = GetDbConnection())
            {
                var count = conn.Execute("UPDATE ViewPostHelperClass SET Post=?,viewTime=? WHERE epId=?;", mo.Post, DateTime.Now.ToLocalTime(), mo.EpId);
                return count == 1;
            }
        }
        #endregion

        public static DownloadGuidClass GetDownload(string guid)
        {
            using (var conn = GetDbConnection())
            {
                return conn.Table<DownloadGuidClass>().First(x => x.Guid == guid);
            }
        }
        public static bool InsertDownload(DownloadGuidClass m)
        {
            using (var conn = GetDbConnection())
            {
                var count = conn.Insert(m);
                return count == 1;
            }
        }
    }

    public class HistoryClass
    {
        [PrimaryKey]
        public string Aid { get; set; }
        public string Title { get; set; }
        public string Up { get; set; }
        public string Image { get; set; }
        public DateTime LookTime { get; set; }
    }

    public class ViewPostHelperClass
    {
        [PrimaryKey]
        public string EpId { get; set; }
        public int Post { get; set; }
        public DateTime ViewTime { get; set; }
    }

    public class DownloadGuidClass
    {
        [PrimaryKey]
        public string Guid { get; set; }
        public string Cid { get; set; }
        public string Aid { get; set; }
        public int Index { get; set; }
        public string Eptitle { get; set; }
        public string Title { get; set; }
        public string Mode { get; set; }
    }
}
