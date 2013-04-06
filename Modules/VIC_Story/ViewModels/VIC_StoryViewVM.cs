using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VIC_Story.Models;
using VIC.User.Models;
using System.ComponentModel.DataAnnotations;

namespace VIC_Story.ViewModels
{
    public class VIC_StoryViewVM : VIC_StoryNewVM
    {
        public string PublicUrl { get; set; }
        public string PublicUrl_Avatar { get; set; }
        public IList<VIC_UpdateStoryPart> eUpdateStorys { get; set; }
        public string sConcern { get; set; }
        public string sArea { get; set; }
        [Required]
        public string ContentComment { get; set; }
        public IList<VIC_ViewPart> eCommentStory { get; set; }
        public IList<VIC_UserPart> eListUser { get; set; }
        public IList<VIC_UserPart> eListAllUser { get; set; }
        public int ID_Story { get; set; }
        public string NameAnynomous { get; set; }
        public string EmailAnynomous { get; set; }
        public string NameUser { get; set; }
        public string EmailUser { get; set; }
        public int ID_User { get; set; }
        public bool IsUser { get; set; }
        public string PublishDate { get; set; }
        public int AmountCare { get; set; }
        public bool IsCare { get; set; }
    }
}