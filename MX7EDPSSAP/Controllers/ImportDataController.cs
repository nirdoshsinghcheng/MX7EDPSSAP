using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Data;
using MX7EDPSSAP.Repository.Base;
using MX7EDPSSAP.Repository.Contract;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.AspNetCore.Hosting.Server;
using Newtonsoft.Json;
using MX7EDPSSAP.Helper;
using MX7EDPSSAP.Model;
using CsvHelper;
using System.Text;
using System.Globalization;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using MX7EDPSSAP.Service.Contract;
using Microsoft.Extensions.Logging;
using MX7EDPSSAP.Repository.Implementation;

namespace MX7EDPSSAP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportDataController : ControllerBase
    {
        SqlParameter[] _parameters = null;
        DataTable dt = new DataTable();
        private readonly ILogger<ImportDataController> _logger;
        private readonly IMasterSvc _masterSvc;
        public ImportDataController(ILogger<ImportDataController> logger, IMasterSvc masterSvc)
        {
            _logger = logger;
            _masterSvc = masterSvc;
        }

        

    }
}
