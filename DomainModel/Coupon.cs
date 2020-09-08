namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Coupon")]
    public partial class Coupon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Coupon()
        {
            PurchaseRecords = new HashSet<PurchaseRecord>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string Coupon_Code { get; set; }

        public DateTime? Generated_At { get; set; }

        public DateTime? Expire_At { get; set; }

        public int? Discount_Percentage { get; set; }

        public int? Quantity { get; set; }

        public Guid? Created_By { get; set; }

        [StringLength(50)]
        public string TobeAppliedOnCategory { get; set; }

        public virtual Account Account { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseRecord> PurchaseRecords { get; set; }
    }
}
