using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using UIOMatic.Controllers;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Extensions;

namespace UIOMaticLovesForms.Providers
{
    public class UIOMaticPrevalueSource : FieldPreValueSourceType
    {
        private List<PreValue> prevalues;

        public UIOMaticPrevalueSource()
        {
            Id = new Guid("489921FA-DAD4-47A6-9682-55F137631867");
            Name = "UI-O-Matic poco";
            Description = "Fetches prevalues from an UI-O-Matic poco";

            this.prevalues = new List<PreValue>();
        }


        [Umbraco.Forms.Core.Attributes.Setting("Type Of Object", description = "Type of the poco", view = "~/App_Plugins/UIOMaticLovesForms/FieldSettingTypes/Pickers.TypeOfObject.html")]
        public string TypeOfObject { get; set; }

        [Umbraco.Forms.Core.Attributes.Setting("Sort Column", description = "Column to sort on")]
        public string SortColumn { get; set; }

        [Umbraco.Forms.Core.Attributes.Setting("Sort Order", description = "Sort order", view = "dropdownlist", prevalues = "Ascending,Descending")]
        public string SortOrder{ get; set; }

        public override List<PreValue> GetPreValues(Field field, Form form)
        {
            this.prevalues.Clear();

            var currentType = Type.GetType(TypeOfObject);

            var primaryKeyColum = "id";

            var primKeyAttri = currentType.GetCustomAttributes().Where(x => x.GetType() == typeof(PrimaryKeyAttribute));
            if (primKeyAttri.Any())
                primaryKeyColum = ((PrimaryKeyAttribute)primKeyAttri.First()).Value;

            foreach (var property in currentType.GetProperties())
            {
                var keyAttri = property.GetCustomAttributes().Where(x => x.GetType() == typeof(PrimaryKeyColumnAttribute));
                if (keyAttri.Any())
                    primaryKeyColum = property.Name;
            }

            var controller = new PetaPocoObjectController();

            int sortOrderCounter = 0;

            foreach (var prevalue in controller.GetAll(TypeOfObject,SortColumn,SortOrder == "Ascending" ? "asc" : "desc"))
            {
                PreValue pv = new PreValue();
                pv.Id = currentType.GetProperty(primaryKeyColum).GetValue(prevalue, null);
                pv.Value = prevalue.ToString().ParsePlaceHolders();
                pv.SortOrder = sortOrderCounter;

                //Add it to the list
                this.prevalues.Add(pv);

                //Increase counter
                sortOrderCounter++;
            }

            return prevalues;
        }

        public override List<Exception> ValidateSettings()
        {
            var exceptions = new List<Exception>();

            if (string.IsNullOrEmpty(TypeOfObject))
            {
                exceptions.Add(new Exception("'TypeOfObject' setting has not been set"));
            }

            return exceptions;
        }
    }
}