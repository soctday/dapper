using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using DapperNet.Web.Common;
using DapperNet.Web.Models;

namespace DapperNet.Web.Controllers
{
    [PerformanceActionAttributeFilter(Message = "controller")]
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        [PerformanceActionAttributeFilter(Message = "action")]
        public ActionResult Index()
        {
            IDbConnection conn = Program.GetOpenConnection();
            for (int i = 0; i < 10000; i++)
            {
                DBUser user = new DBUser();
                user.Name = "张三" + i;
                string sql = "INSERT INTO DBUser(Name)VALUES(@name)";
                conn.Execute(sql, user);
            }

            return View();
        }
        [PerformanceActionAttributeFilter(Message = "action")]
        public ActionResult Ef()
        {
            Test user = new Test();
            for (int i = 0; i < 10000; i++)
            {
                user.Name = "张三" + i;
                CRMDBContext.CurrentThreadCRMDBContext.Test.Add(user);

                CRMDBContext.CurrentThreadCRMDBContext.SaveChanges();
            }
            return View();
        }

    }
}
