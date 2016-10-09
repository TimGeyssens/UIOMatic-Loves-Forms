using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using UIOMatic.Controllers;
using Umbraco.Forms.Core;
using Umbraco.Forms.Data.Storage;
using System.Reflection;
using UIOMatic.Attributes;
using Newtonsoft.Json.Linq;

namespace UIOMaticLovesForms.Providers
{
    public class UIOMaticDataSource : FormDataSourceType
    {

        public UIOMaticDataSource()
        {
            this.Id = new Guid("787C2A7C-92D5-4098-B9FB-8677CD9EBAA7");
            this.Name = "UIOMatic Poco";
            this.Description = "Sends records to a db table";
            this.SupportsGetRecords = false;
            this.SupportsPrevalues = false;
        }

        [Umbraco.Forms.Core.Attributes.Setting("Type Of Object", description = "Type of the poco", view = "~/App_Plugins/UIOMaticLovesForms/FieldSettingTypes/Pickers.TypeOfObject.html")]
        public string TypeOfObject { get; set; }

        public override void Dispose()
        {
           
        }

        public override Dictionary<object, FormDataSourceField> GetAvailableFields()
        {
            var properties = Type.GetType(TypeOfObject).GetProperties();

           var fields = new Dictionary<object, FormDataSourceField>();

            var pc = new PetaPocoObjectController();

            foreach(var prop in pc.GetAllProperties(TypeOfObject))
            {
                FormDataSourceField fdsf = FormDataSourceField.Create();
                fdsf.AllowNulls = false;
                fdsf.AutoIncrement = false;
                fdsf.Type = Type.GetType(prop.Type);
                fdsf.Name = prop.Name;
                fdsf.Key = prop.Key;
                fdsf.IsMandatory = false;
                

                var property = properties.FirstOrDefault(x => x.GetCustomAttribute<UIOMaticFieldAttribute>(true) != null && x.GetCustomAttribute<UIOMaticFieldAttribute>(true).Name == prop.Name);

                if (property != null)
                {
                   

                    var key = prop.Name;

                    var view = property.GetCustomAttribute<UIOMaticFieldAttribute>(true).View;

                    fdsf.IsForeignKey = view == "dropdown";

                   

                    if (fdsf.IsForeignKey)
                    {
                        fdsf.PrevalueKeyField = prop.Config["valueColumn"].ToString();
                        fdsf.PrevalueSource = prop.Config["typeName"].ToString();

                        
                        foreach (var obj in pc.GetAllProperties(prop.Config["typeName"].ToString()))
                        {
                            fdsf.AvailablePrevalueValueFields.Add(obj.Key);
                        }

                        fdsf.AvailablePrevalueValueFields.Add("ToString()");
                    }
                }
                   

                fields.Add(fdsf.Key, fdsf);
            }

            return fields;
        }

        public override Dictionary<object, FormDataSourceField> GetMappedFields()
        {
            return new Dictionary<object, FormDataSourceField>();
        }

        public override Dictionary<object, string> GetPrevalues(Field field, Form form)
        {
            Dictionary<object, string> d = new Dictionary<object, string>();

            if (field.DataSourceFieldKey != null)
            {

                var FieldMappings = form.DataSource.Mappings;
                var map = FieldMappings.Where(x => x.DataFieldKey.ToString().ToLower() == field.DataSourceFieldKey.ToString().ToLower()).FirstOrDefault();

                if (map != null)
                {
                    var controller = new PetaPocoObjectController();

                    var currentType = Type.GetType(map.PrevalueTable);

                    var sortColumn = map.PrevalueValueField == "ToString()" ? string.Empty : map.PrevalueValueField;

                    foreach (var prevalue in controller.GetAll(map.PrevalueTable, sortColumn, "asc"))
                    {
                        var val = map.PrevalueValueField == "ToString()" ? prevalue.ToString() : currentType.GetProperty(map.PrevalueValueField).GetValue(prevalue, null).ToString();

                        d.Add(currentType.GetProperty(map.PrevalueKeyfield).GetValue(prevalue, null)
                            , val);
                    }
                    

                }
            }
            return d;
        }

        public override List<Record> GetRecords(Form form, int page, int maxItems, object sortByField, Sorting Order)
        {
            return new List<Record>();
        }

        public override Record InsertRecord(Record record)
        {
            var fieldMappings = record.GetForm().DataSource.Mappings;

            var inst = new ExpandoObject() as IDictionary<string, Object>;

            foreach (FormDataSourceMapping map in fieldMappings)
            {

                object val = new object();

                if (string.IsNullOrEmpty(map.DefaultValue))
                {

                    foreach (RecordField rf in record.RecordFields.Values)
                    {

                        if (rf.Field.DataSourceFieldKey.ToString() == map.DataFieldKey.ToString())
                        {
                            val = Convert.ChangeType(rf.ValuesAsString(), rf.Field.FieldType.GetDataType());
                        }

                    }

                }
                else
                    val = map.DefaultValue;

                inst.Add(map.DataFieldKey.ToString(), val);


            }

            var pc = new PetaPocoObjectController();
            var x = new ExpandoObject() as IDictionary<string, Object>;
            x.Add("typeOfObject", TypeOfObject);
            x.Add("objectToCreate", inst);
            pc.PostCreate((ExpandoObject)x);
            return record;
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