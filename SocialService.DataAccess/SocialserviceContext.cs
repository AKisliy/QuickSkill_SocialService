﻿using Microsoft.EntityFrameworkCore;

namespace SocialService.DataAccess;

public partial class SocialServiceContext : DbContext
{
    public SocialServiceContext()
    {
    }

    public SocialServiceContext(DbContextOptions<SocialServiceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AnswerEntity> Answers { get; set; }

    public virtual DbSet<CommentEntity> Comments { get; set; }

    public virtual DbSet<DiscussionEntity> Discussions { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Leaderboard> Leaderboards { get; set; }

    public virtual DbSet<LeagueEntity> Leagues { get; set; }

    public virtual DbSet<Lecture> Lectures { get; set; }

    public virtual DbSet<Subscriber> Subscribers { get; set; }

    public virtual DbSet<UserEntity> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=socialservice;Username=alexeykiselev;Password=kisliy");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnswerEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("answer_pkey");

            entity.ToTable("answer");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Body).HasColumnName("body");
            entity.Property(e => e.DiscussionId)
                .ValueGeneratedOnAdd()
                .HasColumnName("discussionid");
            entity.Property(e => e.Likes)
                .HasDefaultValue(0)
                .HasColumnName("likes");
            entity.Property(e => e.PublishedOn).HasColumnName("publishedon");
            entity.Property(e => e.EditedOn).HasColumnName("editedon");
            entity.Property(e => e.UserId)
                .ValueGeneratedOnAdd()
                .HasColumnName("userid");

            entity.HasOne(d => d.Discussion).WithMany(p => p.Answers)
                .HasForeignKey(d => d.DiscussionId)
                .HasConstraintName("answer_discussionid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Answers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("answer_userid_fkey");
        });

        modelBuilder.Entity<CommentEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("comments_pkey");

            entity.ToTable("comments");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LectureId).HasColumnName("lectureid");
            entity.Property(e => e.Likes)
                .HasDefaultValue(0)
                .HasColumnName("likes");
            entity.Property(e => e.PublishedOn).HasColumnName("publishedon");
            entity.Property(e => e.UserId).HasColumnName("userid");
            entity.Property(e => e.EditedOn).HasColumnName("editedon");

            entity.HasOne(d => d.Lecture).WithMany(p => p.Comments)
                .HasForeignKey(d => d.LectureId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("comments_lectureid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("comments_userid_fkey");
        });

        modelBuilder.Entity<DiscussionEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("discussion_pkey");

            entity.ToTable("discussion");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AuthorId)
                .ValueGeneratedOnAdd()
                .HasColumnName("authorid");
            entity.Property(e => e.Body).HasColumnName("body");
            entity.Property(e => e.Likes)
                .HasDefaultValue(0)
                .HasColumnName("likes");
            entity.Property(e => e.PublishedOn).HasColumnName("publishedon");
            entity.Property(e => e.Title).HasColumnName("title");

            entity.HasOne(d => d.Author).WithMany(p => p.Discussions)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("discussion_authorid_fkey");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("feedback_pkey");

            entity.ToTable("feedback");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Body).HasColumnName("body");
            entity.Property(e => e.UserId)
                .ValueGeneratedOnAdd()
                .HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("feedback_userid_fkey");
        });

        modelBuilder.Entity<Leaderboard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("leaderboard_pkey");

            entity.ToTable("leaderboard");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LeagueId)
                .ValueGeneratedOnAdd()
                .HasColumnName("leagueid");

            entity.HasOne(d => d.League).WithMany(p => p.Leaderboards)
                .HasForeignKey(d => d.LeagueId)
                .HasConstraintName("leaderboard_leagueid_fkey");
        });

        modelBuilder.Entity<LeagueEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("league_pkey");

            entity.HasIndex(e => e.LeagueName, "name_unique").IsUnique();
            entity.HasIndex(e => e.HierarchyPlace, "place_unique").IsUnique();

            entity.ToTable("league");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.HierarchyPlace).HasColumnName("hierarchyplace");
            entity.Property(e => e.LeagueName)
                .HasMaxLength(50)
                .HasColumnName("leaguename");
            entity.Property(e => e.Photo).HasColumnName("photo");
        });

        modelBuilder.Entity<Lecture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("lecture_pkey");

            entity.ToTable("lecture");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
        });

        modelBuilder.Entity<Subscriber>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("subscribers");

            entity.Property(e => e.SubscriptionId)
                .ValueGeneratedOnAdd()
                .HasColumnName("subscriptionid");
            entity.Property(e => e.UserId)
                .ValueGeneratedOnAdd()
                .HasColumnName("userid");

            entity.HasOne(d => d.Subscription).WithMany()
                .HasForeignKey(d => d.SubscriptionId)
                .HasConstraintName("subscribers_subscriptionid_fkey");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("subscribers_userid_fkey");
        });

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("firstname");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("lastname");
            entity.Property(e => e.LeaderboardId)
                .ValueGeneratedOnAdd()
                .HasColumnName("leaderboardid");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValueSql("'Default'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Subscribers)
                .HasDefaultValue(0)
                .HasColumnName("subscribers");
            entity.Property(e => e.Subscriptions)
                .HasDefaultValue(0)
                .HasColumnName("subscriptions");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
            entity.Property(e => e.WeeklyXp)
                .HasDefaultValue(0)
                .HasColumnName("weeklyxp");
            entity.Property(e => e.Xp)
                .HasDefaultValue(0)
                .HasColumnName("xp");

            entity.HasOne(d => d.Leaderboard).WithMany(p => p.Users)
                .HasForeignKey(d => d.LeaderboardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_leaderboardid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
