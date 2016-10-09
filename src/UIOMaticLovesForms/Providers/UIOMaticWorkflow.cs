using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using UIOMatic.Controllers;
using UIOMaticLovesForms.Models;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Enums;

namespace UIOMaticLovesForms.Providers
{
    public class UIOMaticWorkflow : WorkflowType
    {

        public UIOMaticWorkflow()
        {
            Id = new Guid("685F4723-72FF-4EF6-B850-A65542A32199");
            Name = "Save as UI-O-Matic Poco (to db)";
            Description = "Saves the form values to the database";
            Icon = "icon-forms-table";
            Group = "Services";
        }

        [Umbraco.Forms.Core.Attributes.Setting("UI-0-Matic Poco", description = "Map poco", view = "~/App_Plugins/UIOMaticLovesForms/FieldSettingTypes/Mappers.Poco.html")]
        public string Fields { get; set; }

        public override WorkflowExecutionStatus Execute(Record record, RecordEventArgs e)
        {
            var maps = JsonConvert.DeserializeObject<PocoMapper>(Fields);

            var mappings = new Dictionary<string, string>();

            foreach (var map in maps.Properties)
            {
                if (map.HasValue() == false)
                    continue;

                var val = map.StaticValue;
                if (string.IsNullOrEmpty(map.Field) == false)
                    val = record.RecordFields[new Guid(map.Field)].ValuesAsString(false);

                mappings.Add(map.Key, val);
            }

            var inst = new ExpandoObject() as IDictionary<string, Object>;
            foreach(var mapping in mappings)
            {
                inst.Add(mapping.Key, mapping.Value);
            }
            var pc = new PetaPocoObjectController();
            var x = new ExpandoObject() as IDictionary<string, Object>;
            x.Add("typeOfObject", maps.TypeOfObject);
            x.Add("objectToCreate", inst);
            pc.PostCreate((ExpandoObject)x);

            return WorkflowExecutionStatus.Completed;
        }

        public override List<Exception> ValidateSettings()
        {
            var exceptions = new List<Exception>();

            if (string.IsNullOrEmpty(Fields))
                exceptions.Add(new Exception("'UI-0-Matic Poco' setting has not been set"));

            return exceptions;
        }
    }
}