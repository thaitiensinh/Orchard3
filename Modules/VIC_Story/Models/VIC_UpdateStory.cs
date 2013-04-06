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
    public class VIC_UpdateStoryRecord : ContentPartRecord
    {
        public virtual string Title { get; set; }
        public virtual string Content { get; set; }
        public virtual DateTime? Date { get; set; }
        public virtual int ID_Story { get; set; }
    }
    public class VIC_UpdateStoryPart : ContentPart<VIC_UpdateStoryRecord>
    {
        [Required]
        public string Title
        {
            get { return Record.Title; }
            set { Record.Title = value; }
        }
        [Required]
        public string Content
        {
            get { return Record.Content; }
            set { Record.Content = value; }
        }
        [Required]
        public DateTime? Date
        {
            get { return Record.Date; }
            set { Record.Date = value; }
        }
        public int ID_Story
        {
            get { return Record.ID_Story; }
            set { Record.ID_Story = value; }
        }
    }
}