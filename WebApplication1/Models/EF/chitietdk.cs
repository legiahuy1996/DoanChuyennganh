namespace WebApplication1.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("chitietdk")]
    public partial class chitietdk
    {
        [Key]
        [StringLength(20)]
        public string machitietdk { get; set; }

        [StringLength(20)]
        public string manhom { get; set; }

        [StringLength(20)]
        public string madk { get; set; }

        [StringLength(20)]
        public string madiem { get; set; }

        public virtual dkmonhoc dkmonhoc { get; set; }

        public virtual nhom nhom { get; set; }
    }
}
