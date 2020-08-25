namespace DomainModel
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SoloTrackMaster")]
    public partial class SoloTrackMaster
    {
        public Guid Id { get; set; }

        public Guid Track_Id { get; set; }

        public Guid PurchaseTrack_RefNo { get; set; }

        public DateTime Submitted_At { get; set; }

        public byte StoreSubmissionStatus { get; set; }

        public virtual PurchaseRecord PurchaseRecord { get; set; }

        public virtual SingleTrackDetail SingleTrackDetail { get; set; }
    }
}
