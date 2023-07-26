using Make_ET.DataModels;
using Make_ET.Redis;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Make_ET.DataModels.CGlobal;

namespace Make_ET.Oracle
{
    public class saveData
    {      
        public void Save(CreaderAll<SECURITY> m_SECURITY)
        {
            ConnectionOracle.ConnectOracle();
            OracleConnection conn = ConnectionOracle._oracleconnection;
            //string sqlQuery = "SELECT COUNT(*) FROM STOCK_HCM";
            int i = 1;
            conn.Open();
            try
            {
                using (OracleCommand truncateCmd = new OracleCommand("TRUNCATE TABLE STOCK_HCM", conn))
                {
                    truncateCmd.ExecuteNonQuery();
                }
                foreach (CGlobal.SECURITY security in m_SECURITY.DataUpdate)
                {
                    using (OracleCommand cmd = new OracleCommand("INSERT_STOCK_HCM", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(":TRANID", OracleDbType.Decimal).Value = i;
                        cmd.Parameters.Add(":STOCKNO", OracleDbType.Decimal).Value = security.StockNo;
                        cmd.Parameters.Add(":STOCKSYMBOL", OracleDbType.NVarchar2, 16).Value = security.StockSymbol;
                        cmd.Parameters.Add(":STOCKTYPE", OracleDbType.NVarchar2, 2).Value = security.StockType;
                        cmd.Parameters.Add(":PRIORCLOSEPRICE", OracleDbType.Double).Value = security.PriorClosePrice;
                        cmd.Parameters.Add(":OPENPRICE", OracleDbType.Double).Value = security.OpenPrice;
                        cmd.Parameters.Add(":LAST", OracleDbType.Double).Value = security.Last;
                        cmd.Parameters.Add(":LASTVOL", OracleDbType.Decimal).Value = security.LastVol;
                        cmd.Parameters.Add(":HIGHEST", OracleDbType.Double).Value = security.Highest;
                        cmd.Parameters.Add(":LOWEST", OracleDbType.Double).Value = security.Lowest;
                        cmd.ExecuteNonQuery();
                    }
                    i++;
                }                           
            }
            
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        
    }
}
