using System;
using System.Collections.Generic;
using System.Data;
using VIC_Story.Models;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.ContentManagement;
using Orchard;

namespace VIC_Story.Datamigration {
    public class Migrations : DataMigrationImpl {
        private readonly IOrchardServices _orchardServices;
        public Migrations(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
            }
        public int Create() {

            SchemaBuilder.CreateTable("VIC_StoryRecord", table => table
                .ContentPartRecord()
                .Column("Picture", DbType.String)
                .Column("StoryName", DbType.String)
                .Column<int>("Concern")
                .Column("Description", DbType.String)
                .Column<int>("Area")
                .Column<DateTime>("StartDate")
                .Column<DateTime>("EndDate")
                .Column("WhyImp", DbType.String, c => c.Unlimited())
                .Column("Solve", DbType.String, c => c.Unlimited())
                .Column("Advance", DbType.String, c => c.Unlimited())
                .Column<int>("ID_User")
                .Column<string>("Status")
                .Column<int>("AmountCare")
                .Column<DateTime>("PublishDate")
            );
            SchemaBuilder.CreateTable("VIC_UpdateStoryRecord", table => table
                .ContentPartRecord()
                .Column("Title", DbType.String)
                .Column<DateTime>("Date")
                .Column("Content", DbType.String)
                .Column<int>("ID_Story")
            );
            SchemaBuilder.CreateTable("VIC_ConcernRecord", table => table
                .ContentPartRecord()
                .Column("ConcernName", DbType.String)
            );
            SchemaBuilder.CreateTable("VIC_AreaRecord", table => table
                .ContentPartRecord()
                .Column("AreaName", DbType.String)      
            );
            //////////////
            SchemaBuilder.CreateTable("VIC_ViewRecord", table => table
                .ContentPartRecord()
                .Column<int>("ID_Story")
                .Column<int>("ID_User")
                .Column<string>("Content")
                .Column<string>("Date")
                .Column<string>("EmailAnonymous")
                .Column<string>("NameAnonymous")
            );

            SchemaBuilder.CreateTable("VIC_StoryConcernHomePageRecord", table => table
                .ContentPartRecord()
                .Column<int>("ID_Story")
                .Column<int>("ID_Concern")
                .Column<DateTime>("PublishDate")
                .Column<bool>("Status_StoryConcernHomepage")
            );

            ContentDefinitionManager.AlterPartDefinition(
                typeof(VIC_StoryPart).Name, cfg => cfg.Attachable());
            ContentDefinitionManager.AlterPartDefinition(
                typeof(VIC_UpdateStoryPart).Name, cfg => cfg.Attachable());
            ContentDefinitionManager.AlterPartDefinition(
                typeof(VIC_ConcernPart).Name, cfg => cfg.Attachable());
            ContentDefinitionManager.AlterPartDefinition(
                typeof(VIC_AreaPart).Name, cfg => cfg.Attachable());
            ContentDefinitionManager.AlterPartDefinition(
               typeof(VIC_ViewPart).Name, cfg => cfg.Attachable());
            ContentDefinitionManager.AlterPartDefinition(
               typeof(VIC_StoryConcernHomepagePart).Name, cfg => cfg.Attachable());
            return 1;
        }
        public int UpdateFrom1()
        {
            ContentDefinitionManager.AlterTypeDefinition("VIC_Story", cfg =>
            {
                cfg.WithPart("VIC_StoryPart");
                cfg.WithPart("CommonPart");
                cfg.Creatable(true);
                cfg.Draftable(false);
            });
            ContentDefinitionManager.AlterTypeDefinition("VIC_UpdateStory", cfg =>
            {
                cfg.WithPart("VIC_UpdateStoryPart");
                cfg.WithPart("CommonPart");
                cfg.Creatable(true);
                cfg.Draftable(false);
            });
            ContentDefinitionManager.AlterTypeDefinition("VIC_Concern", cfg =>
            {
                cfg.WithPart("VIC_ConcernPart");
                cfg.WithPart("CommonPart");
                cfg.Creatable(true);
                cfg.Draftable(false);
            });
            ContentDefinitionManager.AlterTypeDefinition("VIC_Area", cfg =>
            {
                cfg.WithPart("VIC_AreaPart");
                cfg.WithPart("CommonPart");
                cfg.Creatable(true);
                cfg.Draftable(false);
            });

            ContentDefinitionManager.AlterTypeDefinition("VIC_View", cfg =>
            {
                cfg.WithPart("VIC_ViewPart");
                cfg.WithPart("CommonPart");
                cfg.Creatable(true);
                cfg.Draftable(false);
            });
            ContentDefinitionManager.AlterTypeDefinition("VIC_StoryConcernHomepage", cfg =>
            {
                cfg.WithPart("VIC_StoryConcernHomepagePart");
                cfg.WithPart("CommonPart");
                cfg.Creatable(true);
                cfg.Draftable(false);
            });
          
            return 2;
        }
        public int UpdateFrom2()
        {
            //Add Concern
            string[] ConcernArray = new string[] { "Thiện nguyện", "Môi trường", "Giáo dục", 
                                         "Nghệ thuật", "Năng lượng", "Cộng đồng" };
            for (int i = 0; i < ConcernArray.Length; i++)
            {
                var vic_concern = _orchardServices.ContentManager.New("VIC_Concern");
                var part = vic_concern.As<VIC_ConcernPart>();
                part.ConcernName = ConcernArray[i];
                _orchardServices.ContentManager.Create(vic_concern);
            }               
            return 3;
        }
        public int UpdateFrom3()
        {
            string[] AreaArray = new string[] { "Tp.Hồ Chí Minh", "Hà Nội", "Hải Phòng", "Bình Định" };
            for (int i = 0; i < AreaArray.Length; i++)
            {
                var vic_area = _orchardServices.ContentManager.New("VIC_Area");
                var part = vic_area.As<VIC_AreaPart>();
                part.AreaName = AreaArray[i];
                _orchardServices.ContentManager.Create(vic_area);
            }
            return 4;
        }
    }
}