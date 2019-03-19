using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Forum.Models;

namespace Forum.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PostTopic>()
                .HasKey(t => new { t.PostId, t.TopicId });

            builder.Entity<PostComment>()
                .HasKey(t => new { t.PostId, t.CommentId });

            builder.Entity<PostTopic>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.PostTopics)
                .HasForeignKey(pt => pt.PostId);

            builder.Entity<PostTopic>()
                .HasOne(pt => pt.Topics)
                .WithMany(t => t.PostTopics)
                .HasForeignKey(pt => pt.TopicId);

            builder.Entity<PostComment>()
                .HasOne(ct => ct.Post)
                .WithMany(c => c.PostComments)
                .HasForeignKey(ct => ct.PostId);

            builder.Entity<PostComment>()
                .HasOne(ct => ct.Comments)
                .WithMany(c => c.PostComments)
                .HasForeignKey(ct => ct.CommentId);

        }
        public DbSet<ForumPost> ForumPosts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Forum.Models.PostComment> PostComments { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Forum.Models.PostTopic> PostTopics { get; set; }
    }    
}
