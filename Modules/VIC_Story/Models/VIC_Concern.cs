using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using System.Web.Mvc;


namespace VIC_Story.Models
{
    public class VIC_ConcernRecord : ContentPartRecord
    {
        public virtual string ConcernName { get; set; }
    }
    public class VIC_ConcernPart : ContentPart<VIC_ConcernRecord>
    {
        public string ConcernName
        {
            get {return Record.ConcernName;}
            set { Record.ConcernName = value; }
        }
    }
}