using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MusicStreaming.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Genre = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ReleaseYear = table.Column<int>(type: "INTEGER", nullable: false),
                    Genre = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ArtistId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Albums_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Playlists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Duration = table.Column<int>(type: "INTEGER", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Genre = table.Column<string>(type: "TEXT", nullable: false),
                    AlbumId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Songs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Songs_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistSongs",
                columns: table => new
                {
                    PlaylistId = table.Column<int>(type: "INTEGER", nullable: false),
                    SongId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistSongs", x => new { x.PlaylistId, x.SongId });
                    table.ForeignKey(
                        name: "FK_PlaylistSongs_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistSongs_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Artists",
                columns: new[] { "Id", "Genre", "Name" },
                values: new object[,]
                {
                    { 1, "Metal", "Metallica" },
                    { 2, "Pop", "Britney Spears" },
                    { 3, "Electronic", "Aphex Twin" },
                    { 4, "K-pop", "SHINee" },
                    { 5, "Hip-hop", "De La Soul" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateOfBirth", "Email", "Password", "Username" },
                values: new object[,]
                {
                    { "1", new DateTime(1990, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "mail@example.com", "password", "Admin" },
                    { "2", new DateTime(1992, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "maya@example.com", "password12", "maya_k" },
                    { "3", new DateTime(1988, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "omar@example.com", "password13", "omar_h" },
                    { "4", new DateTime(1995, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "layla@example.com", "password23", "layla_az" },
                    { "5", new DateTime(1991, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "karim@example.com", "password123", "karim_h" }
                });

            migrationBuilder.InsertData(
                table: "Albums",
                columns: new[] { "Id", "ArtistId", "Genre", "ReleaseYear", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Metal", 1988, "And Justice for All" },
                    { 2, 2, "Pop", 1999, "Baby One More Time" },
                    { 3, 3, "Electronic", 1995, "I Care Because You Do" },
                    { 4, 4, "K-pop", 2016, "1 of 1" },
                    { 5, 5, "Hip-hop", 1989, "3 Feet High and Rising" },
                    { 6, 5, "Hip-hop", 2020, "Reloaded" },
                    { 7, 1, "Metal", 1991, "Enter the Metal" }
                });

            migrationBuilder.InsertData(
                table: "Playlists",
                columns: new[] { "Id", "CreationDate", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "My Metal Playlist", "1" },
                    { 2, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pop Hits", "1" },
                    { 3, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hip-Hop Essentials", "1" }
                });

            migrationBuilder.InsertData(
                table: "Songs",
                columns: new[] { "Id", "AlbumId", "Duration", "Genre", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { 1, 1, 500, "Metal", new DateTime(1988, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "One" },
                    { 2, 1, 330, "Metal", new DateTime(1988, 7, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Enter Sandman" },
                    { 3, 2, 270, "Pop", new DateTime(1999, 9, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Baby One More Time" },
                    { 4, 2, 300, "Pop", new DateTime(1999, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "You Drive Me Crazy" },
                    { 5, 3, 380, "Electronic", new DateTime(1995, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Windowlicker" },
                    { 6, 3, 200, "Electronic", new DateTime(1995, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ventolin" },
                    { 7, 4, 340, "K-pop", new DateTime(2016, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "View from the Top" },
                    { 8, 4, 250, "K-pop", new DateTime(2016, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tell Me What to Do" },
                    { 9, 5, 230, "Hip-hop", new DateTime(1989, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Potholes in My Lawn" },
                    { 10, 5, 210, "Hip-hop", new DateTime(1989, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Me Myself and I" },
                    { 11, 6, 220, "Hip-hop", new DateTime(2020, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Push the Tempo" },
                    { 12, 7, 350, "Metal", new DateTime(1991, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "We Are The Champions" },
                    { 13, 7, 400, "Metal", new DateTime(1991, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fade To Black" }
                });

            migrationBuilder.InsertData(
                table: "PlaylistSongs",
                columns: new[] { "PlaylistId", "SongId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 12 },
                    { 2, 3 },
                    { 2, 4 },
                    { 2, 13 },
                    { 3, 5 },
                    { 3, 9 },
                    { 3, 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Albums_ArtistId",
                table: "Albums",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_UserId",
                table: "Playlists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistSongs_SongId",
                table: "PlaylistSongs",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_AlbumId",
                table: "Songs",
                column: "AlbumId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaylistSongs");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "Songs");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "Artists");
        }
    }
}
