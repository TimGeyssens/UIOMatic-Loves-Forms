using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UIOMatic.Attributes;
using UIOMatic.Enums;
using UIOMatic.Interfaces;
using UIOMaticLovesForms.Attributes;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace UIOMaticLovesForms.Pocos
{
    [UIOMatic("Movies", "icon-movie", "icon-movie", RenderType = UIOMaticRenderType.List)]
    [TableName("Movies")]
    [CreateTable]
    public class Movie: IUIOMaticModel
    {
        [UIOMaticIgnoreField]
        [UIOMaticIgnoreFromListView]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [UIOMaticNameField]
        public string Title { get; set; }

        [UIOMaticField("Director", "", View = "dropdown",
           Config = "{'typeName': 'UIOMaticLovesForms.Pocos.Person, UIOMaticLovesForms', 'valueColumn': 'Id', 'sortColumn': 'FirstName', 'textTemplate' : 'FirstName + \" \"+ LastName '}")]
        [UIOMaticIgnoreFromListView]
        public int OwnerId { get; set; }

        [UIOMaticField("Release date", "", View = "date")]
        public DateTime ReleaseDate { get; set; }

        [UIOMaticField("Poster", "", View = "file")]
        public string Poster { get; set; }

        public override string ToString()
        {
            return Title;
        }
        public IEnumerable<Exception> Validate()
        {
            var exs = new List<Exception>();

            if (string.IsNullOrEmpty(Title))
                exs.Add(new Exception("Please provide a value for title"));


            return exs;
        }
    }
}