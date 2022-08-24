using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace VirtualLibrary.Models
{
    public partial class VirtualLibraryContext : DbContext
    {
        public VirtualLibraryContext()
        {
        }

        public VirtualLibraryContext(DbContextOptions<VirtualLibraryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BooksBorrowing> BooksBorrowings { get; set; } = null!;
        public virtual DbSet<BooksList> BooksLists { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=VirtualLibrary;Trusted_Connection=False;password=!@12QWaszx;user=sa;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BooksBorrowing>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.BookId, e.BorrowedDateBegining })
                    .HasName("pk");

                entity.ToTable("BooksBorrowing");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BookId).HasColumnName("bookId");

                entity.Property(e => e.BorrowedDateBegining)
                    .HasColumnType("date")
                    .HasColumnName("borrowedDateBegining")
                    .HasDefaultValueSql("(CONVERT([date],getdate(),(105)))");

                entity.Property(e => e.BorrowedDateFinish)
                    .HasColumnType("date")
                    .HasColumnName("borrowedDateFinish")
                    .HasDefaultValueSql("(CONVERT([date],getdate(),(105)))");

                entity.Property(e => e.ReturnedBookDate)
                    .HasColumnType("date")
                    .HasColumnName("returnedBookDate")
                    .HasDefaultValueSql("(CONVERT([date],getdate(),(105)))");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BooksBorrowingBooks)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BooksBorr__bookI__403A8C7D");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.BooksBorrowingIdNavigations)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BooksBorrowi__id__3C69FB99");
            });

            modelBuilder.Entity<BooksList>(entity =>
            {
                entity.HasKey(e => e.BookId)
                    .HasName("PK__BooksLis__8BE5A10DC9E17B5B");

                entity.ToTable("BooksList");

                entity.Property(e => e.BookId).HasColumnName("bookId");

                entity.Property(e => e.Author)
                    .HasMaxLength(50)
                    .HasColumnName("author");

                entity.Property(e => e.BookLanguage).HasMaxLength(50);

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .HasColumnName("country");

                entity.Property(e => e.Genre)
                    .HasMaxLength(50)
                    .HasColumnName("genre");

                entity.Property(e => e.ImageLink)
                    .HasMaxLength(500)
                    .HasColumnName("imageLink");

                entity.Property(e => e.NumOfCopies).HasColumnName("numOfCopies");

                entity.Property(e => e.Pages)
                    .HasMaxLength(50)
                    .HasColumnName("pages");

                entity.Property(e => e.PublishedBookDate)
                    .HasMaxLength(50)
                    .HasColumnName("publishedBookDate");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Bdate)
                    .HasColumnType("date")
                    .HasColumnName("bdate")
                    .HasDefaultValueSql("(CONVERT([date],getdate(),(105)))");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.Fname)
                    .HasMaxLength(50)
                    .HasColumnName("fname");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.Lname)
                    .HasMaxLength(50)
                    .HasColumnName("lname");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .HasColumnName("phone");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
