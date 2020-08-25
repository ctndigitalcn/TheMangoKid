namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ArtworkDetail")]
    public partial class ArtworkDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ArtworkDetail()
        {
            SingleTrackDetails = new HashSet<SingleTrackDetail>();
        }

        public int Id { get; set; }

        [Required]
        public string Artwork_Link { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SingleTrackDetail> SingleTrackDetails { get; set; }
    }
}
