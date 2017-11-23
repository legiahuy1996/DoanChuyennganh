namespace WebApplication1.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("giangvien")]
    public partial class giangvien
    {
        [StringLength(20)]
        public string manhom { get; set; }

        [Key]
        [StringLength(20)]
        public string magv { get; set; }

        [StringLength(50)]
        public string tengv { get; set; }

        public DateTime? ngaysinh { get; set; }

        public bool? gioitinh { get; set; }

        public int? sdt { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        public virtual nhom nhom { get; set; }
    }
}
