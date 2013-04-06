using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.Themes;
using VIC_Story.ViewModels;
using Orchard;
using VIC_Story.Models;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Security;
using System.Globalization;
using Orchard.Media.Models;
using Orchard.Media.Services;
using Orchard.Media.ViewModels;
using Orchard.Localization;
using Orchard.UI.Notify;
using Orchard.Mvc;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using VIC.User.Models;
using CaptchaMvc.HtmlHelpers;
using Orchard.Users.Models;
using Microsoft.Web.Helpers;
using Orchard.Roles.Models;
using Orchard.Core.Contents.Controllers;
using System.Net.Sockets;
using System.Runtime.Remoting.Contexts;
namespace VIC_Story.Controllers
{
    [HandleError, Themed]
    public class HomeController : Controller
    {
        public Localizer T { get; set; }
        private readonly IOrchardServices _orchardServices;
        private readonly IMediaService _mediaService;
        private readonly IContentManager _contentManager;
        public HomeController(IOrchardServices orchardServices, IMediaService mediaService, IContentManager contentManager)
        {
            _orchardServices = orchardServices;
            _mediaService = mediaService;
            _contentManager = contentManager;
        }
        public ActionResult Index()
        {
            var eConcernlist = _orchardServices.ContentManager.Query<VIC_ConcernPart, VIC_ConcernRecord>().List();
            var eListTopStory = _orchardServices.ContentManager.Query<VIC_StoryConcernHomepagePart, VIC_StoryConcernHomepageRecord>().List();
            var eListStory = _orchardServices.ContentManager.Query<VIC_StoryPart, VIC_StoryRecord>().List();
            var eListUser = _orchardServices.ContentManager.Query<VIC_UserPart, VIC_UserRecord>().List();
            var eListStory_Result = (from c in eListTopStory
                                     join p in eListStory on c.ID_Story equals p.Id
                                     select p).ToList();
            var eListUser_Result = (from c in eListStory
                                     join p in eListUser on c.ID_User equals p.UserPart_Id
                                     select p).ToList();
            VIC_StoryHomePageVM model = new VIC_StoryHomePageVM();
            model.eListUser = eListUser_Result;
            model.eListConcern = eConcernlist.ToList();
            model.eListStory = eListStory_Result;
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Index(string id)
        {
            VIC_StoryHomePageVM model = new VIC_StoryHomePageVM();
            model.id = id;
            //return PartialView("TopStory");
            return Index();
        }
        public ActionResult Admin_HomePage()
        {
           // List<SelectListItem> lt2 = new List<SelectListItem>();
           // var eConcernlist = _orchardServices.ContentManager.Query<VIC_ConcernPart, VIC_ConcernRecord>().List();
           // SelectListItem S2 = new SelectListItem();
           // S2.Text = "Chọn khoa";
           // S2.Value = "-1";
           // lt2.Add(S2);
           // foreach (var item in eConcernlist)
           // {


           //     SelectListItem S = new SelectListItem();
           //     S.Text = item.ConcernName;
           //     S.Value = item.Id.ToString();
           //     lt2.Add(S);
           // }
           //ViewBag.ConcernList = lt2;

           //List<SelectListItem> lt3 = new List<SelectListItem>();
           //var eStorylist = _orchardServices.ContentManager.Query<VIC_StoryPart, VIC_StoryRecord>().List();
           //SelectListItem S3 = new SelectListItem();
           //S2.Text = "Chọn khoa";
           //S2.Value = "-1";
           //lt2.Add(S3);
           //foreach (var item in eStorylist)
           //{


           //    SelectListItem S = new SelectListItem();
           //    S.Text = item.StoryName;
           //    S.Value = item.Id.ToString();
           //    lt3.Add(S);
           //}
           //ViewBag.StoryList = lt3;
            //ViewBag.Concernlist = new SelectList(eConcernlist, "ID", "ConcernName");
            //var eListStory = _orchardServices.ContentManager.Query<VIC_StoryPart, VIC_StoryRecord>().List();
            //ViewBag.StoryList = new SelectList(eListStory, "ID", "StoryName");
            var eConcernlist = _orchardServices.ContentManager.Query<VIC_ConcernPart, VIC_ConcernRecord>().List();
           ViewBag.ID_Concern = new SelectList(eConcernlist, "ID", "ConcernName");

            // Create Models view data
            var eListStory = _orchardServices.ContentManager.Query<VIC_StoryPart, VIC_StoryRecord>().List();
            ViewBag.ID_Story = new SelectList(eListStory, "ID", "StoryName");
            return View("Admin_HomePage");
        }
        [HttpPost]
        public ActionResult Admin_HomePage(VIC_StoryHomePageVM model)
        {
            if (ModelState.IsValid == true)
            {
                var vicstory = _orchardServices.ContentManager.New("VIC_StoryConcernHomePage");
                var part = vicstory.As<VIC_StoryConcernHomepagePart>();
                part.ID_Concern = model.ID_Concern;
                part.ID_Story = model.ID_Story;
                part.Status_Story_Concern_Homepage = true;
                part.PublishDate = DateTime.Now;
                part.ContentItem.As<CommonPart>().Owner = _orchardServices.WorkContext.CurrentUser;
                _orchardServices.ContentManager.Create(vicstory);
                return Admin_HomePage();
            }

            else
            {
                return Admin_HomePage();
            }
        }
        [HttpPost]
        public ActionResult IndexDDL(string Concern, string Story)
        {

            //var eConcernlist = _orchardServices.ContentManager.Query<VIC_ConcernPart, VIC_ConcernRecord>().List();
            //ViewBag.Concern = new SelectList(eConcernlist, "ID", "ConcernName");

            //int StoryID = -1;
            ////if (!int.TryParse(VIC_StoryRecord, out StoryID))
            ////{
            ////    ViewBag.YouSelected = "You must select a Country and State";
            ////    return View();
            ////}

            ////var Story = from s in eConcernlist.
            ////            where s.StoryID == StoryID
            ////            select s.StateName;

            ////var Concern = from s in Concern.GetCountries()
            ////              where s.Code == Concern
            ////              select s.Name;


            //ViewBag.YouSelected = "You selected " + country.SingleOrDefault()
            //                     + " And " + state.SingleOrDefault();
            return View("Info");

        }
    }
}