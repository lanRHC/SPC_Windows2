using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Vision2.SQL
{
    public sealed class SQLSeverHelper
    {
        private SqlCommand cmd = null;
        private SqlConnection conn = null;
        private SqlDataReader sdr = null;

        public string StrConn;
        #region 数据库连接字符串需要手动创建
        ///// <summary>
        ///// 私有的构造函数
        ///// </summary> 
        private SQLSeverHelper( string strConn)
        {
            StrConn = strConn;
               //string strConn = ConfigurationManager.ConnectionStrings["MSSql2012"].ConnectionString;
               conn = new SqlConnection(StrConn);
        }
        #endregion

        ///// <summary>
        ///// 设置数据库连接字符串——张连海
        ///// </summary>
        ///// <param name="strConn"></param>
        //public void SetConnection(string strConnName)
        //{
        //    string strConnAll = ConfigurationManager.ConnectionStrings[strConnName].ConnectionString;
        //    String strConn = strConnAll.Substring(strConnAll.IndexOf("data source"), strConnAll.IndexOf("MultipleActiveResultSets") - strConnAll.IndexOf("data source"));
        //    conn = new SqlConnection(strConn);
        //}


        /// <summary>
        /// 获得数据库连接
        /// </summary>
        /// <returns></returns>
        private SqlConnection GetConnection()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <param name="conn">数据库连接</param>
        private  void Close(SqlConnection conn)
        {
            if (conn != null)
            {
                try
                {
                    conn.Close();
                }
                catch (SqlException e)
                {
                    throw e;
                }
            }
        }

        #region  执行带参数的增删改命令： ExecuteNonQuery(string cmmText, SqlParameter[] para, CommandType cmmType)
        /// <summary>
        /// 执行带参数的增删改命令
        /// </summary>
        /// <param name="cmmText"></param>
        /// <param name="para"></param>
        /// <param name="cmmType"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string cmmText, SqlParameter[] para, CommandType cmmType)
        {
            using (cmd = new SqlCommand(cmmText, GetConnection()))
            {
                cmd.CommandType = cmmType;
                cmd.Parameters.AddRange(para);
                return cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region  执行不带参数的增删改命令：ExecuteNonQuery(string cmmText, CommandType cmmType)
        /// <summary>
        /// 执行不带参数的增删改命令
        /// </summary>
        /// <param name="cmmText"></param>
        /// <param name="cmmType"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string cmmText, CommandType cmmType)
        {
            using (cmd = new SqlCommand(cmmText, GetConnection()))
            {
                cmd.CommandType = cmmType;
                return cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region  执行带参数的查询命令：ExecuteQuery(string cmmText, SqlParameter[] para, CommandType cmmType)
        /// <summary>
        /// 执行带参数的查询命令
        /// </summary>
        /// <param name="cmmText"></param>
        /// <param name="para"></param>
        /// <param name="cmmType"></param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string cmmText, SqlParameter[] para, CommandType cmmType)
        {
            DataTable dt = new DataTable();
            cmd = new SqlCommand(cmmText, GetConnection());
            cmd.CommandType = cmmType;
            cmd.Parameters.AddRange(para);
            using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(sdr);
                return dt;
            }
        }
        #endregion

        #region 执行不带参数的查询命令： ExecuteQuery(string cmmText, CommandType cmmType)
        /// <summary>
        /// 执行不带参数的查询命令
        /// </summary>
        /// <param name="cmmText"></param>
        /// <param name="cmmType"></param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string cmmText, CommandType cmmType)
        {
            DataTable dt = new DataTable();
            cmd = new SqlCommand(cmmText, GetConnection());
            cmd.CommandType = cmmType;
            using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(sdr);
                return dt;
            }
        }
        #endregion


        #region 创建数据库表
        /// <summary>
        ///  在指定的数据库中，创建数据表
        /// </summary>
        /// <param name="db">指定的数据库</param>
        /// <param name="dt">要创建的数据表</param>
        /// <param name="dic">数据表中的字段及其数据类型</param>
        /// <param name="connKey">数据库的连接Key</param>
        public static bool CreateDataTable(string db, string dt, string createTable, SqlConnection sqlCommand)
        {
            string createDbStr = " select * from master.dbo.sysdatabases where name " + "= '" + db + "'";
            DataTable dtata = new DataTable();
            SqlCommand sqlCo = new SqlCommand(createDbStr, sqlCommand);
            SqlDataReader sdr = sqlCo.ExecuteReader(CommandBehavior.Default);
            dtata.Load(sdr);
            if (dtata.Rows.Count == 0)
            {
                return false;
            }
            createDbStr = "use " + db + " select 1 from  sysobjects where  id = object_id('" + dt + "') and type = 'U'";
            //在指定的数据库中  查找 该表是否存在
            DataTable dtada = new DataTable();
            sqlCo = new SqlCommand(createDbStr, sqlCommand);
            sdr = sqlCo.ExecuteReader(CommandBehavior.Default);
            sqlCo.Dispose();
            dtada.Load(sdr);
            string createTableStr = "";
            if (dtada.Rows.Count != 0)
            {          //其后判断数据表是否存在，然后创建数据表

                // createTableStr = "use " + db + " drop table " + dt;//删除表:delete from table（表名称）  删除数据表: drop database 数据库
                //sqlCo = new SqlCommand(createTableStr, sqlCommand);
                //sqlCo.ExecuteNonQuery();
                //sqlCo.Dispose();
                return false;
            }

            createTableStr = "use " + db + " " + createTable;
            sqlCo = new SqlCommand(createTableStr, sqlCommand);
            sqlCo.ExecuteNonQuery();
            sqlCo.Dispose();
            sqlCommand.Close();
            return true;
        }
        /// <summary>
        /// 删除数据库表格
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="dt">表格</param>
        /// <param name="sqlCommand"></param>
        /// <returns></returns>
        public static bool DropDataTable(string db, string dt,  SqlConnection sqlCommand)
        {
            string    createTableStr = "use " + db + " drop table " + dt;//删除表:delete from table（表名称）  删除数据表: drop database 数据库
            SqlCommand sqlCo = new SqlCommand(createTableStr, sqlCommand);
            sqlCo.ExecuteNonQuery();
            sqlCo.Dispose();
            return false;
        }
        /// <summary>
        /// 清除数据表数据
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="dt">表格</param>
        /// <param name="sqlCommand"></param>
        /// <returns></returns>
        public static bool DropDataBase(string db, string dt, SqlConnection sqlCommand)
        {
            if (sqlCommand.State == ConnectionState.Closed)
            {
                sqlCommand.Open();
            }
        
            string createTableStr = "use " + db + " truncate table " + dt;//删除表:delete from table（表名称）  删除数据表: drop database 数据库
            SqlCommand sqlCo = new SqlCommand(createTableStr, sqlCommand);
            sqlCo.ExecuteNonQuery();
            sqlCo.Dispose();
            return true;
        }

        public static bool CreateDB()
        {
            //create database 数据库名；

            return false;
        }
        #endregion
    }
}
