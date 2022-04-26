using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Xml;
using MX7EDPSSAP.Helper;
using MX7EDPSSAP.Infrastructure;
using MX7EDPSSAP.Model;
using MX7EDPSSAP.Repository.Contract;
using MX7EDPSSAP.Repository.DataModel;
using MX7EDPSSAP.Service.Contract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using static MX7EDPSSAP.Application.Constants.MasterDataEnum;

namespace MX7EDPSSAP.Service.Implementation
{
    public class MasterSvc : IMasterSvc
    {
        private readonly ILogger<MasterSvc> _logger;
        private readonly IMasterDataRepo _iMasterDataRepo;

        public MasterSvc(ILogger<MasterSvc> logger, IMasterDataRepo iMasterDataRepo)
        {
            _logger = logger;
            _iMasterDataRepo = iMasterDataRepo;
        }
        public async Task<List<dynamic>> GetMasterDataRecordAsync(MasterDataTableName masterDataType)
        {
            try
            {
                List<dynamic> resultListInJson = new List<dynamic>();
                IEnumerable<dynamic> resultList = new List<dynamic>();

                // Retrieve data from db according to master data type
                switch (masterDataType)
                {
                    case MasterDataTableName.Aisle:
                        resultList = await _iMasterDataRepo.GetMasterDataRecord<mdoAisleModel>(masterDataType.ToString());
                        break;

                        #region MyRegion
                        //case MasterDataTableName.ContainerType:
                        //    resultList = await _iMasterDataRepo.GetMasterDataRecord<mdoContainerTypeModel>(masterDataType.ToString());
                        //    break;

                        //case MasterDataTableName.Dolly:
                        //    resultList = await _iMasterDataRepo.GetMasterDataRecord<mdoDollyModel>(masterDataType.ToString());
                        //    break;

                        //case MasterDataTableName.Location:
                        //    resultList = await _iMasterDataRepo.GetMasterDataRecord<mdoLocationModel>(masterDataType.ToString());
                        //    break;

                        //case MasterDataTableName.ProductType:
                        //    resultList = await _iMasterDataRepo.GetMasterDataRecord<mdoProductTypeModel>(masterDataType.ToString());
                        //    break;

                        //case MasterDataTableName.Role:
                        //    resultList = await _iMasterDataRepo.GetMasterDataRecord<mdoRoleModel>(masterDataType.ToString());
                        //    break;

                        //case MasterDataTableName.Route:
                        //    resultList = await _iMasterDataRepo.GetMasterDataRecord<mdoRouteModel>(masterDataType.ToString());
                        //    break;

                        //case MasterDataTableName.Sku:
                        //    resultList = await _iMasterDataRepo.GetMasterDataRecord<mdoSkuModel>(masterDataType.ToString());
                        //    break;

                        //case MasterDataTableName.SkuUom:
                        //    resultList = await _iMasterDataRepo.GetMasterDataRecord<mdoSkuUomModel>(masterDataType.ToString());
                        //    break;

                        //case MasterDataTableName.Status:
                        //    resultList = await _iMasterDataRepo.GetMasterDataRecord<mdoStatusModel>(masterDataType.ToString());
                        //    break;

                        //case MasterDataTableName.Store:
                        //    resultList = await _iMasterDataRepo.GetMasterDataRecord<mdoStoreModel>(masterDataType.ToString());
                        //    break;

                        //case MasterDataTableName.Tote:
                        //    resultList = await _iMasterDataRepo.GetMasterDataRecord<mdoToteModel>(masterDataType.ToString());
                        //    break;

                        //case MasterDataTableName.ToteType:
                        //    resultList = await _iMasterDataRepo.GetMasterDataRecord<mdoToteTypeModel>(masterDataType.ToString());
                        //    break;

                        //case MasterDataTableName.Zone:
                        //    resultList = await _iMasterDataRepo.GetMasterDataRecord<mdoZoneModel>(masterDataType.ToString());
                        //    break;

                        //case MasterDataTableName.User:
                        //case MasterDataTableName.UserRole:
                        //    throw new NotImplementedException("Not implemented yet");
                        //default:
                        //    break; 
                        #endregion
                }

                if (!resultList.Any())
                {
                    return resultListInJson;
                }

                foreach (var res in resultList)
                {
                    // Convert retrieved results to JSON string with customized case resolver
                    string resultInJson = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CustomCamelCaseResolver() });
                    var finalResult = JsonConvert.DeserializeObject(resultInJson);
                    resultListInJson.Add(finalResult);
                }
                return resultListInJson;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<dynamic>> retreiveImport()
        {
            try
            {
                List<dynamic> resultListInJson = new List<dynamic>();
                IEnumerable<dynamic> resultList = new List<dynamic>();
                IEnumerable<dynamic> dd = new List<dynamic>();
                var userid = 1;
                //var dt = new DataTable("dt");
                string csvPath = @"CSVFile\OrderData_711_Store1.csv";
                //string csvPath = "https://drive.google.com/file/d/1WVnYWBcOyhznlAP1gu1yL5sUT87rWMXP/view?usp=sharing";

                var ss = GetStreamFromUrl(csvPath);

                var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Encoding = Encoding.UTF8, // Our file uses UTF-8 encoding
                    HasHeaderRecord = true,
                    HeaderValidated = null,
                    MissingFieldFound = null,
                    IgnoreBlankLines = true,
                    TrimOptions = TrimOptions.Trim
                };
                using (var reader = new StreamReader(ss))                    
                using (var csv = new CsvReader(reader, configuration))
                {

                    dd = csv.GetRecords<RawData>().ToList();
                    var xml = Serializer(typeof(List<RawData>), dd);

                    //var dt = ToDataTable(dd);
                    //using (var dr = new CsvDataReader(csv))
                    //{
                    //   dt.Columns.Add("Id", typeof(int));
                    //   dt.Columns.Add("Name", typeof(string));
                    //    dt.Load(dr);
                    //}

                    //string xml = null;

                    //xml = JsonHelper.ConvertDatatableToXML(dt);
                    resultList = await _iMasterDataRepo.InsertDataRecord<RawData>(xml, userid);


                }
                if (!resultList.Any())
                {
                    return resultListInJson;
                }

                foreach (var res in resultList)
                {
                    // Convert retrieved results to JSON string with customized case resolver
                    string resultInJson = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CustomCamelCaseResolver() });
                    var finalResult = JsonConvert.DeserializeObject(resultInJson);
                    resultListInJson.Add(finalResult);
                }
                return resultListInJson;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private static Stream GetStreamFromUrl(string url)
        {
            byte[] imageData = null;

            using (var wc = new System.Net.WebClient())
                imageData = wc.DownloadData(url);

            return new MemoryStream(imageData);
        }
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        public static string Serializer(Type type, object obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                //Serialized object
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }

    }
}
