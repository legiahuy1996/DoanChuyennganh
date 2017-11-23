namespace WebApplication1.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("diem")]
    public partial class diem
    {
        [Key]
        [StringLength(20)]
        public string madiem { get; set; }

        public double? diemQT { get; set; }

        public double? diemGK { get; set; }

        public double? diemCK { get; set; }

        public double? tongdiem { get; set; }

        [Column("%diemQT")]
        public double? C_diemQT { get; set; }

        [Column("%diemGK")]
        public double? C_diemGK { get; set; }

        [Column("%diemck")]
        public double? C_diemck { get; set; }

        public int? solanthi { get; set; }
    }
}
