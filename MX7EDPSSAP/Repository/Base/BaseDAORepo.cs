using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MX7EDPSSAP.Repository.Base
{
    public class BaseDAORepo
    {
        private readonly ILogger<BaseDAORepo> _logger;
        private readonly static string _connectionString = "Data Source=DESKTOP-O8L3GI3;Integrated Security=true;Initial Catalog=MX7EDPS_DEV;";
        //"Data Source=AD-A700;Initial Catalog=MX7EDPS_DEV;User ID=sa;Password=sa@1234";
        private readonly static int _commandTimeOut = 180;

        public BaseDAORepo()
        {

        }

        public BaseDAORepo(ILogger<BaseDAORepo> logger)
        {
            this._logger = logger;
        }

        public virtual async Task<IEnumerable<T>> ExecuteSelectCommandWithStoredProd<T>(string commandName, SqlParameter[] param)
        {
            using (SqlConnection conn = OpenDatabaseConnection())
            {
                DynamicParameters args = ConvertSqlParamToDapperParam(param);
                IEnumerable<T> result = await conn.QueryAsync<T>(commandName, args, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeOut);
                return result;
            }
        }

        public virtual async Task<int> ExecuteSelectRecordCountCommandWithStoredProd(string commandName, SqlParameter[] param)
        {
            using (SqlConnection conn = OpenDatabaseConnection())
            {
                DynamicParameters args = ConvertSqlParamToDapperParam(param);
                var result = await conn.QueryAsync<int>(commandName, args, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeOut);
                return result.FirstOrDefault();
            }
        }

        public virtual async Task<int> ExecuteInsertRecordReturnNewIdWithStoredProd(string commandName, SqlParameter[] param)
        {
            using (SqlConnection conn = OpenDatabaseConnection())
            {
                DynamicParameters args = ConvertSqlParamToDapperParam(param);
                var createdId = await conn.ExecuteScalarAsync<int>(commandName, args, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeOut);
                return createdId;
            }
        }

        protected SqlConnection OpenDatabaseConnection()
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            return conn;
        }

        protected DynamicParameters ConvertSqlParamToDapperParam(SqlParameter[] param)
        {
            DynamicParameters args = new DynamicParameters();
            param.ToList().ForEach(p => args.Add(p.ParameterName, p.Value));

            return args;
        }
    }
}
