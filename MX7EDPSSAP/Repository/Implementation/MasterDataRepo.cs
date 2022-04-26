﻿using MX7EDPSSAP.Repository.Base;
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

    }
}