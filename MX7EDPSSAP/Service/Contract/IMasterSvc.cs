using System;
using MX7EDPSSAP.Application.Models.Request;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MX7EDPSSAP.Application.Constants.MasterDataEnum;
using MX7EDPSSAP.Model;

namespace MX7EDPSSAP.Service.Contract
{
    public interface IMasterSvc
    {
        Task<List<dynamic>> GetMasterDataRecordAsync(MasterDataTableName masterDataType);
        Task<List<dynamic>> retreiveImport();
    }
}
