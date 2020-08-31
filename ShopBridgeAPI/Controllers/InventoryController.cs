using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using System.Web;
//using ShopBridgeAPI.BL;
using System.Data;
using ShopBridgeAPI.Custom;
using ShopBridgeAPI.Models;
using ShopBridgeAPI.BL;

namespace ShopBridgeAPI.Controllers
{
    public class InventoryController : ApiController
    {


        #region "Get Inventory"

        [ActionName("GetInventory")]
        [HttpGet]
        public JObject GetInventory([FromBody]JObject data)

        {
            GetInventory_BL bl = new GetInventory_BL();
            try
            {
                Dictionary<string, object> DeviceParam = new Dictionary<string, object>();
                DataTable dt = StaticGeneral.GetDataTable("pGetInventory", DeviceParam);

                if (dt != null && dt.Rows.Count > 0)
                {
                    JObject FinalRtn = new JObject();
                    FinalRtn["Data"] = JToken.FromObject(dt);
                    return FinalRtn;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                throw;
            }


        }

        #endregion
        #region "Create Inventory"
        [AllowAnonymous]
        [ActionName("CreateInventory")]
        [HttpPost]

        public JArray CreateInventory([FromBody]JObject data)
        {
            CreateInventory_BL bl = new CreateInventory_BL();
            try
            {
                if (data != null)
                {

                    return bl.CreateInventory(data).ToJArray();


                    // return bl.CrudRegister(data).ToString();
                }



            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());

            }

            return null;
        }
        #endregion
        #region "Delete Inventory"
        [AllowAnonymous]
        [ActionName("DeleteInventory")]
        [HttpPost]

        public JArray DeleteInventory([FromBody]JObject data)
        {
            DeleteInventory_BL bl = new DeleteInventory_BL();
            try
            {
                if (data != null)
                {

                    return bl.DeleteInventory(data).ToJArray();


                    // return bl.CrudRegister(data).ToString();
                }



            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());

            }
            return null;

        }
        #endregion
        #region "Update Inventory"
        [AllowAnonymous]
        [ActionName("UpdateInventory")]
        [HttpPost]

        public JArray UpdateInventory([FromBody]JObject data)
        {
            UpdateInventory_BL bl = new UpdateInventory_BL();
            try
            {
                if (data != null)
                {

                    return bl.UpdateInventory(data).ToJArray();


                    // return bl.CrudRegister(data).ToString();
                }



            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());

            }
            return null;

        }
        #endregion

        #region "Read Inventory"
        [AllowAnonymous]
        [ActionName("ReadInventory")]
        [HttpPost]
        public JArray ReadInventory([FromBody]JObject data)

        {
            ReadInventory_BL bl = new ReadInventory_BL();

           
                if (data != null)
                {

                    return bl.ReadInventory(data).ToJArray();


                    // return bl.CrudRegister(data).ToString();
                }

            return null;

            }
        #endregion


    }
}
