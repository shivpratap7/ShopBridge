using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace ShopBridgeAPI.Custom
{
    public class SiteConfig
    {
        public static string StyleImagePath = "";
      
        public static string SiteRootURL
        {
            get
            {
                return ConfigurationManager.AppSettings["SiteRootURL"].ToString();
            }
        }

        public static string EncryptKey
        {
            get
            {
                return ConfigurationManager.AppSettings["EncryptKey"].ToString();
            }
        }

        public static string AuthServiceHeaderName
        {
            get
            {
                return ConfigurationManager.AppSettings["AuthServiceHeaderName"].ToString();
            }
        }

        public static string LoginKeyName
        {
            get
            {
                return ConfigurationManager.AppSettings["LoginKeyName"].ToString();
            }
        }

        public static string WebSessionID
        {
            get
            {
                return ConfigurationManager.AppSettings["WebSessionID"].ToString();
            }
        }

        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            }
        }

        public static string ConnectionBaseString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionBase"].ConnectionString;
            }
        }

        public static string endPointAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["endPointAddress"].ToString();
            }
        }


        public static string getServerPath
        {
            get
            {
                return System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            }
        }
        public static string SMTPFromEmail
        {
            get
            {
                return ConfigurationManager.AppSettings["SMTPFromEmail"];
            }
        }

        public static string SMTPFromEmailName
        {
            get
            {
                return ConfigurationManager.AppSettings["SMTPFromEmailName"];
            }
        }

        public static string FileTemplate
        {
            get
            {
                return ConfigurationManager.AppSettings["FileTemplate"];
            }
        }

    }
}