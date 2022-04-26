using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MX7EDPSSAP.Application.Constants.GeneralModelEnum;

namespace MX7EDPSSAP.Infrastructure
{
    public class CustomizedModelAttribute : Attribute
    {
        private string _name;
        private ModelValidationType _validationType;
        private string _validationTypeSt;
        private List<ModelValidationType> _vaList = new();
        private List<string> _testString = new();
        private bool _isConvertToCamelCase;

        public CustomizedModelAttribute()
        {
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual ModelValidationType ValidationType
        {
            get { return _validationType; }
            set { _validationType = value; }
        }

        public virtual string ValidationTypeSt
        {
            get { return _validationTypeSt; }
            set { _validationTypeSt = value; }
        }

        public virtual List<ModelValidationType> VaList
        {
            get { return _vaList; }
            set { _vaList = value; }
        }

        public virtual List<string> testString
        {
            get { return _testString; }
            set { _testString = value; }
        }

        public virtual bool IsConvertToCamelCase
        {
            get { return _isConvertToCamelCase; }
            set { _isConvertToCamelCase = value; }
        }

        //public virtual bool IsRequired
        //{

        //}
    }
}