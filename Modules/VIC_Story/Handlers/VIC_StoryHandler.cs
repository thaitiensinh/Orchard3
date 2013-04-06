using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using VIC_Story.Models;
using Orchard.Data;

namespace VIC_Story.Handlers
{
    public class VIC_StoryHandler : ContentHandler
    {

        public VIC_StoryHandler(IRepository<VIC_StoryRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }


    }
}