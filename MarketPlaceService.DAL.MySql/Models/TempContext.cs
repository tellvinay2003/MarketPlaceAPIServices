using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MarketPlaceService.DAL.Models
{
    public partial class TempContext : DbContext
    {
        public TempContext()
        {
        }

        public TempContext(DbContextOptions<TempContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChangeHistory> ChangeHistory { get; set; }
        public virtual DbSet<DataFormat> DataFormat { get; set; }
        public virtual DbSet<JobHistoryStatus> JobHistoryStatus { get; set; }
        public virtual DbSet<JobStatus> JobStatus { get; set; }
        public virtual DbSet<JobType> JobType { get; set; }
        public virtual DbSet<MappingData> MappingData { get; set; }
        public virtual DbSet<MappingDataLink> MappingDataLink { get; set; }
        public virtual DbSet<MappingDirection> MappingDirection { get; set; }
        public virtual DbSet<MarketplaceProduct> MarketplaceProduct { get; set; }
        public virtual DbSet<MarketplaceProductHistory> MarketplaceProductHistory { get; set; }
        public virtual DbSet<MarketplaceProductRating> MarketplaceProductRating { get; set; }
        public virtual DbSet<MasterData> MasterData { get; set; }
        public virtual DbSet<MasterDataHistory> MasterDataHistory { get; set; }
        public virtual DbSet<MasterDataLink> MasterDataLink { get; set; }
        public virtual DbSet<MasterDataLinkHistory> MasterDataLinkHistory { get; set; }
        public virtual DbSet<MasterDataTypes> MasterDataTypes { get; set; }
        public virtual DbSet<MasterDataTypesApplicable> MasterDataTypesApplicable { get; set; }
        public virtual DbSet<MasterRegions> MasterRegions { get; set; }
        public virtual DbSet<MasterRegionsHistory> MasterRegionsHistory { get; set; }
        public virtual DbSet<MessageFields> MessageFields { get; set; }
        public virtual DbSet<MessageTypes> MessageTypes { get; set; }
        public virtual DbSet<ProductType> ProductType { get; set; }
        public virtual DbSet<PublishedProductAllowedSubscriber> PublishedProductAllowedSubscriber { get; set; }
        public virtual DbSet<PublishedProducts> PublishedProducts { get; set; }
        public virtual DbSet<PublishedProductsHistory> PublishedProductsHistory { get; set; }
        public virtual DbSet<PublishedProductsQueue> PublishedProductsQueue { get; set; }
        public virtual DbSet<PublishedProductsQueueHistory> PublishedProductsQueueHistory { get; set; }
        public virtual DbSet<PublishedStatus> PublishedStatus { get; set; }
        public virtual DbSet<Publisher> Publisher { get; set; }
        public virtual DbSet<PublisherAgent> PublisherAgent { get; set; }
        public virtual DbSet<PublisherDefault> PublisherDefault { get; set; }
        public virtual DbSet<PublisherProductStatus> PublisherProductStatus { get; set; }
        public virtual DbSet<PublisherProductSubStatus> PublisherProductSubStatus { get; set; }
        public virtual DbSet<PublisherServiceStatus> PublisherServiceStatus { get; set; }
        public virtual DbSet<PublisherSupplierStatus> PublisherSupplierStatus { get; set; }
        public virtual DbSet<Site> Site { get; set; }
        public virtual DbSet<StaticDataType> StaticDataType { get; set; }
        public virtual DbSet<StaticDataUpdateQueue> StaticDataUpdateQueue { get; set; }
        public virtual DbSet<StaticDataUpdateQueueHistory> StaticDataUpdateQueueHistory { get; set; }
        public virtual DbSet<SubscribeProductStatus> SubscribeProductStatus { get; set; }
        public virtual DbSet<SubscribeProductSubStatus> SubscribeProductSubStatus { get; set; }
        public virtual DbSet<Subscriber> Subscriber { get; set; }
        public virtual DbSet<SubscriberChargingPolicy> SubscriberChargingPolicy { get; set; }
        public virtual DbSet<SubscriberDefault> SubscriberDefault { get; set; }
        public virtual DbSet<SubscriberDefaultSellingPricePolicies> SubscriberDefaultSellingPricePolicies { get; set; }
        public virtual DbSet<SubscriberDefaultSellingPriceServiceType> SubscriberDefaultSellingPriceServiceType { get; set; }
        public virtual DbSet<SubscriberDefaultSellingPrices> SubscriberDefaultSellingPrices { get; set; }
        public virtual DbSet<SubscriberProduct> SubscriberProduct { get; set; }
        public virtual DbSet<SubscriberProductCode> SubscriberProductCode { get; set; }
        public virtual DbSet<SubscriberProductCodeServiceType> SubscriberProductCodeServiceType { get; set; }
        public virtual DbSet<SubscriberProductHistory> SubscriberProductHistory { get; set; }
        public virtual DbSet<SubscriberProductQueue> SubscriberProductQueue { get; set; }
        public virtual DbSet<SubscriberProductQueueHistory> SubscriberProductQueueHistory { get; set; }
        public virtual DbSet<SubscriberProductTsUpdateQueue> SubscriberProductTsUpdateQueue { get; set; }
        public virtual DbSet<SubscriberProductTsUpdateQueueHistory> SubscriberProductTsUpdateQueueHistory { get; set; }
        public virtual DbSet<SubscriberSupplier> SubscriberSupplier { get; set; }
        public virtual DbSet<TransactionLog> TransactionLog { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("server=dev02;user=sa;password=;database=MarketPlaceDB2");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChangeHistory>(entity =>
            {
                entity.HasKey(e => e.Masterdatahistoryid)
                    .HasName("PK__CHANGE_H__B31A784E3EF21F7D");

                entity.ToTable("CHANGE_HISTORY");

                entity.Property(e => e.Masterdatahistoryid)
                    .HasColumnName("MASTERDATAHISTORYID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Action).HasColumnName("ACTION");

                entity.Property(e => e.Datatypeid).HasColumnName("DATATYPEID");

                entity.Property(e => e.Details)
                    .HasColumnName("DETAILS")
                    .HasMaxLength(2000);

                entity.Property(e => e.Modifiedby)
                    .HasColumnName("MODIFIEDBY");

                entity.Property(e => e.Modifieddate)
                    .HasColumnName("MODIFIEDDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Origin).HasColumnName("ORIGIN");

                entity.Property(e => e.Siteid).HasColumnName("SITEID");
            });

            modelBuilder.Entity<DataFormat>(entity =>
            {
                entity.HasKey(e => e.Formatid)
                    .HasName("PK__DATA_FOR__A0C5135C47D39BE8");

                entity.ToTable("DATA_FORMAT");

                entity.HasIndex(e => e.Formatname)
                    .HasName("UQ__DATA_FOR__B60A268A145014C0")
                    .IsUnique();

                entity.Property(e => e.Formatid)
                    .HasColumnName("FORMATID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Formatname)
                    .IsRequired()
                    .HasColumnName("FORMATNAME")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<JobHistoryStatus>(entity =>
            {
                entity.HasKey(e => e.Jobstatusid)
                    .HasName("PK__JOB_HIST__640F8EB5D27EAA42");

                entity.ToTable("JOB_HISTORY_STATUS");

                entity.Property(e => e.Jobstatusid)
                    .HasColumnName("JOBSTATUSID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Jobstatusname)
                    .IsRequired()
                    .HasColumnName("JOBSTATUSNAME")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<JobStatus>(entity =>
            {
                entity.ToTable("JOB_STATUS");

                entity.Property(e => e.Jobstatusid)
                    .HasColumnName("JOBSTATUSID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Jobstatusname)
                    .IsRequired()
                    .HasColumnName("JOBSTATUSNAME")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<JobType>(entity =>
            {
                entity.ToTable("JOB_TYPE");

                entity.Property(e => e.Jobtypeid)
                    .HasColumnName("JOBTYPEID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Jobtypename)
                    .IsRequired()
                    .HasColumnName("JOBTYPENAME")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MappingData>(entity =>
            {
                entity.ToTable("MAPPING_DATA");

                entity.Property(e => e.Mappingdataid)
                    .HasColumnName("MAPPINGDATAID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Datatypeid).HasColumnName("DATATYPEID");

                entity.Property(e => e.Mappingdirectionid).HasColumnName("MAPPINGDIRECTIONID");

                entity.Property(e => e.Siteid).HasColumnName("SITEID");

                entity.Property(e => e.Sourceid).HasColumnName("SOURCEID");

                entity.Property(e => e.Sourcename)
                    .HasColumnName("SOURCENAME")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Targetid).HasColumnName("TARGETID");

                entity.Property(e => e.Targetname)
                    .HasColumnName("TARGETNAME")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Datatype)
                    .WithMany(p => p.MappingData)
                    .HasForeignKey(d => d.Datatypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MAPPING_D__DATAT__5CA1C101");

                entity.HasOne(d => d.Mappingdirection)
                    .WithMany(p => p.MappingData)
                    .HasForeignKey(d => d.Mappingdirectionid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MAPPING_D__MAPPI__5D95E53A");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.MappingData)
                    .HasForeignKey(d => d.Siteid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MAPPING_D__SITEI__5BAD9CC8");
            });

            modelBuilder.Entity<MappingDataLink>(entity =>
            {
                entity.ToTable("MAPPING_DATA_LINK");

                entity.Property(e => e.Mappingdatalinkid)
                    .HasColumnName("MAPPINGDATALINKID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Mappingdataid).HasColumnName("MAPPINGDATAID");

                entity.Property(e => e.Parentdataid).HasColumnName("PARENTDATAID");

                entity.HasOne(d => d.Mappingdata)
                    .WithMany(p => p.MappingDataLink)
                    .HasForeignKey(d => d.Mappingdataid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MAPPING_D__MAPPI__0F2D40CE");
            });

            modelBuilder.Entity<MappingDirection>(entity =>
            {
                entity.ToTable("MAPPING_DIRECTION");

                entity.HasIndex(e => e.Mappingdirectionname)
                    .HasName("UQ__MAPPING___917DC28A2137B194")
                    .IsUnique();

                entity.Property(e => e.Mappingdirectionid).HasColumnName("MAPPINGDIRECTIONID");

                entity.Property(e => e.Mappingdirectionname)
                    .IsRequired()
                    .HasColumnName("MAPPINGDIRECTIONNAME")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MarketplaceProduct>(entity =>
            {
                entity.ToTable("MARKETPLACE_PRODUCT");

                entity.Property(e => e.Marketplaceproductid)
                    .HasColumnName("MARKETPLACEPRODUCTID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Messagetypeid).HasColumnName("MESSAGETYPEID");

                entity.Property(e => e.Processedby)
                    .IsRequired()
                    .HasColumnName("PROCESSEDBY")
                    .HasMaxLength(50);

                entity.Property(e => e.Processedon)
                    .HasColumnName("PROCESSEDON")
                    .HasColumnType("datetime");

                entity.Property(e => e.Productdata)
                    .IsRequired()
                    .HasColumnName("PRODUCTDATA");

                entity.Property(e => e.Productlongname)
                    .IsRequired()
                    .HasColumnName("PRODUCTLONGNAME")
                    .HasMaxLength(100);

                entity.Property(e => e.Productshortname)
                    .IsRequired()
                    .HasColumnName("PRODUCTSHORTNAME")
                    .HasMaxLength(50);

                entity.Property(e => e.Publishedproductid).HasColumnName("PUBLISHEDPRODUCTID");

                entity.Property(e => e.Regionid).HasColumnName("REGIONID");

                entity.Property(e => e.Servicetypeid).HasColumnName("SERVICETYPEID");

                entity.HasOne(d => d.Messagetype)
                    .WithMany(p => p.MarketplaceProduct)
                    .HasForeignKey(d => d.Messagetypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MARKETPLA__MESSA__4959E263");

                entity.HasOne(d => d.Publishedproduct)
                    .WithMany(p => p.MarketplaceProduct)
                    .HasForeignKey(d => d.Publishedproductid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MARKETPLA__PUBLI__467D75B8");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.MarketplaceProduct)
                    .HasForeignKey(d => d.Regionid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MARKETPLA__REGIO__4865BE2A");

                entity.HasOne(d => d.Servicetype)
                    .WithMany(p => p.MarketplaceProduct)
                    .HasForeignKey(d => d.Servicetypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MARKETPLA__SERVI__477199F1");
            });

            modelBuilder.Entity<MarketplaceProductHistory>(entity =>
            {
                entity.HasKey(e => e.Marketplaceproductlogid)
                    .HasName("PK__MARKETPL__203B4B75AAE1AA92");

                entity.ToTable("MARKETPLACE_PRODUCT_HISTORY");

                entity.Property(e => e.Marketplaceproductlogid)
                    .HasColumnName("MARKETPLACEPRODUCTLOGID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Messagetypeid).HasColumnName("MESSAGETYPEID");

                entity.Property(e => e.Processedby)
                    .IsRequired()
                    .HasColumnName("PROCESSEDBY")
                    .HasMaxLength(50);

                entity.Property(e => e.Processedon)
                    .HasColumnName("PROCESSEDON")
                    .HasColumnType("datetime");

                entity.Property(e => e.Productdata)
                    .IsRequired()
                    .HasColumnName("PRODUCTDATA");

                entity.Property(e => e.Productlongname)
                    .IsRequired()
                    .HasColumnName("PRODUCTLONGNAME")
                    .HasMaxLength(100);

                entity.Property(e => e.Productshortname)
                    .IsRequired()
                    .HasColumnName("PRODUCTSHORTNAME")
                    .HasMaxLength(50);

                entity.Property(e => e.Publishedproductid).HasColumnName("PUBLISHEDPRODUCTID");

                entity.Property(e => e.Regionid).HasColumnName("REGIONID");

                entity.Property(e => e.Servicetypeid).HasColumnName("SERVICETYPEID");

                entity.HasOne(d => d.Messagetype)
                    .WithMany(p => p.MarketplaceProductHistory)
                    .HasForeignKey(d => d.Messagetypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MARKETPLA__MESSA__220B0B18");

                entity.HasOne(d => d.Publishedproduct)
                    .WithMany(p => p.MarketplaceProductHistory)
                    .HasForeignKey(d => d.Publishedproductid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MARKETPLA__PUBLI__1F2E9E6D");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.MarketplaceProductHistory)
                    .HasForeignKey(d => d.Regionid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MARKETPLA__REGIO__2116E6DF");

                entity.HasOne(d => d.Servicetype)
                    .WithMany(p => p.MarketplaceProductHistory)
                    .HasForeignKey(d => d.Servicetypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MARKETPLA__SERVI__2022C2A6");
            });

            modelBuilder.Entity<MarketplaceProductRating>(entity =>
            {
                entity.ToTable("MARKETPLACE_PRODUCT_RATING");

                entity.Property(e => e.Marketplaceproductratingid)
                    .HasColumnName("MARKETPLACEPRODUCTRATINGID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Marketplaceproductid).HasColumnName("MARKETPLACEPRODUCTID");

                entity.Property(e => e.Ratingid).HasColumnName("RATINGID");

                entity.HasOne(d => d.Marketplaceproduct)
                    .WithMany(p => p.MarketplaceProductRating)
                    .HasForeignKey(d => d.Marketplaceproductid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MARKETPLA__MARKE__53D770D6");

                entity.HasOne(d => d.Rating)
                    .WithMany(p => p.MarketplaceProductRating)
                    .HasForeignKey(d => d.Ratingid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MARKETPLA__RATIN__54CB950F");
            });

            modelBuilder.Entity<MasterData>(entity =>
            {
                entity.ToTable("MASTER_DATA");

                entity.Property(e => e.Masterdataid).HasColumnName("MASTERDATAID");

                entity.Property(e => e.Datatypeid).HasColumnName("DATATYPEID");

                entity.Property(e => e.Masterdataname)
                    .IsRequired()
                    .HasColumnName("MASTERDATANAME")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Datatype)
                    .WithMany(p => p.MasterData)
                    .HasForeignKey(d => d.Datatypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MASTER_DA__DATAT__489AC854");
            });

            modelBuilder.Entity<MasterDataHistory>(entity =>
            {
                entity.ToTable("MASTER_DATA_HISTORY");

                entity.Property(e => e.Masterdatahistoryid)
                    .HasColumnName("MASTERDATAHISTORYID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Action).HasColumnName("ACTION");

                entity.Property(e => e.Datatypeid).HasColumnName("DATATYPEID");

                entity.Property(e => e.Masterdataid).HasColumnName("MASTERDATAID");

                entity.Property(e => e.Masterdataname)
                    .HasColumnName("MASTERDATANAME")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Updateddate)
                    .HasColumnName("UPDATEDDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Username)
                    .HasColumnName("USERNAME")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MasterDataLink>(entity =>
            {
                entity.ToTable("MASTER_DATA_LINK");

                entity.Property(e => e.Masterdatalinkid).HasColumnName("MASTERDATALINKID");

                entity.Property(e => e.Masterdataid).HasColumnName("MASTERDATAID");

                entity.Property(e => e.Parentmasterdataid).HasColumnName("PARENTMASTERDATAID");

                entity.HasOne(d => d.Masterdata)
                    .WithMany(p => p.MasterDataLinkMasterdata)
                    .HasForeignKey(d => d.Masterdataid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MASTER_DA__MASTE__4C6B5938");

                entity.HasOne(d => d.Parentmasterdata)
                    .WithMany(p => p.MasterDataLinkParentmasterdata)
                    .HasForeignKey(d => d.Parentmasterdataid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MASTER_DA__PAREN__4B7734FF");
            });

            modelBuilder.Entity<MasterDataLinkHistory>(entity =>
            {
                entity.ToTable("MASTER_DATA_LINK_HISTORY");

                entity.Property(e => e.Masterdatalinkhistoryid)
                    .HasColumnName("MASTERDATALINKHISTORYID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Action).HasColumnName("ACTION");

                entity.Property(e => e.Masterdataid).HasColumnName("MASTERDATAID");

                entity.Property(e => e.Parentmasterdataid).HasColumnName("PARENTMASTERDATAID");

                entity.HasOne(d => d.Masterdata)
                    .WithMany(p => p.MasterDataLinkHistory)
                    .HasForeignKey(d => d.Masterdataid)
                    .HasConstraintName("FK__MASTER_DA__MASTE__55009F39");
            });

            modelBuilder.Entity<MasterDataTypes>(entity =>
            {
                entity.HasKey(e => e.Datatypeid)
                    .HasName("PK__MASTER_D__1DCBE444E0FFBC15");

                entity.ToTable("MASTER_DATA_TYPES");

                entity.HasIndex(e => e.Datatypename)
                    .HasName("UQ__MASTER_D__F3966E4AF21A7E2F")
                    .IsUnique();

                entity.Property(e => e.Datatypeid)
                    .HasColumnName("DATATYPEID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Datatypename)
                    .IsRequired()
                    .HasColumnName("DATATYPENAME")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Mappinguiformat).HasColumnName("MAPPINGUIFORMAT");

                entity.Property(e => e.Masteruiformat).HasColumnName("MASTERUIFORMAT");

                entity.HasOne(d => d.MappinguiformatNavigation)
                    .WithMany(p => p.MasterDataTypesMappinguiformatNavigation)
                    .HasForeignKey(d => d.Mappinguiformat)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MASTER_DA__MAPPI__41EDCAC5");

                entity.HasOne(d => d.MasteruiformatNavigation)
                    .WithMany(p => p.MasterDataTypesMasteruiformatNavigation)
                    .HasForeignKey(d => d.Masteruiformat)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MASTER_DA__MASTE__40F9A68C");
            });

            modelBuilder.Entity<MasterDataTypesApplicable>(entity =>
            {
                entity.HasKey(e => e.Masterdatatypeapplicableid)
                    .HasName("PK__MASTER_D__E1E37884D72FCBA3");

                entity.ToTable("MASTER_DATA_TYPES_APPLICABLE");

                entity.Property(e => e.Masterdatatypeapplicableid).HasColumnName("MASTERDATATYPEAPPLICABLEID");

                entity.Property(e => e.Datatypeid).HasColumnName("DATATYPEID");

                entity.Property(e => e.Ispublisher).HasColumnName("ISPUBLISHER");

                entity.Property(e => e.Issubscriber).HasColumnName("ISSUBSCRIBER");

                entity.Property(e => e.Mappingdirectionid).HasColumnName("MAPPINGDIRECTIONID");

                entity.HasOne(d => d.Datatype)
                    .WithMany(p => p.MasterDataTypesApplicable)
                    .HasForeignKey(d => d.Datatypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MASTER_DA__DATAT__44CA3770");

                entity.HasOne(d => d.Mappingdirection)
                    .WithMany(p => p.MasterDataTypesApplicable)
                    .HasForeignKey(d => d.Mappingdirectionid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MASTER_DA__MAPPI__45BE5BA9");
            });

            modelBuilder.Entity<MasterRegions>(entity =>
            {
                entity.HasKey(e => e.Regionid)
                    .HasName("PK__MASTER_R__FC359E265AA1EE54");

                entity.ToTable("MASTER_REGIONS");

                entity.HasIndex(e => new { e.Regionname, e.Parentregionid })
                    .HasName("UC_Master_regions_name_parentid")
                    .IsUnique();

                entity.Property(e => e.Regionid).HasColumnName("REGIONID");

                entity.Property(e => e.Level).HasColumnName("LEVEL");

                entity.Property(e => e.Parentregionid).HasColumnName("PARENTREGIONID");

                entity.Property(e => e.Regionname)
                    .IsRequired()
                    .HasColumnName("REGIONNAME")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MasterRegionsHistory>(entity =>
            {
                entity.ToTable("MASTER_REGIONS_HISTORY");

                entity.Property(e => e.Masterregionshistoryid)
                    .HasColumnName("MASTERREGIONSHISTORYID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Action).HasColumnName("ACTION");

                entity.Property(e => e.Level).HasColumnName("LEVEL");

                entity.Property(e => e.Parentregionid).HasColumnName("PARENTREGIONID");

                entity.Property(e => e.Regionid).HasColumnName("REGIONID");

                entity.Property(e => e.Regionname)
                    .HasColumnName("REGIONNAME")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Updateddate)
                    .HasColumnName("UPDATEDDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Username)
                    .HasColumnName("USERNAME")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MessageFields>(entity =>
            {
                entity.HasKey(e => e.Fieldid)
                    .HasName("PK__MESSAGE___707268026334BAA3");

                entity.ToTable("MESSAGE_FIELDS");

                entity.Property(e => e.Fieldid).HasColumnName("FIELDID");

                entity.Property(e => e.Fieldname)
                    .HasColumnName("FIELDNAME")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Fieldpath)
                    .HasColumnName("FIELDPATH")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Ismappingmandatory).HasColumnName("ISMAPPINGMANDATORY");

                entity.Property(e => e.Mappingdatatype).HasColumnName("MAPPINGDATATYPE");

                entity.Property(e => e.Messagetypeid).HasColumnName("MESSAGETYPEID");

                entity.Property(e => e.Removetag)
                    .HasColumnName("REMOVETAG")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.MappingdatatypeNavigation)
                    .WithMany(p => p.MessageFields)
                    .HasForeignKey(d => d.Mappingdatatype)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MESSAGE_F__MAPPI__2DB1C7EE");

                entity.HasOne(d => d.Messagetype)
                    .WithMany(p => p.MessageFields)
                    .HasForeignKey(d => d.Messagetypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MESSAGE_F__MESSA__2CBDA3B5");
            });

            modelBuilder.Entity<MessageTypes>(entity =>
            {
                entity.HasKey(e => e.Messagetypeid)
                    .HasName("PK__MESSAGE___190FD77CD4CB8969");

                entity.ToTable("MESSAGE_TYPES");

                entity.Property(e => e.Messagetypeid)
                    .HasColumnName("MESSAGETYPEID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Messagetypename)
                    .IsRequired()
                    .HasColumnName("MESSAGETYPENAME")
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<ProductType>(entity =>
            {
                entity.ToTable("Product_Type");

                entity.Property(e => e.ProductTypeId).ValueGeneratedNever();

                entity.Property(e => e.ProductTypeName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PublishedProductAllowedSubscriber>(entity =>
            {
                entity.ToTable("PUBLISHED_PRODUCT_ALLOWED_SUBSCRIBER");

                entity.Property(e => e.Publishedproductallowedsubscriberid)
                    .HasColumnName("PUBLISHEDPRODUCTALLOWEDSUBSCRIBERID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Publishedproductid).HasColumnName("PUBLISHEDPRODUCTID");

                entity.Property(e => e.Subscriberid).HasColumnName("SUBSCRIBERID");

                entity.HasOne(d => d.Publishedproduct)
                    .WithMany(p => p.PublishedProductAllowedSubscriber)
                    .HasForeignKey(d => d.Publishedproductid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PUBLISHED__PUBLI__60C757A0");

                entity.HasOne(d => d.Subscriber)
                    .WithMany(p => p.PublishedProductAllowedSubscriber)
                    .HasForeignKey(d => d.Subscriberid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PUBLISHED__SUBSC__61BB7BD9");
            });

            modelBuilder.Entity<PublishedProducts>(entity =>
            {
                entity.HasKey(e => e.PublishedProductId)
                    .HasName("PK__Publishe__D5B5099E189A69C9");

                entity.ToTable("Published_Products");

                entity.Property(e => e.PublishedProductId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Messagetypeid).HasColumnName("MESSAGETYPEID");

                entity.Property(e => e.ProcessedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ProcessedOn).HasColumnType("datetime");

                entity.Property(e => e.ProcessingNote)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.ProductData).IsRequired();

                entity.Property(e => e.Productdatadiff).HasColumnName("PRODUCTDATADIFF");

                entity.Property(e => e.Productsubstatusid).HasColumnName("PRODUCTSUBSTATUSID");

                entity.Property(e => e.PublishedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PublishedOn).HasColumnType("datetime");

                entity.Property(e => e.Publisherproductstatusid).HasColumnName("PUBLISHERPRODUCTSTATUSID");

                entity.Property(e => e.Traceid).HasColumnName("TRACEID");

                entity.HasOne(d => d.Messagetype)
                    .WithMany(p => p.PublishedProducts)
                    .HasForeignKey(d => d.Messagetypeid)
                    .HasConstraintName("FK__Published__MESSA__73501C2F");

                entity.HasOne(d => d.Productsubstatus)
                    .WithMany(p => p.PublishedProducts)
                    .HasForeignKey(d => d.Productsubstatusid)
                    .HasConstraintName("FK__Published__PRODU__41B8C09B");

                entity.HasOne(d => d.PublishedStatus)
                    .WithMany(p => p.PublishedProducts)
                    .HasForeignKey(d => d.PublishedStatusId)
                    .HasConstraintName("FK__Published__Publi__49C3F6B7");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.PublishedProducts)
                    .HasForeignKey(d => d.PublisherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Published__Publi__48CFD27E");

                entity.HasOne(d => d.Publisherproductstatus)
                    .WithMany(p => p.PublishedProducts)
                    .HasForeignKey(d => d.Publisherproductstatusid)
                    .HasConstraintName("FK__Published__PUBLI__6D6238AF");
            });

            modelBuilder.Entity<PublishedProductsHistory>(entity =>
            {
                entity.HasKey(e => e.PublishedProductHistoryId)
                    .HasName("PK__Publishe__8649C05817930AD2");

                entity.ToTable("Published_Products_History");

                entity.Property(e => e.PublishedProductHistoryId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Messagetypeid).HasColumnName("MESSAGETYPEID");

                entity.Property(e => e.ProcessedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ProcessedOn).HasColumnType("datetime");

                entity.Property(e => e.ProcessingNote)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.ProductData).IsRequired();

                entity.Property(e => e.Productdatadiff).HasColumnName("PRODUCTDATADIFF");

                entity.Property(e => e.Productsubstatusid).HasColumnName("PRODUCTSUBSTATUSID");

                entity.Property(e => e.CreatedBy).HasColumnName("CREATEDBY");

                entity.Property(e => e.PublishedOn).HasColumnType("datetime");

                entity.Property(e => e.Publishedproductqueueid).HasColumnName("PUBLISHEDPRODUCTQUEUEID");

                entity.Property(e => e.Publisherproductstatusid).HasColumnName("PUBLISHERPRODUCTSTATUSID");

                entity.Property(e => e.Traceid).HasColumnName("TRACEID");

                entity.HasOne(d => d.Messagetype)
                    .WithMany(p => p.PublishedProductsHistory)
                    .HasForeignKey(d => d.Messagetypeid)
                    .HasConstraintName("FK__Published__MESSA__74444068");

                entity.HasOne(d => d.Productsubstatus)
                    .WithMany(p => p.PublishedProductsHistory)
                    .HasForeignKey(d => d.Productsubstatusid)
                    .HasConstraintName("FK__Published__PRODU__42ACE4D4");

                entity.HasOne(d => d.Publisherproductstatus)
                    .WithMany(p => p.PublishedProductsHistory)
                    .HasForeignKey(d => d.Publisherproductstatusid)
                    .HasConstraintName("FK__Published__PUBLI__6E565CE8");
            });

            modelBuilder.Entity<PublishedProductsQueue>(entity =>
            {
                entity.HasKey(e => e.PublishedProductQueueId)
                    .HasName("PK__Publishe__1C61D019C3861C18");

                entity.ToTable("Published_Products_Queue");

                entity.Property(e => e.PublishedProductQueueId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreationDateTime).HasColumnType("datetime");

                entity.Property(e => e.JobEndDateTime).HasColumnType("datetime");

                entity.Property(e => e.JobStartDateTime).HasColumnType("datetime");

                entity.Property(e => e.Jobstatusid).HasColumnName("JOBSTATUSID");

                entity.Property(e => e.Jobtypeid).HasColumnName("JOBTYPEID");

                entity.Property(e => e.ProcessedBy).HasMaxLength(100);

                entity.Property(e => e.ProcessingNote).HasMaxLength(4000);

                entity.Property(e => e.Traceid).HasColumnName("TRACEID");

                entity.HasOne(d => d.Jobstatus)
                    .WithMany(p => p.PublishedProductsQueue)
                    .HasForeignKey(d => d.Jobstatusid)
                    .HasConstraintName("FK__Published__JOBST__6F4A8121");

                entity.HasOne(d => d.Jobtype)
                    .WithMany(p => p.PublishedProductsQueue)
                    .HasForeignKey(d => d.Jobtypeid)
                    .HasConstraintName("FK__Published__JOBTY__668030F6");

                entity.HasOne(d => d.PublishedProduct)
                    .WithMany(p => p.PublishedProductsQueue)
                    .HasForeignKey(d => d.PublishedProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Published__Publi__5165187F");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.PublishedProductsQueue)
                    .HasForeignKey(d => d.PublisherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Published__Publi__52593CB8");
            });

            modelBuilder.Entity<PublishedProductsQueueHistory>(entity =>
            {
                entity.ToTable("Published_Products_Queue_History");

                entity.Property(e => e.PublishedProductsQueueHistoryId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreationDateTime).HasColumnType("datetime");

                entity.Property(e => e.JobEndDateTime).HasColumnType("datetime");

                entity.Property(e => e.JobStartDateTime).HasColumnType("datetime");

                entity.Property(e => e.Jobhistorystatusid).HasColumnName("JOBHISTORYSTATUSID");

                entity.Property(e => e.Jobtypeid).HasColumnName("JOBTYPEID");

                entity.Property(e => e.ProcessedBy).HasMaxLength(100);

                entity.Property(e => e.ProcessingNote).HasMaxLength(4000);

                entity.Property(e => e.Productsubstatusid).HasColumnName("PRODUCTSUBSTATUSID");

                entity.Property(e => e.Publishedproductqueueid).HasColumnName("PUBLISHEDPRODUCTQUEUEID");

                entity.Property(e => e.Traceid).HasColumnName("TRACEID");

                entity.HasOne(d => d.Jobhistorystatus)
                    .WithMany(p => p.PublishedProductsQueueHistory)
                    .HasForeignKey(d => d.Jobhistorystatusid)
                    .HasConstraintName("FK__Published__JOBHI__703EA55A");

                entity.HasOne(d => d.Jobtype)
                    .WithMany(p => p.PublishedProductsQueueHistory)
                    .HasForeignKey(d => d.Jobtypeid)
                    .HasConstraintName("FK__Published__JOBTY__6774552F");

                entity.HasOne(d => d.Productsubstatus)
                    .WithMany(p => p.PublishedProductsQueueHistory)
                    .HasForeignKey(d => d.Productsubstatusid)
                    .HasConstraintName("FK__Published__PRODU__4460231C");
            });

            modelBuilder.Entity<PublishedStatus>(entity =>
            {
                entity.ToTable("Published_Status");

                entity.Property(e => e.PublishedStatusId).ValueGeneratedNever();

                entity.Property(e => e.PublishedStatusName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.HasIndex(e => e.PublisherName)
                    .HasName("UC_Publisher_name")
                    .IsUnique();

                entity.HasIndex(e => new { e.SiteId, e.OrganizationId })
                    .HasName("UC_Publisher_site_org")
                    .IsUnique();

                entity.Property(e => e.PublisherId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.PublisherName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.Publisher)
                    .HasForeignKey(d => d.SiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Publisher__SiteI__412EB0B6");
            });

            modelBuilder.Entity<PublisherAgent>(entity =>
            {
                entity.ToTable("Publisher_Agent");

                entity.Property(e => e.PublisherAgentId)
                    .HasColumnName("PublisherAgentID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.AgentId).HasColumnName("AgentID");

                entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.PublisherAgent)
                    .HasForeignKey(d => d.PublisherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Publisher__Publi__59FA5E80");

                entity.HasOne(d => d.SubscriberDNavigation)
                    .WithMany(p => p.PublisherAgent)
                    .HasForeignKey(d => d.SubscriberD)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Publisher__Subsc__5AEE82B9");
            });

            modelBuilder.Entity<PublisherDefault>(entity =>
            {
                entity.ToTable("Publisher_Default");

                entity.Property(e => e.PublisherDefaultId)
                    .HasColumnName("PublisherDefaultID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ContractDate).HasColumnType("datetime");

                entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.PublisherDefault)
                    .HasForeignKey(d => d.PublisherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Publisher__Publi__5EBF139D");
            });

            modelBuilder.Entity<PublisherProductStatus>(entity =>
            {
                entity.HasKey(e => e.Productstatusid)
                    .HasName("PK__PUBLISHE__298754F636BD19B2");

                entity.ToTable("PUBLISHER_PRODUCT_STATUS");

                entity.Property(e => e.Productstatusid)
                    .HasColumnName("PRODUCTSTATUSID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Productstatusname)
                    .IsRequired()
                    .HasColumnName("PRODUCTSTATUSNAME")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PublisherProductSubStatus>(entity =>
            {
                entity.HasKey(e => e.Productsubstatusid)
                    .HasName("PK__PUBLISHE__89C69A6289B35B00");

                entity.ToTable("PUBLISHER_PRODUCT_SUB_STATUS");

                entity.Property(e => e.Productsubstatusid)
                    .HasColumnName("PRODUCTSUBSTATUSID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Productsubstatusname)
                    .IsRequired()
                    .HasColumnName("PRODUCTSUBSTATUSNAME")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PublisherServiceStatus>(entity =>
            {
                entity.ToTable("Publisher_Service_Status");

                entity.Property(e => e.PublisherServiceStatusId)
                    .HasColumnName("PublisherServiceStatusID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

                entity.Property(e => e.ServiceStatusId).HasColumnName("ServiceStatusID");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.PublisherServiceStatus)
                    .HasForeignKey(d => d.PublisherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Publisher__Publi__628FA481");
            });

            modelBuilder.Entity<PublisherSupplierStatus>(entity =>
            {
                entity.ToTable("Publisher_Supplier_Status");

                entity.Property(e => e.PublisherSupplierStatusId)
                    .HasColumnName("PublisherSupplierStatusID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

                entity.Property(e => e.SupplierStatusId).HasColumnName("SupplierStatusID");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.PublisherSupplierStatus)
                    .HasForeignKey(d => d.PublisherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Publisher__Publi__66603565");
            });

            modelBuilder.Entity<Site>(entity =>
            {
                entity.HasIndex(e => e.SiteName)
                    .HasName("UC_Site_name")
                    .IsUnique();

                entity.HasIndex(e => e.Url)
                    .HasName("UC_Site_url")
                    .IsUnique();

                entity.Property(e => e.SiteId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.SiteName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<StaticDataType>(entity =>
            {
                entity.ToTable("STATIC_DATA_TYPE");

                entity.Property(e => e.Staticdatatypeid)
                    .HasColumnName("STATICDATATYPEID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Staticdatatypename)
                    .HasColumnName("STATICDATATYPENAME")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StaticDataUpdateQueue>(entity =>
            {
                entity.ToTable("STATIC_DATA_UPDATE_QUEUE");

                entity.Property(e => e.Staticdataupdatequeueid)
                    .HasColumnName("STATICDATAUPDATEQUEUEID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Jobcreationdatetime)
                    .HasColumnName("JOBCREATIONDATETIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Jobenddatetime)
                    .HasColumnName("JOBENDDATETIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Jobstartdatetime)
                    .HasColumnName("JOBSTARTDATETIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Jobstatusid).HasColumnName("JOBSTATUSID");

                entity.Property(e => e.Processingnote)
                    .IsRequired()
                    .HasColumnName("PROCESSINGNOTE")
                    .IsUnicode(false);

                entity.Property(e => e.Retrycount).HasColumnName("RETRYCOUNT");

                entity.Property(e => e.Serviceid)
                    .HasColumnName("SERVICEID")
                    .IsUnicode(false);

                entity.Property(e => e.Siteid).HasColumnName("SITEID");

                entity.Property(e => e.Staticdataid).HasColumnName("STATICDATAID");

                entity.Property(e => e.Staticdatatypeid).HasColumnName("STATICDATATYPEID");

                entity.HasOne(d => d.Jobstatus)
                    .WithMany(p => p.StaticDataUpdateQueue)
                    .HasForeignKey(d => d.Jobstatusid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__STATIC_DA__JOBST__77AABCF8");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.StaticDataUpdateQueue)
                    .HasForeignKey(d => d.Siteid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__STATIC_DA__SITEI__76B698BF");

                entity.HasOne(d => d.Staticdatatype)
                    .WithMany(p => p.StaticDataUpdateQueue)
                    .HasForeignKey(d => d.Staticdatatypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__STATIC_DA__STATI__75C27486");
            });

            modelBuilder.Entity<StaticDataUpdateQueueHistory>(entity =>
            {
                entity.HasKey(e => e.Staticdataupdatequeuehistory1)
                    .HasName("PK__STATIC_D__97471FDC84EFF9F6");

                entity.ToTable("STATIC_DATA_UPDATE_QUEUE_HISTORY");

                entity.Property(e => e.Staticdataupdatequeuehistory1)
                    .HasColumnName("STATICDATAUPDATEQUEUEHISTORY")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Jobcreationdatetime)
                    .HasColumnName("JOBCREATIONDATETIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Jobenddatetime)
                    .HasColumnName("JOBENDDATETIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Jobhistorystatusid).HasColumnName("JOBHISTORYSTATUSID");

                entity.Property(e => e.Jobstartdatetime)
                    .HasColumnName("JOBSTARTDATETIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Processingnote)
                    .IsRequired()
                    .HasColumnName("PROCESSINGNOTE")
                    .IsUnicode(false);

                entity.Property(e => e.Retrycount).HasColumnName("RETRYCOUNT");

                entity.Property(e => e.Serviceid)
                    .HasColumnName("SERVICEID")
                    .IsUnicode(false);

                entity.Property(e => e.Siteid).HasColumnName("SITEID");

                entity.Property(e => e.Staticdataid).HasColumnName("STATICDATAID");

                entity.Property(e => e.Staticdatatypeid).HasColumnName("STATICDATATYPEID");

                entity.Property(e => e.Staticdataupdatequeueid).HasColumnName("STATICDATAUPDATEQUEUEID");

                entity.HasOne(d => d.Jobhistorystatus)
                    .WithMany(p => p.StaticDataUpdateQueueHistory)
                    .HasForeignKey(d => d.Jobhistorystatusid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__STATIC_DA__JOBHI__031C6FA4");

                entity.HasOne(d => d.Staticdatatype)
                    .WithMany(p => p.StaticDataUpdateQueueHistory)
                    .HasForeignKey(d => d.Staticdatatypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__STATIC_DA__STATI__02284B6B");
            });

            modelBuilder.Entity<SubscribeProductStatus>(entity =>
            {
                entity.HasKey(e => e.Productstatusid)
                    .HasName("PK__SUBSCRIB__298754F62F724A61");

                entity.ToTable("SUBSCRIBE_PRODUCT_STATUS");

                entity.Property(e => e.Productstatusid)
                    .HasColumnName("PRODUCTSTATUSID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Productstatusname)
                    .HasColumnName("PRODUCTSTATUSNAME")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SubscribeProductSubStatus>(entity =>
            {
                entity.HasKey(e => e.Productsubstatusid)
                    .HasName("PK__SUBSCRIB__89C69A62F18678E7");

                entity.ToTable("SUBSCRIBE_PRODUCT_SUB_STATUS");

                entity.Property(e => e.Productsubstatusid)
                    .HasColumnName("PRODUCTSUBSTATUSID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Productsubstatusname)
                    .HasColumnName("PRODUCTSUBSTATUSNAME")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Subscriber>(entity =>
            {
                entity.HasIndex(e => new { e.SiteId, e.OrganizationId })
                    .HasName("UC_Subscriber_site_org")
                    .IsUnique();

                entity.Property(e => e.SubscriberId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.SubscriberName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.Subscriber)
                    .HasForeignKey(d => d.SiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Subscribe__SiteI__44FF419A");
            });

            modelBuilder.Entity<SubscriberChargingPolicy>(entity =>
            {
                entity.ToTable("Subscriber_Charging_policy");

                entity.Property(e => e.SubscriberChargingPolicyId)
                    .HasColumnName("SubscriberChargingPolicyID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ExtraChargingPolicyId).HasColumnName("ExtraChargingPolicyID");

                entity.Property(e => e.OptionChargingPolicyId).HasColumnName("OptionChargingPolicyID");

                entity.Property(e => e.ServiceTypeTypeId).HasColumnName("ServiceTypeTypeID");

                entity.Property(e => e.SubscriberId).HasColumnName("SubscriberID");

                entity.HasOne(d => d.Subscriber)
                    .WithMany(p => p.SubscriberChargingPolicy)
                    .HasForeignKey(d => d.SubscriberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Subscribe__Subsc__6A30C649");
            });

            modelBuilder.Entity<SubscriberDefault>(entity =>
            {
                entity.ToTable("Subscriber_Default");

                entity.Property(e => e.SubscriberDefaultId)
                    .HasColumnName("SubscriberDefaultID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.BuyBookingTypeId).HasColumnName("BuyBookingTypeID");

                entity.Property(e => e.BuyPriceTypeId).HasColumnName("BuyPriceTypeID");

                entity.Property(e => e.CommunicationTypeId).HasColumnName("CommunicationTypeID");

                entity.Property(e => e.Enddateoffsetdays).HasColumnName("ENDDATEOFFSETDAYS");

                entity.Property(e => e.SeasonTypeId).HasColumnName("SeasonTypeID");

                entity.Property(e => e.Startdateoffsetdays).HasColumnName("STARTDATEOFFSETDAYS");

                entity.Property(e => e.SubscriberId).HasColumnName("SubscriberID");

                entity.HasOne(d => d.Subscriber)
                    .WithMany(p => p.SubscriberDefault)
                    .HasForeignKey(d => d.SubscriberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Subscribe__Subsc__6E01572D");
            });

            modelBuilder.Entity<SubscriberDefaultSellingPricePolicies>(entity =>
            {
                entity.HasKey(e => e.Subscriberdefaultsellingpricepolicyid)
                    .HasName("PK__SUBSCRIB__C950DB7DDBA6C543");

                entity.ToTable("SUBSCRIBER_DEFAULT_SELLING_PRICE_POLICIES");

                entity.HasIndex(e => new { e.Subscriberdefaultsellingpriceid, e.Pricetypeid, e.Bookingtypeid })
                    .HasName("UC_SUBSCRIBER_DEFAULT_SELLING_PRICE_POLICIES")
                    .IsUnique();

                entity.Property(e => e.Subscriberdefaultsellingpricepolicyid).HasColumnName("SUBSCRIBERDEFAULTSELLINGPRICEPOLICYID");

                entity.Property(e => e.Bookingtypeid).HasColumnName("BOOKINGTYPEID");

                entity.Property(e => e.Pricetypeid).HasColumnName("PRICETYPEID");

                entity.Property(e => e.Subscriberdefaultsellingpriceid).HasColumnName("SUBSCRIBERDEFAULTSELLINGPRICEID");

                entity.Property(e => e.Taxid).HasColumnName("TAXID");

                entity.HasOne(d => d.Subscriberdefaultsellingprice)
                    .WithMany(p => p.SubscriberDefaultSellingPricePolicies)
                    .HasForeignKey(d => d.Subscriberdefaultsellingpriceid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__SUBSC__5B0E7E4A");
            });

            modelBuilder.Entity<SubscriberDefaultSellingPriceServiceType>(entity =>
            {
                entity.ToTable("SUBSCRIBER_DEFAULT_SELLING_PRICE_SERVICE_TYPE");

                entity.HasIndex(e => new { e.Subscriberdefaultsellingpriceid, e.Servicetypeid })
                    .HasName("UC_SUBSCRIBER_DEFAULT_SELLING_PRICE_SERVICE_TYPE")
                    .IsUnique();

                entity.Property(e => e.Subscriberdefaultsellingpriceservicetypeid).HasColumnName("SUBSCRIBERDEFAULTSELLINGPRICESERVICETYPEID");

                entity.Property(e => e.Servicetypeid).HasColumnName("SERVICETYPEID");

                entity.Property(e => e.Subscriberdefaultsellingpriceid).HasColumnName("SUBSCRIBERDEFAULTSELLINGPRICEID");

                entity.HasOne(d => d.Subscriberdefaultsellingprice)
                    .WithMany(p => p.SubscriberDefaultSellingPriceServiceType)
                    .HasForeignKey(d => d.Subscriberdefaultsellingpriceid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__SUBSC__573DED66");
            });

            modelBuilder.Entity<SubscriberDefaultSellingPrices>(entity =>
            {
                entity.HasKey(e => e.Subscriberdefaultsellingpriceid)
                    .HasName("PK__SUBSCRIB__D8A5FC0E6D88A866");

                entity.ToTable("SUBSCRIBER_DEFAULT_SELLING_PRICES");

                entity.Property(e => e.Subscriberdefaultsellingpriceid).HasColumnName("SUBSCRIBERDEFAULTSELLINGPRICEID");

                entity.Property(e => e.Regionid).HasColumnName("REGIONID");

                entity.Property(e => e.Sequence).HasColumnName("SEQUENCE");

                entity.Property(e => e.Subscriberid).HasColumnName("SUBSCRIBERID");

                entity.HasOne(d => d.Subscriber)
                    .WithMany(p => p.SubscriberDefaultSellingPrices)
                    .HasForeignKey(d => d.Subscriberid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__SUBSC__536D5C82");
            });

            modelBuilder.Entity<SubscriberProduct>(entity =>
            {
                entity.ToTable("SUBSCRIBER_PRODUCT");

                entity.Property(e => e.Subscriberproductid)
                    .HasColumnName("SUBSCRIBERPRODUCTID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Marketplaceproductid).HasColumnName("MARKETPLACEPRODUCTID");

                entity.Property(e => e.Messagetypeid).HasColumnName("MESSAGETYPEID");

                entity.Property(e => e.Processedby)
                    .IsRequired()
                    .HasColumnName("PROCESSEDBY")
                    .HasMaxLength(50);

                entity.Property(e => e.Processedon)
                    .HasColumnName("PROCESSEDON")
                    .HasColumnType("datetime");

                entity.Property(e => e.Productdata)
                    .IsRequired()
                    .HasColumnName("PRODUCTDATA");

                entity.Property(e => e.Productdatadiff).HasColumnName("PRODUCTDATADIFF");

                entity.Property(e => e.Productstatusid).HasColumnName("PRODUCTSTATUSID");

                entity.Property(e => e.Productstatusnote)
                    .HasColumnName("PRODUCTSTATUSNOTE")
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Productsubstatusid).HasColumnName("PRODUCTSUBSTATUSID");

                entity.Property(e => e.Productversion).HasColumnName("PRODUCTVERSION");

                entity.Property(e => e.Subscribedby)
                    .IsRequired()
                    .HasColumnName("SUBSCRIBEDBY")
                    .HasMaxLength(50);

                entity.Property(e => e.Subscribedon)
                    .HasColumnName("SUBSCRIBEDON")
                    .HasColumnType("datetime");

                entity.Property(e => e.Subscriberid).HasColumnName("SUBSCRIBERID");

                entity.Property(e => e.Traceid).HasColumnName("TRACEID");

                entity.Property(e => e.Tsid).HasColumnName("TSID");

                entity.HasOne(d => d.Marketplaceproduct)
                    .WithMany(p => p.SubscriberProduct)
                    .HasForeignKey(d => d.Marketplaceproductid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__MARKE__25DB9BFC");

                entity.HasOne(d => d.Messagetype)
                    .WithMany(p => p.SubscriberProduct)
                    .HasForeignKey(d => d.Messagetypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__MESSA__28B808A7");

                entity.HasOne(d => d.Productstatus)
                    .WithMany(p => p.SubscriberProduct)
                    .HasForeignKey(d => d.Productstatusid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__PRODU__27C3E46E");

                entity.HasOne(d => d.Productsubstatus)
                    .WithMany(p => p.SubscriberProduct)
                    .HasForeignKey(d => d.Productsubstatusid)
                    .HasConstraintName("FK__SUBSCRIBE__PRODU__29AC2CE0");

                entity.HasOne(d => d.Subscriber)
                    .WithMany(p => p.SubscriberProduct)
                    .HasForeignKey(d => d.Subscriberid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__SUBSC__26CFC035");
            });

            modelBuilder.Entity<SubscriberProductCode>(entity =>
            {
                entity.ToTable("Subscriber_Product_Code");

                entity.Property(e => e.SubscriberProductCodeId)
                    .HasColumnName("SubscriberProductCodeID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ApplytoExtras)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ApplytoOptions)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ProductCodeId).HasColumnName("ProductCodeID");

                entity.Property(e => e.RegionId).HasColumnName("RegionID");

                entity.Property(e => e.SubscriberId).HasColumnName("subscriberID");

                entity.HasOne(d => d.Subscriber)
                    .WithMany(p => p.SubscriberProductCode)
                    .HasForeignKey(d => d.SubscriberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Subscribe__subsc__71D1E811");
            });

            modelBuilder.Entity<SubscriberProductCodeServiceType>(entity =>
            {
                entity.ToTable("Subscriber_Product_Code_Service_type");

                entity.Property(e => e.SubscriberProductCodeServiceTypeId)
                    .HasColumnName("SubscriberProductCodeServiceTypeID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ServiceTypeId).HasColumnName("ServiceTypeID");

                entity.Property(e => e.SubscriberProductCodeId).HasColumnName("SubscriberProductCodeID");

                entity.HasOne(d => d.SubscriberProductCode)
                    .WithMany(p => p.SubscriberProductCodeServiceType)
                    .HasForeignKey(d => d.SubscriberProductCodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Subscribe__Subsc__02084FDA");
            });

            modelBuilder.Entity<SubscriberProductHistory>(entity =>
            {
                entity.ToTable("SUBSCRIBER_PRODUCT_HISTORY");

                entity.Property(e => e.Subscriberproducthistoryid)
                    .HasColumnName("SUBSCRIBERPRODUCTHISTORYID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Marketplaceproductid).HasColumnName("MARKETPLACEPRODUCTID");

                entity.Property(e => e.Messagetypeid).HasColumnName("MESSAGETYPEID");

                entity.Property(e => e.Processedby)
                    .IsRequired()
                    .HasColumnName("PROCESSEDBY")
                    .HasMaxLength(50);

                entity.Property(e => e.Processedon)
                    .HasColumnName("PROCESSEDON")
                    .HasColumnType("datetime");

                entity.Property(e => e.Productdata)
                    .IsRequired()
                    .HasColumnName("PRODUCTDATA");

                entity.Property(e => e.Productdatadiff).HasColumnName("PRODUCTDATADIFF");

                entity.Property(e => e.Productstatusid).HasColumnName("PRODUCTSTATUSID");

                entity.Property(e => e.Productstatusnote)
                    .HasColumnName("PRODUCTSTATUSNOTE")
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Productsubstatusid).HasColumnName("PRODUCTSUBSTATUSID");

                entity.Property(e => e.Productversion).HasColumnName("PRODUCTVERSION");

                entity.Property(e => e.CreatedBy).HasColumnName("CREATEDBY");

                entity.Property(e => e.Subscribedon)
                    .HasColumnName("SUBSCRIBEDON")
                    .HasColumnType("datetime");

                entity.Property(e => e.Subscriberid).HasColumnName("SUBSCRIBERID");

                entity.Property(e => e.Subscriberproductid).HasColumnName("SUBSCRIBERPRODUCTID");

                entity.Property(e => e.Subscriberproductqueueid).HasColumnName("SUBSCRIBERPRODUCTQUEUEID");

                entity.Property(e => e.Traceid).HasColumnName("TRACEID");

                entity.HasOne(d => d.Marketplaceproduct)
                    .WithMany(p => p.SubscriberProductHistory)
                    .HasForeignKey(d => d.Marketplaceproductid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__MARKE__2E70E1FD");

                entity.HasOne(d => d.Messagetype)
                    .WithMany(p => p.SubscriberProductHistory)
                    .HasForeignKey(d => d.Messagetypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__MESSA__314D4EA8");

                entity.HasOne(d => d.Productstatus)
                    .WithMany(p => p.SubscriberProductHistory)
                    .HasForeignKey(d => d.Productstatusid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__PRODU__30592A6F");

                entity.HasOne(d => d.Productsubstatus)
                    .WithMany(p => p.SubscriberProductHistory)
                    .HasForeignKey(d => d.Productsubstatusid)
                    .HasConstraintName("FK__SUBSCRIBE__PRODU__324172E1");

                entity.HasOne(d => d.Subscriber)
                    .WithMany(p => p.SubscriberProductHistory)
                    .HasForeignKey(d => d.Subscriberid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__SUBSC__2F650636");

                entity.HasOne(d => d.Subscriberproduct)
                    .WithMany(p => p.SubscriberProductHistory)
                    .HasForeignKey(d => d.Subscriberproductid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__SUBSC__2D7CBDC4");
            });

            modelBuilder.Entity<SubscriberProductQueue>(entity =>
            {
                entity.ToTable("SUBSCRIBER_PRODUCT_QUEUE");

                entity.Property(e => e.Subscriberproductqueueid)
                    .HasColumnName("SUBSCRIBERPRODUCTQUEUEID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Jobcreationdatetime)
                    .HasColumnName("JOBCREATIONDATETIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Jobenddatetime)
                    .HasColumnName("JOBENDDATETIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Jobnote)
                    .HasColumnName("JOBNOTE")
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Jobstartdatetime)
                    .HasColumnName("JOBSTARTDATETIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Jobstatusid).HasColumnName("JOBSTATUSID");

                entity.Property(e => e.Jobtypeid).HasColumnName("JOBTYPEID");

                entity.Property(e => e.Marketplaceproductid).HasColumnName("MARKETPLACEPRODUCTID");

                entity.Property(e => e.Messagetypeid).HasColumnName("MESSAGETYPEID");

                entity.Property(e => e.Processedby)
                    .HasColumnName("PROCESSEDBY")
                    .HasMaxLength(100);

                entity.Property(e => e.Retrycount).HasColumnName("RETRYCOUNT");

                entity.Property(e => e.Subscriberid).HasColumnName("SUBSCRIBERID");

                entity.Property(e => e.Subscriberproductid).HasColumnName("SUBSCRIBERPRODUCTID");

                entity.Property(e => e.Traceid).HasColumnName("TRACEID");

                entity.HasOne(d => d.Jobstatus)
                    .WithMany(p => p.SubscriberProductQueue)
                    .HasForeignKey(d => d.Jobstatusid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__JOBST__04459E07");

                entity.HasOne(d => d.Jobtype)
                    .WithMany(p => p.SubscriberProductQueue)
                    .HasForeignKey(d => d.Jobtypeid)
                    .HasConstraintName("FK__SUBSCRIBE__JOBTY__6497E884");

                entity.HasOne(d => d.Marketplaceproduct)
                    .WithMany(p => p.SubscriberProductQueue)
                    .HasForeignKey(d => d.Marketplaceproductid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__MARKE__025D5595");

                entity.HasOne(d => d.Messagetype)
                    .WithMany(p => p.SubscriberProductQueue)
                    .HasForeignKey(d => d.Messagetypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__MESSA__0539C240");

                entity.HasOne(d => d.Subscriber)
                    .WithMany(p => p.SubscriberProductQueue)
                    .HasForeignKey(d => d.Subscriberid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__SUBSC__035179CE");

                entity.HasOne(d => d.Subscriberproduct)
                    .WithMany(p => p.SubscriberProductQueue)
                    .HasForeignKey(d => d.Subscriberproductid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__SUBSC__0169315C");
            });

            modelBuilder.Entity<SubscriberProductQueueHistory>(entity =>
            {
                entity.ToTable("SUBSCRIBER_PRODUCT_QUEUE_HISTORY");

                entity.Property(e => e.Subscriberproductqueuehistoryid)
                    .HasColumnName("SUBSCRIBERPRODUCTQUEUEHISTORYID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Jobcreationdatetime)
                    .HasColumnName("JOBCREATIONDATETIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Jobenddatetime)
                    .HasColumnName("JOBENDDATETIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Jobhistorystatusid).HasColumnName("JOBHISTORYSTATUSID");

                entity.Property(e => e.Jobnote)
                    .HasColumnName("JOBNOTE")
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Jobstartdatetime)
                    .HasColumnName("JOBSTARTDATETIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Jobtypeid).HasColumnName("JOBTYPEID");

                entity.Property(e => e.Marketplaceproductid).HasColumnName("MARKETPLACEPRODUCTID");

                entity.Property(e => e.Messagetypeid).HasColumnName("MESSAGETYPEID");

                entity.Property(e => e.Processedby)
                    .HasColumnName("PROCESSEDBY")
                    .HasMaxLength(100);

                entity.Property(e => e.Productsubstatusid).HasColumnName("PRODUCTSUBSTATUSID");

                entity.Property(e => e.Retrycount).HasColumnName("RETRYCOUNT");

                entity.Property(e => e.Subscriberid).HasColumnName("SUBSCRIBERID");

                entity.Property(e => e.Subscriberproductid).HasColumnName("SUBSCRIBERPRODUCTID");

                entity.Property(e => e.Subscriberproductqueueid).HasColumnName("SUBSCRIBERPRODUCTQUEUEID");

                entity.Property(e => e.Traceid).HasColumnName("TRACEID");

                entity.HasOne(d => d.Jobhistorystatus)
                    .WithMany(p => p.SubscriberProductQueueHistory)
                    .HasForeignKey(d => d.Jobhistorystatusid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__JOBHI__0BE6BFCF");

                entity.HasOne(d => d.Jobtype)
                    .WithMany(p => p.SubscriberProductQueueHistory)
                    .HasForeignKey(d => d.Jobtypeid)
                    .HasConstraintName("FK__SUBSCRIBE__JOBTY__658C0CBD");

                entity.HasOne(d => d.Marketplaceproduct)
                    .WithMany(p => p.SubscriberProductQueueHistory)
                    .HasForeignKey(d => d.Marketplaceproductid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__MARKE__09FE775D");

                entity.HasOne(d => d.Messagetype)
                    .WithMany(p => p.SubscriberProductQueueHistory)
                    .HasForeignKey(d => d.Messagetypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__MESSA__0CDAE408");

                entity.HasOne(d => d.Productsubstatus)
                    .WithMany(p => p.SubscriberProductQueueHistory)
                    .HasForeignKey(d => d.Productsubstatusid)
                    .HasConstraintName("FK__SUBSCRIBE__PRODU__0DCF0841");

                entity.HasOne(d => d.Subscriber)
                    .WithMany(p => p.SubscriberProductQueueHistory)
                    .HasForeignKey(d => d.Subscriberid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__SUBSC__0AF29B96");

                entity.HasOne(d => d.Subscriberproduct)
                    .WithMany(p => p.SubscriberProductQueueHistory)
                    .HasForeignKey(d => d.Subscriberproductid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__SUBSC__090A5324");
            });

            modelBuilder.Entity<SubscriberProductTsUpdateQueue>(entity =>
            {
                entity.ToTable("SUBSCRIBER_PRODUCT_TS_UPDATE_QUEUE");

                entity.Property(e => e.Subscriberproducttsupdatequeueid)
                    .HasColumnName("SUBSCRIBERPRODUCTTSUPDATEQUEUEID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Jobcreationdatetime)
                    .HasColumnName("JOBCREATIONDATETIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Jobenddatetime)
                    .HasColumnName("JOBENDDATETIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Jobnote)
                    .HasColumnName("JOBNOTE")
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Jobstartdatetime)
                    .HasColumnName("JOBSTARTDATETIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Jobstatusid).HasColumnName("JOBSTATUSID");

                entity.Property(e => e.Marketplaceproductid).HasColumnName("MARKETPLACEPRODUCTID");

                entity.Property(e => e.Messagetypeid).HasColumnName("MESSAGETYPEID");

                entity.Property(e => e.Retrycount).HasColumnName("RETRYCOUNT");

                entity.Property(e => e.Subscriberid).HasColumnName("SUBSCRIBERID");

                entity.Property(e => e.Tsid).HasColumnName("TSID");

                entity.HasOne(d => d.Jobstatus)
                    .WithMany(p => p.SubscriberProductTsUpdateQueue)
                    .HasForeignKey(d => d.Jobstatusid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__JOBST__36D11DD4");

                entity.HasOne(d => d.Marketplaceproduct)
                    .WithMany(p => p.SubscriberProductTsUpdateQueue)
                    .HasForeignKey(d => d.Marketplaceproductid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__MARKE__34E8D562");

                entity.HasOne(d => d.Messagetype)
                    .WithMany(p => p.SubscriberProductTsUpdateQueue)
                    .HasForeignKey(d => d.Messagetypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__MESSA__37C5420D");

                entity.HasOne(d => d.Subscriber)
                    .WithMany(p => p.SubscriberProductTsUpdateQueue)
                    .HasForeignKey(d => d.Subscriberid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__SUBSC__35DCF99B");
            });

            modelBuilder.Entity<SubscriberProductTsUpdateQueueHistory>(entity =>
            {
                entity.ToTable("SUBSCRIBER_PRODUCT_TS_UPDATE_QUEUE_HISTORY");

                entity.Property(e => e.Subscriberproducttsupdatequeuehistoryid)
                    .HasColumnName("SUBSCRIBERPRODUCTTSUPDATEQUEUEHISTORYID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Jobcreationdatetime)
                    .HasColumnName("JOBCREATIONDATETIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Jobenddatetime)
                    .HasColumnName("JOBENDDATETIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Jobhistorystatusid).HasColumnName("JOBHISTORYSTATUSID");

                entity.Property(e => e.Jobnote)
                    .HasColumnName("JOBNOTE")
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Jobstartdatetime)
                    .HasColumnName("JOBSTARTDATETIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Marketplaceproductid).HasColumnName("MARKETPLACEPRODUCTID");

                entity.Property(e => e.Messagetypeid).HasColumnName("MESSAGETYPEID");

                entity.Property(e => e.Retrycount).HasColumnName("RETRYCOUNT");

                entity.Property(e => e.Subscriberid).HasColumnName("SUBSCRIBERID");

                entity.Property(e => e.Traceid).HasColumnName("TRACEID");

                entity.Property(e => e.Tsid).HasColumnName("TSID");

                entity.HasOne(d => d.Jobhistorystatus)
                    .WithMany(p => p.SubscriberProductTsUpdateQueueHistory)
                    .HasForeignKey(d => d.Jobhistorystatusid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__JOBHI__3D7E1B63");

                entity.HasOne(d => d.Marketplaceproduct)
                    .WithMany(p => p.SubscriberProductTsUpdateQueueHistory)
                    .HasForeignKey(d => d.Marketplaceproductid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__MARKE__3B95D2F1");

                entity.HasOne(d => d.Messagetype)
                    .WithMany(p => p.SubscriberProductTsUpdateQueueHistory)
                    .HasForeignKey(d => d.Messagetypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__MESSA__3E723F9C");

                entity.HasOne(d => d.Subscriber)
                    .WithMany(p => p.SubscriberProductTsUpdateQueueHistory)
                    .HasForeignKey(d => d.Subscriberid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SUBSCRIBE__SUBSC__3C89F72A");
            });

            modelBuilder.Entity<SubscriberSupplier>(entity =>
            {
                entity.ToTable("Subscriber_Supplier");

                entity.Property(e => e.SubscriberSupplierId)
                    .HasColumnName("SubscriberSupplierID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

                entity.Property(e => e.SubscriberId).HasColumnName("SubscriberID");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.SubscriberSupplier)
                    .HasForeignKey(d => d.PublisherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Subscribe__Publi__7B5B524B");

                entity.HasOne(d => d.Subscriber)
                    .WithMany(p => p.SubscriberSupplier)
                    .HasForeignKey(d => d.SubscriberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Subscribe__Subsc__7A672E12");
            });

            modelBuilder.Entity<TransactionLog>(entity =>
            {
                entity.HasKey(e => e.TransactionId)
                    .HasName("PK__Transact__55433A6BAC1A07EF");

                entity.ToTable("Transaction_Log");

                entity.Property(e => e.TransactionId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.InitiatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.InitiatedOn).HasColumnType("datetime");

                entity.Property(e => e.TransactionData).IsRequired();

                entity.Property(e => e.TransactionStatus)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TransactionType)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
