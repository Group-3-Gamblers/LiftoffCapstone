﻿using GameHopper.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



namespace GameHopper;

public class GameDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public GameDbContext(DbContextOptions<GameDbContext> options) : base(options)
    {
    }

        public DbSet<Game>? Games { get; set; }

        public DbSet<GameMaster>? GameMasters { get; set; }

        public DbSet<Player>? Players { get; set; }

        public DbSet<Category>? Categories { get; set; }

        public DbSet<Tag>? Tags { get; set; }
        public DbSet<Request>? Requests { get; set; }

        public DbSet<BlogEntry>? Blogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Game>()
            .HasOne(g => g.Category)
            .WithMany(c => c.Games)
            .HasForeignKey(g => g.CategoryId);

        modelBuilder.Entity<Game>()
            .HasOne(g => g.GameMaster)
            .WithMany(gm => gm.CreatedGames)
            .HasForeignKey(g => g.GameMasterId);

        modelBuilder.Entity<Game>()
            .HasMany(g => g.Tags)
            .WithMany(t => t.Games)
            .UsingEntity(gt => gt.ToTable("GameTags"));

        modelBuilder.Entity<Game>()
            .HasMany(g => g.Players)
            .WithMany(p => p.CurrentGames)
            .UsingEntity(gp => gp.ToTable("GamePlayers"));

        // modelBuilder.Entity<Player>()
        //     .HasOne(g => g.Blog)
        //     .WithMany(gm => gm.Author)
        //     .HasForeignKey(g => g.AuthorId);
    
        }
    }



