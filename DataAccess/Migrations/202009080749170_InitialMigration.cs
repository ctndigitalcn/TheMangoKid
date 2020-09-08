namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Email = c.String(nullable: false, unicode: false),
                        Password = c.String(nullable: false, unicode: false),
                        Account_Creation_Date = c.DateTime(nullable: false),
                        Account_Status = c.Byte(nullable: false),
                        Account_Role = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Role", t => t.Account_Role, cascadeDelete: true)
                .Index(t => t.Account_Role);
            
            CreateTable(
                "dbo.BankDetail",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Account_Id = c.Guid(),
                        PayeeFirstName = c.String(maxLength: 50, unicode: false),
                        PayeeLastName = c.String(maxLength: 50, unicode: false),
                        PayeeBankName = c.String(unicode: false),
                        PayeeBankIfscNumber = c.String(maxLength: 50, unicode: false),
                        PayeeBankBranch = c.String(unicode: false),
                        PayeeBankAccountNumber = c.String(maxLength: 50, unicode: false),
                        Detail_Submitted_At = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.Account_Id, cascadeDelete: true)
                .Index(t => t.Account_Id);
            
            CreateTable(
                "dbo.Coupon",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Coupon_Code = c.String(maxLength: 50, unicode: false),
                        Generated_At = c.DateTime(),
                        Expire_At = c.DateTime(),
                        Discount_Percentage = c.Int(),
                        Quantity = c.Int(),
                        Created_By = c.Guid(),
                        TobeAppliedOnCategory = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.Created_By, cascadeDelete: true)
                .Index(t => t.Created_By);
            
            CreateTable(
                "dbo.PurchaseRecord",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Account_Id = c.Guid(),
                        Purchased_Category = c.String(maxLength: 15, unicode: false),
                        PurchaseDate = c.DateTime(),
                        Usage_Date = c.DateTime(),
                        Usage_Exp_Date = c.DateTime(),
                        CouponCodeApplied = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Coupon", t => t.CouponCodeApplied)
                .ForeignKey("dbo.Account", t => t.Account_Id, cascadeDelete: true)
                .Index(t => t.Account_Id)
                .Index(t => t.CouponCodeApplied);
            
            CreateTable(
                "dbo.Album",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Album_Name = c.String(maxLength: 50, unicode: false),
                        Album_Creation_Date = c.DateTime(),
                        Total_Track = c.Int(),
                        Submitted_Track = c.Int(),
                        PurchaseTrack_RefNo = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PurchaseRecord", t => t.PurchaseTrack_RefNo, cascadeDelete: true)
                .Index(t => t.PurchaseTrack_RefNo);
            
            CreateTable(
                "dbo.AlbumTrackMaster",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Album_Id = c.Guid(),
                        Track_Id = c.Guid(),
                        Submitted_At = c.DateTime(),
                        StoreSubmissionStatus = c.Byte(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SingleTrackDetail", t => t.Track_Id, cascadeDelete: true)
                .ForeignKey("dbo.Album", t => t.Album_Id, cascadeDelete: true)
                .Index(t => t.Album_Id)
                .Index(t => t.Track_Id);
            
            CreateTable(
                "dbo.SingleTrackDetail",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TrackTitle = c.String(maxLength: 50, unicode: false),
                        ArtistName = c.String(unicode: false, storeType: "text"),
                        ArtistAlreadyInSpotify = c.Byte(),
                        ArtistSpotifyUrl = c.String(unicode: false),
                        ReleaseDate = c.DateTime(storeType: "date"),
                        Genre = c.String(maxLength: 50, unicode: false),
                        CopyrightClaimerName = c.String(maxLength: 50, unicode: false),
                        AuthorName = c.String(maxLength: 50, unicode: false),
                        ComposerName = c.String(maxLength: 50, unicode: false),
                        ArrangerName = c.String(maxLength: 50, unicode: false),
                        ProducerName = c.String(maxLength: 50),
                        AlreadyHaveAnISRC = c.Byte(),
                        ISRC_Number = c.String(maxLength: 50, unicode: false),
                        PriceTier = c.Int(),
                        ExplicitContent = c.Byte(),
                        IsTrackInstrumental = c.Byte(),
                        LyricsLanguage = c.String(maxLength: 50, unicode: false),
                        TrackZipFileLink = c.String(unicode: false, storeType: "text"),
                        ArtworkFileLink = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EpTrackMaster",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Ep_Id = c.Guid(),
                        Track_Id = c.Guid(),
                        Submitted_At = c.DateTime(),
                        StoreSubmissionStatus = c.Byte(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExtendedPlay", t => t.Ep_Id, cascadeDelete: true)
                .ForeignKey("dbo.SingleTrackDetail", t => t.Track_Id, cascadeDelete: true)
                .Index(t => t.Ep_Id)
                .Index(t => t.Track_Id);
            
            CreateTable(
                "dbo.ExtendedPlay",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Ep_Name = c.String(maxLength: 50, unicode: false),
                        Ep_Creation_Date = c.DateTime(),
                        Total_Track = c.Int(),
                        Submitted_Track = c.Int(),
                        PurchaseTrack_RefNo = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PurchaseRecord", t => t.PurchaseTrack_RefNo, cascadeDelete: true)
                .Index(t => t.PurchaseTrack_RefNo);
            
            CreateTable(
                "dbo.SoloTrackMaster",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Track_Id = c.Guid(),
                        PurchaseTrack_RefNo = c.Guid(),
                        Submitted_At = c.DateTime(),
                        StoreSubmissionStatus = c.Byte(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SingleTrackDetail", t => t.Track_Id, cascadeDelete: true)
                .ForeignKey("dbo.PurchaseRecord", t => t.PurchaseTrack_RefNo, cascadeDelete: true)
                .Index(t => t.Track_Id)
                .Index(t => t.PurchaseTrack_RefNo);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Role_Name = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserDetail",
                c => new
                    {
                        User_Account_Id = c.Guid(nullable: false),
                        User_First_Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        User_Last_Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        User_Mobile_Number = c.String(nullable: false, maxLength: 13, unicode: false),
                        User_Address = c.String(nullable: false, unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.User_Account_Id)
                .ForeignKey("dbo.Account", t => t.User_Account_Id, cascadeDelete: true)
                .Index(t => t.User_Account_Id);
            
            CreateTable(
                "dbo.PriceInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Category_Name = c.String(maxLength: 50, unicode: false),
                        BasePrice = c.Decimal(precision: 18, scale: 2),
                        Gst = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDetail", "User_Account_Id", "dbo.Account");
            DropForeignKey("dbo.Account", "Account_Role", "dbo.Role");
            DropForeignKey("dbo.PurchaseRecord", "Account_Id", "dbo.Account");
            DropForeignKey("dbo.Coupon", "Created_By", "dbo.Account");
            DropForeignKey("dbo.PurchaseRecord", "CouponCodeApplied", "dbo.Coupon");
            DropForeignKey("dbo.SoloTrackMaster", "PurchaseTrack_RefNo", "dbo.PurchaseRecord");
            DropForeignKey("dbo.ExtendedPlay", "PurchaseTrack_RefNo", "dbo.PurchaseRecord");
            DropForeignKey("dbo.Album", "PurchaseTrack_RefNo", "dbo.PurchaseRecord");
            DropForeignKey("dbo.AlbumTrackMaster", "Album_Id", "dbo.Album");
            DropForeignKey("dbo.SoloTrackMaster", "Track_Id", "dbo.SingleTrackDetail");
            DropForeignKey("dbo.EpTrackMaster", "Track_Id", "dbo.SingleTrackDetail");
            DropForeignKey("dbo.EpTrackMaster", "Ep_Id", "dbo.ExtendedPlay");
            DropForeignKey("dbo.AlbumTrackMaster", "Track_Id", "dbo.SingleTrackDetail");
            DropForeignKey("dbo.BankDetail", "Account_Id", "dbo.Account");
            DropIndex("dbo.UserDetail", new[] { "User_Account_Id" });
            DropIndex("dbo.SoloTrackMaster", new[] { "PurchaseTrack_RefNo" });
            DropIndex("dbo.SoloTrackMaster", new[] { "Track_Id" });
            DropIndex("dbo.ExtendedPlay", new[] { "PurchaseTrack_RefNo" });
            DropIndex("dbo.EpTrackMaster", new[] { "Track_Id" });
            DropIndex("dbo.EpTrackMaster", new[] { "Ep_Id" });
            DropIndex("dbo.AlbumTrackMaster", new[] { "Track_Id" });
            DropIndex("dbo.AlbumTrackMaster", new[] { "Album_Id" });
            DropIndex("dbo.Album", new[] { "PurchaseTrack_RefNo" });
            DropIndex("dbo.PurchaseRecord", new[] { "CouponCodeApplied" });
            DropIndex("dbo.PurchaseRecord", new[] { "Account_Id" });
            DropIndex("dbo.Coupon", new[] { "Created_By" });
            DropIndex("dbo.BankDetail", new[] { "Account_Id" });
            DropIndex("dbo.Account", new[] { "Account_Role" });
            DropTable("dbo.PriceInfo");
            DropTable("dbo.UserDetail");
            DropTable("dbo.Role");
            DropTable("dbo.SoloTrackMaster");
            DropTable("dbo.ExtendedPlay");
            DropTable("dbo.EpTrackMaster");
            DropTable("dbo.SingleTrackDetail");
            DropTable("dbo.AlbumTrackMaster");
            DropTable("dbo.Album");
            DropTable("dbo.PurchaseRecord");
            DropTable("dbo.Coupon");
            DropTable("dbo.BankDetail");
            DropTable("dbo.Account");
        }
    }
}
