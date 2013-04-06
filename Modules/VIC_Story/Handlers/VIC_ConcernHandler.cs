using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using VIC_Story.Models;
using Orchard.Data;
namespace VIC_Story.Handlers
{
    public class VIC_ConcernHandler : ContentHandler
    {
        public VIC_ConcernHandler(IRepository<VIC_ConcernRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}