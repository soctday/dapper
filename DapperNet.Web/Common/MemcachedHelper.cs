using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Memcached.ClientLibrary;

namespace DapperNet.Web.Common
{
    public class MemcachedHelper
    {
        public static void SetMc(object obj, string cacheName)
        {
            string poolname = "default";
            cacheName = GetMD5(cacheName);
            //客户端实例  
            MemcachedClient mc = new Memcached.ClientLibrary.MemcachedClient();
            mc.PoolName = poolname; //如果实例化pool时没有poolname,该行可以不用。  
            mc.EnableCompression = true;
            mc.CompressionThreshold = 10240;
            if (obj != null)
                mc.Set(cacheName, obj, DateTime.Now.AddHours(3));
        }
        public static void SetMc(object obj, string cacheName, int minutes)
        {
            string poolname = "default";
            cacheName = GetMD5(cacheName);
            //客户端实例  
            MemcachedClient mc = new Memcached.ClientLibrary.MemcachedClient();
            mc.PoolName = poolname; //如果实例化pool时没有poolname,该行可以不用。  
            mc.CompressionThreshold = 10240;
            if (obj != null)
                mc.Set(cacheName, obj, DateTime.Now.AddMinutes(minutes));
        }
        public static object GetMc(string cacheName)
        {
            string poolname = "default";
            cacheName = GetMD5(cacheName);
            //客户端实例  
            MemcachedClient mc = new Memcached.ClientLibrary.MemcachedClient();
            mc.PoolName = poolname; //如果实例化pool时没有poolname,该行可以不用。  
            mc.EnableCompression = true;
            return (object)mc.Get(cacheName);

        }
        public static void UpdateMc(object obj, string cacheName)
        {
            string poolname = "default";
            cacheName = GetMD5(cacheName);
            //客户端实例  
            MemcachedClient mc = new Memcached.ClientLibrary.MemcachedClient();
            mc.PoolName = poolname; //如果实例化pool时没有poolname,该行可以不用。  
            mc.Delete(cacheName);
            if (obj != null)
            {
                mc.EnableCompression = true;
                mc.Set(cacheName, obj);
            }
        }
        public static void DelMc(string cacheName)
        {
            string poolname = "default";
            cacheName = GetMD5(cacheName);
            //客户端实例  
            MemcachedClient mc = new Memcached.ClientLibrary.MemcachedClient();
            mc.PoolName = poolname; //如果实例化pool时没有poolname,该行可以不用。  
            mc.Delete(cacheName);
        }
        /// <summary>  
        /// Cache MD5  
        /// </summary>  
        /// <param name="str"></param>  
        /// <returns></returns>  
        public static string GetMD5(string str)
        {
            int size = 16;
            if (size == 16)
            {//16位  
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                str = BitConverter.ToString(md5.ComputeHash(System.Text.UTF8Encoding.Default.GetBytes(str)), 4, 8);
                str = str.Replace("-", "");
            }
            else
            {  //32位  
                byte[] b = System.Text.Encoding.Default.GetBytes(str);
                b = new MD5CryptoServiceProvider().ComputeHash(b);
                for (int i = 0; i < b.Length; i++)
                {
                    str += b[i].ToString("x").PadLeft(2, '0');
                }
            }
            return str;
        }
    }
}