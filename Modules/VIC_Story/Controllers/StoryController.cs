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
    public class StoryController : Controller
    {
        public string sStatus_Pending = "Chờ xác nhận";
        public string sStatus_Finish = "Đã kết thúc";
        public string sStatus_Lock = "Đang bị khóa";
        public string sStatus_Posted = "Đang được đăng";
        public Localizer T { get; set; }
        private readonly IOrchardServices _orchardServices;
        private readonly IMediaService _mediaService;
        private readonly IContentManager _contentManager;
        /*Hàm khởi tạo*/
        public StoryController(IOrchardServices orchardServices, IMediaService mediaService, IContentManager contentManager)
        {
            _orchardServices = orchardServices;
            _mediaService = mediaService;
            _contentManager = contentManager;
        }

        public ActionResult Index()
        {
            var eConcernlist = _orchardServices.ContentManager.Query<VIC_ConcernPart, VIC_ConcernRecord>().List();
            VIC_StoryHomePageVM model = new VIC_StoryHomePageVM();
            model.eListConcern = eConcernlist.ToList();
            return View(model);
        }
        /*Hàm tạo câu chuyện-Get*/
        [AlwaysAccessible]
        public ActionResult CreateStory()
        {
            //Nếu chưa đăng nhập thì return LogOn
            if (_orchardServices.WorkContext.CurrentUser == null)
            {
                return RedirectToAction("LogOn", "Account", new { Area = "VIC.User", ReturnUrl = Request.RawUrl });
            }
            string sFolderName = "image";
            string sMediaPath = "image";
            var eConcernlist = _orchardServices.ContentManager.Query<VIC_ConcernPart, VIC_ConcernRecord>().List();
            ViewBag.Concern = new SelectList(eConcernlist, "ID", "ConcernName");
            var eArealist = _orchardServices.ContentManager.Query<VIC_AreaPart, VIC_AreaRecord>().List();
            ViewBag.Area = new SelectList(eArealist, "ID", "AreaName");

            var currentSite = _orchardServices.WorkContext.CurrentSite;

            var model = new VIC_StoryNewVM
            {
                FolderName = sFolderName,
                MediaPath = sMediaPath,
                AllowedExtensions = currentSite.As<MediaSettingsPart>().UploadAllowedFileTypeWhitelist
            };

            if (currentSite.SuperUser.Equals(_orchardServices.WorkContext.CurrentUser.UserName, StringComparison.Ordinal))
            {
                model.AllowedExtensions = String.Empty;
            }

            return View(model);
        }
        /*Hàm thay đổi tên file hình*/
        private string GenerateImageFileName(string sFilename)
        {
            string sAccount = _orchardServices.WorkContext.CurrentUser.UserName;
            return (sAccount) + "_" + DateTime.Now.Ticks + "_" + sFilename;
        }
        /*Hàm thay đổi kích thước file hình*/
        public byte[] ResizeStream(Stream sFileStream)
        {

            int iMaxWidth = 640, iMaxHeight = 480;
            int iNewWidth, iNewHeight;
            var image = Image.FromStream(sFileStream);
            double dlWidthRatio = (double)image.Width / iMaxWidth;
            double dlHeightRatio = (double)image.Height / iMaxHeight;
            if (dlWidthRatio <= 1 && dlHeightRatio <= 1)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    sFileStream.Position = 0;
                    sFileStream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
            else
            {
                if (dlWidthRatio > dlHeightRatio)
                {
                    iNewWidth = iMaxWidth;
                    iNewHeight = (int)(image.Height / dlWidthRatio);
                }
                else
                {
                    iNewHeight = iMaxHeight;
                    iNewWidth = (int)(image.Width / dlHeightRatio);
                }
            }

            var thumbnailBitmap = new Bitmap(iNewWidth, iNewHeight);

            var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
            thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            var imageRectangle = new Rectangle(0, 0, iNewWidth, iNewHeight);
            thumbnailGraph.DrawImage(image, imageRectangle);
            using (MemoryStream memory = new MemoryStream())
            {
                thumbnailBitmap.Save(memory, image.RawFormat);
                thumbnailGraph.Dispose();
                thumbnailBitmap.Dispose();
                image.Dispose();
                return memory.ToArray();
            }
        }
        /*Hàm tạo câu chuyện - Post*/
        [AlwaysAccessible]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateStory(VIC_StoryNewVM model)
        {
            string sFilename = Request.Files[0].FileName;
            string sAccount = _orchardServices.WorkContext.CurrentUser.UserName;
            string sNewfilename = GenerateImageFileName(sFilename);
            if (String.IsNullOrWhiteSpace(Request.Files[0].FileName))
            {
                ModelState.AddModelError("File", T("Thiếu hình ảnh chủ đề câu chuyện").ToString());
            }

            if (!ModelState.IsValid)
                return CreateStory();
            try
            {
                byte[] abData = ResizeStream((Request.Files[0] as HttpPostedFileBase).InputStream);
                _mediaService.UploadMediaFile(model.MediaPath, sNewfilename, abData, false);
                //_mediaService.UploadMediaFile(model.MediaPath, Request.Files[fileName], model.ExtractZip);
            }
            catch (ArgumentException e)
            {
                _orchardServices.Notifier.Error(T("Uploading media file failed: {0}", e.Message));
                return CreateStory();
            }
            if (ModelState.IsValid == true)
            {
                var vicstory = _orchardServices.ContentManager.New("VIC_Story");
                var part = vicstory.As<VIC_StoryPart>();
                part.Picture = sNewfilename;
                part.StoryName = model.StoryName;
                part.Concern = model.Concern;
                part.Description = model.Description;
                part.Area = model.Area;
                part.StartDate = model.StartDate;
                part.EndDate = model.EndDate;
                part.WhyImp = model.WhyImp;
                part.Solve = model.Solve;
                part.Advance = model.Advance;
                part.Status = sStatus_Pending;
                part.PublishDate = DateTime.Now;
                part.AmountCare = 0;
                part.ContentItem.As<CommonPart>().Owner = _orchardServices.WorkContext.CurrentUser;
                part.ID_User = _orchardServices.WorkContext.CurrentUser.Id;
                _orchardServices.ContentManager.Create(vicstory);
                _orchardServices.Notifier.Information(T("Tạo thành công câu chuyện"));
                return Redirect("ViewStory/" + vicstory.Id);

            }
            else
            {
                return CreateStory();
            }

        }
        /*Hàm xem câu chuyện*/
        [AllowAnonymous]
        public ActionResult ViewStory(int id)
        {

            var eListUpdate = _orchardServices.ContentManager.Query<VIC_UpdateStoryPart, VIC_UpdateStoryRecord>().List();
            //var story = _orchardServices.ContentManager.Query<VIC_StoryPart, VIC_StoryRecord>().List();

            var eListUser = _orchardServices.ContentManager.Query<VIC_UserPart, VIC_UserRecord>().List();
            var eListComment = _orchardServices.ContentManager.Query<VIC_ViewPart, VIC_ViewRecord>().List();
            var eResult_Update = (from c in eListUpdate
                                  // join p in story on c.ID_Story equals id
                                  where c.ID_Story == id
                                  select c).ToList();
            var eResult_User = (from c in eListUser
                                join p in eListComment on c.UserPart_Id equals p.ID_User
                                select c).ToList();
            var eResult_Comment = (from c in eListComment
                                   // join p in story on c.ID_Story equals id
                                   where c.ID_Story == id
                                   select c).ToList();
            var eListConcern = _orchardServices.ContentManager.Query<VIC_ConcernPart, VIC_ConcernRecord>().List();
            var eListArea = _orchardServices.ContentManager.Query<VIC_AreaPart, VIC_AreaRecord>().List();

            var vic_story = _contentManager.Get(id);
           
            
            string sWhyImp = vic_story.As<VIC_StoryPart>().WhyImp;
            string sAdvance = vic_story.As<VIC_StoryPart>().Advance;
            string sSolve = vic_story.As<VIC_StoryPart>().Solve;
            DateTime? dStartDate = vic_story.As<VIC_StoryPart>().StartDate;
            DateTime? dEndDate = vic_story.As<VIC_StoryPart>().EndDate;
            string sDescription = vic_story.As<VIC_StoryPart>().Description;
            string sStoryName = vic_story.As<VIC_StoryPart>().StoryName;
            int iID_User = vic_story.As<VIC_StoryPart>().ID_User;
            string sPublishDate = vic_story.As<VIC_StoryPart>().PublishDate.ToString();
            int iAmountCare = vic_story.As<VIC_StoryPart>().AmountCare;
            VIC_StoryViewVM model = new VIC_StoryViewVM();
            foreach (var i in eListConcern)
            {
                if (i.Id == vic_story.As<VIC_StoryPart>().Concern)
                    model.sConcern = i.ConcernName;
            }
            foreach (var i in eListArea)
            {
                if (i.Id == vic_story.As<VIC_StoryPart>().Area)
                    model.sArea = i.AreaName;
            }
            model.ID_Story = id;
            model.eCommentStory = eResult_Comment;
            if (_orchardServices.WorkContext.CurrentUser == null)
            {
                model.IsUser = false;
            }
            else
            {
                model.IsUser = true;
                model.EmailUser = _orchardServices.WorkContext.CurrentUser.UserName;
                var user = eListUser.Where(u => u.UserPart_Id == _orchardServices.WorkContext.CurrentUser.Id).FirstOrDefault();
                if (!(user == null))
                    model.NameUser = user.FirstName+user.LastName;

            }
            model.ID_User = iID_User;
            model.eListUser = eResult_User;
            model.eListAllUser = eListUser.ToList();
            model.eUpdateStorys = eResult_Update;
            model.Advance = sAdvance;
            model.Solve = sSolve;
            model.StartDate = dStartDate;
            model.EndDate = dEndDate;
            model.Description = sDescription;
            model.StoryName = sStoryName;
            model.WhyImp = sWhyImp;
            model.PublicUrl = _mediaService.GetMediaPublicUrl("image", vic_story.As<VIC_StoryPart>().Picture);
            foreach (var i in eListUser)
            {
                if (i.UserPart_Id == vic_story.As<VIC_StoryPart>().ID_User)
                {
                    var vic_user = _contentManager.Get(i.Id);
                    model.PublicUrl_Avatar = _mediaService.GetMediaPublicUrl("image", vic_user.As<VIC_UserPart>().Avatar);
                }

            }
            model.PublishDate = sPublishDate;
            model.AmountCare = iAmountCare;
            HttpCookie reqCookies = Request.Cookies[id+""];
            if (reqCookies != null)
            {
                model.IsCare = true;
            }
            else
            {
                model.IsCare = false;
            }
            return View(model);
        }

        [HttpPost, ValidateInput(false), ActionName("ViewStory")]
        [FormValueRequired("submit.Binhluan")]
        public ActionResult ViewStoryCommentPost(VIC_StoryViewVM model, int id)
        {
            bool blCheckGoogle = true;
            System.Net.Sockets.TcpClient client = new TcpClient();
            try
            {
                client.Connect("http://www.google.com", 80);
                
            }
            catch (SocketException ex)
            {
                blCheckGoogle = false;
            }
            finally
            {
                client.Close();
            }
            if (blCheckGoogle)
            {


                if (!ReCaptcha.Validate(privateKey: "6LdeSd4SAAAAAM-O8aJAkvFB4CYz-feIV-gC8fEC"))
                {
                    _orchardServices.Notifier.Error(T("Please enter correct Values exactly as you can see in the Picture!"));
                    return RedirectToAction("ViewStory/" + (id));
                }
            }
            if (model.ContentComment == null)
            {
                _orchardServices.Notifier.Error(T("Thiếu thông tin!"));
                return RedirectToAction("ViewStory/" + (id));
            }
            if (!model.IsUser)
            {
                if (model.NameAnynomous == null || model.EmailAnynomous == null)
                {
                    _orchardServices.Notifier.Error(T("Thiếu thông tin!"));
                    return RedirectToAction("ViewStory/" + (id));
                }
            }
            var vicstory = _orchardServices.ContentManager.New("VIC_View");
            var part = vicstory.As<VIC_ViewPart>();
            //Lưu vào database
            part.Content = model.ContentComment;
            if (model.IsUser)
            {
                part.ID_User = _orchardServices.WorkContext.CurrentUser.Id;
            }
            else
            {
                part.NameAnonymous = model.NameAnynomous;
                part.EmailAnonymous = model.EmailAnynomous;
            }

            part.ID_Story = id;
            part.Date = DateTime.Now.ToString();
            part.ContentItem.As<CommonPart>().Owner = _orchardServices.WorkContext.CurrentUser;
            _orchardServices.ContentManager.Create(vicstory);
            _orchardServices.Notifier.Information(T("Comment Story's Saved"));
            model.ContentComment = null;


            return Redirect(Request.UrlReferrer.ToString());

        }

        [HttpPost, ActionName("ViewStory")]
        [FormValueRequired("submit.Quantam")]
        public ActionResult ViewStoryCarePost(int id)
        {
            var vic_story = _contentManager.Get(id);
            vic_story.As<VIC_StoryPart>().AmountCare = vic_story.As<VIC_StoryPart>().AmountCare + 1;
            return RedirectToAction("ViewStory/" + (id));
        }

        /*Hàm chỉnh sửa câu chuyện - Get */
        public ActionResult EditStory(int id)
        {
            if (_orchardServices.WorkContext.CurrentUser == null)
            {
                return RedirectToAction("LogOn", "Account", new { Area = "VIC.User", ReturnUrl = Request.RawUrl });
            }
            var vic_story = _contentManager.Get(id);
            if (!(vic_story.As<VIC_StoryPart>().ID_User == _orchardServices.WorkContext.CurrentUser.Id))
                return new HttpUnauthorizedResult();
            VIC_StoryPart part = new VIC_StoryPart();

            string sWhyImp = vic_story.As<VIC_StoryPart>().WhyImp;
            string sAdvance = vic_story.As<VIC_StoryPart>().Advance;
            string sSolve = vic_story.As<VIC_StoryPart>().Solve;
            DateTime? StartDate = vic_story.As<VIC_StoryPart>().StartDate;
            DateTime? EndDate = vic_story.As<VIC_StoryPart>().EndDate;
            int iArea = vic_story.As<VIC_StoryPart>().Area;
            string sDescription = vic_story.As<VIC_StoryPart>().Description;
            int iConcern = vic_story.As<VIC_StoryPart>().Concern;
            string sStoryName = vic_story.As<VIC_StoryPart>().StoryName;
            VIC_StoryEditVM model = new VIC_StoryEditVM();

            model.FolderName = "image";
            model.MediaPath = "image";
            model.WhyImp = sWhyImp;
            model.Advance = sAdvance;
            model.Solve = sSolve;
            model.StartDate = StartDate;
            model.EndDate = EndDate;
            model.Area = iArea;
            model.Description = sDescription;
            model.Concern = iConcern;
            model.StoryName = sStoryName;
            model.PublicUrl = _mediaService.GetMediaPublicUrl("image", vic_story.As<VIC_StoryPart>().Picture);
            var eConcernlist = _orchardServices.ContentManager.Query<VIC_ConcernPart, VIC_ConcernRecord>().List();
            ViewBag.Concern = new SelectList(eConcernlist, "ID", "ConcernName", iConcern);
            var eArealist = _orchardServices.ContentManager.Query<VIC_AreaPart, VIC_AreaRecord>().List();
            ViewBag.Area = new SelectList(eArealist, "ID", "AreaName", iArea);
            return View(model);
        }
        /*Hàm chỉnh sửa câu chuyện - Post */
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditStory(VIC_StoryNewVM model, int id)
        {
            string sFilename = "";
            string sNewfilename = "";
            /*Nếu không chỉnh sửa hình câu chuyện thì sẽ bỏ qua phần này*/
            if (!String.IsNullOrWhiteSpace(Request.Files[0].FileName))
            {
                sFilename = Request.Files[0].FileName;
                sNewfilename = GenerateImageFileName(sFilename);
                try
                {
                    byte[] abData = ResizeStream((Request.Files[0] as HttpPostedFileBase).InputStream);
                    _mediaService.UploadMediaFile(model.MediaPath, sNewfilename, abData, false);
                    //_mediaService.UploadMediaFile(model.MediaPath, Request.Files[fileName], model.ExtractZip);
                }
                catch (ArgumentException e)
                {
                    _orchardServices.Notifier.Error(T("Uploading media file failed: {0}", e.Message));
                    return CreateStory();
                }
            }
            if (ModelState.IsValid)
            {

                var vic_story = _contentManager.Get(id);
                if (!String.IsNullOrWhiteSpace(Request.Files[0].FileName))
                {
                    _mediaService.DeleteFile(model.MediaPath, vic_story.As<VIC_StoryPart>().Picture);
                    vic_story.As<VIC_StoryPart>().Picture = sNewfilename;
                }
                vic_story.As<VIC_StoryPart>().WhyImp = model.WhyImp;
                vic_story.As<VIC_StoryPart>().Concern = model.Concern;
                vic_story.As<VIC_StoryPart>().Description = model.Description;
                vic_story.As<VIC_StoryPart>().StartDate = model.StartDate;
                vic_story.As<VIC_StoryPart>().EndDate = model.EndDate;
                vic_story.As<VIC_StoryPart>().StoryName = model.StoryName;
                vic_story.As<VIC_StoryPart>().Solve = model.Solve;
                vic_story.As<VIC_StoryPart>().Advance = model.Advance;
                vic_story.As<VIC_StoryPart>().Area = model.Area;
                _orchardServices.Notifier.Information(T("Story's Saved"));
                return EditStory(id);
            }
            else
            {
                return EditStory(id);
            }
        }

        /*Hàm cập nhật câu chuyện - Get */
        public ActionResult UpdateStory(int id)
        {
            if (_orchardServices.WorkContext.CurrentUser == null)
            {
                return RedirectToAction("LogOn", "Account", new { Area = "VIC.User", ReturnUrl = Request.RawUrl });
            }
            var vic_story = _contentManager.Get(id);
            if (!(vic_story.As<VIC_StoryPart>().ID_User == _orchardServices.WorkContext.CurrentUser.Id))
                return new HttpUnauthorizedResult();
            var model = new VIC_StoryUpdateVM
            {
                ID_Story = id
            };
            return View(model);
        }
        /*Hàm cập nhật câu chuyện - Post */
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateStory(VIC_StoryUpdateVM model, int id)
        {
            //Cập nhật model
            //UpdateModel(model);
            if (ModelState.IsValid == true)
            {
                var vicstory = _orchardServices.ContentManager.New("VIC_UpdateStory");
                var part = vicstory.As<VIC_UpdateStoryPart>();
                //Lưu vào database
                part.Title = model.Title;
                CultureInfo provider = new CultureInfo("vi-VN");
                model.Date = (DateTime?)(Convert.ToDateTime(model.Date.ToString(), provider));
                part.Date = model.Date;
                part.Content = model.Content;
                part.ContentItem.As<CommonPart>().Owner = _orchardServices.WorkContext.CurrentUser;
                part.ID_Story = model.ID_Story;
                _orchardServices.ContentManager.Create(vicstory);
                _orchardServices.Notifier.Information(T("Update Story's Saved"));
                return UpdateStory(id);
            }
            else
            {
                return UpdateStory(id);
            }
        }
        /*Kho chuyện*/
        public ActionResult StoryWarehouse()
        {
            
            var eListUser = _orchardServices.ContentManager.Query<VIC_UserPart, VIC_UserRecord>().List();
            var eListStory = _orchardServices.ContentManager.Query<VIC_StoryPart, VIC_StoryRecord>().List();
            var eResult_User = (from c in eListUser
                                join p in eListStory on c.UserPart_Id equals p.ID_User
                                select c).ToList();
            VIC_StoryWarehouseVM model = new VIC_StoryWarehouseVM();
            model.eListUser1 = eResult_User;
            model.Story = eListStory.ToList();
            return View(model);
        }
        /*Mod list danh sách câu chuyện */
        public ActionResult Mod_ListStory(StoryIndexOptions options)
        {
            if (!HasUser())
            {
                return new HttpUnauthorizedResult();
            }
            else
            {
                bool hasMod=false;
                var eListUser = _orchardServices.ContentManager.Query<VIC_UserPart, VIC_UserRecord>().List();
                var eListRole = _orchardServices.WorkContext.CurrentUser.As<UserRolesPart>().Roles;
                foreach (var e in eListRole)
                {
                    if (e == "Moderator")
                    {
                        hasMod = true;
                        break;
                    }
                }
                if (!hasMod)
                {
                    return new HttpUnauthorizedResult();
                }
                if (options == null)
                    options = new StoryIndexOptions();

                var eListStory = _orchardServices.ContentManager.Query<VIC_StoryPart, VIC_StoryRecord>().List();
                switch (options.Filter)
                {
                    case StorysFilter.Dang_duoc_dang:
                        eListStory = eListStory.Where(u => u.Status == sStatus_Posted);
                        break;
                    case StorysFilter.Dang_cho_xet_duyet:
                        eListStory = eListStory.Where(u => u.Status == sStatus_Pending);
                        break;
                    case StorysFilter.Dang_bi_khoa:
                        eListStory = eListStory.Where(u => u.Status == sStatus_Lock);
                        break;
                    case StorysFilter.Da_ket_thuc:
                        eListStory = eListStory.Where(u => u.Status == sStatus_Finish);
                        break;
                }
                if (!String.IsNullOrWhiteSpace(options.Search))
                {
                    eListStory = eListStory.Where(u => u.StoryName.Contains(options.Search) || u.Description.Contains(options.Search));
                }
                switch (options.Order)
                {
                    case StorysOrder.Ten_cau_chuyen:
                        eListStory = eListStory.OrderBy(u => u.StoryName);
                        break;
                    case StorysOrder.Ngay_dang:
                        eListStory = eListStory.OrderBy(u => u.StartDate);
                        break;
                }
                var eResult_User = (from c in eListUser
                                    join p in eListStory on c.UserPart_Id equals p.ID_User
                                    select c).ToList();
                VIC_StoryWarehouseVM model = new VIC_StoryWarehouseVM();
                model.eListUser1 = eResult_User;
                model.Story = eListStory.ToList();
                model.Options = options;
                if (_orchardServices.WorkContext.CurrentUser == null)
                {
                    model.IsUser = false;
                }
                else
                {
                    model.IsUser = true;
                    var eResult_Story_User = (from c in eListStory
                                              //join p in eListStory on c.UserPart_Id equals p.ID_User
                                              where c.ID_User == _orchardServices.WorkContext.CurrentUser.Id
                                              select c).ToList();
                    model.Story_User = eResult_Story_User;
                }

                return View(model);
            }
        }
        public ActionResult Approve_Story(int id)
        {
            var vic_story = _contentManager.Get(id);
            vic_story.As<VIC_StoryPart>().Status = sStatus_Posted;
            _orchardServices.Notifier.Information(T("Câu chuyện đã được đăng"));
            return RedirectToAction("Mod_ListStory");
        }
        public ActionResult Lock_Story(int id)
        {
            var vic_story = _contentManager.Get(id);
            vic_story.As<VIC_StoryPart>().Status = sStatus_Lock;
            _orchardServices.Notifier.Information(T("Câu chuyện đã được khóa"));
            return RedirectToAction("Mod_ListStory");
        }
        public ActionResult Unlock_Story(int id)
        {
            var vic_story = _contentManager.Get(id);
            vic_story.As<VIC_StoryPart>().Status = sStatus_Posted;
            _orchardServices.Notifier.Information(T("Câu chuyện đã được đăng"));
            return RedirectToAction("Mod_ListStory");
        }
        public ActionResult Finish_Story(int id)
        {
            var vic_story = _contentManager.Get(id);
            vic_story.As<VIC_StoryPart>().Status = sStatus_Finish;
            _orchardServices.Notifier.Information(T("Câu chuyện đã được kết thúc"));
            return RedirectToAction("MyStory");
        }
        public ActionResult MyStory()
        {
            var eListUser = _orchardServices.ContentManager.Query<VIC_UserPart, VIC_UserRecord>().List();
            var eListStory = _orchardServices.ContentManager.Query<VIC_StoryPart, VIC_StoryRecord>().List();
            var eResult_User = (from c in eListUser
                                join p in eListStory on c.UserPart_Id equals p.ID_User
                                select c).ToList();
            VIC_StoryWarehouseVM model = new VIC_StoryWarehouseVM();
            model.Story = eListStory.ToList();
            //Nếu chưa đăng nhập thì return LogOn
            if (_orchardServices.WorkContext.CurrentUser == null)
            {
                return RedirectToAction("LogOn", "Account", new { Area = "VIC.User", ReturnUrl = Request.RawUrl });
            }
            else
            {
                model.eListUser1 = eResult_User;
                model.IsUser = true;
                var eResult_Story_User = (from c in eListStory
                                          //join p in eListStory on c.UserPart_Id equals p.ID_User
                                          where c.ID_User == _orchardServices.WorkContext.CurrentUser.Id
                                          select c).ToList();
                model.Story_User = eResult_Story_User;
            }
            return View(model);
        }
        public bool HasUser()
        {
            if (_orchardServices.WorkContext.CurrentUser == null)
            {
                return false;
            }
            else { return true; }
        }
    }
}