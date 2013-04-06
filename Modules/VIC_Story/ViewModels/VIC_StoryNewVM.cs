using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Orchard.Media.ViewModels;

namespace VIC_Story.ViewModels
{
    public class VIC_StoryNewVM
    {
        [Required(ErrorMessage = "Thiếu tên câu chuyện")]
        [StringLength(60,ErrorMessage="Tên câu chuyện quá dài")]
        public string StoryName { get; set; }
        [Required(ErrorMessage = "Thiếu chủ đề quan tâm")]
        public int Concern { get; set; }
        [Required(ErrorMessage = "Thiếu mô tả ngắn câu chuyện")]
        [StringLength(135, ErrorMessage = "Mô tả quá dài")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Thiếu khu vực thực hiện")]
        public int Area { get; set; }
        [Required(ErrorMessage = "Thiếu thời gian bắt đầu")]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = "Thiếu thời gian kết thúc")]
        public DateTime? EndDate { get; set; }
        [Required(ErrorMessage = "Thiếu vì sao quan trọng với bạn")]
        [StringLength(10000, ErrorMessage = "Mô tả quá dài")]
        public string WhyImp { get; set; }
        [Required(ErrorMessage = "Thiếu cách giải quyết của bạn")]
        [StringLength(10000, ErrorMessage = "Mô tả quá dài")]
        [AllowHtml]
        public string Solve { get; set; }
        [Required(ErrorMessage = "Thiếu những thuận lợi và khó khăn của bạn")]
        [StringLength(10000, ErrorMessage = "Mô tả quá dài")]
        [AllowHtml]
        public string Advance { get; set; }
        public VIC_StoryNewVM() {
            ExtractZip = false;
        }     
        public string FolderName { get; set; }
        public string MediaPath { get; set; }
        public bool ExtractZip { get; set; }
        public string AllowedExtensions { get; set; }
    }
}