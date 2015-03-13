using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using DapperNet.Web.Models;

namespace DapperNet.Web.Common
{
    public partial class CRMDBContext : DbContext
    {
        #region CurrentThreadCRMDBContext
        [ThreadStatic]
        private static CRMDBContext _instance = null;
        public static CRMDBContext CurrentThreadCRMDBContext
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CRMDBContext();
                }
                return _instance;
                //return new CRMDBContext();
            }
        }

        public static void DisposeCurrentThreadCRMDBContext()
        {
            if (_instance != null)
            {
                _instance.Dispose();
                _instance = null;
            }
        }
        #endregion

        //数据库访问连接字符串
        private readonly static string CONNECTION_STRING = "name=MassVigSystem";

        public CRMDBContext() : base(CONNECTION_STRING) { }
        public CRMDBContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

        public DbSet<DBUser> DBUser { get; set; }

        /// <summary>
        /// 实体创建,需加入映射关系
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();//防止黑幕交易 要不然每次都要访问 EdmMetadata这个表
        }


    }
}
