namespace WebApplication1.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PDT")]
    public partial class PDT
    {
        [Key]
        [StringLength(50)]
        public string msnv { get; set; }

        public DateTime? ngaysinh { get; set; }

        public bool? gioiting { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        [StringLength(11)]
        public string sdt { get; set; }

        [StringLength(50)]
        public string diachi { get; set; }

        [StringLength(50)]
        public string matkhau { get; set; }

        [StringLength(50)]
        public string hoten { get; set; }
    }
}
