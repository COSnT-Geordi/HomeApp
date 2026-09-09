using HomeApp.SqlModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeApp.Common
{
    public class HomeDbContext : DbContext
    {
        public HomeDbContext(DbContextOptions<HomeDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("user").HasMany(e => e.Articles).WithOne(e => e.User).HasForeignKey(k => k.UserId);
            modelBuilder.Entity<User>().HasMany(e => e.Albums).WithOne(e => e.User).HasForeignKey(k => k.UserId);
            modelBuilder.Entity<User>().HasMany(e => e.DbFiles).WithOne(e => e.User).HasForeignKey(k => k.UserId);

            modelBuilder.Entity<Article>().ToTable("article").HasOne(e => e.User).WithMany(e => e.Articles).HasForeignKey(k => k.UserId);
            modelBuilder.Entity<Article>().HasMany(e => e.UserSharingsArticle).WithOne(e => e.Article).HasForeignKey(k => k.ArticleID);
            modelBuilder.Entity<Article>().HasOne(e => e.ArticleCategory).WithMany(e => e.Articles).HasForeignKey(k => k.ArticleCategoryID);

            modelBuilder.Entity<NoteList>().ToTable("note_list").HasMany(e => e.Notes).WithOne(e => e.NoteList).HasForeignKey(k => k.NoteListId);
            modelBuilder.Entity<Note>().ToTable("note").HasOne(e => e.NoteList).WithMany(e => e.Notes).HasForeignKey(k => k.NoteListId);
            modelBuilder.Entity<DbFile>().ToTable("db_file").HasMany(e => e.Albums).WithMany(e => e.DbFiles).UsingEntity(j => j.ToTable("album_db_file"));
            //modelBuilder.Entity<AlbumDbFile>().ToTable("album_db_file").HasKey("album_id", "db_file_id");

            modelBuilder.Entity<Album>().ToTable("album").HasMany(e => e.DbFiles).WithMany(e => e.Albums)
                .UsingEntity<Dictionary<string, object>>("album_db_file", j => j
             .HasOne<DbFile>()
             .WithMany()
             .HasForeignKey("db_file_id")
             .HasPrincipalKey(e => e.ID),

         j => j
             .HasOne<Album>()
             .WithMany()
             .HasForeignKey("album_id")
             .HasPrincipalKey(e => e.ID),

         j =>
         {
             j.ToTable("album_db_file");
             j.Property<long>("album_id");
             j.Property<long>("db_file_id");
             j.HasKey("album_id", "db_file_id");
         });

            modelBuilder.Entity<Album>().HasOne(e => e.User).WithMany(e => e.Albums).HasForeignKey(e => e.UserId);


            modelBuilder.Entity<UserSharing>().ToTable("user_sharing").HasOne(e => e.User).WithMany(e => e.UserSharings).HasForeignKey(e => e.UserId);
            modelBuilder.Entity<UserSharing>().HasMany(e => e.UserSharingUsers).WithOne(e => e.UserSharing).HasForeignKey(e => e.UserSharingID);
            modelBuilder.Entity<UserSharing>().HasMany(e => e.UserSharingAlbums).WithOne(e => e.UserSharing).HasForeignKey(e => e.UserSharingID);
            modelBuilder.Entity<UserSharing>().HasMany(e => e.UserSharingArticles).WithOne(e => e.UserSharing).HasForeignKey(e => e.UserSharingID);
            modelBuilder.Entity<UserSharing>().HasMany(e => e.UserSharingDbFiles).WithOne(e => e.UserSharing).HasForeignKey(e => e.UserSharingID);


            //modelBuilder.Entity<UserSharingUser>().ToTable("user_sharing_user").HasOne(e => e.UserSharing).WithMany(e => e.UserSharingUsers).HasForeignKey(e=>e.UserSharingID);
            modelBuilder.Entity<UserSharingUser>().ToTable("user_sharing_user").HasKey(e => new { e.UserSharingID, e.UserID });
            modelBuilder.Entity<UserSharingUser>()
                .ToTable("user_sharing_user")
                 .HasKey(e => new { e.UserSharingID, e.UserID });

            modelBuilder.Entity<UserSharingUser>()
                .Property(e => e.UserSharingID)
                .HasColumnName("user_sharing_id");

            modelBuilder.Entity<UserSharingUser>()
                .Property(e => e.UserID)
                .HasColumnName("user_id");

            modelBuilder.Entity<UserSharingUser>()
                .HasOne(e => e.UserSharing)
                .WithMany(e => e.UserSharingUsers)
                .HasForeignKey(e => e.UserSharingID);

            modelBuilder.Entity<UserSharingUser>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserID);
            modelBuilder.Entity<UserSharingAlbum>().ToTable("user_sharing_album").HasOne(e => e.UserSharing).WithMany(e => e.UserSharingAlbums).HasForeignKey(e => e.UserSharingID);
            modelBuilder.Entity<UserSharingArticle>().ToTable("user_sharing_article").HasOne(e => e.UserSharing).WithMany(e => e.UserSharingArticles).HasForeignKey(e => e.UserSharingID);

        }
        public DbSet<Article> Articles { get; set; }
        public DbSet<NoteList> NoteLists { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<DbFile> DbFiles { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSharing> UserSharings { get; set; }
        public DbSet<ArticleCategory> ArticleCategories { get; set; }


    }
}
