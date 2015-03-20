using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DapperNet.Web.Common;
using DapperNet.Web.Models;

namespace DapperNet.Web.Controllers
{
    [PerformanceActionAttributeFilter(Message = "controller")]
    public class MemcachedController : Controller
    {
        //
        // GET: /Memcached/

        public ActionResult Index()
        {
            return View();
        }

        [PerformanceActionAttributeFilter(Message = "action")]
        public ActionResult Detail(int id)
        {

            string cacheKey = "DBUser_List";
            object objModel = Common.MemcachedHelper.GetMc(cacheKey);
            if (objModel == null)
            {
                try
                {
                    List<DBUser> list = CRMDBContext.CurrentThreadCRMDBContext.DBUser.ToList();
                    objModel = list;
                    if (objModel != null)
                    {
                        int cacheTime = 1;            //memcached自动超时时间,比如设置10.当前这个cache item 就会在10分钟后自动失效,如有新请求,则重新add  
                        Common.MemcachedHelper.SetMc(objModel, cacheKey, cacheTime);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return View((List<DBUser>)objModel);
        }

        [PerformanceActionAttributeFilter(Message = "action")]
        public ActionResult D(int id)
        {


           // DBUser orderModel = CRMDBContext.CurrentThreadCRMDBContext.DBUser.FirstOrDefault(obj => obj.Id == id);

            List<DBUser> list = CRMDBContext.CurrentThreadCRMDBContext.DBUser.ToList();
            return View(list);
        }




    }
}
