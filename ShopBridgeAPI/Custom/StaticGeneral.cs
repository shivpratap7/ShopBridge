using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web.Configuration;
using TuesPechkin;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.Data.OleDb;
using System.Data;
using System.IO;

namespace ShopBridgeAPI.Custom
{

    public class StaticGeneral
    {

        static object LockOut = 1;
        static IConverter converter;

        //static StaticGeneral()
        //{
        //    converter = new ThreadSafeConverter(new RemotingToolset<PdfToolset>(new WinAnyCPUEmbeddedDeployment(new TempFolderDeployment())));
        //}

        public static string GetDBConnectionString()
        {
            return GetDBConnectionString("Connection");
        }

        public static DataSet GetDataSet(SqlCommand scmd)
        {
            if (scmd.Connection == null)
            {
                scmd.Connection = new SqlConnection(SiteConfig.ConnectionString);
            }
            SqlDataAdapter sda = new SqlDataAdapter(scmd);
            DataSet dt = new DataSet();
            sda.Fill(dt);
            return dt;
        }

        public static DataTable GetDataTable(SqlCommand scmd, int TimeOut = -1)
        {
            if (scmd.Connection == null)
            {
                scmd.Connection = new SqlConnection(SiteConfig.ConnectionString);
            }
            if (TimeOut != -1)
            {
                scmd.CommandTimeout = TimeOut;
            }
            SqlDataAdapter sda = new SqlDataAdapter(scmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public static DataTable GetDataTable(SqlCommand scmd, string strConnection, int TimeOut = -1)
        {
            if (scmd.Connection == null)
            {
                scmd.Connection = new SqlConnection(strConnection);
            }
            if (TimeOut != -1)
            {
                scmd.CommandTimeout = TimeOut;
            }
            SqlDataAdapter sda = new SqlDataAdapter(scmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public static DataTable GetDataTable(string DataDBName, string SPorQuery, Dictionary<string, object> Parameters, bool isSP = true, int TimeOut = -1)
        {
            SqlCommand sCmd = new SqlCommand(SPorQuery);
            if (isSP)
            {
                sCmd.CommandType = CommandType.StoredProcedure;
            }

            if (TimeOut != -1)
            {
                sCmd.CommandTimeout = TimeOut;
            }
            if (Parameters != null)
            {
                foreach (KeyValuePair<string, object> Para in Parameters)
                {
                    sCmd.Parameters.AddWithValue(Para.Key, (Para.Value == null ? DBNull.Value : Para.Value));
                }
            }
            return GetDataTable(sCmd, GetDBConnectionString("ConnectionBase", DataDBName));
        }

        public static DataTable GetDataTable(string SPorQuery, Dictionary<string, object> Parameters, bool isSP = true, int TimeOut = -1)
        {
            SqlCommand sCmd = new SqlCommand(SPorQuery);
            if (isSP)
            {
                sCmd.CommandType = CommandType.StoredProcedure;
            }

            if (TimeOut != -1)
            {
                sCmd.CommandTimeout = TimeOut;
            }
            if (Parameters != null)
            {
                foreach (KeyValuePair<string, object> Para in Parameters)
                {
                    sCmd.Parameters.AddWithValue(Para.Key, (Para.Value == null ? DBNull.Value : Para.Value));
                }
            }
            return GetDataTable(sCmd);
        }

        public static DataSet GetDataSet(string SPorQuery, Dictionary<string, object> Parameters, bool isSP = true, int TimeOut = -1)
        {
            SqlCommand sCmd = new SqlCommand(SPorQuery);
            if (isSP)
            {
                sCmd.CommandType = CommandType.StoredProcedure;
            }

            if (TimeOut != -1)
            {
                sCmd.CommandTimeout = TimeOut;
            }
            foreach (KeyValuePair<string, object> Para in Parameters)
            {
                sCmd.Parameters.AddWithValue(Para.Key, (Para.Value == null ? DBNull.Value : Para.Value));
            }
            return GetDataSet(sCmd);
        }

        public static DataTable GetDataTable(string strSQL)
        {
            SqlConnection sCon = new SqlConnection(GetDBConnectionString());
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, sCon);
            DataTable dtRtn = new DataTable();
            sda.Fill(dtRtn);
            return dtRtn;
        }

        public static DataTable GetDataTable(string strSQL, DataSet ds, bool IsTableTypSP, List<string> ExcludeParams = null, Dictionary<string, object> IncludeParams = null)
        {
            foreach (DataTable dt in ds.Tables)
            {
                IncludeParams.Add(dt.TableName, dt);
            }

            List<SqlParameter> sp = GetParameterList(null, ExcludeParams, IncludeParams);

            SqlConnection sCon = new SqlConnection(GetDBConnectionString());
            SqlCommand cmd = new SqlCommand(strSQL, sCon);

            cmd.Parameters.AddRange(sp.ToArray());
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dtRtn = new DataTable();
            sda.Fill(dtRtn);
            return dtRtn;
        }

        public static DataSet GetDataSet(string strSQL, DataSet ds, List<string> ExcludeParams = null, Dictionary<string, object> IncludeParams = null)
        {
            List<SqlParameter> sp = GetParameterList(null, ExcludeParams, IncludeParams);

            SqlConnection sCon = new SqlConnection(GetDBConnectionString());
            SqlCommand cmd = new SqlCommand(strSQL, sCon);

            foreach (DataTable dt in ds.Tables)
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    cmd.Parameters.AddWithValue(dt.TableName, dt);
                }
            }

            cmd.Parameters.AddRange(sp.ToArray());
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet dsRtn = new DataSet();
            sda.Fill(dsRtn);
            return dsRtn;
        }

        public static DataTable GetDataTable(string strSQL, DataTable dt, bool PassTablePara, Dictionary<string, object> IncludeParams = null)
        {
            List<SqlParameter> sp = GetParameterList(null, null, IncludeParams);

            SqlConnection sCon = new SqlConnection(GetDBConnectionString());
            SqlCommand cmd = new SqlCommand(strSQL, sCon);
            cmd.Parameters.AddWithValue(dt.TableName, dt);
            cmd.Parameters.AddRange(sp.ToArray());
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dsRtn = new DataTable();
            sda.Fill(dsRtn);
            return dsRtn;
        }

        public static DataTable GetDataTable(string strSQL, JObject data, List<string> ExcludeParams = null, Dictionary<string, object> IncludeParams = null)
        {
            List<SqlParameter> sp = GetParameterList(data, ExcludeParams, IncludeParams);

            SqlConnection sCon = new SqlConnection(GetDBConnectionString());
            SqlCommand cmd = new SqlCommand(strSQL, sCon);

            //foreach(SqlParameter sq in sp)
            //{
            //    cmd.Parameters.Add(sq);
            //}
            cmd.Parameters.AddRange(sp.ToArray());
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dtRtn = new DataTable();
            sda.Fill(dtRtn);
            return dtRtn;
        }

        public static DataSet GetDataSet(string strSQL)
        {
            SqlConnection sCon = new SqlConnection(GetDBConnectionString());
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, sCon);
            DataSet dsRtn = new DataSet();
            sda.Fill(dsRtn);
            return dsRtn;
        }

