namespace WebApplication1.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sinhvien")]
    public partial class sinhvien
    {
        [Key]
        [StringLength(50)]
        public string mssv { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ngaysinh { get; set; }

        [StringLength(50)]
        public string diachi { get; set; }

        public bool? gioitinh { get; set; }

        [StringLength(11)]
        public string sdt { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        [StringLength(25)]
        public string matkhau { get; set; }

        [StringLength(20)]
        public string madk { get; set; }

        [StringLength(20)]
        public string lop { get; set; }

        [StringLength(20)]
        public string makhoa { get; set; }

        [StringLength(250)]
        public string hoten { get; set; }

        public virtual dkmonhoc dkmonhoc { get; set; }

        public virtual khoa khoa { get; set; }
    }
}
