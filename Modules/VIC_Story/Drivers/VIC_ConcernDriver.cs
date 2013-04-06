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
    public class VIC_ConcernDriver :ContentPartDriver<VIC_ConcernPart>
    {
       private readonly IOrchardServices _orchardServices;
       public VIC_ConcernDriver(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices; 
        }
        protected override DriverResult Display(
            VIC_ConcernPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_VIC_Story",
                () => shapeHelper.Parts_VIC_Story(
                   ));
        }

        //GET
        protected override DriverResult Editor(VIC_ConcernPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_VIC_Story_Edit",
                () => shapeHelper.EditorTemplate(
                   ));
        }

        //POST
        protected override DriverResult Editor(
            VIC_ConcernPart part, IUpdateModel updater, dynamic shapeHelper)
        {

            if (updater.TryUpdateModel(part, Prefix, null, null) == true)
            {
                part.ContentItem.As<CommonPart>().Owner = _orchardServices.WorkContext.CurrentUser;
            }
            return Editor(part, shapeHelper);
        }
    }
}