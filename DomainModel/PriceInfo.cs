namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PriceInfo")]
    public partial class PriceInfo
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Category_Name { get; set; }

        public decimal? BasePrice { get; set; }

        public decimal? Gst { get; set; }
    }
}
