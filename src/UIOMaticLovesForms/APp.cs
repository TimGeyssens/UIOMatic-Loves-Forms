using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UIOMaticLovesForms.Pocos;
using Umbraco.Core;

namespace UIOMaticLovesForms
{
    public class App: ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            UIOMatic.Controllers.PetaPocoObjectController.BuildedQuery += PetaPocoObjectController_BuildedQuery;
        }

        private void PetaPocoObjectController_BuildedQuery(object sender, UIOMatic.QueryEventArgs e)
        {
            if (e.CurrentType ==typeof(Dog))
            {

                e.Query = Umbraco.Core.Persistence.Sql.Builder
                    .Append("SELECT Dogs.Id, Dogs.Name, Dogs.IsCastrated, Dogs.Birthday, Dogs.OwnerId, People.Firstname + ' ' + People.Lastname as OwnerName")
                    .Append("FROM Dogs")
                    .Append("INNER JOIN People ON Dogs.OwnerId = People.Id")
                    .Append(string.IsNullOrEmpty(e.SearhTerm) ? "" : string.Format(
                        @"WHERE Dogs.Name like '%{0}%' 
                            or People.Firstname like '%{0}%' 
                            or People.Lastname like '%{0}%'", e.SearhTerm))
                    .Append("ORDER BY " + (string.IsNullOrEmpty(e.SortColumn) ? " Id desc" : e.SortColumn + " " + e.SortOrder));


            }
        }
    }
}