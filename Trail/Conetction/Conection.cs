using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;
using System.Data;

namespace StudentsAPI.Conetction
{
    public class Conection
    {
        public string DefaultConnection { get; set; }
        public string ConnectionName { get; set; }
        public string ConnectionType { get; set; }
        
        public string Password { get; set; }

        public string Servername { get; set; }


        public static SqlConnectionStringBuilder GlbSqlConStrBuild;
        public static DateTime StrFromdate;
        public static DateTime StrTodate;

        public static string StrServerName;
        public static string StrsqlUserName;
        public static string Strsqlpass;
        public static string StrDataBasename;
        public static string StrDatabasePath;
        public static SqlConnection SqlCon = new SqlConnection();
        public static SqlConnection SqlmasCon = new SqlConnection();

        public static string Lngoprcode;
        public static string StrOpername;
        public static bool BlnInstall = false;

        public static string strMysqlServerName;
        public static string strMysqlDatabaseName;
        public static string strMysqlUserName;
        public static string strMysqlPassword;
        public static string StrMysqlDatabasePath;

        public static string StrLogUserName;
        public static string StrLogUserCode;

        public static string StrLoguserDept;

        public bool isOpen;

        public static bool BlnTriupaticon;
        public static bool BlnBengalurecon;
        public static bool Blndbp;
        public static bool BlnPeeny;

        public static bool BlnDetails;
        public static bool BlnSummery;

        private string constring;


        public SqlConnection ConnectionString()
        {

            SqlConnection SqlCon = new SqlConnection("Server=" + server() + ";Database=" + database() + ";User Id=" + User() + ";Password=" + password() + ";TrustServerCertificate=True");
            string conection = SqlCon.ToString();
            SqlCon.Open();
            return SqlCon;

        }

        public SqlConnection openconnection(SqlConnection con)
        {
            con.Open();
            return con;
        }

        public SqlConnection closeconnection(SqlConnection con)
        {
            con.Close();
            return con;
        }
        public String server()
        {
            return "ABHISHEK\\SQLEXPRESS";
        }

        public String database()
        {
            return "BikeStores";
        }
        public String User()
        {
            return "abhishekreddy";
        }

        public String password()
        {
            return "14389069";
        }
        public DataTable Excecutionsclarar(string Querry)
        {
            DataTable data = new DataTable();
            SqlConnection Db = ConnectionString();
            try
            {
                if (Db.State == ConnectionState.Closed)
                {
                    openconnection(ConnectionString());
                }

                using (SqlDataAdapter sda = new SqlDataAdapter(Querry, Db))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(data);
                }
                closeconnection(Db);
                return data;
            }
            catch (Exception ex)
            {
                return data;
            }
        }
        public Boolean Excecutions(string Querry)
        {
            try
            {
                SqlConnection Db = ConnectionString();
                if (Db.State == ConnectionState.Closed)
                {
                    openconnection(ConnectionString());
                }

                SqlCommand cmdd = new SqlCommand(Querry, Db);
                int ex = 0;
                ex = cmdd.ExecuteNonQuery();
                closeconnection(Db);
                if (ex >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
