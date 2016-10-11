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
    [UIOMatic("Comments", "icon-edit", "icon-edit", RenderType = UIOMaticRenderType.List)]
    [TableName("Comments")]
    [PrimaryKey("id", autoIncrement = true)]
    [ExplicitColumns]
    public class Comment : IUIOMaticModel
    {
        [UIOMaticIgnoreField]
        [UIOMaticIgnoreFromListView]
        [Column("id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [UIOMaticIgnoreFromListView]
        [UIOMaticField("Page", "Page the comment was posted on", View = "pickers.content")]
        [Column("UmbracoId")]
        public int UmbracoId { get; set; }

        [Column("Name")]
        [UIOMaticField("Name", "Name of the person that submitted the comment")]
        public string Name { get; set; }

        [Column("Email")]
        [UIOMaticField("Email", "Email of the person that submitted the comment")]
        public string Email { get; set; }

        [Column("Website")]
        [UIOMaticField("Website", "Website of the person that submitted the comment")]
        public string Website { get; set; }

        [Column("Message")]
        [UIOMaticField("Message", "The actual comment", View = "textarea")]
        [SpecialDbType(SpecialDbTypes.NTEXT)]
        public string Message { get; set; }


        public IEnumerable<Exception> Validate()
        {
            var exs = new List<Exception>();

            if (string.IsNullOrEmpty(Name))
                exs.Add(new Exception("Please provide a value for name"));

            if (string.IsNullOrEmpty(Email))
                exs.Add(new Exception("Please provide a value for email"));

            if (string.IsNullOrEmpty(Website))
                exs.Add(new Exception("Please provide a value for website"));

            return exs;
        }
    }
}