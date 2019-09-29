using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Windows.Storage;

namespace BiliBili3
{
    public static class SqlHelper
    {
        private static readonly SqlContext context = new SqlContext();

        #region viewHistory
        public static List<HistoryClass> GetHistoryList(int mode)
        {
            IQueryable<HistoryClass> dbPerson = context.HistoryClass.OrderByDescending(x => x.LookTime);
            switch (mode)
            {
                case 0:
                    dbPerson = dbPerson.Take(30);
                    break;
                case 1:
                    dbPerson = dbPerson.Take(50);
                    break;
            }
            return dbPerson.ToList();
        }

        public static bool AddCommicHistory(HistoryClass mo)
        {
            context.HistoryClass.Add(mo);
            var count = context.SaveChanges();
            return count == 1;
        }

        public static HistoryClass GetComicHistory(string aid)
        {
            return context.HistoryClass.FirstOrDefault(p => p.Aid == aid);
        }

        public static bool UpdateComicHistory(HistoryClass mo)
        {
            context.HistoryClass.Update(mo);
            var count = context.SaveChanges();
            return count == 1;
        }

        public static void ClearHistory()
        {
            context.Database.ExecuteSqlCommand("DELETE FROM HistoryClass");
        }
        #endregion

        #region 观看进度
        public static ViewPostHelperClass GetViewPost(string id)
        {
            return context.ViewPostHelperClass.FirstOrDefault(s => s.EpId == id);
        }

        public static bool AddViewPost(ViewPostHelperClass mo)
        {
            context.ViewPostHelperClass.Add(mo);
            var count = context.SaveChanges();
            return count == 1;
        }

        public static bool GetPostIsViewPost(string id)
        {
            // 受影响行数。
            return context.ViewPostHelperClass.Any(p => p.EpId == id);
        }

        public static bool UpdateViewPost(ViewPostHelperClass mo)
        {
            context.ViewPostHelperClass.Update(mo);
            var count = context.SaveChanges();
            return count == 1;
        }
        #endregion

        public static DownloadGuidClass GetDownload(string guid)
        {
            return context.DownloadGuidClass.First(x => x.Guid == guid);
        }

        public static bool InsertDownload(DownloadGuidClass m)
        {
            context.DownloadGuidClass.Add(m);
            var count = context.SaveChanges();
            return count == 1;
        }
    }

    public class SqlContext : DbContext
    {
        public DbSet<HistoryClass> HistoryClass { get; set; }
        public DbSet<ViewPostHelperClass> ViewPostHelperClass { get; set; }
        public DbSet<DownloadGuidClass> DownloadGuidClass { get; set; }

        private static readonly string DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "RRMJData.db");

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Filename={DbPath}");
        }
    }

    public class HistoryClass
    {
        [Key]
        public string Aid { get; set; }
        public string Title { get; set; }
        public string Up { get; set; }
        public string Image { get; set; }
        public DateTime LookTime { get; set; }
    }

    public class ViewPostHelperClass
    {
        [Key]
        public string EpId { get; set; }
        public int Post { get; set; }
        public DateTime ViewTime { get; set; }
    }

    public class DownloadGuidClass
    {
        [Key]
        public string Guid { get; set; }
        public string Cid { get; set; }
        public string Aid { get; set; }
        public int Index { get; set; }
        public string Eptitle { get; set; }
        public string Title { get; set; }
        public string Mode { get; set; }
    }
}
