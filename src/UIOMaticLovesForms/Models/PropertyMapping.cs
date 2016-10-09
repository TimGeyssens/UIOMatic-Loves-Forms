using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace UIOMaticLovesForms.Models
{
    [DataContract(Name = "propertyMapping")]
    public class PropertyMapping
    {
        [DataMember(Name = "Key")]
        public string Key { get; set; }

        [DataMember(Name = "field")]
        public string Field { get; set; }

        [DataMember(Name = "staticValue")]
        public string StaticValue { get; set; }

        public bool HasValue()
        {
            return !string.IsNullOrEmpty(Field) || !string.IsNullOrEmpty(StaticValue);
        }
    }
}