using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using VIC_Story.Models;

namespace VIC_Story.Handlers
{
    public class VIC_ViewHandler : ContentHandler
    {
        public VIC_ViewHandler(IRepository<VIC_ViewRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}