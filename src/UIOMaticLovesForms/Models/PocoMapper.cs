using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace UIOMaticLovesForms.Models
{
    [DataContract(Name = "pocoMapper")]
    public class PocoMapper
    {
        [DataMember(Name = "typeOfObject")]
        public string TypeOfObject { get; set; }

        [DataMember(Name = "properties")]
        public IEnumerable<PropertyMapping> Properties { get; set; }
    }
}