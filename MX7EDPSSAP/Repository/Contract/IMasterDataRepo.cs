using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MX7EDPSSAP.Repository.Contract
{
    public interface IMasterDataRepo
    {
        // Generic Master Data related functions
        Task<IEnumerable<T>> GetMasterDataRecord<T>(string masterDataTable);
        Task<IEnumerable<T>> InsertDataRecord<T>(string xml,int userid);
        //Task<T> GetMasterDataRecordById<T>(string masterDataTable, decimal masterDataId);
        //Task<int> CheckMasterDataCodeExists(string masterDataTable, string masterDataCode);
        //Task<T> GetMasterDataRecordByCode<T>(string masterDataTable, string masterDataCode);
    }
}
