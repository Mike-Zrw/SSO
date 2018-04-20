using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsUserCenter.Repository.DBUtility
{
    public static class DbLib
    {

        #region [SQL 字段处理方法]
        public static short IsSmallInt(SqlDataReader reader, int n)
        {
            if (reader.IsDBNull(n) == true)
            {
                return 0;
            }
            else
            {
                return reader.GetInt16(n);
            }
        }
        public static int IsInt(SqlDataReader reader, int n)
        {
            if (reader.IsDBNull(n) == true)
            {
                return 0;
            }
            else
            {
                return reader.GetInt32(n);
            }
        }
        public static long IsLong(SqlDataReader reader, int n)
        {
            if (reader.IsDBNull(n) == true)
            {
                return 0;
            }
            else
            {
                return reader.GetInt64(n);
            }
        }
        public static string IsString(SqlDataReader reader, int n)
        {
            if (reader.IsDBNull(n) == true)
            {
                return "";
            }
            else
            {
                return reader.GetString(n);
            }
        }
        public static Decimal IsDecimal(SqlDataReader reader, int n)
        {
            if (reader.IsDBNull(n) == true)
            {
                return 0;
            }
            else
            {
                return System.Convert.ToDecimal(reader.GetValue(n));
            }
        }
        public static DateTime IsDateTime(SqlDataReader reader, int n)
        {
            if (reader.IsDBNull(n) == true)
            {
                return DateTime.MinValue;
            }
            else
            {
                return reader.GetDateTime(n);
            }
        }
        public static float IsFloat(SqlDataReader reader, int n)
        {
            if (reader.IsDBNull(n) == true)
            {
                return 0;
            }
            else
            {
                return (float)Convert.ToSingle(reader.GetValue(n));
            }
        }
        public static double IsDouble(SqlDataReader reader, int n)
        {
            if (reader.IsDBNull(n) == true)
            {
                return 0;
            }
            else
            {
                return System.Convert.ToDouble(reader.GetValue(n));
            }
        }
        public static bool IsBit(SqlDataReader reader, int n)
        {
            if (reader.IsDBNull(n) == true)
            {
                return false;
            }
            else
            {
                return (System.Convert.ToInt32(reader.GetValue(n)) == 1);
            }
        }
        /// <summary>
        /// 用于转换 int型 Output输出参数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ObjectToInt(object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else if (obj == DBNull.Value)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        public static byte IsByte(SqlDataReader reader, int n)
        {
            if (reader.IsDBNull(n) == true)
            {
                return 0;
            }
            else
            {
                return reader.GetByte(n);
            }
        }

        public static Guid IsGuid(SqlDataReader reader, int n)
        {
            if (reader.IsDBNull(n) == true)
            {
                return new Guid();
            }
            else
            {
                return reader.GetGuid(n);
            }
        }

        public static TimeSpan IsTimeSpan(SqlDataReader reader, int n)
        {
            if (reader.IsDBNull(n) == true)
            {
                return new TimeSpan();
            }
            else
            {
                return reader.GetTimeSpan(n);
            }
        }
        public static TimeSpan? IsTimeSpanNull(SqlDataReader reader, int n)
        {
            if (reader.IsDBNull(n) == true)
            {
                return null;
            }
            else
            {
                return reader.GetTimeSpan(n);
            }
        }
        public static byte? IsByteNull(SqlDataReader reader, int n)
        {
            if (reader.IsDBNull(n) == true)
            {
                return null;
            }
            else
            {
                return reader.GetByte(n);
            }
        }
        internal static int? IsIntNull(SqlDataReader reader, int n)
        {
            if (reader.IsDBNull(n) == true)
            {
                return null;
            }
            else
            {
                return reader.GetInt32(n);
            }
        }

        internal static long? IsLongNull(SqlDataReader reader, int n)
        {
            if (reader.IsDBNull(n) == true)
            {
                return null;
            }
            else
            {
                return reader.GetInt64(n);
            }
        }
        internal static DateTime? IsDateTimeNull(SqlDataReader reader, int n)
        {
            if (reader.IsDBNull(n) == true)
            {
                return null;
            }
            else
            {
                return reader.GetDateTime(n);
            }
        }

        internal static decimal? IsDecimalNull(SqlDataReader reader, int n)
        {
            if (reader.IsDBNull(n) == true)
            {
                return null;
            }
            else
            {
                return System.Convert.ToDecimal(reader.GetValue(n));
            }
        }

        internal static bool? IsBitNull(SqlDataReader reader, int n)
        {
            if (reader.IsDBNull(n) == true)
            {
                return null;
            }
            else
            {
                return (System.Convert.ToInt32(reader.GetValue(n)) == 1);
            }
        }
        #endregion [SQL 字段处理方法]
    }
}
