using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace prjTravelAlbumSys.Models
{
    public partial class TravelAlbumDbContext : DbContext
    {
        public TravelAlbumDbContext()
        {
        }

        public TravelAlbumDbContext(DbContextOptions<TravelAlbumDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TAlbum> TAlbums { get; set; }
        public virtual DbSet<TCategory> TCategories { get; set; }
        public virtual DbSet<TComment> TComments { get; set; }
        public virtual DbSet<TMember> TMembers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\user\\source\\repos\\prjTravelAlbumSys\\prjTravelAlbumSys\\App_Data\\dbTravelAlbum.mdf;Integrated Security=True;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TAlbum>(entity =>
            {
                entity.HasKey(e => e.FAlbumId)
                    .HasName("PK__tmp_ms_x__D284AAB4E6AA996E");

                entity.ToTable("tAlbum");

                entity.Property(e => e.FAlbumId).HasColumnName("fAlbumId");

                entity.Property(e => e.FAlbum)
                    .HasMaxLength(50)
                    .HasColumnName("fAlbum");

                entity.Property(e => e.FCid).HasColumnName("fCid");

                entity.Property(e => e.FCommentNum).HasColumnName("fCommentNum");

                entity.Property(e => e.FDescription).HasColumnName("fDescription");

                entity.Property(e => e.FReleaseTime)
                    .HasColumnType("datetime")
                    .HasColumnName("fReleaseTime");

                entity.Property(e => e.FTitle)
                    .HasMaxLength(50)
                    .HasColumnName("fTitle");

                entity.Property(e => e.FUid)
                    .HasMaxLength(50)
                    .HasColumnName("fUid");
            });

            modelBuilder.Entity<TCategory>(entity =>
            {
                entity.HasKey(e => e.FCid)
                    .HasName("PK__tCategor__ADED052C2F66C633");

                entity.ToTable("tCategory");

                entity.Property(e => e.FCid).HasColumnName("fCid");

                entity.Property(e => e.FCname)
                    .HasMaxLength(50)
                    .HasColumnName("fCName");
            });

            modelBuilder.Entity<TComment>(entity =>
            {
                entity.HasKey(e => e.FCommentId)
                    .HasName("PK__tComment__F4718CE266BEB31F");

                entity.ToTable("tComment");

                entity.Property(e => e.FCommentId).HasColumnName("fCommentId");

                entity.Property(e => e.FAlbumId).HasColumnName("fAlbumId");

                entity.Property(e => e.FComment).HasColumnName("fComment");

                entity.Property(e => e.FName)
                    .HasMaxLength(50)
                    .HasColumnName("fName");

                entity.Property(e => e.FReleaseTime)
                    .HasColumnType("datetime")
                    .HasColumnName("fReleaseTime");

                entity.Property(e => e.FUid)
                    .HasMaxLength(50)
                    .HasColumnName("fUid");
            });

            modelBuilder.Entity<TMember>(entity =>
            {
                entity.HasKey(e => e.FUid)
                    .HasName("PK__tMember__B791A2ADA66D277F");

                entity.ToTable("tMember");

                entity.Property(e => e.FUid)
                    .HasMaxLength(50)
                    .HasColumnName("fUid");

                entity.Property(e => e.FMail)
                    .HasMaxLength(50)
                    .HasColumnName("fMail");

                entity.Property(e => e.FName)
                    .HasMaxLength(50)
                    .HasColumnName("fName");

                entity.Property(e => e.FPwd)
                    .HasMaxLength(50)
                    .HasColumnName("fPwd");

                entity.Property(e => e.FRole)
                    .HasMaxLength(50)
                    .HasColumnName("fRole");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
