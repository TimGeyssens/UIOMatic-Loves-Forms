using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UIOMatic.Attributes;
using UIOMaticLovesForms.Attributes;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace UIOMaticLovesForms.Pocos
{
    [UIOMatic("PollAnswers", "icon-poll", "icon-poll", RenderType = UIOMatic.Enums.UIOMaticRenderType.List)]
    [TableName("PollAnswers")]
    public class PollAnswer
    {
        [UIOMaticIgnoreField]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        public string Answer { get; set; }
    }
}