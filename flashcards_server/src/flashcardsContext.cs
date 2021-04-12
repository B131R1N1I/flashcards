using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace flashcards_server
{
    public partial class flashcardsContext : DbContext
    {
        public flashcardsContext()
        {
        }

        public flashcardsContext(DbContextOptions<flashcardsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActiveSet> activeSets { get; set; }
        public virtual DbSet<Card.Card> cards { get; set; }
        public virtual DbSet<CardStatus> cardStatuses { get; set; }
        public virtual DbSet<Set.Set> sets { get; set; }
        public virtual DbSet<User.User> users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Database=flashcards;Username=flashcards_app;Password=fc_app");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.UTF-8");

            modelBuilder.Entity<ActiveSet>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("active_sets");

                entity.Property(e => e.setId).HasColumnName("set_id");

                entity.Property(e => e.userId).HasColumnName("user_id");

                entity.HasOne(d => d.set)
                    .WithMany()
                    .HasForeignKey(d => d.setId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("id_set");

                entity.HasOne(d => d.user)
                    .WithMany()
                    .HasForeignKey(d => d.userId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("id_user");
            });

            modelBuilder.Entity<Card.Card>(entity =>
            {
                entity.ToTable("cards");
                
                entity.HasIndex(e => e.id, "cards_id_key")
                    .IsUnique();

                entity.Property(e => e.id).HasColumnName("id");

                entity.Property(e => e.answer)
                    .IsRequired()
                    .HasColumnName("answer");

                entity.Property(e => e.inSet).HasColumnName("in_set");

                entity.Property(e => e.picture).HasColumnName("picture");

                entity.Property(e => e.isPublic)
                    .IsRequired()
                    .HasColumnName("is_public");

                entity.Property(e => e.ownerId)
                    .IsRequired()
                    .HasColumnName("owner_id");

                entity.Property(e => e.question)
                    .IsRequired()
                    .HasColumnName("question");

                entity.HasOne(d => d.inSetNavigation)
                    .WithMany(p => p.cards)
                    .HasForeignKey(d => d.inSet)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("set_id");
            });

            modelBuilder.Entity<CardStatus>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("card_status");

                entity.Property(e => e.active).HasColumnName("active");

                entity.Property(e => e.cardId).HasColumnName("card_id");

                entity.Property(e => e.difficult).HasColumnName("difficult");

                entity.Property(e => e.lastReview).HasColumnName("last_review");

                entity.Property(e => e.nextReview).HasColumnName("next_review");

                entity.Property(e => e.userId).HasColumnName("user_id");

                entity.HasOne(d => d.card)
                    .WithMany()
                    .HasForeignKey(d => d.cardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("id_card");

                entity.HasOne(d => d.user)
                    .WithMany()
                    .HasForeignKey(d => d.userId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("id_user");
            });

            modelBuilder.Entity<Set.Set>(entity =>
            {
                entity.ToTable("sets");

                entity.HasIndex(e => e.name, "sets_name_key")
                    .IsUnique();
                
                entity.HasIndex(e => e.id, "sets_id_key")
                    .IsUnique();

                entity.Property(e => e.id).HasColumnName("id");

                entity.Property(e => e.createdDate).HasColumnName("created_date");

                entity.Property(e => e.creatorId).HasColumnName("creator_id");

                entity.Property(e => e.isPublic).HasColumnName("is_public");

                entity.Property(e => e.lastModification).HasColumnName("last_modification");

                entity.Property(e => e.name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.ownerId).HasColumnName("owner_id");

                entity.HasOne(d => d.creator)
                    .WithMany(p => p.sets)
                    .HasForeignKey(d => d.creatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_creator");
            });

            modelBuilder.Entity<User.User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Id, "users_id_key")
                    .IsUnique();
                    
                entity.HasIndex(e => e.Email, "users_email_key")
                    .IsUnique();

                entity.HasIndex(e => e.UserName, "users_username_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id")
                    .IsRequired()
                    .HasColumnName("id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email");

                entity.Property(e => e.EmailConfirmed)
                    .IsRequired()
                    .HasColumnName("email_confirmed");

                entity.Property(e => e.name).HasColumnName("name");

                entity.Property(e => e.password)
                    .IsRequired()
                    .HasColumnName("password");

                entity.Property(e => e.surname).HasColumnName("surname");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
