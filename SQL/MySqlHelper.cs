using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Vision2.SQL
{


        public class MySqlHelperEx
        {
            public  string connstr = "server=127.0.0.1;database=SMT;username=root;password=shifong203;";



        #region 执行连接成功

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public  bool Connstr()
        {
            bool reslut = false;
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                DataSet ds = new DataSet();
                try
                {
                    conn.Open();
                    reslut = true;
                }
                catch (Exception ex)
                {
                    throw ex;
  
                }
                finally
                {
                    conn.Close();
                }
                return reslut;
            }
        }
        #endregion
        #region 执行查询语句，返回MySqlDataReader

        /// <summary>
        /// 执行查询语句，返回MySqlDataReader
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public  MySqlDataReader ExecuteReader(string sqlString)
            {
                MySqlConnection connection = new MySqlConnection(connstr);
                MySqlCommand cmd = new MySqlCommand(sqlString, connection);
                MySqlDataReader myReader = null;
                try
                {
                    connection.Open();
                    myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    return myReader;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    connection.Close();
                throw e;
                }
                finally
                {
                    if (myReader == null)
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
            #endregion

            #region 执行带参数的查询语句，返回 MySqlDataReader

            /// <summary>
            /// 执行带参数的查询语句，返回MySqlDataReader
            /// </summary>
            /// <param name="sqlString"></param>
            /// <param name="cmdParms"></param>
            /// <returns></returns>
            public  MySqlDataReader ExecuteReader(string sqlString, params MySqlParameter[] cmdParms)
            {
                MySqlConnection connection = new MySqlConnection(connstr);
                MySqlCommand cmd = new MySqlCommand();
                MySqlDataReader myReader = null;
                try
                {
                    PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                    myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    cmd.Parameters.Clear();
                    return myReader;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    connection.Close();
                    throw e;
                }
                finally
                {
                    if (myReader == null)
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
            #endregion

            #region 执行sql语句,返回执行行数

            /// <summary>
            /// 执行sql语句,返回执行行数
            /// </summary>
            /// <param name="sql"></param>
            /// <returns></returns>
            public  int ExecuteNonQuerySql(string sql)
            {
                using (MySqlConnection conn = new MySqlConnection(connstr))
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        try
                        {
                            conn.Open();
                            int rows = cmd.ExecuteNonQuery();
                            return rows;
                        }
                        catch (MySql.Data.MySqlClient.MySqlException e)
                        {
                            //conn.Close();
                            throw e;
                        }
                        finally
                        {
                            cmd.Dispose();
                            conn.Close();
                        }
                    }
                }

                return -1;
            }
        #endregion
        public Boolean IsExistTable(string pTableName, string pDB_NAME)
        {
            string isExitTableSql = @"select * from " + pDB_NAME + @"..sysobjects 
                                          where name = '" + pTableName + @"'";
            isExitTableSql=     "SELECT * FROM information_schema.SCHEMATA where SCHEMA_NAME='smt';";
            MySqlDataAdapter adp = null;
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                using (MySqlCommand cmd = new MySqlCommand(isExitTableSql, conn))
                {
                    try
                    {
                        conn.Open();
                         adp = new MySqlDataAdapter(isExitTableSql, conn);
                        DataSet ds = new DataSet();
                        adp.Fill(ds);
                        int rows = cmd.ExecuteNonQuery();
                        if (ds.Tables[0].Rows.Count>0)
                        {
                            return true;
                        }
            
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        conn.Close();
                        //throw e;
                        Console.WriteLine(e.Message);
                    }
                    finally
                    {
                        adp.Dispose();
                        cmd.Dispose();
                        conn.Close();
                    }
                }
            }

            return false;
            //SqlConnection connCheckTable = new SqlConnection(connstr);
            //if (connCheckTable.State != ConnectionState.Open) connCheckTable.Open();

          
     //       SqlCommand cmd = new SqlCommand(isExitTableSql, connCheckTable);

            //object pResult = cmd.ExecuteScalar();
            //if (pResult == null || pResult == System.DBNull.Value) return false;

            return true;

        }

        #region 执行带参数的sql语句，并返回执行行数
        /// <summary>
        /// 执行带参数的sql语句，并返回执行行数
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public  int ExecuteSql(string sqlString, params MySqlParameter[] cmdParms)
            {
                using (MySqlConnection connection = new MySqlConnection(connstr))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                            int rows = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                            return rows;
                        }
                        catch (System.Data.SqlClient.SqlException E)
                        {
                         throw E;
                        }
                        finally
                        {
                            cmd.Dispose();
                            connection.Close();
                        }
                    }
                }
            }
        #endregion
 


            #region 执行查询语句，返回DataSet

            /// <summary>
            /// 执行查询语句，返回DataSet
            /// </summary>
            /// <param name="sql"></param>
            /// <returns></returns>
            public  DataSet GetDataSet(string sql)
            {
                using (MySqlConnection conn = new MySqlConnection(connstr+ "default command timeout=999999"))
                {
                   
                    DataSet ds = new DataSet();
                    try
                    {
                        conn.Open();
                    //MySqlDataReader sqlDataReader= new MySqlDataAdapter(sql, conn);

                    MySqlDataAdapter DataAdapter = new MySqlDataAdapter(sql, conn);
                
                        DataAdapter.Fill(ds);
                        DataAdapter.Dispose();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
   
                    }
                    finally
                    {
                        conn.Close();
                    }
                    return ds;
                }
            }
            #endregion

            #region 执行带参数的查询语句，返回DataSet

            /// <summary>
            /// 执行带参数的查询语句，返回DataSet
            /// </summary>
            /// <param name="sqlString"></param>
            /// <param name="cmdParms"></param>
            /// <returns></returns>
            public  DataSet GetDataSet(string sqlString, params MySqlParameter[] cmdParms)
            {
                using (MySqlConnection connection = new MySqlConnection(connstr))
                {
                    MySqlCommand cmd = new MySqlCommand();
                    PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        try
                        {
                            da.Fill(ds, "ds");
                            cmd.Parameters.Clear();
                        }
                        catch (System.Data.SqlClient.SqlException ex)
                        {
                        throw ex;
                        }
                        finally
                        {
                            cmd.Dispose();
                            connection.Close();
                        }
                        return ds;
                    }
                }
            }
            #endregion

            #region 执行带参数的sql语句，并返回 object

            /// <summary>
            /// 执行带参数的sql语句，并返回object
            /// </summary>
            /// <param name="sqlString"></param>
            /// <param name="cmdParms"></param>
            /// <returns></returns>
            public  object GetSingle(string sqlString, params MySqlParameter[] cmdParms)
            {
                using (MySqlConnection connection = new MySqlConnection(connstr))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                            object obj = cmd.ExecuteScalar();
                            cmd.Parameters.Clear();
                            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                            {
                                return null;
                            }
                            else
                            {
                                return obj;
                            }
                        }
                        catch (System.Data.SqlClient.SqlException e)
                        {
                            throw new Exception(e.Message);
                        }
                        finally
                        {
                            cmd.Dispose();
                            connection.Close();
                        }
                    }
                }
            }

            #endregion

            /// <summary>
            /// 执行存储过程,返回数据集
            /// </summary>
            /// <param name="storedProcName">存储过程名</param>
            /// <param name="parameters">存储过程参数</param>
            /// <returns>DataSet</returns>
            public  DataSet RunProcedureForDataSet(string storedProcName, IDataParameter[] parameters)
            {
                using (MySqlConnection connection = new MySqlConnection(connstr))
                {
                    DataSet dataSet = new DataSet();
                    connection.Open();
                    MySqlDataAdapter sqlDA = new MySqlDataAdapter();
                    sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                    sqlDA.Fill(dataSet);
                    connection.Close();
                    return dataSet;
                }
            }

            /// <summary>
            /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
            /// </summary>
            /// <param name="connection">数据库连接</param>
            /// <param name="storedProcName">存储过程名</param>
            /// <param name="parameters">存储过程参数</param>
            /// <returns>SqlCommand</returns>
            private static MySqlCommand BuildQueryCommand(MySqlConnection connection, string storedProcName,
                IDataParameter[] parameters)
            {
                MySqlCommand command = new MySqlCommand(storedProcName, connection);
                command.CommandType = CommandType.StoredProcedure;
                foreach (MySqlParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
                return command;
            }

            #region 装载MySqlCommand对象

            /// <summary>
            /// 装载MySqlCommand对象
            /// </summary>
            private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, string cmdText,
                MySqlParameter[] cmdParms)
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                cmd.Connection = conn;
                cmd.CommandText = cmdText;
                if (trans != null)
                {
                    cmd.Transaction = trans;
                }
                cmd.CommandType = CommandType.Text; //cmdType;
                if (cmdParms != null)
                {
                    foreach (MySqlParameter parm in cmdParms)
                    {
                        cmd.Parameters.Add(parm);
                    }
                }
            }
            #endregion

        }
    

}
