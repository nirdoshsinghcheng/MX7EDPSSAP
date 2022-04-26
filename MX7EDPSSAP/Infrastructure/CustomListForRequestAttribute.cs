﻿using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace MX7EDPSSAP.Infrastructure
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CustomListForRequestAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as IEnumerable;
            return list != null && list.GetEnumerator().MoveNext();
        }
    }
}