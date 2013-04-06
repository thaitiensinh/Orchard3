using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using VIC_Story.Models;
using Orchard.Data;

namespace VIC_Story.Handlers
{
    public class VIC_AreaHandler: ContentHandler
    {
        public VIC_AreaHandler(IRepository<VIC_AreaRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}