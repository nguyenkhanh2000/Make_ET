using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Make_ET.Oracle
{
    public class ConnectionOracle
    {
        public static OracleConnection _oracleconnection;
        public static void ConnectOracle()
        {
            //string connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=DESKTOP-G0JGG6F)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl)));User Id=SYS;Password=Toilaso1;";
            string connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=DESKTOP-G0JGG6F)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl)));User Id=SYS;Password=Toilaso1;DBA Privilege=SYSDBA;";
            //string connectionString ="User Id=<SYS>;Password=<Toilaso1>;Data Source=<ORCL>";
            _oracleconnection = new OracleConnection(connectionString);    
        }
    }
}
