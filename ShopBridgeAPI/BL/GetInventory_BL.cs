using Newtonsoft.Json.Linq;
using ShopBridgeAPI.Custom;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ShopBridgeAPI.BL
{
    public class GetInventory_BL
    {


        public DataTable RtnObject { get; private set; }

        public DataTable GetInventory(JObject Data)

        {

            DataTable dtData = StaticGeneral.GetDataTable("pGetInventory", Data, null, null);


            return RtnObject;
        }
    }
}