using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ShopBridgeAPI.BL
{
    public class DeleteInventory_BL
    {

        public DataTable DeleteInventory(JObject data)
        {
            Dictionary<string, object> IncludeParam = new Dictionary<string, object>();
            IncludeParam.Add("id", data["anyproduct"].ToString());

            DataTable dtData = Custom.StaticGeneral.GetDataTable("pDeleteInventory", null, null, IncludeParam);
            return dtData;
        }

    }
}