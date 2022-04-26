using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using MX7EDPSSAP.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MX7EDPSSAP.Infrastructure;

namespace MX7EDPSSAP.Infrastructure
{
    public class CustomCamelCaseResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty prop = base.CreateProperty(member, memberSerialization);
            var memberCustomProp = member.GetCustomAttribute<CustomizedModelAttribute>();
            if (memberCustomProp != null)
            {
                if (!memberCustomProp.IsConvertToCamelCase) return prop;

                prop.PropertyName = CommonHelper.ConvertSnakeCaseToCamelCase(member.Name);
            }
            return prop;
        }
    }
}
