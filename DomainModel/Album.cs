namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Album")]
    public partial class Album
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Album()
        {
            AlbumTrackMasters = new HashSet<AlbumTrackMaster>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Album_Name { get; set; }

        public DateTime Album_Creation_Date { get; set; }

        public int Total_Track { get; set; }

        public int Submitted_Track { get; set; }

        public Guid PurchaseTrack_RefNo { get; set; }

        public virtual PurchaseRecord PurchaseRecord { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlbumTrackMaster> AlbumTrackMasters { get; set; }
    }
}
