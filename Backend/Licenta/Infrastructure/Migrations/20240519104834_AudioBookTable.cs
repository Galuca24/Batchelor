using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AudioBookTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.AddColumn<Guid>(
                name: "AudioBookId",
                table: "Reviews",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AudioBookId",
                table: "Ratings",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AudioBookId",
                table: "Loans",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AudioBooks",
                columns: table => new
                {
                    AudioBookId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ISBN = table.Column<string>(type: "text", nullable: false),
                    Author = table.Column<string>(type: "text", nullable: false),
                    Genre = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    AudioUrl = table.Column<string>(type: "text", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AudioBooks", x => x.AudioBookId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_AudioBookId",
                table: "Reviews",
                column: "AudioBookId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_AudioBookId",
                table: "Ratings",
                column: "AudioBookId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_AudioBookId",
                table: "Loans",
                column: "AudioBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_AudioBooks_AudioBookId",
                table: "Loans",
                column: "AudioBookId",
                principalTable: "AudioBooks",
                principalColumn: "AudioBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AudioBooks_AudioBookId",
                table: "Ratings",
                column: "AudioBookId",
                principalTable: "AudioBooks",
                principalColumn: "AudioBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AudioBooks_AudioBookId",
                table: "Reviews",
                column: "AudioBookId",
                principalTable: "AudioBooks",
                principalColumn: "AudioBookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_AudioBooks_AudioBookId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AudioBooks_AudioBookId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AudioBooks_AudioBookId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "AudioBooks");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_AudioBookId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_AudioBookId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Loans_AudioBookId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "AudioBookId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "AudioBookId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "AudioBookId",
                table: "Loans");

            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.ConversationId);
                    table.ForeignKey(
                        name: "FK_Conversations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

           
        }
    }
}