        public static DataSet GetDataSet(string strSQL, string strSqlConnName)
        {
            SqlConnection sCon = new SqlConnection(GetDBConnectionString(strSqlConnName));
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, sCon);
            DataSet dsRtn = new DataSet();
            sda.Fill(dsRtn);
            return dsRtn;
        }

        public static int ExecuteNonQuery(string strSQL)
        {
            SqlConnection sCon = new SqlConnection(GetDBConnectionString());
            sCon.Open();
            SqlCommand sCmd = new SqlCommand(strSQL, sCon);

            int iRtn = sCmd.ExecuteNonQuery();
            sCon.Close();
            return iRtn;
        }
        public static int ExecuteSQLCommand(SqlCommand sqlCmd)
        {
            SqlConnection sCon = new SqlConnection(GetDBConnectionString());
            sCon.Open();
            sqlCmd.Connection = sCon;
            int iRtn = sqlCmd.ExecuteNonQuery();
            sCon.Close();
            return iRtn;
        }

        public static List<SqlParameter> GetParameterList(JObject data, List<string> ExcludeParams = null, Dictionary<string, object> IncludeParams = null)
        {
            List<SqlParameter> Rtn = new List<SqlParameter>();
            if (ExcludeParams == null)
            {
                ExcludeParams = new List<string>();
            }

            if (IncludeParams != null)
            {
                foreach (var obj in IncludeParams)
                {
                    if (obj.Value.ToString() == "null" || obj.Value.ToString() == "undefined" || obj.Value.ToString() == "''" || obj.Value.ToString() == "" || obj.Value.ToString() == "{}")
                    {
                        SqlParameter dbPara1 = new SqlParameter(obj.Key.ToString().Trim(), DBNull.Value);
                        Rtn.Add(dbPara1);
                    }
                    else
                    {
                        SqlParameter dbPara1 = new SqlParameter(obj.Key.ToString().Trim(), obj.Value.ToString());

                        Rtn.Add(dbPara1);
                    }
                }
            }

            if (data != null)
            {
                foreach (var obj in data)
                {
                    if (ExcludeParams.Where(x => x.ToLower() == obj.Key.ToLower()).Count() > 0)
                    {
                        continue;
                    }

                    if (obj.Value.ToString() == "null" || obj.Value.ToString() == "undefined" || obj.Value.ToString() == "''" || obj.Value.ToString() == "" || obj.Value.ToString() == "{}")
                    {
                        SqlParameter dbPara1 = new SqlParameter(obj.Key.ToString().Trim(), DBNull.Value);
                        Rtn.Add(dbPara1);
                    }
                    else
                    {
                        SqlParameter dbPara1 = new SqlParameter(obj.Key.ToString().Trim(), obj.Value.ToString());
                        Rtn.Add(dbPara1);
                    }
                }
            }
            return Rtn;
        }


