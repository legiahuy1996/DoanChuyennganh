namespace WebApplication1.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KetQuaHocTap")]
    public partial class KetQuaHocTap
    {
        [Key]
        [StringLength(20)]
        public string mssv { get; set; }

        [StringLength(250)]
        public string hoten { get; set; }

        [StringLength(50)]
        public string lop { get; set; }

        [StringLength(250)]
        public string tenmh { get; set; }

        public double? tongdiem { get; set; }

        public double? diemQT { get; set; }

        public double? diemGK { get; set; }

        public double? diemCK { get; set; }

        public int? solanthi { get; set; }
    }
}
