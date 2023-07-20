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
        CGlobal.SECURITY[] arrsttSECURITY;       
        public void  Save()
        {        
            ConnectionOracle.ConnectOracle();
            OracleConnection conn = ConnectionOracle._oracleconnection;
            conn.Open(); 
            try
            {
                int i = 1;
                foreach(CGlobal.SECURITY security in arrsttSECURITY)
                {
                    using (OracleCommand cmd = new OracleCommand("INSERT INTO STOCK_HCM (TRANID, STOCKNO, STOCKSYMBOL, STOCKTYPE, PRIORCLOSEPRICE, OPENPRICE, LAST, LASTVOL, HIGHEST, LOWEST) " +
                                                                "VALUES (:TRANID, :STOCKNO, :STOCKSYMBOL, :STOCKTYPE, :PRIORCLOSEPRICE, :OPENPRICE, :LAST, :LASTVOL, :HIGHEST, :LOWEST)", conn))
                    {
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
                    //using(OracleCommand cmds = new OracleCommand("INSERT INTO TBL_IG3_SI(ID, SYMBOL, BOARDCODE, SECURITYTYPE, BASICPRICE, MATCHPRICE, OPENPRICE, CLOSERPRICE, MIDPX, HIGHESTPRICE, LOWESTPRICE, NM_TOTALTRADEDQTTY)" +
                    //    "VALUES(:ID, :SYMBOL, :BOARDCODE, :SECURITYTYPE, :BASICPRICE, :MATCHPRICE, :OPENPRICE, :CLOSERPRICE, :MIDPX, :HIGHESTPRICE, :LOWESTPRICE, :NM_TOTALTRADEDQTTY)", conn))
                    //{
                    //    cmds.Parameters.Add(":ID", OracleDbType.Decimal).Value = i;
                    //    cmds.Parameters.Add(":SYMBOL",OracleDbType.NVarchar2).Value = security.StockSymbol;
                    //}
                    i++;
                }
                
            }catch(Exception ex)
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
