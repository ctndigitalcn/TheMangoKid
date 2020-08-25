namespace DomainModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("UserDetail")]
    public partial class UserDetail
    {
        [Key]
        public Guid User_Account_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string User_First_Name { get; set; }

        [Required]
        [StringLength(50)]
        public string User_Last_Name { get; set; }

        [Required]
        [StringLength(13)]
        public string User_Mobile_Number { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string User_Address { get; set; }

        public virtual Account Account { get; set; }
    }
}
