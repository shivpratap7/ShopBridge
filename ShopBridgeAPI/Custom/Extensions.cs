using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace ShopBridgeAPI.Custom
{
    public static class Extensions
    {

        public static DataTable GetDataTableFromJSONArray(this DataTable TableStruc, JArray JActArray)
        {
            IFormatProvider culter = new CultureInfo("en-US");
            if (JActArray != null && JActArray.Count() > 0)
            {
                foreach (JObject jActObj in JActArray)
                {
                    DataRow dr = TableStruc.NewRow();
                    foreach (DataColumn dc in TableStruc.Columns)
                    {
                        if (dc.DataType == typeof(DateTime))
                        {
                            dr[dc.ColumnName] = jActObj[dc.ColumnName].ToString() == "" ? (object)DBNull.Value :
                                Convert.ToDateTime(DateTime.ParseExact(jActObj[dc.ColumnName].ToString(), "d/M/yyyy", culter));
                              
                        }
                        else
                        {
                            dr[dc.ColumnName] = jActObj[dc.ColumnName].ToString() == "" ? (object)DBNull.Value : jActObj[dc.ColumnName]; //Getintdecimalnullvalue(dc.GetType(), jActObj[dc.ColumnName].ToString());
                        }
                    }
                    TableStruc.Rows.Add(dr);
                }
            }
            return TableStruc;
        }


        public static object Getintdecimalnullvalue(Type T, object columnValue)
        {
            if (columnValue == null || columnValue == "" || columnValue == "{}")
            {
                if (T == typeof(int))
                {
                    return (int?)null;
                }
                else if (T.GetType() == typeof(decimal))
                {
                    return (decimal?)null;
                }
                else
                {
                    return columnValue;
                }
            }
            else
            {
                return columnValue;
            }
        }

        public static JObject toJObjectWithRelations(this DataSet ds, List<string> TablesToInclude)
        {
            List<Dictionary<string, object>> Rtn = new List<Dictionary<string, object>>();

            Dictionary<string, object> Rtn1 = new Dictionary<string, object>();

            //Loop on Tables
            foreach (string str in TablesToInclude)
            {
                DataTable dt = ds.Tables[str];

                List<Dictionary<string, object>> dataRows = new List<Dictionary<string, object>>();
                dt.Rows.Cast<DataRow>().ToList().ForEach(dataRow =>
                {
                    var row = new Dictionary<string, object>();
                    dt.Columns.Cast<DataColumn>().ToList().ForEach(column =>
                    {
                        row.Add(column.ColumnName, dataRow[column]);
                    });

                    foreach (DataRelation dr in dt.ChildRelations)
                    {
                        var ChildRows = new List<Dictionary<string, object>>();

                        dataRow.GetChildRows(dr).ToList().ForEach(child =>
                        {
                            var ChildRow = new Dictionary<string, object>();

                            child.Table.Columns.Cast<DataColumn>().ToList().ForEach(column =>
                            {

                                DataTable dtChild = ds.Tables[column.ColumnName];
                                if (dtChild != null)
                                {
                                    var NewChildRows = new List<Dictionary<string, object>>();

                                    foreach (DataRelation drNew in dtChild.ChildRelations)
                                    {
                                        child.GetChildRows(drNew).ToList().ForEach(childNew =>
                                        {
                                            var NewChildRow = new Dictionary<string, object>();
                                            childNew.Table.Columns.Cast<DataColumn>().ToList().ForEach(columnNew =>
                                            {
                                                NewChildRow.Add(columnNew.ColumnName, childNew[columnNew]);
                                            });
                                            NewChildRows.Add(NewChildRow);
                                        });
                                        ChildRow.Add(drNew.RelationName, NewChildRows);
                                    }
                                }
                                else
                                {
                                    ChildRow.Add(column.ColumnName, child[column]);
                                }
                            });
                            ChildRows.Add(ChildRow);
                        });

                        row.Add(dr.RelationName, ChildRows);
                    }

                    dataRows.Add(row);
                });

                Rtn1.Add(dt.TableName, dataRows);

                var finalTable = new Dictionary<string, object>();
                finalTable.Add(dt.TableName, dataRows);

                Rtn.Add(finalTable);
            }

            return JObject.FromObject(Rtn1);

            //return JArray.FromObject(Rtn);            
        }

        public static JArray ToJArray(this DataTable dt)
        {
            //return new JavaScriptSerializer().Serialize(GetRowToDictionary(dt));

            //Dictionary<string, object> d = new Dictionary<string, object>();

            //d.Add(dt.TableName, GetRowToDictionary(dt));

            var x = GetRowToDictionary(dt);

            return JArray.FromObject(x);

        }

        public static JArray ToJArray(this DataSet ds)
        {
            List<Dictionary<string, object>> d = new List<Dictionary<string, object>>();
            foreach (DataTable table in ds.Tables)
            {
                Dictionary<string, object> d1 = new Dictionary<string, object>();

                d1.Add(table.TableName, GetRowToDictionary(table));

                d.Add(d1);
            }

            return JArray.FromObject(d);
        }

        public static string ToJSON(this DataTable dt)
        {
            //return new JavaScriptSerializer().Serialize(GetRowToDictionary(dt));

            Dictionary<string, object> d = new Dictionary<string, object>();

            d.Add(dt.TableName, GetRowToDictionary(dt));

            var serializer = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
            return serializer.Serialize(d);

        }

        private static List<Dictionary<string, object>> GetRowToDictionary(DataTable dt)
        {
            List<Dictionary<string, object>> dataRows = new List<Dictionary<string, object>>();
            dt.Rows.Cast<DataRow>().ToList().ForEach(dataRow =>
            {
                var row = new Dictionary<string, object>();
                dt.Columns.Cast<DataColumn>().ToList().ForEach(column =>
                {
                    row.Add(column.ColumnName, dataRow[column]);
                });
                dataRows.Add(row);
            });
            return dataRows;
        }

        public static string ToJSON(this DataSet data)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            foreach (DataTable table in data.Tables)
            {
                d.Add(table.TableName, GetRowToDictionary(table));
            }
            var serializer = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
            return serializer.Serialize(d);

            //return new JavaScriptSerializer().Serialize(d);
        }

        public static DataTable JsonStringToDataTable(string jsonString)
        {
            DataTable dt = new DataTable();
            string[] jsonStringArray = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
            List<string> ColumnsName = new List<string>();
            foreach (string jSA in jsonStringArray)
            {
                string[] jsonStringData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                foreach (string ColumnsNameData in jsonStringData)
                {
                    try
                    {
                        int idx = ColumnsNameData.IndexOf(":");
                        string ColumnsNameString = ColumnsNameData.Substring(0, idx - 1).Replace("\"", "");
                        if (!ColumnsName.Contains(ColumnsNameString))
                        {
                            ColumnsName.Add(ColumnsNameString);
                        }
                    }
                    catch (Exception)
                    {
                        throw new Exception(string.Format("Error Parsing Column Name : {0}", ColumnsNameData));
                    }
                }
                break;
            }
            foreach (string AddColumnName in ColumnsName)
            {
                dt.Columns.Add(AddColumnName.Trim());
            }
            foreach (string jSA in jsonStringArray)
            {
                string[] RowData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                DataRow nr = dt.NewRow();
                foreach (string rowData in RowData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string RowColumns = rowData.Substring(0, idx - 1).Replace("\"", "").Trim();
                        object RowDataString = rowData.Substring(idx + 1).Replace("\"", "").Trim();

                        if (RowDataString.ToString() == "null" || RowDataString.ToString() == "undefined" ||
                            RowDataString.ToString() == "''" || RowDataString.ToString() == "" || RowDataString.ToString() == "{}")
                        {
                            RowDataString = DBNull.Value;
                        }
                        else if (RowDataString.ToString() == "false" || RowDataString.ToString() == "true")
                        {
                            RowDataString = RowDataString.ToString() == "false" ? "False" : "True";
                        }

                        nr[RowColumns.Trim()] = RowDataString;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                dt.Rows.Add(nr);
            }
            return dt;
        }


    }
}