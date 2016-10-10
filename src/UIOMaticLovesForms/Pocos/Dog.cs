using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UIOMatic.Attributes;
using UIOMatic.Enums;
using UIOMatic.Interfaces;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace UIOMaticLovesForms.Pocos
{
    [UIOMatic("Dogs", "icon-users", "icon-user", RenderType = UIOMaticRenderType.List)]
    [TableName("Dogs")]
    public class Dog : IUIOMaticModel
    {
        public Dog() { }

        [UIOMaticIgnoreField]
        [UIOMaticIgnoreFromListView]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [UIOMaticNameField]
        public string Name { get; set; }

        [UIOMaticField("Is castrated", "Has the dog been castrated")]
        public bool IsCastrated { get; set; }

        [UIOMaticField("Birthday", "When was the dog born", View = "date")]
        public DateTime Birthday { get; set; }

        [UIOMaticField("Owner", "Select the owner of the dog", View = "dropdown",
            Config = "{'typeName': 'UIOMaticLovesForms.Pocos.Person, UIOMaticLovesForms', 'valueColumn': 'Id', 'sortColumn': 'FirstName', 'textTemplate' : 'FirstName + \" \"+ LastName '}")]
        [UIOMaticIgnoreFromListView]
        public int OwnerId { get; set; }

        [ResultColumn]
        [UIOMaticIgnoreField]
        public string OwnerName { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public IEnumerable<Exception> Validate()
        {
            var exs = new List<Exception>();

            if (string.IsNullOrEmpty(Name))
                exs.Add(new Exception("Please provide a value for name"));

            if (OwnerId == 0)
                exs.Add(new Exception("Please select an owner"));


            return exs;
        }
    }
}