namespace WebApplication1.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("monhoc")]
    public partial class monhoc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public monhoc()
        {
            nhoms = new HashSet<nhom>();
        }

        [Key]
        [StringLength(20)]
        public string mamh { get; set; }

        [Required]
        [StringLength(50)]
        public string tenmh { get; set; }

        public int? sotc { get; set; }

        [Required]
        [StringLength(20)]
        public string makhoa { get; set; }

        public virtual khoa khoa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<nhom> nhoms { get; set; }
    }
}