        static string GetDBConnectionString(string strWhichConnection)
        {
            try
            {
                return WebConfigurationManager.ConnectionStrings[strWhichConnection].ConnectionString.Trim();
            }
            catch
            {
                throw new Exception("Could not find [" + strWhichConnection + "] in <connectionStrings> section of your web.config file." + Environment.NewLine + "Please check your web.config file.");
            }
        }

        static string GetDBConnectionString(string strWhichConnection, string strDataDBName)
        {
            try
            {
                string strConstring = WebConfigurationManager.ConnectionStrings[strWhichConnection].ConnectionString.Trim();
                strConstring = strConstring.Replace("AccData_Base", strDataDBName);
                return strConstring;
            }
            catch
            {
                throw new Exception("Could not find [" + strWhichConnection + "] in <connectionStrings> section of your web.config file." + Environment.NewLine + "Please check your web.config file.");
            }
        }



        public static string GetAppSettingValue(string strKey)
        {

            try
            {
                return WebConfigurationManager.AppSettings[strKey].Trim();
            }
            catch
            {
                throw new Exception("[" + strKey + "] not defined in AppSettings of web.config file." + Environment.NewLine + "Please check web.config.");
            }
            throw new Exception("No App.Settings defined in web.config." + Environment.NewLine + "Please check web.config.");
        }
        public static bool SQLCompatibleText(string strText)
        {
            bool InvalidText = false;
            InvalidText = !(strText.IndexOf("'") >= 0);
            if (InvalidText) InvalidText = !(strText.IndexOf("--") >= 0);
            if (InvalidText) InvalidText = !(strText.IndexOf("/*") >= 0);
            return InvalidText;
        }
        public static bool SQLCompatibleText(string strText, bool DisAllowSpace)
        {
            if (DisAllowSpace)
            {
                //return !(strText.IndexOf(" ") >= 0);
                bool rtn = !(strText.IndexOf(" ") >= 0);
                if (!rtn) return rtn;
            }
            return SQLCompatibleText(strText);
        }
        public static bool IsSQLInjectionSafe(string strInString)
        {
            if (strInString.Trim().Length == 0 ||
               strInString.Trim().ToLower().IndexOf(" ") >= 0 ||
               strInString.Trim().ToLower().IndexOf("'") >= 0 ||
               strInString.Trim().ToLower().IndexOf("script") >= 0 ||
               strInString.Trim().ToLower().IndexOf("<") > 0)
            {
                return false;
            }
            return true;
        }

