using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Xml;
using MX7EDPSSAP.Helper;
using MX7EDPSSAP.Helpers;
using MX7EDPSSAP.Infrastructure;
using MX7EDPSSAP.Model;
using MX7EDPSSAP.Repository.Contract;
using MX7EDPSSAP.Repository.DataModel;
using MX7EDPSSAP.Service.Contract;
using NetBarcode;
using Newtonsoft.Json;
using System;
using System.Collections;
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

                string csvPath = @"CSVFile\Chilled_Order_21052022_v1.csv";

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

                    List<dynamic> resultInJson2 = new List<dynamic>();
                    string resultInJson = JsonConvert.SerializeObject(dd);

                    resultList = await _iMasterDataRepo.InsertDataRecord<RawData>(resultInJson.ToString(), userid);

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

        public async Task<List<dynamic>> retreiveSOHImport()
        {
            try
            {
                List<dynamic> resultListInJson = new List<dynamic>();
                IEnumerable<dynamic> resultList = new List<dynamic>();
                IEnumerable<dynamic> dd = new List<dynamic>();
                var userid = 1;
                //var dt = new DataTable("dt");
                string csvPath = @"CSVFile\SOH1.csv";

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

                    dd = csv.GetRecords<SOHRawData>().ToList();

                    List<dynamic> resultInJson2 = new List<dynamic>();
                    string resultInJson = JsonConvert.SerializeObject(dd);

                    resultList = await _iMasterDataRepo.InsertSOHDataRecord<SOHRawData>(resultInJson.ToString(), userid);

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

        public async Task<List<dynamic>> exportPutData(string store_code)
        {
            try
            {
                List<dynamic> resultListInJson = new List<dynamic>();
                IEnumerable<dynamic> resultList = new List<dynamic>();
                IEnumerable<dynamic> dd = new List<dynamic>();
                var userid = 1;
                //var dt = new DataTable("dt");
                string csvPath = @"ExportCSVFile";
                var type = "PutData";
                Random rnd = new Random();
                int rnum = rnd.Next();
                resultList = await _iMasterDataRepo.exportPutData<PutData>(store_code, userid);

                if (!resultList.Any())
                {
                    return resultListInJson;
                }
                else
                {
                    //var firstOrDefault= resultList.Select(item => item.store_code).FirstOrDefault();

                    var csv_name = "vPutData_" + DateTime.Now.ToString("yyyyMMdd") + "_" + rnum + ".csv";
                    if (!Directory.Exists(csvPath))
                    {
                        Directory.CreateDirectory(csvPath);
                        csvPath = Path.Combine(csvPath, csv_name);
                    }
                    else
                    {
                        csvPath = Path.Combine(csvPath, csv_name);
                    }
                    using (var writer = new StreamWriter(csvPath))
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.WriteRecords(resultList);
                    }
                    if (File.Exists(csvPath))
                    {
                        resultList = await _iMasterDataRepo.updatepathPutData<PutData>(store_code, type, csv_name, userid);
                    }

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

        public async Task<List<dynamic>> exportPutDetailData(string store_code)
        {
            try
            {
                List<dynamic> resultListInJson = new List<dynamic>();
                IEnumerable<dynamic> resultList = new List<dynamic>();
                IEnumerable<dynamic> dd = new List<dynamic>();
                var userid = 1;
                //var dt = new DataTable("dt");
                string csvPath = @"ExportCSVFile";
                var type = "PutDetailData";
                Random rnd = new Random();
                int rnum = rnd.Next();
                resultList = await _iMasterDataRepo.exportPutDetailData<PutDetailData>(store_code, userid);

                if (!resultList.Any())
                {
                    return resultListInJson;
                }
                else
                {
                    // var firstOrDefault = resultList.Select(item => item.store_code).FirstOrDefault();

                    var csv_name = "vPutDetailData_" + DateTime.Now.ToString("yyyyMMdd") + "_" + rnum + ".csv";
                    if (!Directory.Exists(csvPath))
                    {
                        Directory.CreateDirectory(csvPath);
                        csvPath = Path.Combine(csvPath, csv_name);
                    }
                    else
                    {
                        csvPath = Path.Combine(csvPath, csv_name);
                    }
                    using (var writer = new StreamWriter(csvPath))
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.WriteRecords(resultList);
                    }
                    if (File.Exists(csvPath))
                    {
                        resultList = await _iMasterDataRepo.updatepathPutData<PutDetailData>(store_code, type, csv_name, userid);
                    }

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


        public async Task<dynamic> getCDO_data()
        {
            try
            {               
                IEnumerable<dynamic> resultList = new List<dynamic>();
                int userid =1;
                resultList = await _iMasterDataRepo.getCDOData<cdo>(userid);
                var data = CreateDataTable(resultList);
                var dhtml = HtmlmapperCDO(data);
                return dhtml;                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<dynamic> getCDM_data()
        {
            try
            {
                IEnumerable<dynamic> resultList = new List<dynamic>();
                int userid = 1;
                resultList = await _iMasterDataRepo.getCDMData<cdm>(userid);
                var data = CreateDataTable(resultList);
                var dhtml = HtmlmapperCDM(data);
                return dhtml;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public string HtmlmapperCDO(DataTable dt)
        {
            StringBuilder htmlStr = new StringBuilder("");
            var imgpath= Directory.GetCurrentDirectory() + "\\Images\\cdo.png";

            byte[] imageArray = System.IO.File.ReadAllBytes(imgpath);
            string base64ImageRepresentation = "data:image/png;base64," + Convert.ToBase64String(imageArray);
            

            var barcode = "data:image/png;base64,"+GenerateBacode(dt.Rows[0]["StoNo"].ToString());
            int distinctmilkcrate = dt.AsEnumerable().Select(r => r.Field<string>("MilkCrateid")).Distinct().Count();
            int TotalOrdPcs = dt.AsEnumerable().Where(
                row => row.Field<string>("Type")=="Milk").Sum(row => row.Field<int>("OrdPcs"));
            int TotalPutPcs = dt.AsEnumerable().Where(row => row.Field<string>("Type") == "Milk").Sum(row => row.Field<int>("PutPcs"));
            int TotalDiffPcs = dt.AsEnumerable().Where(row => row.Field<string>("Type") == "Milk").Sum(row => row.Field<int>("DiffPcs"));

            int distinctcrate = dt.AsEnumerable().Select(r => r.Field<string>("MilkCrateid")).Distinct().Count();
            int TotalCrateOrdPcs = dt.AsEnumerable().Where(row => row.Field<string>("Type") == "Crate").Sum(row => row.Field<int>("OrdPcs"));
            int TotalCratePutPcs = dt.AsEnumerable().Where(row => row.Field<string>("Type") == "Crate").Sum(row => row.Field<int>("PutPcs"));
            int TotalCrateDiffPcs = dt.AsEnumerable().Where(row => row.Field<string>("Type") == "Crate").Sum(row => row.Field<int>("DiffPcs"));

            htmlStr.Append("<table border='1' style=' border-collapse: collapse;text-align: center ' cellpadding='5'>");
            htmlStr.Append("<tr><td colspan='5' align='left'><img src="+ base64ImageRepresentation + " height='100' align='middle'>");
            htmlStr.Append("<span style=' font-size: 27px;  margin-left: 15px;  font-weight: 700; '>CDO (Chilled DC Delivery Order)</span>");   
            htmlStr.Append("</td><td colspan='3'><img src="+ barcode + " height='100'>");
            htmlStr.Append("</td></tr><tr><th align='left'>  Store No. : </th><td colspan='4'  align='left'>"+ dt.Rows[0]["StoreCode"].ToString() + "</td><th align='left'>  Order date : </th>");
            htmlStr.Append("<td colspan='2'  align='left'>" + dt.Rows[0]["OrderDate"].ToString() + "</td></tr><tr><th align='left'>  Route Code : </th><td colspan='4'  align='left'>" + dt.Rows[0]["RouteCode"].ToString() + "</td>");
            htmlStr.Append("<th align='left'> Pick Date: </th><td colspan='2'  align='left'>" + dt.Rows[0]["PickDate"].ToString() + "</td></tr><tr><th align='left'> Stop Code : </th><td colspan='4'  align='left'>" + dt.Rows[0]["StopCode"].ToString() + "</td>");
            htmlStr.Append("<th align='left'>  STO NO : </th><td colspan='2'  align='left'>" + dt.Rows[0]["StoNo"].ToString() + "</td></tr>");
            htmlStr.Append("<tr><th align='left'>Perishable</th><th colspan='7'>(WAJIB memulangkan kesemua Milk Crate & Food Tray di penghantaran berikutnya)</th></tr>");
            htmlStr.Append("<tr><th>No</th><th> Milk Crate ID</th><th>Seal ID</th><th>SKU</th><th style='width:300px'>Description</th><th>Ord (pcs)</th><th>Put (pcs)</th><th>Diff (pcs)</th></tr>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Type"].ToString() == "Milk")
                {
                    var count = 0;
                    htmlStr.Append("<tr><td>" + count + "</td><td>" + dt.Rows[i]["MilkCrateid"].ToString() + "</td><td>" + dt.Rows[i]["Sealid"].ToString() + "</td><td>" + dt.Rows[i]["Sku"].ToString() + "</td><td>" + dt.Rows[i]["Descrip"].ToString() + "</td><td>" + dt.Rows[i]["OrdPcs"].ToString() + "</td><td>" + dt.Rows[i]["PutPcs"].ToString() + "</td><td>" + dt.Rows[i]["DiffPcs"].ToString() + "</td></tr>");
                    count = count + 1;
                }
            }
           
            //htmlStr.Append("<tr><td>2</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
            htmlStr.Append("<tr><th></th><th>"+distinctmilkcrate+"</th><th>Totes</th><th></th><th align='left'>Total for PERISHABLE </th><th>"+ TotalOrdPcs + "</th><th>"+ TotalPutPcs + "</th><th>"+ TotalDiffPcs + "</th></tr>");            
            htmlStr.Append("<tr><th align='left'>Fresh Food & Cheese</th><th colspan='7'></th></tr>");
            htmlStr.Append("<tr><th>No</th><th> Food Tray ID</th><th>Seal ID</th><th>SKU</th><th style='width:300px'>Description</th><th>Ord (pcs)</th><th>Put (pcs)</th><th>Diff (pcs)</th></tr>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Type"].ToString() == "Crate")
                {
                    var count = 0;
                    htmlStr.Append("<tr><td>" + count + "</td><td>" + dt.Rows[i]["MilkCrateid"].ToString() + "</td><td>" + dt.Rows[i]["Sealid"].ToString() + "</td><td>" + dt.Rows[i]["Sku"].ToString() + "</td><td>" + dt.Rows[i]["Descrip"].ToString() + "</td><td>" + dt.Rows[i]["OrdPcs"].ToString() + "</td><td>" + dt.Rows[i]["PutPcs"].ToString() + "</td><td>" + dt.Rows[i]["DiffPcs"].ToString() + "</td></tr>");
                    count = count + 1;
                }
            }

            //htmlStr.Append("<tr><td>2</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
            htmlStr.Append("<tr><th></th><th>" + distinctcrate + "</th><th>Totes</th><th></th><th align='left'>Total for Fresh Food & Cheese </th><th>" + TotalCrateOrdPcs + "</th><th>" + TotalCratePutPcs + "</th><th>" + TotalCrateDiffPcs + "</th></tr></table>");

            return htmlStr.ToString();

    }

        public string HtmlmapperCDM(DataTable dt)
        {
            StringBuilder htmlStr = new StringBuilder("");
            var imgpath = Directory.GetCurrentDirectory() + "\\Images\\cdo.png";
            byte[] imageArray = System.IO.File.ReadAllBytes(imgpath);
            string base64ImageRepresentation = "data:image/png;base64," + Convert.ToBase64String(imageArray);
                       
            int TotalTrayA = dt.AsEnumerable().Sum(row => row.Field<int>("TrayA"));
            int TotalTrayB = dt.AsEnumerable().Sum(row => row.Field<int>("TrayB"));
            int TotalTrayC = dt.AsEnumerable().Sum(row => row.Field<int>("TrayC"));
            int TotalTrayD = dt.AsEnumerable().Sum(row => row.Field<int>("TrayD"));
            int TotalTrayE = dt.AsEnumerable().Sum(row => row.Field<int>("TrayE"));
            int TotalTrayF = dt.AsEnumerable().Sum(row => row.Field<int>("TrayF"));
            int TotalTrayG = dt.AsEnumerable().Sum(row => row.Field<int>("TrayG"));

            htmlStr.Append("<table><tr><td valign='top'><table border='1' style=' border-collapse: collapse;text-align: center ' cellpadding='5'>");
            htmlStr.Append("<tbody><tr><td colspan='4' align='left'>chilled DC Delivery mainfest</td><td colspan='10' align='left'>Pick/put Date : "+ dt.Rows[0]["PickDate"].ToString() + "</td>");
            htmlStr.Append("<td colspan='2' align='left'>Loading time  :</td><td colspan='4' rowspan='4' align='left'><img src="+ base64ImageRepresentation + " height='100'></td>");
            htmlStr.Append("<td colspan='4' rowspan='4' style=' font-size: 36px; font-weight: 800;  background: #ededed;'> DM <div style=' font-size: 10px;  font-weight: 300 '>Deleivery mainfest</div></td>");
            htmlStr.Append("</tr><tr><td rowspan='2' style=' background: #ededed; color: blue; '> MWF</td><td rowspan='2' colspan='3' style='  background: #ededed; font-weight: 600; font-size: 20px; color: blue; '> "+ dt.Rows[0]["RouteCode"].ToString() + "</td>");
            htmlStr.Append("<td colspan='10' align='left'>Day : "+ dt.Rows[0]["PickDay"].ToString() + "</td><td colspan='2' align='left'>Department Time : </td></tr><tr>");
            htmlStr.Append("<td colspan='10' align='left'>Arrival time : </td><td colspan='2' align='left'>Retrun  Time to CDC : </td></tr>");
            htmlStr.Append("<tr><td colspan='4' align='left' style='color: blue;'>Printed: "+ dt.Rows[0]["PrintDate"].ToString() + " </td><td colspan='10' align='left'>Resend Loading Date </td></tr>");
            htmlStr.Append("<tr><th rowspan='2' style='writing-mode: vertical-lr;background: #ededed;'>STOP No.</th>");
            htmlStr.Append("<th rowspan='2' style='writing-mode: vertical-lr;background: #ededed;'>Store no.</th>");
            htmlStr.Append("<th rowspan='2' style='background: #ededed; '>Store Name</th>");
            htmlStr.Append("<th rowspan='2' style='background: #ededed;'>To  Number Barcode</th>");
            htmlStr.Append("<th rowspan='2' style='writing-mode: vertical-lr;background: #ededed;'>Time in <div>(hh:mm)</div></th>");
            htmlStr.Append("<th rowspan='2' style='writing-mode: vertical-lr;background: #ededed;'>Tray A</th>");
            htmlStr.Append("<th rowspan='2' style='writing-mode: vertical-lr;background: #ededed;'>Tray B</th>");
            htmlStr.Append("<th rowspan='2' style='writing-mode: vertical-lr;background: #ededed;'>Tray C</th>");
            htmlStr.Append("<th rowspan='2' style='writing-mode: vertical-lr;background: #ededed;'>Tray D</th>");
            htmlStr.Append("<th rowspan='2' style='writing-mode: vertical-lr;background: #ededed;'>Tray E</th>");
            htmlStr.Append("<th rowspan='2' style='writing-mode: vertical-lr;background: #ededed;'>Tray F</th>");
            htmlStr.Append("<th rowspan='2' style='writing-mode: vertical-lr;background: #ededed;'>Tray G</th>");
            htmlStr.Append("<th rowspan='2' colspan='2' style='width: 86px;background: #ededed;'>All Qty Delivered in Good Conditions? (Circle)</th>");
            htmlStr.Append("<th rowspan='2' colspan='2' style='width: 447px;background: #ededed;'>Recieved by store</th>");
            htmlStr.Append("<th colspan='7' style='background: #ededed;'>Return</th></tr><tr>");
            htmlStr.Append("<th style='writing-mode: vertical-lr;background: #ededed;'>Tray A</th>");
            htmlStr.Append("<th style='writing-mode: vertical-lr;background: #ededed;'>Tray B</th>");
            htmlStr.Append("<th style='writing-mode: vertical-lr;background: #ededed;'>Tray C</th>");
            htmlStr.Append("<th style='writing-mode: vertical-lr;background: #ededed;'>Tray D</th>");
            htmlStr.Append("<th style='writing-mode: vertical-lr;background: #ededed;'>Tray E</th>");
            htmlStr.Append("<th style='writing-mode: vertical-lr;background: #ededed;'>Tray F</th>");
            htmlStr.Append("<th style='writing-mode: vertical-lr;background: #ededed;'>Tray G</th></tr><!-- body-->");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var barcode = "data:image/png;base64," + GenerateBacode(dt.Rows[i]["StoNo"].ToString());

                htmlStr.Append("<tr><td rowspan='8' style='background: #ededed;'>"+dt.Rows[i]["StopCode"].ToString()+"</td>");
                htmlStr.Append("<td rowspan='8'>" + dt.Rows[i]["StoNo"].ToString() + "</td><td rowspan='8'>" + dt.Rows[i]["StoreCode"].ToString() + "</td><td rowspan='8'><img src="+barcode+" height='60'></td>");
                htmlStr.Append("<td rowspan='8'>-</td><td rowspan='8'>" + dt.Rows[i]["TrayA"].ToString() + "</td><td rowspan='4'>" + dt.Rows[i]["TrayB"].ToString() + "</td><td rowspan='4'>" + dt.Rows[i]["TrayC"].ToString() + "</td><td rowspan='4'>" + dt.Rows[i]["TrayD"].ToString() + "</td>");
                htmlStr.Append("<td rowspan='4'>" + dt.Rows[i]["TrayE"].ToString() + "</td><td rowspan='4'>" + dt.Rows[i]["TrayF"].ToString() + "</td><td rowspan='4'>" + dt.Rows[i]["TrayG"].ToString() + "</td><td rowspan='8'>YES</td><td rowspan='8'>NO</td>");
                htmlStr.Append("<td style='text-align: left;height: 131px;' valign='top'>Name :</td><td style='  text-align: left; ' valign='top'>IC#:</td>");
                htmlStr.Append("<td rowspan='4'>1</td><td rowspan='4'>1</td><td rowspan='4'>1</td><td rowspan='4'>1</td>");
                htmlStr.Append("<td rowspan='4'>1</td><td rowspan='4'>1</td><td rowspan='4'>1</td></tr><tr><td rowspan='7' style=' text-align: left; ' valign='top'>Remark</td>");
                htmlStr.Append("<td rowspan='6' style='text-align: left;height: 100px;' valign='top'> Chop &amp; Sign</td></tr><tr></tr>");
                htmlStr.Append("<tr></tr><tr><td rowspan='4'>--</td><td rowspan='4'>--</td><td rowspan='4'>--</td><td rowspan='4'>--</td><td rowspan='4'>--</td>");
                htmlStr.Append("<td rowspan='4'>--</td><td rowspan='4'>--</td><td rowspan='4'>--</td><td rowspan='4'>--</td><td rowspan='4'>--</td>");
                htmlStr.Append("<td rowspan='4'>--</td><td rowspan='4'>--</td><td rowspan='4'>--</td></tr><tr></tr><tr></tr><tr><td style='text-align: left; '>Date</td></tr>");
            }
            htmlStr.Append("<tr><td colspan='5' align='right'>Total</td><td>" + TotalTrayA + "</td><td>" + TotalTrayB + "</td><td>" + TotalTrayC + "</td><td>" + TotalTrayD + "</td><td>" + TotalTrayE + "</td><td>" + TotalTrayF + "</td><td>" + TotalTrayG+ "</td>");
            htmlStr.Append("<td></td><td> </td><td> </td><td> </td><td>1</td><td>2</td><td>2</td><td>2</td><td>2</td><td>2</td><td>2</td></tr>");
            
            htmlStr.Append("</tbody></table></td><td valign='top' style='width:100px;'><table style='width:100%;  border-collapse: collapse;  text-align: center;margin-bottom:10px' border='1' cellpadding='5'>");
           
            htmlStr.Append("<tbody><tr><th style='background: #ededed;'>Hot Lines Chilled DC</th></tr><tr><td>TBA</td></tr><tr>");
            htmlStr.Append("<td>TBA</td></tr><tr><td>TBA</td></tr></tbody></table>");
            htmlStr.Append("<table style='border-collapse: collapse;  text-align: center;' border='1' cellpadding='5'><tr >");
            htmlStr.Append("<td colspan='5'><i> Aduan permasalahan mengenai penghantaran ini bolehlah email kepada:<a href=''>chilled.dc@7eleven.com.my</a></i>");
            htmlStr.Append("</td></tr><tr><th align='left' style='background:#ededed'>Transport ID:</th><td colspan='3'> </td></tr>");
            htmlStr.Append("<tr><th align='left' style='background:#ededed'>Lorry No.:</th><td colspan='3'> </td></tr><tr>");
            htmlStr.Append("<th style='background:#ededed'> Start Odometer : </th><td colspan='3'> </td></tr><tr>");
            htmlStr.Append("<th style='background:#ededed' align='left'>End Odometer:</th><td colspan='3'> </td>");
            htmlStr.Append("</tr><tr><th style='background:#ededed' align='left'>Side Door Serial No: </th>");
            htmlStr.Append("<td colspan='3'></td></tr><tr><th style='background:#ededed' align='left'>Lorry Driver Name:</th>");
            htmlStr.Append("<td colspan='3'></td></tr><tr><th style='background:#ededed' align='left'>I/C Number: </th>");
            htmlStr.Append("<td colspan='3'></td></tr><tr><th style='background:#ededed' align='left'>Lorry Driver Phone Number  : </th>");
            htmlStr.Append("<td colspan='3'> </td></tr><tr style='background:#ededed'><th align='left'>RETURN</th>");
            htmlStr.Append("<th align='left'>Qty</th><th align='left'>Short</th><th align='left'>Extra</th></tr><tr>");
            htmlStr.Append("<th align='left' style='background:#ededed'>Tray A</th><td></td><td></td><td></td></tr>");
            htmlStr.Append("<tr><th align='left' style='background:#ededed'>Tray B</th><td></td><td></td><td></td></tr>");
            htmlStr.Append("<tr><th align='left' style='background:#ededed'>Tray C</th><td></td><td></td><td></td></tr>");
            htmlStr.Append("<tr><th align='left' style='background:#ededed'>Tray D</th><td></td><td></td><td></td></tr>");
            htmlStr.Append("<tr><th align='left' style='background:#ededed'>Tray E</th><td></td><td></td><td></td></tr>");
            htmlStr.Append("<tr><th align='left' style='background:#ededed'>Tray F</th><td></td><td></td><td></td></tr>");
            htmlStr.Append("<tr><th align='left' style='background:#ededed'>Tray G</th><td></td><td></td><td></td></tr><tr>");
            htmlStr.Append("<th colspan='5' style='height:100px' valign='top'>  Received By (Chilled DC) Chop </th></tr><tr><th colspan='5'   align='left'> Name & IC# :   </th>");
            htmlStr.Append("</tr><tr><th colspan='5'   align='left'>Date :   </th></tr></table></td></tr></table>");
            return htmlStr.ToString();

        }
        private string GenerateBacode(string _data)
        {
            
            var barcode = new Barcode(_data, NetBarcode.Type.Code128, true);
            //var value = barcode.SaveImageFile("./path", ImageFormat.Png); // formats: Bmp, Gif, Jpeg, Png...
            var value1 = barcode.GetBase64Image();
            //var image = barcode.GetImage();
            return value1;
        }
        public static DataTable CreateDataTable(IEnumerable source)
        {
            var table = new DataTable();
            int index = 0;
            var properties = new List<PropertyInfo>();
            foreach (var obj in source)
            {
                if (index == 0)
                {
                    foreach (var property in obj.GetType().GetProperties())
                    {
                        if (Nullable.GetUnderlyingType(property.PropertyType) != null)
                        {
                            continue;
                        }
                        properties.Add(property);
                        table.Columns.Add(new DataColumn(property.Name, property.PropertyType));
                    }
                }
                object[] values = new object[properties.Count];
                for (int i = 0; i < properties.Count; i++)
                {
                    values[i] = properties[i].GetValue(obj);
                }
                table.Rows.Add(values);
                index++;
            }
            return table;
        }

        
    }
}
