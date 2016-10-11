using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UIOMaticLovesForms.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CreateTableAttribute: Attribute
    {
    }
}