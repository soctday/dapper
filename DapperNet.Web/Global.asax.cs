using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Memcached.ClientLibrary;

namespace DapperNet.Web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void cachePool()
        {
            string poolname = "default";
            String[] serverlist = { "127.0.0.1:11211" };          //->这里如果需要部署分布式, 继续在后面追加ip即可！例如String[] serverlist = { "192.168.1.1:11211","192.168.1.2:11211" };  
            SockIOPool pool = SockIOPool.GetInstance(poolname);
            pool.SetServers(serverlist); //设置服务器列表  
            //各服务器之间负载均衡的设置  
            pool.SetWeights(new int[] { 1 });                 // ->这里部署分布式的时候是个很重要的参数,  相当于设置cache 值的主次！  
            //socket pool设置  
            pool.InitConnections = 5; //初始化时创建的连接数  
            pool.MinConnections = 5; //最小连接数  
            pool.MaxConnections = 2000; //最大连接数  

            //连接的最大空闲时间，下面设置为6个小时（单位ms），超过这个设置时间，连接会被释放掉  
            pool.MaxIdle = 1000 * 60 * 60 * 6;
            //通讯的超时时间，下面设置为3秒（单位ms），.NET版本没有实现  
            pool.SocketTimeout = 1000 * 3;
            //socket连接的超时时间，下面设置表示连接不超时，即一直保持连接状态  
            pool.SocketConnectTimeout = 0;
            pool.Nagle = false; //是否对TCP/IP通讯使用Nalgle算法，.NET版本没有实现  
            //维护线程的间隔激活时间，下面设置为60秒（单位s），设置为0表示不启用维护线程  
            pool.MaintenanceSleep = 60;
            //socket单次任务的最大时间，超过这个时间socket会被强行中断掉（当前任务失败）  
            pool.MaxBusy = 1000 * 10;

            pool.Failover = true;

            pool.Initialize();
        }
        protected void Application_Start()
        {

            cachePool();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}