namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("BankDetail")]
    public partial class BankDetail
    {
        public Guid Id { get; set; }

        public Guid? Account_Id { get; set; }

        [StringLength(50)]
        public string PayeeFirstName { get; set; }

        [StringLength(50)]
        public string PayeeLastName { get; set; }

        public string PayeeBankName { get; set; }

        [StringLength(50)]
        public string PayeeBankIfscNumber { get; set; }

        public string PayeeBankBranch { get; set; }

        [StringLength(50)]
        public string PayeeBankAccountNumber { get; set; }

        public DateTime? Detail_Submitted_At { get; set; }

        public virtual Account Account { get; set; }
    }
}
