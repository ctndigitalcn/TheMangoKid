namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AlbumTrackMaster")]
    public partial class AlbumTrackMaster
    {
        public Guid Id { get; set; }

        public Guid? Album_Id { get; set; }

        public Guid? Track_Id { get; set; }

        public DateTime? Submitted_At { get; set; }

        // 0 = rejected; 1 = submitted, 2 = pending
        public int? StoreSubmissionStatus { get; set; }

        public virtual Album Album { get; set; }

        public virtual SingleTrackDetail SingleTrackDetail { get; set; }
    }
}
