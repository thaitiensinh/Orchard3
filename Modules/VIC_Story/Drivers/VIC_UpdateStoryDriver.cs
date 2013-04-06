using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Drivers;
using VIC_Story.Models;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard;

namespace VIC_Story.Drivers
{
    public class VIC_UpdateStoryDriver : ContentPartDriver<VIC_UpdateStoryPart>
    {
        private readonly IOrchardServices _orchardServices;
        public VIC_UpdateStoryDriver(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices; 
        }
      
        protected override DriverResult Display(
            VIC_UpdateStoryPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_VIC_UpdateStory",
                () => shapeHelper.Parts_VIC_Story(
                    Picture: part.Title));
        }

        //GET
        protected override DriverResult Editor(VIC_UpdateStoryPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_VIC_UpdateStory",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/VIC_UpdateStory",
                    Model: part,
                    Prefix: Prefix));
        }

        //POST
        protected override DriverResult Editor(
            VIC_UpdateStoryPart part, IUpdateModel updater, dynamic shapeHelper)
        {

            if (updater.TryUpdateModel(part, Prefix, null, null) == true)
            {
                part.ContentItem.As<CommonPart>().Owner = _orchardServices.WorkContext.CurrentUser;
            }
            return Editor(part, shapeHelper);
        }
    
    }
}