        public static bool DisAllowKeys(string strCheckIn, string strDisallow)
        {
            char[] chrSplit = strDisallow.ToCharArray();
            foreach (char item in chrSplit)
            {
                if (strCheckIn.ToLower().IndexOf(item.ToString().ToLower()) >= 0)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsNumeric(string strData)
        {
            decimal dcl = 0;
            try
            {
                dcl = Convert.ToDecimal(strData);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static decimal ConvertToNumeric(string strData)
        {
            decimal dcl = 0;
            try
            {
                dcl = Convert.ToDecimal(strData);
                return dcl;
            }
            catch
            {
                return 0;
            }
        }
        public static decimal ConvertToNumeric(string strData, string strSpecificStringToRemove)
        {
            decimal dcl = 0;
            try
            {
                dcl = Convert.ToDecimal(strData.Replace(strSpecificStringToRemove, ""));
                return dcl;
            }
            catch
            {
                return 0;
            }
        }
        public static decimal ConvertToNumeric(string strData, bool RemoveAllStrings)
        {
            decimal dcl = 0;
            string strNumericValue = "";
            try
            {
                for (int i = 0; i <= strData.Length - 1; i++)
                {
                    if (IsNumeric(strData.Substring(i, 1)))
                    {
                        strNumericValue += strData.Substring(i, 1);
                    }
                }
                dcl = ConvertToNumeric(strNumericValue);
                return dcl;
            }
            catch
            {
                return 0;
            }
        }
        public static int GetCharCountInString(string strText, string strCharToCount)
        {
            int CharCount = 0;
            for (int i = 0; i <= strText.Length - 1; i++)
            {
                if (strText.Substring(i, 1).ToLower().Trim().Equals(strCharToCount.ToLower().Trim()))
                {
                    CharCount = CharCount + 1;
                }
            }
            return CharCount;
        }
        public static string GetWebSiteName()
        {
            return ConfigurationManager.AppSettings["SiteName"].Trim();
        }

     /*   public static string EncryptedText(string strToEncrypt)
        {
            Encrypto enc = new Encrypto();
            return enc.GetEncryptedText("shree", strToEncrypt);
        }

        public static string DecryptedText(string strToDecrypt)
        {
            Crypto enc = new Encrypto();
            return enc.GetDecryptedText("shree", strToDecrypt);
        }*/

        public static object GetParameterValue(JObject objToFind, string keyName)
        {

            if (objToFind[keyName] == null)
            {
                return DBNull.Value;
            }

            JToken obj = objToFind[keyName] as JToken;

            if (obj.ToString() == "" || obj.ToString() == "{}")
            {
                return DBNull.Value;
            }
            else
            {

                return obj.ToObject<object>();
            }
        }

        public static string CheckNull(object obj, bool EncloseQuotes, bool ReturnNull)
        {
            if (obj == null || obj == DBNull.Value || string.IsNullOrEmpty(Convert.ToString(obj)))
            {
                return ReturnNull ? "null" : (EncloseQuotes ? "''" : "");
            }

            if (EncloseQuotes)
            {
                return Convert.ToString("'" + obj.ToString() + "'");
            }
            return obj.ToString();
        }

        public static void LogRequests(string DeviceCode, string RequestBody, string RequestHeaders, string Method, string RequestURI)
        {
            Dictionary<string, object> IncludeParam = new Dictionary<string, object>();
            IncludeParam.Add("@DeviceCode", DeviceCode.ToString());
            //IncludeParam.Add("@SectionName", SectionName.ToString());
            IncludeParam.Add("@RequestBody", RequestBody.ToString());
            IncludeParam.Add("@RequestHeaders", RequestHeaders.ToString());
            IncludeParam.Add("@Method", Method.ToString());
            IncludeParam.Add("@RequestURI", RequestURI.ToString());
            DataTable dt = StaticGeneral.GetDataTable("pRequestLog_Aasan", IncludeParam);
        }

        public static void LogException(string SessionCode, string exMessage, string SectionName, string RequestBody, string RequestHeaders)
        {
            Dictionary<string, object> IncludeParam = new Dictionary<string, object>();
            IncludeParam.Add("@SessionCode", SessionCode.ToString());
            IncludeParam.Add("@SectionName", SectionName.ToString());
            IncludeParam.Add("@ErrorMsg", exMessage.ToString());
            IncludeParam.Add("@RequestBody", RequestBody.ToString());
            IncludeParam.Add("@RequestHeaders", RequestHeaders.ToString());
            DataTable dt = StaticGeneral.GetDataTable("ErrorLog_Insert", IncludeParam);
        }

        #region "Excel Mechanism"
        public static string getExcelConnectionString(string strFilePath, string strExtension)
        {
            strExtension = strExtension.ToLower();
            if (strExtension == ".txt" || strExtension == ".csv")
            {
                if (Environment.Is64BitOperatingSystem) //32bt mein yeh nahi hai
                {
                    return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strFilePath.Trim() + ";Extended Properties=\"text;HDR=Yes;FMT=Delimited\"";
                }
                return "Provider=Microsoft.Jet.OLEDB.12.0;Data Source=" + strFilePath.Trim() + ";Extended Properties=\"text;HDR=Yes;FMT=Delimited\"";
            }
            else if (strExtension == ".xlsx")
            {
                return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strFilePath.Trim() + ";Extended Properties=\"Excel 12.0 Xml;HDR=Yes\"";
            }
            else
            {
                if (Environment.Is64BitOperatingSystem) //32bt mein yeh nahi hai
                {
                    return "Provider=Microsoft.ACE.OLEDB.12.0;" +
                        "Data Source=" + strFilePath + ";" +
                        "Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                }
                return "Provider=Microsoft.Jet.OLEDB.4.0;" +
                        "Data Source=" + strFilePath + ";" +
                        "Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
            }
        }
        public static OleDbConnection getExcelConnection(string strExcelConnectionString)
        {
            return new OleDbConnection(strExcelConnectionString);
        }
        public static DataTable getExcelDataTable(string strQuery, string strExcelConnectionString)
        {
            return getExcelDataTable(strQuery, getExcelConnection(strExcelConnectionString));
        }
        public static DataTable getExcelDataTable(string strQuery, OleDbConnection oleConn)
        {
            DataTable dt = new DataTable();
            OleDbCommand Ocmd = new OleDbCommand(strQuery, oleConn);
            OleDbDataAdapter Oda = new OleDbDataAdapter(Ocmd);
            Oda.Fill(dt);
            return dt;
        }
        #endregion

        public static string CheckNull(JToken obj, string ValueIfNull = "NULL")
        {
            object o = obj.ToObject<object>();

            if (obj == null || obj.ToString() == "" || obj.ToString() == "{}")
            {
                return ValueIfNull;
            }

            if (o is DateTime)
            {
                DateTime dt = new DateTime();
                if (DateTime.TryParse(o.ToString(), out dt))
                {
                    return Convert.ToDateTime(o.ToString()).ToString("MM/dd/yyyy hh:mm:ss.fff");
                }
            }
            if (o is bool)
            {
                return (bool)o ? "1" : "0";
            }
            return Convert.ToString(o);
        }


        public static string StringCheckNull(JToken obj, string ValueIfNull = "NULL")
        {
            object o = obj.ToObject<object>();

            if (obj == null || obj.ToString() == "" || obj.ToString() == "{}")
            {
                return ValueIfNull;
            }

            if (o is DateTime)
            {
                DateTime dt = new DateTime();
                if (DateTime.TryParse(o.ToString(), out dt))
                {
                    return "'" + Convert.ToDateTime(o.ToString()).ToString("MM/dd/yyyy hh:mm:ss.fff") + "'";
                }
            }
            if (o is bool)
            {
                return (bool)o ? "1" : "0";
            }
            return "'" + Convert.ToString(o) + "'";
        }

        #region "SPGeneration"
        public static string GetConnectionString(string strServerName = "", string strDataBaseName = "", string strUserName = "", string strPassword = "", bool blSqlAuth = false)
        {
            string strConnectionString = "Server=" + strServerName + ";";
            if (blSqlAuth)
            {
                strConnectionString += "User=" + strUserName + ";pwd=" + strPassword + ";";
            }
            else
            {
                strConnectionString += "Integrated Security=SSPI;";
            }

            if (strDataBaseName == "")
            {
                strConnectionString += "Initial Catalog=master;";
            }
            else
            {
                strConnectionString += "Initial Catalog=" + strDataBaseName + ";";
            }
            return strConnectionString;
        }

        public static DataTable GetDataTable(string SPorQuery, Dictionary<string, object> Parameters, string strConnection, bool isSP = true, int TimeOut = -1)
        {
            SqlCommand sCmd = new SqlCommand(SPorQuery);
            if (isSP)
            {
                sCmd.CommandType = CommandType.StoredProcedure;
            }

            if (TimeOut != -1)
            {
                sCmd.CommandTimeout = TimeOut;
            }
            if (Parameters != null)
            {
                foreach (KeyValuePair<string, object> Para in Parameters)
                {
                    sCmd.Parameters.AddWithValue(Para.Key, (Para.Value == null ? DBNull.Value : Para.Value));
                }
            }
            return GetDataTable(sCmd, strConnection);
        }

       /* public static string[] GenerateSqlInsUpdDeleteSp(string strConstring, string strTableName)
        {
            StringBuilder sbQuery = new StringBuilder();
            if (strTableName.Trim().Length == 0)
            {
                return null;
            }

            string[] strSPS = null;
            StringBuilder sbError = new StringBuilder();
            StringBuilder sbProcess = new StringBuilder();

            try
            {
                SPGeneratorClass spGen = new SPGeneratorClass(strConstring, strTableName, "");
                spGen.InsertSP = true;
                spGen.UpdateSP = true;
                spGen.DeleteSP = true;
                spGen.DocViewSP = false;

                spGen.InsertExcludeColumns.Add("Locked", "0");
                spGen.InsertExcludeColumns.Add("EntryDateTime", "getdate()");
                spGen.InsertExcludeColumns.Add("InsSessionID", "@SessionID");
                spGen.InsertExcludeColumns.Add("UpdSessionID", "NULL");
                spGen.InsertExcludeColumns.Add("ModifyCount", "0");
                spGen.InsertExcludeColumns.Add("IsDeleted", "0");
                spGen.InsertExcludeColumns.Add("Replicated", "0");

                spGen.UpdateExcludeColumns.Add("Locked", "");
                spGen.UpdateExcludeColumns.Add("EntryDateTime", "");
                spGen.UpdateExcludeColumns.Add("InsSessionID", "");
                spGen.UpdateExcludeColumns.Add("UpdSessionID", "@SessionID");
                spGen.UpdateExcludeColumns.Add("ModifyCount", "Isnull(ModifyCount,0) + 1");
                spGen.UpdateExcludeColumns.Add("IsDeleted", "");
                spGen.UpdateExcludeColumns.Add("Replicated", "0");
                spGen.DeleteIncludeColumns.Add("SessionID", "int");

                strSPS = spGen.GenerateSPS();

                //if (Insert)
                //{
                //    sbProcess.AppendLine(strSPS[0]);
                //}
                //if (Update)
                //{
                //    sbProcess.AppendLine(strSPS[1]);
                //}
                //if (Delete)
                //{
                //    sbProcess.AppendLine(strSPS[2]);
                //}
                //sbProcess.AppendLine(strSPS[5]);

            }
            catch (Exception ex)
            {
                sbError.AppendLine("Error Generating SP " + Environment.NewLine + ex.ToString());
            }

            return strSPS;
        }*/
        #endregion

        public static DataTable GetDataTableFromTableType(string DataBaseName, string TableType)
        {
            DataTable dt = new DataTable();

            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("TableTypeName", TableType);

            DataTable dtTableTypeCols = GetDataTable(DataBaseName, "pGetTableTypeColumns", Parameters);

            if (dtTableTypeCols != null && dtTableTypeCols.Rows.Count > 0)
            {
                foreach (DataRow dr in dtTableTypeCols.Rows)
                {
                    dt.Columns.Add(dr["name"].ToString(), Nullable.GetUnderlyingType(
            SqlTypeToType(dr["SqlType"].ToString())) ?? SqlTypeToType(dr["SqlType"].ToString()));
                }
            }

            dt.AcceptChanges();

            return dt;
        }

        public static Type SqlTypeToType(string type)
        {
            string[] tokens = type.Split(new char[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            string typeFamily = tokens[0].ToLowerInvariant();
            string size = tokens.Length > 1 ? tokens[1] : string.Empty;

            switch (typeFamily)
            {
                case "bigint":
                    return typeof(long);
                case "binary":
                    return size == "1" ? typeof(byte) : typeof(byte[]);
                case "bit":
                    return typeof(bool);
                case "char":
                    return size == "1" ? typeof(char) : typeof(string);
                case "datetime":
                    return typeof(DateTime);
                case "datetime2":
                    return typeof(DateTime);
                case "decimal":
                    return typeof(decimal?);
                case "numeric":
                    return typeof(decimal?);
                case "float":
                    return typeof(double);
                case "image":
                    return typeof(byte[]);
                case "int":
                    return typeof(int?);
                case "money":
                    return typeof(decimal);
                case "nchar":
                    return size == "1" ? typeof(char) : typeof(string);
                case "ntext":
                    return typeof(string);
                case "nvarchar":
                    return typeof(string);
                case "real":
                    return typeof(float);
                case "uniqueidentifier":
                    return typeof(Guid);
                case "smalldatetime":
                    return typeof(DateTime);
                case "smallint":
                    return typeof(short);
                case "smallmoney":
                    return typeof(decimal);
                case "sql_variant":
                    return typeof(object);
                case "text":
                    return typeof(string);
                case "time":
                    return typeof(TimeSpan);
                case "tinyint":
                    return typeof(byte);
                case "varbinary":
                    return typeof(byte[]);
                case "varchar":
                    return typeof(string);
                case "variant":
                    return typeof(string);
                case "xml":
                    return typeof(string);
                default:
                    throw new ArgumentException(string.Format("There is no .Net type specified for mapping T-SQL type '{0}'.", type));
            }
        }

      /*  public static string CreatePdfFile(string HtmlContent, string CurrentDate, string DocTitle, string FilePreffix)
        {
            string FileFullPath = "";

            var document = new HtmlToPdfDocument
            {
                GlobalSettings = {
                                    ProduceOutline = true,
                                    DocumentTitle = DocTitle,
                                    PaperSize = PaperKind.A4, // Implicit conversion to PechkinPaperSize
                                    Margins = {
                                                All = 1.375,
                                                Unit = TuesPechkin.Unit.Centimeters
                                              }
                                 },

                Objects = {
                                new ObjectSettings
                                {
                                    HtmlText = HtmlContent,
                                    WebSettings = new WebSettings
                                                  {
                                                        DefaultEncoding = "UTF-8",
                                                        LoadImages = true,
                                                        PrintBackground = true
                                                  },
                                    HeaderSettings = new HeaderSettings{CenterText =  ""},
                                    FooterSettings = new FooterSettings{LeftText = "[page]"}
                                 }
                            }
            };

            byte[] result;

            lock (LockOut)
            {
                result = converter.Convert(document);
            }

            string _FileName = "";

            try
            {
                _FileName = HttpContext.Current.Server.MapPath("~/FileUpload/BalanceSheet/" + FilePreffix + "_" + CurrentDate);
                FileStream _FileStream = new FileStream(_FileName, FileMode.Create, FileAccess.Write);
                _FileStream.Write(result, 0, result.Length);
                _FileStream.Close();

                FileFullPath = _FileName;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return FileFullPath;
        }*/
    }
}