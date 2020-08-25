namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ExtendedPlay")]
    public partial class ExtendedPlay
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ExtendedPlay()
        {
            EpTrackMasters = new HashSet<EpTrackMaster>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Ep_Name { get; set; }

        public DateTime Ep_Creation_Date { get; set; }

        public int Total_Track { get; set; }

        public int Submitted_Track { get; set; }

        public Guid PurchaseTrack_RefNo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EpTrackMaster> EpTrackMasters { get; set; }

        public virtual PurchaseRecord PurchaseRecord { get; set; }
    }
}
