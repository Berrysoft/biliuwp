using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BiliBili3
{
    public static class SqlHelper
    {
        public static SqlContext CreateContext()
        {
            SqlContext context = new SqlContext();
            context.Database.EnsureCreated();
            return context;
        }

        #region viewHistory
        public static List<HistoryClass> GetHistoryList(this SqlContext context, int mode)
        {
            IQueryable<HistoryClass> dbPerson = context.History.OrderByDescending(x => x.LookTime);
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

        public static bool AddCommicHistory(this SqlContext context, HistoryClass mo)
        {
            context.History.Add(mo);
            var count = context.SaveChanges();
            return count == 1;
        }

        public static HistoryClass GetComicHistory(this SqlContext context, string aid)
        {
            return context.History.FirstOrDefault(p => p.Aid == aid);
        }

        public static bool UpdateComicHistory(this SqlContext context, HistoryClass mo)
        {
            context.History.Update(mo);
            var count = context.SaveChanges();
            return count == 1;
        }

        public static void ClearHistory(this SqlContext context)
        {
            context.Database.ExecuteSqlCommand("DELETE FROM HistoryClass");
        }
        #endregion

        #region 观看进度
        public static ViewPostHelperClass GetViewPost(this SqlContext context, string id)
        {
            return context.ViewPost.FirstOrDefault(s => s.EpId == id);
        }

        public static bool AddViewPost(this SqlContext context, ViewPostHelperClass mo)
        {
            context.ViewPost.Add(mo);
            var count = context.SaveChanges();
            return count == 1;
        }

        public static bool GetPostIsViewPost(this SqlContext context, string id)
        {
            // 受影响行数。
            return context.ViewPost.Any(p => p.EpId == id);
        }

        public static bool UpdateViewPost(this SqlContext context, ViewPostHelperClass mo)
        {
            context.ViewPost.Update(mo);
            var count = context.SaveChanges();
            return count == 1;
        }
        #endregion

        public static DownloadGuidClass GetDownload(this SqlContext context, string guid)
        {
            return context.Download.First(x => x.Guid == guid);
        }

        public static bool InsertDownload(this SqlContext context, DownloadGuidClass m)
        {
            context.Download.Add(m);
            var count = context.SaveChanges();
            return count == 1;
        }
    }

    public class SqlContext : DbContext
    {
        public DbSet<HistoryClass> History { get; set; }
        public DbSet<ViewPostHelperClass> ViewPost { get; set; }
        public DbSet<DownloadGuidClass> Download { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source=RRMJData.db");
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
