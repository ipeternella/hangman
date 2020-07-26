﻿// <auto-generated />
using System;
using Hangman.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Hangman.Migrations
{
    [DbContext(typeof(HangmanDbContext))]
    [Migration("20200726215052_GuessWordLetterMigration")]
    partial class GuessWordLetterMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Hangman.Models.GameRoom", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("GameRooms");
                });

            modelBuilder.Entity("Hangman.Models.GameRoomPlayer", b =>
                {
                    b.Property<Guid>("GameRoomId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsBanned")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsHost")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsInRoom")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("GameRoomId", "PlayerId");

                    b.HasIndex("PlayerId");

                    b.ToTable("GameRoomPlayers");
                });

            modelBuilder.Entity("Hangman.Models.GuessLetter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("GuessWordId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("GuessWordId");

                    b.ToTable("GuessLetter");
                });

            modelBuilder.Entity("Hangman.Models.GuessWord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("GameRoomId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("GameRoomId");

                    b.ToTable("GuessWord");
                });

            modelBuilder.Entity("Hangman.Models.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Hangman.Models.GameRoomPlayer", b =>
                {
                    b.HasOne("Hangman.Models.GameRoom", "GameRoom")
                        .WithMany("GameRoomPlayers")
                        .HasForeignKey("GameRoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hangman.Models.Player", "Player")
                        .WithMany("GameRoomPlayers")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Hangman.Models.GuessLetter", b =>
                {
                    b.HasOne("Hangman.Models.GuessWord", "GuessWord")
                        .WithMany("GuessLetters")
                        .HasForeignKey("GuessWordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Hangman.Models.GuessWord", b =>
                {
                    b.HasOne("Hangman.Models.GameRoom", "GameRoom")
                        .WithMany("GuessWords")
                        .HasForeignKey("GameRoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
