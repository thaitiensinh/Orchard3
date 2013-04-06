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
    public class VIC_StoryRecord : ContentPartRecord
    {
        public virtual string Picture { get; set; }
        public virtual string StoryName {get; set;}
        public virtual int Concern {get; set;}
        public virtual string Description {get; set;}
        public virtual int Area { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual string WhyImp { get; set; }
        public virtual string Solve { get; set; }
        public virtual string Advance { get; set; }
        public virtual string Status { get; set; }
        public virtual int ID_User { get; set; }
        public virtual int AmountCare { get; set; }
        public virtual DateTime? PublishDate { get; set; }
        
    }

    public class VIC_StoryPart : ContentPart<VIC_StoryRecord>
    {
        [Required]
        public string Picture
        {
            get { return Record.Picture; }
            set { Record.Picture = value; }
        }
        [Required]
        public string StoryName
        {
            get { return Record.StoryName; }
            set { Record.StoryName = value; }
        }
        public int Concern
        {
            get { return Record.Concern; }
            set { Record.Concern = value; }
        }
        [Required]
        public string Description
        {
            get { return Record.Description; }
            set { Record.Description = value; }
        }
        [Required]
        public int Area
        {
            get { return Record.Area; }
            set { Record.Area = value; }
        }
        [Required]
        public DateTime? StartDate
        {
            get { return Record.StartDate; }
            set { Record.StartDate = value; }
        }
        [Required]
        public DateTime? EndDate
        {
            get { return Record.EndDate; }
            set { Record.EndDate = value; }
        }
        [Required]
        public string WhyImp
        {
            get { return Record.WhyImp; }
            set { Record.WhyImp = value; }
        }
        [Required]
        [AllowHtml]
        public string Solve
        {
            get { return Record.Solve; }
            set { Record.Solve = value; }
        }
        [Required]
        [AllowHtml]
        public string Advance
        {
            get { return Record.Advance; }
            set { Record.Advance = value; }
        }
        public string Status
        {
            get { return Record.Status; }
            set { Record.Status = value; }
        }

        public int ID_User
        {
            get { return Record.ID_User; }
            set { Record.ID_User = value; }
        }
        public DateTime? PublishDate
        {
            get { return Record.PublishDate; }
            set { Record.PublishDate = value; }
        }
        public int AmountCare
        {
            get { return Record.AmountCare; }
            set { Record.AmountCare = value; }
        }
    }
}