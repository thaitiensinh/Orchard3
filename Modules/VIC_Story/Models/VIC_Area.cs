using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Records;
using Orchard.ContentManagement;

namespace VIC_Story.Models
{

    public class VIC_AreaRecord : ContentPartRecord
    {
        public virtual string AreaName { get; set; }
    }
    public class VIC_AreaPart : ContentPart<VIC_AreaRecord>
    {
        public string AreaName
        {
            get { return Record.AreaName; }
            set { Record.AreaName = value; }
        }
    }
}
