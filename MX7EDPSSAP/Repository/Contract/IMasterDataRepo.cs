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
        Task<IEnumerable<T>> InsertDataRecord<T>(string json,int userid);
        Task<IEnumerable<T>> InsertSOHDataRecord<T>(string json, int userid);
        Task<IEnumerable<T>> exportPutData<T>(string store_code,int userid);
        Task<IEnumerable<T>> exportPutDetailData<T>(string store_code,int userid);
        Task<IEnumerable<T>> updatepathPutData<T>(string store_code, string type, string path, int userid);
        Task<IEnumerable<T>> getCDOData<T>(int userid);
        Task<IEnumerable<T>> getCDMData<T>(int userid);
        Task<IEnumerable<T>> updatefilepathCDM_CDO<T>(string routecode,string filepath,string type, int userid);
        
        //Task<T> GetMasterDataRecordById<T>(string masterDataTable, decimal masterDataId);
        //Task<int> CheckMasterDataCodeExists(string masterDataTable, string masterDataCode);
        //Task<T> GetMasterDataRecordByCode<T>(string masterDataTable, string masterDataCode);
    }
}
