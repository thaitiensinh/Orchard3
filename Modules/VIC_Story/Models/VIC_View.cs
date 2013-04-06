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
    public class VIC_ViewRecord : ContentPartRecord
    {
        public virtual int ID_Story { get; set; }
        public virtual string Content { get; set; }
        public virtual string Date { get; set; }
        public virtual int ID_User { get; set; }
        public virtual string NameAnonymous { get; set; }
        public virtual string EmailAnonymous { get; set; }
    }
    public class VIC_ViewPart : ContentPart<VIC_ViewRecord>
    {
        public int ID_Story
        {
            get { return Record.ID_Story; }
            set { Record.ID_Story = value; }
        }
        public string Content
        {
            get { return Record.Content; }
            set { Record.Content = value; }
        }
        public string Date
        {
            get { return Record.Date; }
            set { Record.Date = value; }
        }
        public int ID_User
        {
            get { return Record.ID_User; }
            set { Record.ID_User = value; }
        }
        public string NameAnonymous
        {
            get { return Record.NameAnonymous; }
            set { Record.NameAnonymous = value; }
        }
        public string EmailAnonymous
        {
            get { return Record.EmailAnonymous; }
            set { Record.EmailAnonymous = value; }
        }
    }
}