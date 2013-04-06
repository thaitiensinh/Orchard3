using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VIC_Story.ViewModels
{
    public class VIC_StoryUpdateVM
    {
        [Required(ErrorMessage = "Thiếu tiêu đề cập nhật")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Thiếu nội dung cập nhật")]
        public string Content { get; set; }
        [Required(ErrorMessage = "Thiếu ngày cập nhật")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Date { get; set; }
        public int ID_Story { get; set; }
    }
}