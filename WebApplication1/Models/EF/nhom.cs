namespace WebApplication1.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("nhom")]
    public partial class nhom
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public nhom()
        {
            chitietdks = new HashSet<chitietdk>();
            giangviens = new HashSet<giangvien>();
        }

        [Key]
        [StringLength(20)]
        public string manhom { get; set; }

        [StringLength(20)]
        public string mamh { get; set; }

        [StringLength(20)]
        public string tennhom { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<chitietdk> chitietdks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<giangvien> giangviens { get; set; }

        public virtual monhoc monhoc { get; set; }
    }
}
