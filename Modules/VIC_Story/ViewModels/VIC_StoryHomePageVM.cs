using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VIC_Story.Models;
using System.Web.Mvc;
using VIC.User.Models;

namespace VIC_Story.ViewModels
{
    public class VIC_StoryHomePageVM
    {
        public IList<VIC_ConcernPart> eListConcern { get; set; }
        public IList<VIC_StoryPart> eListStory { get; set; }
        public IList<VIC_UserPart> eListUser { get; set; }
        public List<SelectListItem> list { get; set; }
        public int ID_Concern { get; set; }
        public int ID_Story { get; set; }
        public bool Status { get; set; }
        public string id { get; set; }
    }
}