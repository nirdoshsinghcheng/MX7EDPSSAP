using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MX7EDPSSAP.Helpers
{
    public static class EnumHelper
    {
        public static string GetEnumCode(this Enum value)
        {
            Type enumType = value.GetType();
            FieldInfo fi = enumType.GetField(value.ToString());
            EnumCodeAttribute[] attrs = fi.GetCustomAttributes(typeof(EnumCodeAttribute), false) as EnumCodeAttribute[];
            if (attrs.Length > 0) return attrs[0].Value;
            return value.ToString();
        }

        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
        public class EnumCodeAttribute : Attribute
        {
            public EnumCodeAttribute(string value)
            {
                Value = value;
            }

            public string Value { get; }
        }
    }
}
