using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ShopBridgeAPI.BL
{
    public class UpdateInventory_BL
    {

        public DataTable UpdateInventory(JObject data)
        {
         
            Dictionary<string, object> IncludeParam = new Dictionary<string, object>();
            IncludeParam.Add("id", data["id"].ToString());

            IncludeParam.Add("name", data["name"].ToString());
            IncludeParam.Add("description", data["description"].ToString());
            IncludeParam.Add("price", data["price"].ToString());

            DataTable dtData = Custom.StaticGeneral.GetDataTable("pUpdateInventory", null, null, IncludeParam);
            return dtData;
        }
    
    }
}