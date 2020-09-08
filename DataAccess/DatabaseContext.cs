namespace DataAccess
{
    using System.Data.Entity;
    using DomainModel;

    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("name=DatabaseContext")
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<AlbumTrackMaster> AlbumTrackMasters { get; set; }
        public virtual DbSet<BankDetail> BankDetails { get; set; }
        public virtual DbSet<Coupon> Coupons { get; set; }
        public virtual DbSet<EpTrackMaster> EpTrackMasters { get; set; }
        public virtual DbSet<ExtendedPlay> ExtendedPlays { get; set; }
        public virtual DbSet<PriceInfo> PriceInfoes { get; set; }
        public virtual DbSet<PurchaseRecord> PurchaseRecords { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SingleTrackDetail> SingleTrackDetails { get; set; }
        public virtual DbSet<SoloTrackMaster> SoloTrackMasters { get; set; }
        public virtual DbSet<UserDetail> UserDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.BankDetails)
                .WithOptional(e => e.Account)
                .HasForeignKey(e => e.Account_Id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Coupons)
                .WithOptional(e => e.Account)
                .HasForeignKey(e => e.Created_By)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Account>()
                .HasMany(e => e.PurchaseRecords)
                .WithOptional(e => e.Account)
                .HasForeignKey(e => e.Account_Id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Account>()
                .HasOptional(e => e.UserDetail)
                .WithRequired(e => e.Account)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Album>()
                .Property(e => e.Album_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Album>()
                .HasMany(e => e.AlbumTrackMasters)
                .WithOptional(e => e.Album)
                .HasForeignKey(e => e.Album_Id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<BankDetail>()
                .Property(e => e.PayeeFirstName)
                .IsUnicode(false);

            modelBuilder.Entity<BankDetail>()
                .Property(e => e.PayeeLastName)
                .IsUnicode(false);

            modelBuilder.Entity<BankDetail>()
                .Property(e => e.PayeeBankName)
                .IsUnicode(false);

            modelBuilder.Entity<BankDetail>()
                .Property(e => e.PayeeBankIfscNumber)
                .IsUnicode(false);

            modelBuilder.Entity<BankDetail>()
                .Property(e => e.PayeeBankBranch)
                .IsUnicode(false);

            modelBuilder.Entity<BankDetail>()
                .Property(e => e.PayeeBankAccountNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Coupon>()
                .Property(e => e.Coupon_Code)
                .IsUnicode(false);

            modelBuilder.Entity<Coupon>()
                .Property(e => e.TobeAppliedOnCategory)
                .IsUnicode(false);

            modelBuilder.Entity<Coupon>()
                .HasMany(e => e.PurchaseRecords)
                .WithOptional(e => e.Coupon)
                .HasForeignKey(e => e.CouponCodeApplied);

            modelBuilder.Entity<ExtendedPlay>()
                .Property(e => e.Ep_Name)
                .IsUnicode(false);

            modelBuilder.Entity<ExtendedPlay>()
                .HasMany(e => e.EpTrackMasters)
                .WithOptional(e => e.ExtendedPlay)
                .HasForeignKey(e => e.Ep_Id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<PriceInfo>()
                .Property(e => e.Category_Name)
                .IsUnicode(false);

            modelBuilder.Entity<PurchaseRecord>()
                .Property(e => e.Purchased_Category)
                .IsUnicode(false);

            modelBuilder.Entity<PurchaseRecord>()
                .HasMany(e => e.Albums)
                .WithOptional(e => e.PurchaseRecord)
                .HasForeignKey(e => e.PurchaseTrack_RefNo)
                .WillCascadeOnDelete();

            modelBuilder.Entity<PurchaseRecord>()
                .HasMany(e => e.ExtendedPlays)
                .WithOptional(e => e.PurchaseRecord)
                .HasForeignKey(e => e.PurchaseTrack_RefNo)
                .WillCascadeOnDelete();

            modelBuilder.Entity<PurchaseRecord>()
                .HasMany(e => e.SoloTrackMasters)
                .WithOptional(e => e.PurchaseRecord)
                .HasForeignKey(e => e.PurchaseTrack_RefNo)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Role>()
                .Property(e => e.Role_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Accounts)
                .WithOptional(e => e.Role)
                .HasForeignKey(e => e.Account_Role)
                .WillCascadeOnDelete();

            modelBuilder.Entity<SingleTrackDetail>()
                .Property(e => e.TrackTitle)
                .IsUnicode(false);

            modelBuilder.Entity<SingleTrackDetail>()
                .Property(e => e.ArtistName)
                .IsUnicode(false);

            modelBuilder.Entity<SingleTrackDetail>()
                .Property(e => e.ArtistSpotifyUrl)
                .IsUnicode(false);

            modelBuilder.Entity<SingleTrackDetail>()
                .Property(e => e.Genre)
                .IsUnicode(false);

            modelBuilder.Entity<SingleTrackDetail>()
                .Property(e => e.CopyrightClaimerName)
                .IsUnicode(false);

            modelBuilder.Entity<SingleTrackDetail>()
                .Property(e => e.AuthorName)
                .IsUnicode(false);

            modelBuilder.Entity<SingleTrackDetail>()
                .Property(e => e.ComposerName)
                .IsUnicode(false);

            modelBuilder.Entity<SingleTrackDetail>()
                .Property(e => e.ArrangerName)
                .IsUnicode(false);

            modelBuilder.Entity<SingleTrackDetail>()
                .Property(e => e.ISRC_Number)
                .IsUnicode(false);

            modelBuilder.Entity<SingleTrackDetail>()
                .Property(e => e.LyricsLanguage)
                .IsUnicode(false);

            modelBuilder.Entity<SingleTrackDetail>()
                .Property(e => e.TrackZipFileLink)
                .IsUnicode(false);

            modelBuilder.Entity<SingleTrackDetail>()
                .Property(e => e.ArtworkFileLink)
                .IsUnicode(false);

            modelBuilder.Entity<SingleTrackDetail>()
                .HasMany(e => e.AlbumTrackMasters)
                .WithOptional(e => e.SingleTrackDetail)
                .HasForeignKey(e => e.Track_Id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<SingleTrackDetail>()
                .HasMany(e => e.EpTrackMasters)
                .WithOptional(e => e.SingleTrackDetail)
                .HasForeignKey(e => e.Track_Id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<SingleTrackDetail>()
                .HasMany(e => e.SoloTrackMasters)
                .WithOptional(e => e.SingleTrackDetail)
                .HasForeignKey(e => e.Track_Id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<UserDetail>()
                .Property(e => e.User_First_Name)
                .IsUnicode(false);

            modelBuilder.Entity<UserDetail>()
                .Property(e => e.User_Last_Name)
                .IsUnicode(false);

            modelBuilder.Entity<UserDetail>()
                .Property(e => e.User_Mobile_Number)
                .IsUnicode(false);

            modelBuilder.Entity<UserDetail>()
                .Property(e => e.User_Address)
                .IsUnicode(false);
        }
    }
}
