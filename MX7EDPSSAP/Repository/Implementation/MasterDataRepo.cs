using MX7EDPSSAP.Repository.Base;
using MX7EDPSSAP.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MX7EDPSSAP.Repository.Implementation
{
    public class MasterDataRepo : BaseDAORepo, IMasterDataRepo
    {
        public MasterDataRepo()
        {
        }

        public async Task<IEnumerable<T>> GetMasterDataRecord<T>(string masterDataTable)
        {
            string commandName = "spGetMasterDataRecords";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Table_Name", masterDataTable),
                new SqlParameter("@IsActive", 1)
            };

            return await ExecuteSelectCommandWithStoredProd<T>(commandName, parameters);
        }

        public async Task<IEnumerable<T>> InsertDataRecord<T>(string xml,int userid)
        {
            string commandName = "spInsertRawData";
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("@data",xml),
               new SqlParameter("@userid",userid)
            };

            return await ExecuteSelectCommandWithStoredProd<T>(commandName, parameters);
        }

        public async Task<IEnumerable<T>> InsertSOHDataRecord<T>(string json, int userid)
        {
            string commandName = "spInsertSOHRawData";
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("@data",json),
               new SqlParameter("@userid",userid)
            };

            return await ExecuteSelectCommandWithStoredProd<T>(commandName, parameters);
        }

        public async Task<IEnumerable<T>> exportPutData<T>(string store_code, int userid)
        {
            string commandName = "spGetPutData";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@@data",store_code),
               new SqlParameter("@userid",userid)
            };

            return await ExecuteSelectCommandWithStoredProd<T>(commandName, parameters);
        }
        public async Task<IEnumerable<T>> exportPutDetailData<T>(string store_code, int userid)
        {
            string commandName = "spGetPutDetailData";
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("@@data",store_code),
               new SqlParameter("@userid",userid)
            };

            return await ExecuteSelectCommandWithStoredProd<T>(commandName, parameters);
        }
        public async Task<IEnumerable<T>> updatepathPutData<T>(string store_code,string type,string path,int userid)
        {
            string commandName = "spUpdatePutData";
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("@store_code",store_code),
               new SqlParameter("@type",type),
               new SqlParameter("@path",path),
               new SqlParameter("@userid",userid)
            };

            return await ExecuteSelectCommandWithStoredProd<T>(commandName, parameters);
        }
      

        public async Task<IEnumerable<T>> getCDOData<T>(int userid)
        {

            string commandName = "spGetCDO_DATA";
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("@userid",userid)
            };
            return await ExecuteSelectCommandWithStoredProd<T>(commandName, parameters);
        }
        public async Task<IEnumerable<T>> getCDMData<T>(int userid)
        {
            string commandName = "spGetCDM_DATA";
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("@userid",userid)
            };
            return await ExecuteSelectCommandWithStoredProd<T>(commandName, parameters);
        }

        public async Task<IEnumerable<T>> updatefilepathCDM_CDO<T>(string routecode,string filepath, string type, int userid)
        {
            string commandName = "spupdatefilepathCDM_CDO";
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("@userid",userid)
              ,new SqlParameter("@routecode",routecode)
              ,new SqlParameter("@filepath",filepath)
              ,new SqlParameter("@type",type)
            };
            return await ExecuteSelectCommandWithStoredProd<T>(commandName, parameters);
        }

    }
}
