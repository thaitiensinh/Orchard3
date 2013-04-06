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
    public class VIC_StoryConcernHomepageRecord : ContentPartRecord
    {
        public virtual int ID_Story { get; set; }
        public virtual int ID_Concern { get; set; }
        public virtual DateTime? PublishDate { get; set; }
        public virtual bool Status_StoryConcernHomepage { get; set; }
    }
    public class VIC_StoryConcernHomepagePart : ContentPart<VIC_StoryConcernHomepageRecord>
    {
        public int ID_Story
        {
            get { return Record.ID_Story; }
            set { Record.ID_Story = value; }
        }
        public int ID_Concern
        {
            get { return Record.ID_Concern; }
            set { Record.ID_Concern = value; }
        }
        public DateTime? PublishDate
        {
            get { return Record.PublishDate; }
            set { Record.PublishDate = value; }
        }
        public bool Status_Story_Concern_Homepage
        {
            get { return Record.Status_StoryConcernHomepage; }
            set { Record.Status_StoryConcernHomepage = value; }
        }
    }
}