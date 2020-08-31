using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ShopBridgeAPI.BL
{
    public class ReadInventory_BL
    {

      

        public DataTable ReadInventory(JObject data)
        {

            Dictionary<string, object> IncludeParam = new Dictionary<string, object>();
            IncludeParam.Add("id", data["Readproduct"].ToString());
            DataTable dtData = Custom.StaticGeneral.GetDataTable("pReadInventory", null, null, IncludeParam);
            return dtData;
        }
    }
}