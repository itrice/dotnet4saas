using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;



namespace iTrice.SAAS.TenantManager.Migrations
{
    public class DBMigration
    {
        public static void CreateDB(IConfiguration configuration, string newDBName)
        {            
            var constr = configuration.GetConnectionString("master");
            SqlHelper.connectionString = constr;
            SqlHelper.ExecuteNonQuery($"CREATE DATABASE[{newDBName}]");
        }
    }

    public class SqlHelper
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string connectionString = "";

        public void closeCon()
        {

        }

        #region ExecuteNonQuery 执行T-SQL语句，并返回受影响行数 增|删|改
        /// <summary>
        /// 执行T-SQL语句 (增|删|改)，并返回受影响行数
        /// </summary>
        /// <param name="cmdType">cmdType</param>
        /// <param name="cmdText">带参的SQL语句 cmdText|存储过程名称</param>
        /// <param name="commandParams">参数集合</param>
        /// <returns>返回所受影响的行数int</returns>
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] commandParams)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    PrepareCommand(con, cmd, cmdType, cmdText, commandParams);
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
        }

        /// <summary>
        /// 执行T-SQL语句，并返回受影响行数
        /// </summary>
        /// <param name="cmdText">带参的SQL语句 cmdText</param>
        /// <param name="commandParams">参数集合</param>
        /// <returns>返回受影响的行数</returns>
        public static int ExecuteNonQuery(string cmdText, params SqlParameter[] commandParams)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    PrepareCommand(con, cmd, CommandType.Text, cmdText, commandParams);
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
        }

        /// <summary>
        /// 执行T-SQL语句，并返回受影响行数
        /// </summary>
        /// <param name="cmdText">带参的SQL语句 cmdText</param>
        /// <returns>返回受影响的行数int</returns>
        public static int ExecuteNonQuery(string cmdText)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    con.Open();
                    int val = cmd.ExecuteNonQuery();
                    return val;
                }
            }
        }
        #endregion

        #region 执行增/删/改存储过程
        /// <summary>
        ///  执行增/删/改存储过程
        /// </summary>
        /// <param name="procName">存储过程名字</param>
        /// <param name="paras">参数集合</param>
        public void ExecuteUpdateProcedure(string procName, SqlParameter[] paras)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procName, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(paras);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

            }


        }
        #endregion

        #region ExecuteReader 执行查询,返回SqlDataReader
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="cmdType">Commond类型</param>
        /// <param name="cmdText">带参的SQL语句 cmdText</param>
        /// <param name="commandParameters">参数集合</param>
        /// <returns>返回查询结果SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(con, cmd, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                con.Close();
                throw;
            }
        }

        public static SqlDataReader ExecuteReader(string cmdText)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = cmdText;
                SqlDataReader sdr = cmd.ExecuteReader();
                return sdr;
            }
            catch
            {
                con.Close();
                throw;
            }
        }

        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// 根据指定的SQL语句,返回DATASET
        /// </summary>
        /// <param name="cmdType">Command类型</param>
        /// <param name="cmdText">带参的SQL语句 comText</param>
        /// <param name="para">参数集合</param>
        /// <returns>返回结果DataSet</returns>
        public static DataSet ExecuteDataSet(CommandType cmdType, string cmdText, params SqlParameter[] para)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();

                using (SqlCommand cmd = new SqlCommand())
                {
                    DataSet ds = new DataSet();
                    PrepareCommand(con, cmd, cmdType, cmdText, para);
                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);

                    return ds;
                }
            }
        }

        /// <summary>
        /// 根据指定的SQL语句,返回DATASET
        /// </summary>
        /// <param name="cmdType">Command类型</param>
        /// <param name="cmdText">带参的SQL语句 comText</param>
        /// <param name="para">参数集合</param>
        /// <returns>返回结果DataSet</returns>
        public static DataSet ExecuteDataSet(string cmdtext)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                using (SqlCommand cmd = new SqlCommand())
                {
                    DataSet ds = new DataSet();
                    PrepareCommand(con, cmd, CommandType.Text, cmdtext, null);
                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);

                    return ds;
                }
            }
        }

        /// <summary>
        /// 根据指定的SQL语句,返回DATASET
        /// </summary>
        /// <param name="cmdtext">要执行带参的SQL语句</param>
        /// <param name="para">参数</param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string cmdtext, params SqlParameter[] para)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                using (SqlCommand cmd = new SqlCommand())
                {
                    DataSet ds = new DataSet();
                    PrepareCommand(con, cmd, CommandType.Text, cmdtext, para);
                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);

                    return ds;
                }
            }
        }

        #endregion


        #region ExecuteScalar 执行查询并返回第一行，忽略其他行

        /// <summary>
        /// 执行查询并返回第一行，忽略其他行
        /// </summary>
        /// <param name="cmdType">Command类型 CommandType</param>
        /// <param name="cmdText">带参的SQL语句 cmdText</param>
        /// <param name="commandParameters">参数集合</param>
        /// <returns>返回执行结果Object</returns>
        public static object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    PrepareCommand(con, cmd, cmdType, cmdText, commandParameters);
                    object val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
        }


        public static object ExecuteScalar(string cmdText)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    con.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        public static object ExecuteScalar(string cmdText, params SqlParameter[] commandParameters)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    PrepareCommand(con, cmd, CommandType.Text, cmdText, commandParameters);
                    object val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
        }
        #endregion

        #region 获取指定表中指定字段的最大值
        /// <summary>
        /// 获取指定表中指定字段的最大值
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="field">字段</param>
        /// <returns>Return Type:Int</returns>
        public static int GetMaxID(string tableName, string field)
        {
            string s = "select Max({0}) from {1}";
            s = string.Format(s, field, tableName);
            int i = Convert.ToInt32(SqlHelper.ExecuteScalar(s) == DBNull.Value ? "0" : SqlHelper.ExecuteScalar(s));
            return i;
        }
        #endregion

        #region 建立SqlCommand
        /// <summary>
        /// 建立SqlCommand
        /// </summary>
        /// <param name="con">SqlConnection　对象</param>
        /// <param name="cmd">要建立的Command</param>
        /// <param name="cmdType">CommandType</param>
        /// <param name="cmdText">执行的SQL语句</param>
        /// <param name="cmdParms">参数</param>
        private static void PrepareCommand(SqlConnection con, SqlCommand cmd, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            cmd.Connection = con;
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            if (cmdParms != null)
            {
                foreach (SqlParameter para in cmdParms)
                {
                    cmd.Parameters.Add(para);
                }
            }
        }

        #endregion

        #region 更新某一字段的值

        /// <summary>
        /// 更新某表中某一字段的值
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="fieldName">更新的字段名</param>
        /// <param name="fieldValue">更新的字段的值</param>
        /// <param name="identityFieldName">标识字段的名称</param>
        /// <param name="identityValue">标识字段的值</param>
        public static void UpdateAfield(string tablename, string fieldName, string fieldValue, string identityFieldName, string identityValue)
        {
            string s = "Update {0} set {1}='{2}' where {3}='{4}'";
            s = string.Format(s, tablename, fieldName, fieldValue, identityFieldName, identityValue);
            ExecuteNonQuery(s);
        }

        #endregion

        #region 分页获取

        /// <summary>
        /// 分页获取数据列表 适用于SQL2000
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="key">主键</param>
        /// <param name="where">查询条件</param>
        /// <param name="pagesize">每页记录数</param>
        /// <param name="pageindex">页索引</param>
        /// <param name="orderfield">排序字段</param>
        /// <param name="ordertype">排序方式 1=ASC 0=DESC</param>
        /// <param name="fieldlist">查找的字段</param>
        /// <param name="recordcount">总记录数</param>
        /// <returns></returns>
        public static DataTable GetDataByPager2000(string tablename, string key, string where, int pagesize, int pageindex, string orderfield, int ordertype, string fieldlist, int recordcount)
        {
            string cmd = "ProcCustomPage";
            SqlParameter[] para = new SqlParameter[9];
            para[0] = new SqlParameter("@Table_Name", tablename);
            para[1] = new SqlParameter("@Sign_Record", key);
            para[2] = new SqlParameter("@Filter_Condition", where);
            para[3] = new SqlParameter("@Page_Size", pagesize);
            para[4] = new SqlParameter("@Page_Index", pageindex);
            para[5] = new SqlParameter("@TaxisField", orderfield);
            para[6] = new SqlParameter("@Taxis_Sign", ordertype);
            para[7] = new SqlParameter("@Find_RecordList", fieldlist);
            para[8] = new SqlParameter("@Record_Count", recordcount);

            return ExecuteDataSet(CommandType.StoredProcedure, cmd, para).Tables[0];

        }


        /// <summary>
        /// 分页获取数据列表 适用于SQL2005
        /// </summary>
        /// <param name="SelectList">选取字段列表</param>
        /// <param name="tablename">数据源名称表名或视图名称</param>
        /// <param name="where">筛选条件</param>
        /// <param name="OrderExpression">排序 必须指定一个排序字段</param>
        /// <param name="pageindex">页索引 从0开始</param>
        /// <param name="pagesize">每页记录数</param>
        /// <returns></returns>
        public static DataTable GetDataByPager2005(string SelectList, string tablename, string where, string OrderExpression, int pageindex, int pagesize)
        {
            string cmd = "GetRecordFromPage2005";
            SqlParameter[] para = new SqlParameter[6];
            para[0] = new SqlParameter("@SelectList", SelectList);
            para[1] = new SqlParameter("@TableSource", tablename);
            para[2] = new SqlParameter("@SearchCondition", where);
            para[3] = new SqlParameter("@OrderExpression", OrderExpression);
            para[4] = new SqlParameter("@pageindex", pageindex);
            para[5] = new SqlParameter("@pagesize", pagesize);

            return ExecuteDataSet(CommandType.StoredProcedure, cmd, para).Tables[0];
        }

        #endregion

        #region 获取某表中的总记录数

        /// <summary>
        /// 获取某表中的总记录数
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        public static int GetRecordCount(string tablename)
        {
            string s = "select count(*) from {0}";
            s = string.Format(s, tablename);
            return Convert.ToInt32(ExecuteScalar(s));
        }

        public static int GetRecordCount(string tablename, string where)
        {
            string s = "select count(*) from {0} where {1}";
            s = string.Format(s, tablename, where);
            return Convert.ToInt32(ExecuteScalar(s));
        }

        #endregion

        #region 根据条件获取指定表中的数据

        /// <summary>
        /// 根据条件获取指定表中的数据
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string tablename, string where)
        {
            string s = "select * from " + tablename;
            if (where != "")
            {
                s += " where " + where;
            }

            return SqlHelper.ExecuteDataSet(s).Tables[0];
        }


        #endregion

        #region 根据ID 获取一行数据

        /// <summary>
        /// 根据主键Id,获取一行数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="keyName">主键名称</param>
        /// <param name="value">值</param>
        /// <param name="msg">返回信息</param>
        /// <returns></returns>
        public static DataRow GetADataRow(string tableName, string keyName, string value, out string msg)
        {
            try
            {
                string s = "select * from {0} where {1}='{2}'";
                s = string.Format(s, tableName, keyName, value);
                DataTable dt = ExecuteDataSet(s).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    msg = "OK";
                    return dt.Rows[0];
                }
                else
                {
                    msg = "";
                    return null;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }

        #endregion

        #region 由Object取值
        /// <summary>
        /// 取得Int值,如果为Null 则返回０
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetInt(object obj)
        {
            if (obj.ToString() != "")
            {
                return int.Parse(obj.ToString());
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 取得byte值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte Getbyte(object obj)
        {
            if (obj.ToString() != "")
            {
                return byte.Parse(obj.ToString());
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获得Long值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long GetLong(object obj)
        {
            if (obj.ToString() != "")
            {
                return long.Parse(obj.ToString());
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 取得Decimal值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal GetDecimal(object obj)
        {
            if (obj.ToString() != "")
            {
                return decimal.Parse(obj.ToString());
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 取得Guid值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Guid GetGuid(object obj)
        {
            if (obj.ToString() != "")
            {
                return new Guid(obj.ToString());
            }
            else
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// 取得DateTime值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(object obj)
        {
            if (obj.ToString() != "")
            {
                return DateTime.Parse(obj.ToString());
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// 取得bool值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetBool(object obj)
        {
            if (obj.ToString() == "1" || obj.ToString().ToLower() == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 取得byte[]
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Byte[] GetByte(object obj)
        {
            if (obj.ToString() != "")
            {
                return (Byte[])obj;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 取得string值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetString(object obj)
        {
            if (obj != null && obj != DBNull.Value)
            {
                return obj.ToString();
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region 分页存储过程

        #region  sql 2000 分页存储过程
        /*
 * 
 * CREATE  PROCEDURE [dbo].[ProcCustomPage]
    (
        @Table_Name               varchar(5000),      	    --表名
        @Sign_Record              varchar(50),       		--主键
        @Filter_Condition         varchar(1000),     	    --筛选条件,不带where
        @Page_Size                int,               		--页大小
        @Page_Index               int,          			--页索引     			
        @TaxisField               varchar(1000),            --排序字段
        @Taxis_Sign               int,               		--排序方式 1为 DESC, 0为 ASC
        @Find_RecordList          varchar(1000),        	--查找的字段
        @Record_Count             int                		--总记录数
     )
     AS
        BEGIN 
        DECLARE  @Start_Number          int
        DECLARE  @End_Number            int
        DECLARE  @TopN_Number           int
     DECLARE  @sSQL                  varchar(8000)
             if(@Find_RecordList='')
             BEGIN
                  SELECT @Find_RecordList='*'
             END
     SELECT @Start_Number =(@Page_Index-1) * @Page_Size
        IF @Start_Number<=0
     SElECT @Start_Number=0
        SELECT @End_Number=@Start_Number+@Page_Size
        IF @End_Number>@Record_Count
     SELECT @End_Number=@Record_Count
     SELECT @TopN_Number=@End_Number-@Start_Number
     IF @TopN_Number<=0
     SELECT @TopN_Number=0
        print @TopN_Number
     print @Start_Number
     print @End_Number
     print @Record_Count
             IF @TaxisField=''
             begin
                select  @TaxisField=@Sign_Record
             end
     IF @Taxis_Sign=0
        BEGIN
            IF @Filter_Condition=''
             BEGIN
                SELECT @sSQL='SELECT '+@Find_RecordList+' FROM '+@Table_Name+' 
                    WHERE '+@Sign_Record+' in (SELECT TOP '+CAST(@TopN_Number AS VARCHAR(10))+' '+@Sign_Record+' FROM '+@Table_Name+' 
                     WHERE '+@Sign_Record+' in (SELECT TOP '+CAST(@End_Number AS VARCHAR(10))+' '+@Sign_Record+' FROM '+@Table_Name+' 
                 ORDER BY '+@TaxisField+') order by '+@TaxisField+' DESC)order by '+@TaxisField
            END
            ELSE
            BEGIN
            SELECT @sSQL='SELECT '+@Find_RecordList+' FROM '+@Table_Name+' 
         WHERE '+@Sign_Record+' in (SELECT TOP '+CAST(@TopN_Number AS VARCHAR(10))+' '+@Sign_Record+' FROM '+@Table_Name+' 
         WHERE '+@Sign_Record+' in (SELECT TOP '+CAST(@End_Number AS VARCHAR(10))+' '+@Sign_Record+' FROM '+@Table_Name+' 
         WHERE '+@Filter_Condition+' ORDER BY '+@TaxisField+') and '+@Filter_Condition+' order by '+@TaxisField+' DESC) and '+@Filter_Condition+' order by '+@TaxisField
             END
        END
    ELSE
        BEGIN
        IF @Filter_Condition=''
            BEGIN
                SELECT @sSQL='SELECT '+@Find_RecordList+' FROM '+@Table_Name+' 
             WHERE '+@Sign_Record+' in (SELECT TOP '+CAST(@TopN_Number AS VARCHAR(10))+' '+@Sign_Record+' FROM '+@Table_Name+' 
             WHERE '+@Sign_Record+' in (SELECT TOP '+CAST(@End_Number AS VARCHAR(10))+' '+@Sign_Record+' FROM '+@Table_Name+' 
             ORDER BY '+@TaxisField+' DESC) order by '+@TaxisField+')order by '+@TaxisField+' DESC'
         END
        ELSE
        BEGIN
            SELECT @sSQL='SELECT '+@Find_RecordList+' FROM '+@Table_Name+' 
         WHERE '+@Sign_Record+' in (SELECT TOP '+CAST(@TopN_Number AS VARCHAR(10))+' '+@Sign_Record+' FROM '+@Table_Name+' 
         WHERE '+@Sign_Record+' in (SELECT TOP '+CAST(@End_Number AS VARCHAR(10))+' '+@Sign_Record+' FROM '+@Table_Name+' 
         WHERE '+@Filter_Condition+' ORDER BY '+@TaxisField+' DESC) and '+@Filter_Condition+' order by '+@TaxisField+') and '+@Filter_Condition+' order by '+@TaxisField+' DESC'
     END
        END
        EXEC (@sSQL)
        IF @@ERROR<>0
        RETURN -3              
     RETURN 0
     END

     PRINT  @sSQL
    GO

 * */


        #endregion

        #region SQL2005 分页存储过程
        /*
 * 
CREATE PROCEDURE [dbo].[GetRecordFromPage] 
@SelectList            VARCHAR(2000),    --欲选择字段列表
@TableSource        VARCHAR(100),    --表名或视图表 
@SearchCondition    VARCHAR(2000),    --查询条件
@OrderExpression    VARCHAR(1000),    --排序表达式
@PageIndex            INT = 1,        --页号,从0开始
@PageSize            INT = 10        --页尺寸
AS 
BEGIN
IF @SelectList IS NULL OR LTRIM(RTRIM(@SelectList)) = ''
BEGIN
    SET @SelectList = '*'
END
PRINT @SelectList

SET @SearchCondition = ISNULL(@SearchCondition,'')
SET @SearchCondition = LTRIM(RTRIM(@SearchCondition))
IF @SearchCondition <> ''
BEGIN
    IF UPPER(SUBSTRING(@SearchCondition,1,5)) <> 'WHERE'
    BEGIN
        SET @SearchCondition = 'WHERE ' + @SearchCondition
    END
END
PRINT @SearchCondition

SET @OrderExpression = ISNULL(@OrderExpression,'')
SET @OrderExpression = LTRIM(RTRIM(@OrderExpression))
IF @OrderExpression <> ''
BEGIN
    IF UPPER(SUBSTRING(@OrderExpression,1,5)) <> 'WHERE'
    BEGIN
        SET @OrderExpression = 'ORDER BY ' + @OrderExpression
    END
END
PRINT @OrderExpression

IF @PageIndex IS NULL OR @PageIndex < 1
BEGIN
    SET @PageIndex = 1
END
PRINT @PageIndex
IF @PageSize IS NULL OR @PageSize < 1
BEGIN
    SET @PageSize = 10
END
PRINT  @PageSize

DECLARE @SqlQuery VARCHAR(4000)

SET @SqlQuery='SELECT '+@SelectList+',RowNumber 
FROM 
    (SELECT ' + @SelectList + ',ROW_NUMBER() OVER( '+ @OrderExpression +') AS RowNumber 
      FROM '+@TableSource+' '+ @SearchCondition +') AS RowNumberTableSource 
WHERE RowNumber BETWEEN ' + CAST(((@PageIndex - 1)* @PageSize+1) AS VARCHAR) 
+ ' AND ' + 
CAST((@PageIndex * @PageSize) AS VARCHAR) 
--    ORDER BY ' + @OrderExpression
PRINT @SqlQuery
SET NOCOUNT ON
EXECUTE(@SqlQuery)
SET NOCOUNT OFF

RETURN @@RowCount
END
 **/

        #endregion

        #endregion


    }
}
