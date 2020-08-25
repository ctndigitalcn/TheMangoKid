namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PurchaseRecord")]
    public partial class PurchaseRecord
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PurchaseRecord()
        {
            Albums = new HashSet<Album>();
            ExtendedPlays = new HashSet<ExtendedPlay>();
            SoloTrackMasters = new HashSet<SoloTrackMaster>();
        }

        public Guid Id { get; set; }

        public Guid? Account_Id { get; set; }

        [StringLength(15)]
        public string Purchased_Category { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public DateTime? Usage_Date { get; set; }

        public DateTime? Usage_Exp_Date { get; set; }

        public int? CouponCodeApplied { get; set; }

        public virtual Account Account { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Album> Albums { get; set; }

        public virtual Coupon Coupon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExtendedPlay> ExtendedPlays { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SoloTrackMaster> SoloTrackMasters { get; set; }
    }
}
