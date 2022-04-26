using MX7EDPSSAP.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MX7EDPSSAP.Repository.DataModel
{
    public class mdoAisleModel
    {
        [CustomizedModel(IsConvertToCamelCase = true)]
        public int id { get; set; }

        [CustomizedModel(IsConvertToCamelCase = true)]
        public string code { get; set; }

        [CustomizedModel(IsConvertToCamelCase = true)]
        public string descs { get; set; }

        [CustomizedModel(IsConvertToCamelCase = true)]
        public int max_num_of_zone { get; set; }

        [CustomizedModel(IsConvertToCamelCase = true)]
        public bool is_active { get; set; }

        [JsonIgnore]
        public int created_by_user_id { get; set; }

        [CustomizedModel(IsConvertToCamelCase = true)]
        public string created_by { get; set; }

        [CustomizedModel(IsConvertToCamelCase = true)]
        public DateTime created_date { get; set; }

        [JsonIgnore]
        public string modified_by_user_id { get; set; }

        [CustomizedModel(IsConvertToCamelCase = true)]
        public string modified_by { get; set; }

        [CustomizedModel(IsConvertToCamelCase = true)]
        public DateTime? modified_date { get; set; }
    }
}
