using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BoardWidth = table.Column<int>(type: "INTEGER", nullable: false),
                    BoardHeight = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerOneStarts = table.Column<bool>(type: "INTEGER", nullable: false),
                    CompulsoryJumps = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CheckersGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    GameOptionsId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GameOverAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    GameWonByPlayer = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    PlayerOneColor = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerOneId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerTwoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckersGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckersGames_GameOptions_GameOptionsId",
                        column: x => x.GameOptionsId,
                        principalTable: "GameOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckersGames_Players_PlayerOneId",
                        column: x => x.PlayerOneId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckersGames_Players_PlayerTwoId",
                        column: x => x.PlayerTwoId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SerializedBoardState = table.Column<string>(type: "TEXT", nullable: false),
                    PlayerOneIsCurrent = table.Column<bool>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CheckersGameId = table.Column<int>(type: "INTEGER", nullable: false),
                    SerializedNextMoves = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameStates_CheckersGames_CheckersGameId",
                        column: x => x.CheckersGameId,
                        principalTable: "CheckersGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckersGames_GameOptionsId",
                table: "CheckersGames",
                column: "GameOptionsId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckersGames_PlayerOneId",
                table: "CheckersGames",
                column: "PlayerOneId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckersGames_PlayerTwoId",
                table: "CheckersGames",
                column: "PlayerTwoId");

            migrationBuilder.CreateIndex(
                name: "IX_GameStates_CheckersGameId",
                table: "GameStates",
                column: "CheckersGameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameStates");

            migrationBuilder.DropTable(
                name: "CheckersGames");

            migrationBuilder.DropTable(
                name: "GameOptions");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
