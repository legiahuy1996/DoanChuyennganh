 namespace WebApplication1.Models.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<chitietdk> chitietdks { get; set; }
        public virtual DbSet<diem> diems { get; set; }
        public virtual DbSet<dkmonhoc> dkmonhocs { get; set; }
        public virtual DbSet<giangvien> giangviens { get; set; }
        public virtual DbSet<hocky> hockies { get; set; }
        public virtual DbSet<KetQuaHocTap> KetQuaHocTaps { get; set; }
        public virtual DbSet<khoa> khoas { get; set; }
        public virtual DbSet<monhoc> monhocs { get; set; }
        public virtual DbSet<nhom> nhoms { get; set; }
        public virtual DbSet<PDT> PDTs { get; set; }
        public virtual DbSet<sinhvien> sinhviens { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<chitietdk>()
                .Property(e => e.machitietdk)
                .IsUnicode(false);

            modelBuilder.Entity<chitietdk>()
                .Property(e => e.manhom)
                .IsUnicode(false);

            modelBuilder.Entity<chitietdk>()
                .Property(e => e.madk)
                .IsUnicode(false);

            modelBuilder.Entity<chitietdk>()
                .Property(e => e.madiem)
                .IsUnicode(false);

            modelBuilder.Entity<diem>()
                .Property(e => e.madiem)
                .IsUnicode(false);

            modelBuilder.Entity<dkmonhoc>()
                .Property(e => e.madk)
                .IsUnicode(false);

            modelBuilder.Entity<giangvien>()
                .Property(e => e.manhom)
                .IsUnicode(false);

            modelBuilder.Entity<giangvien>()
                .Property(e => e.magv)
                .IsUnicode(false);

            modelBuilder.Entity<hocky>()
                .Property(e => e.mahk)
                .IsUnicode(false);

            modelBuilder.Entity<hocky>()
                .Property(e => e.madk)
                .IsUnicode(false);

            modelBuilder.Entity<KetQuaHocTap>()
                .Property(e => e.mssv)
                .IsUnicode(false);

            modelBuilder.Entity<khoa>()
                .Property(e => e.makhoa)
                .IsUnicode(false);

            modelBuilder.Entity<khoa>()
                .HasMany(e => e.monhocs)
                .WithRequired(e => e.khoa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<monhoc>()
                .Property(e => e.mamh)
                .IsUnicode(false);

            modelBuilder.Entity<monhoc>()
                .Property(e => e.makhoa)
                .IsUnicode(false);

            modelBuilder.Entity<nhom>()
                .Property(e => e.manhom)
                .IsUnicode(false);

            modelBuilder.Entity<nhom>()
                .Property(e => e.mamh)
                .IsUnicode(false);

            modelBuilder.Entity<nhom>()
                .Property(e => e.tennhom)
                .IsUnicode(false);

            modelBuilder.Entity<PDT>()
                .Property(e => e.msnv)
                .IsUnicode(false);

            modelBuilder.Entity<PDT>()
                .Property(e => e.matkhau)
                .IsUnicode(false);

            modelBuilder.Entity<sinhvien>()
                .Property(e => e.mssv)
                .IsUnicode(false);

            modelBuilder.Entity<sinhvien>()
                .Property(e => e.matkhau)
                .IsUnicode(false);

            modelBuilder.Entity<sinhvien>()
                .Property(e => e.madk)
                .IsUnicode(false);

            modelBuilder.Entity<sinhvien>()
                .Property(e => e.lop)
                .IsUnicode(false);

            modelBuilder.Entity<sinhvien>()
                .Property(e => e.makhoa)
                .IsUnicode(false);
        }
    }
}
