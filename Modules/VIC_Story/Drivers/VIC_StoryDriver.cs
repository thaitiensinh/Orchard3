using VIC_Story.Models;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement;
using VIC_Story.ViewModels;
using Orchard.Core.Common.Models;
using Orchard;
using System.Collections.Generic;
using System.Web.Mvc;

namespace VIC_Story.Drivers
{
    public class VIC_StoryDriver : ContentPartDriver<VIC_StoryPart>
    {
        private readonly IOrchardServices _orchardServices;
        public VIC_StoryDriver(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices; 
        }
        protected override DriverResult Display(
            VIC_StoryPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_VIC_Story",
                () => shapeHelper.Parts_VIC_Story(
                    Picture: part.Picture,
                    StoryName: part.StoryName,
                    Concern: part.Concern,
                    Description: part.Description,
                    Area: part.Area,
                    StartDate: part.StartDate,
                    EndDate: part.EndDate,
                    WhyImp: part.WhyImp,
                    Solve: part.Solve,
                    Advance: part.Advance));
        }

        //GET
        protected override DriverResult Editor(VIC_StoryPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_VIC_Story_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/VIC_Story_Edit",
                    Model: new VIC_StoryNewVM(),
                    Prefix: Prefix));
        }

        //POST
        protected override DriverResult Editor(
            VIC_StoryPart part, IUpdateModel updater, dynamic shapeHelper)
        {

            if (updater.TryUpdateModel(part, Prefix, null, null) == true)
            {
                part.ContentItem.As<CommonPart>().Owner = _orchardServices.WorkContext.CurrentUser;
            }
            return Editor(part, shapeHelper);
        }
    }

}
