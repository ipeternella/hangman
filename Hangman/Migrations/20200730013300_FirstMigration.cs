﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hangman.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameRooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuessWord",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    GameRoomId = table.Column<Guid>(nullable: false),
                    Word = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuessWord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuessWord_GameRooms_GameRoomId",
                        column: x => x.GameRoomId,
                        principalTable: "GameRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameRoomPlayers",
                columns: table => new
                {
                    GameRoomId = table.Column<Guid>(nullable: false),
                    PlayerId = table.Column<Guid>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    IsHost = table.Column<bool>(nullable: false),
                    IsBanned = table.Column<bool>(nullable: false),
                    IsInRoom = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRoomPlayers", x => new { x.GameRoomId, x.PlayerId });
                    table.ForeignKey(
                        name: "FK_GameRoomPlayers_GameRooms_GameRoomId",
                        column: x => x.GameRoomId,
                        principalTable: "GameRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameRoomPlayers_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameRound",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    Health = table.Column<int>(nullable: false),
                    IsOver = table.Column<bool>(nullable: false),
                    GuessWordId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRound", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameRound_GuessWord_GuessWordId",
                        column: x => x.GuessWordId,
                        principalTable: "GuessWord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuessLetter",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    GuessWordId = table.Column<Guid>(nullable: false),
                    Letter = table.Column<string>(maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuessLetter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuessLetter_GuessWord_GuessWordId",
                        column: x => x.GuessWordId,
                        principalTable: "GuessWord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameRoomPlayers_PlayerId",
                table: "GameRoomPlayers",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_GameRound_GuessWordId",
                table: "GameRound",
                column: "GuessWordId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuessLetter_GuessWordId",
                table: "GuessLetter",
                column: "GuessWordId");

            migrationBuilder.CreateIndex(
                name: "IX_GuessWord_GameRoomId",
                table: "GuessWord",
                column: "GameRoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameRoomPlayers");

            migrationBuilder.DropTable(
                name: "GameRound");

            migrationBuilder.DropTable(
                name: "GuessLetter");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "GuessWord");

            migrationBuilder.DropTable(
                name: "GameRooms");
        }
    }
}
