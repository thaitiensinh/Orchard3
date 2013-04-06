using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace VIC_Story.ViewModels
{
    public class VIC_StoryEditVM : VIC_StoryNewVM
    {
        public string PublicUrl { get; set; }
    }
}