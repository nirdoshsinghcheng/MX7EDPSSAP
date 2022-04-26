
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
//using System.Web.Script.Serialization;

namespace MX7EDPSSAP.Helper
{
    public static class JsonHelper
    {
        public static dynamic deserializeDynamicObject(string json)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            dynamic d = jss.Deserialize<dynamic>(json);
            return d;
        }

        public static Dictionary<string, string> deserializeKeyValuePairList(string Strjson)
        {
            //var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //var dict = serializer.Deserialize<Dictionary<string, string>>(Strjson);
            //return dict;
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(Strjson);
            return dict;
        }


        public static string ConvertDatatableToXML(DataTable dt)
        {
            MemoryStream str = new MemoryStream();
            dt.WriteXml(str, true);
            str.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(str);
            string xmlstr;
            xmlstr = sr.ReadToEnd();
            return (xmlstr);
        }

       


        public static string ConvertDataTableToJsonString(DataTable dt, int dateformat = 0)
        {
            //JsonSerializerSettings jSettings = new JsonSerializerSettings()
            //{
            //    ContractResolver = new LowercaseContractResolver()
            //};
            ////jSettings.Converters.Add(new MyDateTimeConvertor());
            //var jsondataLimit = JsonConvert.SerializeObject(dt, jSettings);

            //int d = jsondataLimit.Length;
            //if (d > 1800000)
            //{
            //    DataTable dtPage = dt.AsEnumerable().Take(dt.Rows.Count - 100).CopyToDataTable();
            //    jsondataLimit = ConvertDataTableToJsonString(dtPage);
            //}

            JsonSerializerSettings settings = new JsonSerializerSettings();
            if (dateformat == 0)
            {
                settings.Converters.Add(new IsoDateTimeConverter
                {
                    DateTimeFormat = "dd-MMM-yyyy",
                });
            }

            else
            {
                // settings.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = "dd-MMM-yyyy hh:mm tt" });
                settings.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = "dd-MMM-yyyy HH:mm:ss tt" });
            }
            settings.ContractResolver = new LowercaseContractResolver();
            var jsondataLimit = JsonConvert.SerializeObject(dt, settings);

            return jsondataLimit;
        }
        public static string ConvertArrayListToJsonString(ArrayList dt)
        {
            var jsondataLimit = JsonConvert.SerializeObject(dt, new JsonSerializerSettings()
            {
                ContractResolver = new LowercaseContractResolver()
            });
            return jsondataLimit;
        }
        public static string ConvertDataSetToJsonString(DataSet ds, int dateformat = 0)
        {
            JsonSerializerSettings jSettings = new JsonSerializerSettings();
            if (dateformat == 0)
            {
                jSettings.Converters.Add(new IsoDateTimeConverter
                {
                    DateTimeFormat = "dd-MMM-yyyy",
                });
            }
            else
            {
                jSettings.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = "dd-MMM-yyyy HH:mm" });
                //ContractResolver = new LowercaseContractResolver()
            }
            jSettings.ContractResolver = new LowercaseContractResolver();
            //jSettings.Converters.Add(new MyDateTimeConvertor());
            var jsondataLimit = JsonConvert.SerializeObject(ds, jSettings);

            //int d = jsondataLimit.Length;
            //if (d > 1800000)
            //{
            //    DataTable dtPage = dt.AsEnumerable().Take(dt.Rows.Count - 100).CopyToDataTable();
            //    jsondataLimit = ConvertDataTableToJsonString(dtPage);
            //}
            return jsondataLimit;
        }
    }

    //public class MyDateTimeConvertor : DateTimeConverterBase
    //{
    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        return DateTime.Parse(reader.Value.ToString());
    //    }
    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        writer.WriteValue(((DateTime)value).ToString("dd-MMM-yyyy"));
    //    }
    //}

    public class LowercaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string key)
        {
            return key.ToLower();
        }
    }
}
