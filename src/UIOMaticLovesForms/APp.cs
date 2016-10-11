using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Persistence.SqlSyntax;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using UIOMaticLovesForms.Attributes;
using UIOMatic.Attributes;

namespace UIOMaticLovesForms
{
    public class App: ApplicationEventHandler
    {
       

       
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var ctx = applicationContext.DatabaseContext;
            var db = new DatabaseSchemaHelper(ctx.Database, applicationContext.ProfilingLogger.Logger, ctx.SqlSyntax);

            foreach (var type in Umbraco.Core.TypeFinder.FindClassesWithAttribute<CreateTableAttribute>())
            {
                var tableNameAttri = (TableNameAttribute)Attribute.GetCustomAttribute(type, typeof(TableNameAttribute));

                //Check if the DB table does NOT exist
                if (!db.TableExist(tableNameAttri.Value))
                {
                    //Create DB table - and set overwrite to false
                    db.CreateTable(false,type);
                }
            }

            //UIOMatic.Controllers.PetaPocoObjectController.BuildedQuery += PetaPocoObjectController_BuildedQuery;
        }

        //private void PetaPocoObjectController_BuildedQuery(object sender, UIOMatic.QueryEventArgs e)
        //{
        //    if (e.CurrentType ==typeof(Dog))
        //    {

        //        e.Query = Umbraco.Core.Persistence.Sql.Builder
        //            .Append("SELECT Dogs.Id, Dogs.Name, Dogs.IsCastrated, Dogs.Birthday, Dogs.OwnerId, People.Firstname + ' ' + People.Lastname as OwnerName")
        //            .Append("FROM Dogs")
        //            .Append("INNER JOIN People ON Dogs.OwnerId = People.Id")
        //            .Append(string.IsNullOrEmpty(e.SearhTerm) ? "" : string.Format(
        //                @"WHERE Dogs.Name like '%{0}%' 
        //                    or People.Firstname like '%{0}%' 
        //                    or People.Lastname like '%{0}%'", e.SearhTerm))
        //            .Append("ORDER BY " + (string.IsNullOrEmpty(e.SortColumn) ? " Id desc" : e.SortColumn + " " + e.SortOrder));


        //    }
        //}
    }
}