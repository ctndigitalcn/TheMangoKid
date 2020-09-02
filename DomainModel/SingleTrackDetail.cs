namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SingleTrackDetail")]
    public partial class SingleTrackDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SingleTrackDetail()
        {
            AlbumTrackMasters = new HashSet<AlbumTrackMaster>();
            EpTrackMasters = new HashSet<EpTrackMaster>();
            SoloTrackMasters = new HashSet<SoloTrackMaster>();
        }

        public Guid Id { get; set; }

        [StringLength(50)]
        public string TrackTitle { get; set; }

        [Column(TypeName = "text")]
        public string ArtistName { get; set; }

        public byte? ArtistAlreadyInSpotify { get; set; }

        public string ArtistSpotifyUrl { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ReleaseDate { get; set; }

        [StringLength(50)]
        public string Genre { get; set; }

        [StringLength(50)]
        public string CopyrightClaimerName { get; set; }

        [StringLength(50)]
        public string AuthorName { get; set; }

        [StringLength(50)]
        public string ComposerName { get; set; }

        [StringLength(50)]
        public string ArrangerName { get; set; }

        [StringLength(50)]
        public string ProducerName { get; set; }

        public byte? AlreadyHaveAnISRC { get; set; }

        [StringLength(50)]
        public string ISRC_Number { get; set; }

        public int? PriceTier { get; set; }

        public byte? ExplicitContent { get; set; }

        public byte? IsTrackInstrumental { get; set; }

        [StringLength(50)]
        public string LyricsLanguage { get; set; }

        [Column(TypeName = "text")]
        public string TrackZipFileLink { get; set; }

        public int? Artwork_Id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlbumTrackMaster> AlbumTrackMasters { get; set; }

        public virtual ArtworkDetail ArtworkDetail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EpTrackMaster> EpTrackMasters { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SoloTrackMaster> SoloTrackMasters { get; set; }
    }
}
