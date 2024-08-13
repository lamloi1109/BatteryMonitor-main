using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace BatteryMonitor.OracleSQL
{
    public class OracleSQL
    {
        // 'Public ora_connectstring = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=172.16.40.33)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=toptest)));User ID=kungtay;Password=kungtay;"

        private string oracleIp;
        private string oraclePort;
        private string oracleConnectionString;
        private string serviceName;
        private string dataUserId;
        private string dataPassword;


        public OracleSQL(
         string oracleIp,
         string oraclePort,
         string oracleConnectionString,
         string serviceName,
         string dataUserId,
         string dataPassword
            )
        {
            this.oracleIp = oracleIp;
            this.oraclePort = oraclePort;
            this.oracleConnectionString = oracleConnectionString;
            this.serviceName = serviceName;
            this.dataUserId = dataUserId;
            this.dataPassword = dataPassword;
        }

        public OracleSQL(string serverName, string dataUserId, string dataPassword)
        {
            if (serverName == "PRO")
            {
                this.oracleIp = "172.16.40.31";
                this.oraclePort = "1521";
                this.serviceName = "topprod";
                this.dataUserId = dataUserId;
                this.dataPassword = dataPassword;
            }
            if (serverName == "TEST")
            {
                this.oracleIp = "172.16.40.33";
                this.oraclePort = "1521";
                this.serviceName = "toptest";
                this.dataUserId = dataUserId;
                this.dataPassword = dataPassword;
            }
        }

        // Method to connect to Oracle SQL using specified connection string
        public void ConnectToOracleSQL()
        {
            // Connect to Oracle SQL


        }

        // set the connection string
        public void SetConnectionString()
        {
            this.oracleConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + this.oracleIp + ")(PORT=" + this.oraclePort + "))(CONNECT_DATA=(SERVICE_NAME=" + this.serviceName + ")));User ID= " + this.dataUserId + ";Password=" + this.dataPassword + ";";
        }

        // get the connection string
        public string GetConnectionString()
        {
            return this.oracleConnectionString;
        }
        // set ip address
        public void SetOracleIp(string ip)
        {
            this.oracleIp = ip;
        }

        // get ip address
        public string GetOracleIp()
        {
            return this.oracleIp;
        }

        // get string list Data
        public List<string> getStringListData(string sqlQuery, OracleConnection oracleConnection)
        {
            List<string> result = new List<string>();
            OracleCommand command = new OracleCommand(sqlQuery, oracleConnection);
            OracleDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(reader[0].ToString());
            }

            return result;
        }
        // update Data
        public void updateData(string sqlQuery, OracleConnection oracleConnection)
        {
            OracleCommand command = new OracleCommand(sqlQuery, oracleConnection);
            command.ExecuteNonQuery();
        }

        // delete Data
        public void deleteData(string sqlQuery, OracleConnection oracleConnection)
        {
            OracleCommand command = new OracleCommand(sqlQuery, oracleConnection);
            command.ExecuteNonQuery();
        }

        // add data
        public void addData(string sqlQuery, OracleConnection oracleConnection)
        {
            OracleCommand command = new OracleCommand(sqlQuery, oracleConnection);
            command.ExecuteNonQuery();
        }

    }
}
