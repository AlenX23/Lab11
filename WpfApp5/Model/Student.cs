namespace WpfApp5.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime Admission { get; set; }
        public int? GroupId { get; set; }
        public virtual Group Group { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}