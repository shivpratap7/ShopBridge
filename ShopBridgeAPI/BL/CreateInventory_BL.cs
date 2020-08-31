using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ShopBridgeAPI.BL
{
    public class CreateInventory_BL
    {
        public DataTable CreateInventory(JObject data)
        {
            Dictionary<string, object> IncludeParam = new Dictionary<string, object>();
            IncludeParam.Add("name", data["name"].ToString());
            IncludeParam.Add("description", data["description"].ToString());
            IncludeParam.Add("price", data["price"].ToString());

            DataTable dtData = Custom.StaticGeneral.GetDataTable("pCreateInventoryInsert", null, null, IncludeParam);
            return dtData;
        }
    }
}