namespace WebApplication1.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("hocky")]
    public partial class hocky
    {
        [Key]
        [StringLength(20)]
        public string mahk { get; set; }

        [Column("hocky")]
        public int hocky1 { get; set; }

        public int nam { get; set; }

        [StringLength(20)]
        public string madk { get; set; }

        public virtual dkmonhoc dkmonhoc { get; set; }
    }
}
