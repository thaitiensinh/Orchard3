﻿using VIC_Story.ViewModels;
using Orchard.Core.Common.Models;
using Orchard;
using System.Collections.Generic;
using System.Web.Mvc;
using Orchard.ContentManagement.Drivers;
using VIC_Story.Models;
using Orchard.ContentManagement;

namespace VIC_Story.Drivers
{
    public class VIC_AreaDriver : ContentPartDriver<VIC_AreaPart>
    {
        private readonly IOrchardServices _orchardServices;
        public VIC_AreaDriver(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
        }
        protected override DriverResult Display(
            VIC_AreaPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_VIC_Story",
                () => shapeHelper.Parts_VIC_Story(
                   ));
        }

        //GET
        protected override DriverResult Editor(VIC_AreaPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_VIC_Story_Edit",
                () => shapeHelper.EditorTemplate(
                   ));
        }

        //POST
        protected override DriverResult Editor(
            VIC_AreaPart part, IUpdateModel updater, dynamic shapeHelper)
        {

            if (updater.TryUpdateModel(part, Prefix, null, null) == true)
            {
                part.ContentItem.As<CommonPart>().Owner = _orchardServices.WorkContext.CurrentUser;
            }
            return Editor(part, shapeHelper);
        }

    }
}