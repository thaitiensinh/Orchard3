using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VIC_Story.Models;
using VIC.User.Models;

namespace VIC_Story.ViewModels
{
    public class VIC_StoryWarehouseVM :VIC_StoryViewVM
    {
        public IList<VIC_StoryPart> Story { get; set; }
        public IList<VIC_UserPart> eListUser1 { get; set; }
        public IList<VIC_StoryPart> Story_User { get; set; }

        public StoryIndexOptions Options { get; set; }
        public dynamic Pager { get; set; }
    }

    public class StoryIndexOptions
    {
        public string Search { get; set; }
        public StorysOrder Order { get; set; }
        public StorysFilter Filter { get; set; }
    }

    public enum StorysOrder
    {
        Ten_cau_chuyen,
        Ngay_dang
    }

    public enum StorysFilter
    {
        Tat_ca,
        Dang_duoc_dang,
        Dang_cho_xet_duyet,
        Da_ket_thuc,
        Dang_bi_khoa
    }
}