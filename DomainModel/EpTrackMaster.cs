namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EpTrackMaster")]
    public partial class EpTrackMaster
    {
        public Guid Id { get; set; }

        public Guid? Ep_Id { get; set; }

        public Guid? Track_Id { get; set; }

        public DateTime? Submitted_At { get; set; }

        // 0 = rejected; 1 = submitted, 2 = pending
        public int? StoreSubmissionStatus { get; set; }

        public virtual ExtendedPlay ExtendedPlay { get; set; }

        public virtual SingleTrackDetail SingleTrackDetail { get; set; }
    }
}
