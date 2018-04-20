using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Reflection;
using AbsUserCenter.Tool;
using System.ComponentModel.DataAnnotations;
using AbsUserCenter.Core.Config;

namespace AbsUserCenter.Repository.DBUtility
{
    /// <summary>
    /// The SqlHelper class is intended to encapsulate high performance, 
    /// scalable best practices for common uses of SqlClient.
    /// </summary>
    public abstract class SqlHelper
    {
        public static readonly string RwViewConnString = ConfigOptions.ConnectionStrings.RwViewSQLConnString;
        public static readonly string RoViewConnString = ConfigOptions.ConnectionStrings.RoViewSQLConnString;
        public static int SqlHelperNonQueryCommandTimeout = ConfigOptions.AppSettings.SqlHelperNonQueryCommandTimeout;
        public static int SqlHelperQueryCommandTimeout = ConfigOptions.AppSettings.SqlHelperQueryCommandTimeout;
        //参数缓存存储
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());


        /// <summary>
        /// 执行不返回结果集的SQL语句
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlHelper.DbNullSqlParameter(commandParameters);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = SqlHelperNonQueryCommandTimeout;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);

                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// 执行不返回结果集的SQL语句
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlHelper.DbNullSqlParameter(commandParameters);
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// 执行不返回结果集的SQL语句
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlHelper.DbNullSqlParameter(commandParameters);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = SqlHelperNonQueryCommandTimeout;
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// 执行返回结果集的SQL语句
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                cmd.CommandTimeout = SqlHelperQueryCommandTimeout;
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }
        /// <summary>
        /// 设置请求时间 执行返回结果集的SQL语句
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="time"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, int time, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = time;
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }
        /// <summary>
        /// 执行返回结果集的SQL语句(不清除SQL参数)
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReaderWithoutClearParammeters(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = SqlHelperQueryCommandTimeout;
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// 执行返回SqlCommand操作类对象
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static SqlCommand ExecuteSqlCommand(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                cmd.CommandTimeout = SqlHelperQueryCommandTimeout;
                return cmd;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// 执行返回结果集的SQL语句
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = SqlHelperQueryCommandTimeout;
            PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return rdr;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = SqlHelperQueryCommandTimeout;
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            SqlDataReader rdr = cmd.ExecuteReader();
            cmd.Parameters.Clear();
            return rdr;
        }
        /// <summary>
        ///  执行返回结果集的SQL语句
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlHelper.DbNullSqlParameter(commandParameters);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = SqlHelperQueryCommandTimeout;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        ///  执行返回结果集的SQL语句
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlHelper.DbNullSqlParameter(commandParameters);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = SqlHelperQueryCommandTimeout;
            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlHelper.DbNullSqlParameter(commandParameters);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = SqlHelperQueryCommandTimeout;
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// 在同一个事务里执行多条SQL语句
        /// lx.yin
        /// 2016-09-02
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="lstSql"></param>
        /// <returns></returns>
        public static string ExecMoreSql(string connectionString, List<string> lstSql)
        {
            string Message = "";
            SqlConnection conn = new SqlConnection(connectionString);
            SqlTransaction Trans;
            conn.Open();
            SqlCommand comm = new SqlCommand();
            comm.CommandTimeout = SqlHelperQueryCommandTimeout;
            comm.Connection = conn;
            Trans = conn.BeginTransaction();
            comm.Transaction = Trans;
            string sqls = "";
            try
            {
                //依次执行传入的SQL语句
                for (int i = 0; i < lstSql.Count; i++)
                {
                    if (lstSql[i].ToString() != "")
                    {
                        sqls = lstSql[i].ToString();
                        comm.CommandText = lstSql[i].ToString();
                        comm.ExecuteNonQuery();
                    }

                }
                Trans.Commit();
            }
            catch (Exception e)
            {
                Trans.Rollback();
                Message = e.Message;
            }
            finally
            {
                conn.Close();
            }
            return Message;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="commandParameters"></param>
        public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static SqlParameter[] GetCachedParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                {
                    cmd.Parameters.Add(parm);

                    #region lxyin 20160328 测试用
                    //string sqlPara = "Insert Into @tmpRows(";
                    //string sqlValue = "";
                    //try
                    //{
                    //    if (parm.Value != null)
                    //    {
                    //        DataTable dts = (DataTable)parm.Value;
                    //        int clomunIndex = 0;
                    //        foreach (DataColumn item in dts.Columns)
                    //        {
                    //            sqlPara += item.ColumnName + ",";
                    //            if (item.DataType == typeof(int) || item.DataType == typeof(decimal))
                    //            {
                    //                sqlValue += string.IsNullOrEmpty(dts.Rows[0][clomunIndex].ToString()) ? "null," : dts.Rows[0][clomunIndex].ToString() + ",";
                    //            }
                    //            else
                    //            {
                    //                sqlValue += "'" + dts.Rows[0][clomunIndex].ToString() + "',";
                    //            }
                    //            clomunIndex++;
                    //        }
                    //        sqlPara = sqlPara.Substring(0, sqlPara.Length - 1) + ")";
                    //        sqlValue = sqlValue.Substring(0, sqlValue.Length - 1);
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    sqlValue = ex.ToString();
                    //} 
                    #endregion
                }
            }
        }
        public static DataSet GetExecuteDataSet(int begin, int end, string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = SqlHelperQueryCommandTimeout;
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                adp.Fill(ds, begin, end, "data");
                adp = null;
            }
            cmd = null;
            return ds;
        }

        /// <summary>  
        /// 判断SqlDataReader是否存在某列  
        /// </summary>  
        /// <param name="dr">SqlDataReader</param>  
        /// <param name="columnName">列名</param>  
        /// <returns></returns>  
        private static bool readerExists(SqlDataReader dr, string columnName)
        {

            dr.GetSchemaTable().DefaultView.RowFilter = "ColumnName= '" + columnName + "'";

            return (dr.GetSchemaTable().DefaultView.Count > 0);

        }

        ///<summary>  
        ///利用反射和泛型将SqlDataReader转换成List模型  
        ///</summary>  
        ///<param name="sql">查询sql语句</param>  
        ///<returns></returns>  

        public static List<T> ExecuteReaderToList<T>(SqlDataReader reader) where T : new()
        {
            List<T> list;
            Type type = typeof(T);
            string tempName = string.Empty;
            if (reader.HasRows)
            {
                list = new List<T>();
                while (reader.Read())
                {
                    T t = new T();
                    PropertyInfo[] propertys = t.GetType().GetProperties();

                    foreach (PropertyInfo pi in propertys)
                    {
                        tempName = pi.Name;

                        if (readerExists(reader, tempName))
                        {
                            if (!pi.CanWrite)
                            {
                                continue;
                            }
                            var value = reader[tempName];

                            if (value != DBNull.Value)
                            {
                                pi.SetValue(t, value, null);
                            }

                        }

                    }

                    list.Add(t);

                }
                return list;
            }
            return null;
        }
        ///<summary>  
        ///利用反射和泛型将SqlDataReader转换成HashSet模型  
        ///</summary>  
        ///<param name="sql">查询sql语句</param>  
        ///<returns></returns>  

        public static HashSet<T> ExecuteReaderToHashSet<T>(SqlDataReader reader) where T : new()
        {
            HashSet<T> list;
            Type type = typeof(T);
            string tempName = string.Empty;
            if (reader.HasRows)
            {
                list = new HashSet<T>();
                while (reader.Read())
                {
                    T t = new T();
                    PropertyInfo[] propertys = t.GetType().GetProperties();

                    foreach (PropertyInfo pi in propertys)
                    {
                        tempName = pi.Name;

                        if (readerExists(reader, tempName))
                        {
                            if (!pi.CanWrite)
                            {
                                continue;
                            }
                            var value = reader[tempName];

                            if (value != DBNull.Value)
                            {
                                pi.SetValue(t, value, null);
                            }

                        }

                    }

                    list.Add(t);

                }
                return list;
            }
            return null;
        }
        public static void DbNullSqlParameter(params SqlParameter[] param)
        {
            if (param != null && param.Length > 0)
            {
                foreach (SqlParameter item in param)
                {
                    if (item.Value == null)
                    {
                        item.Value = DBNull.Value;
                    }
                }
            }
        }


        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="o"></param>
        public static void BulkInsertAll<T>(IEnumerable<T> entities, SqlTransaction tran)
        {
            entities = entities.ToArray();
            Type t = typeof(T);
            var properties = t.GetProperties().Where(EventTypeFilter).ToArray();
            var table = new DataTable();

            foreach (var property in properties)
            {
                Type propertyType = property.PropertyType;
                if (propertyType.IsGenericType &&
                    propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = Nullable.GetUnderlyingType(propertyType);
                }

                table.Columns.Add(new DataColumn(property.Name, propertyType));
            }

            foreach (var entity in entities)
            {
                table.Rows.Add(properties.Select(
                  property => GetPropertyValue(
                  property.GetValue(entity, null))).ToArray());
            }
            using (var bulkCopy = new SqlBulkCopy(tran.Connection, SqlBulkCopyOptions.KeepNulls, tran)
            {
                DestinationTableName = t.Name
            })
            {
                bulkCopy.WriteToServer(table);
            }


        }

        private static bool EventTypeFilter(System.Reflection.PropertyInfo p)
        {
            var attribute = Attribute.GetCustomAttribute(p,
                typeof(AssociationAttribute)) as AssociationAttribute;

            if (attribute == null) return true;
            if (attribute.IsForeignKey == false) return true;

            return false;
        }

        private static object GetPropertyValue(object o)
        {
            if (o == null)
                return DBNull.Value;
            return o;
        }
        static public object SqlParamterNullValue(object obj)
        {
            if (obj == null)
                return DBNull.Value;
            return obj;
        }
    }
}