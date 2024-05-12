using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatRoom.WebApplication.Migrations
{
    /// <inheritdoc />
    public partial class T001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ent_room",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ent_room", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ent_user",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ent_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ent_chat_message",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(280)", maxLength: 280, nullable: false),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ent_chat_message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ent_chat_message_ent_room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "ent_room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ent_chat_message_ent_user_UserId",
                        column: x => x.UserId,
                        principalTable: "ent_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ent_chat_message_RoomId_ReceivedAt",
                table: "ent_chat_message",
                columns: new[] { "RoomId", "ReceivedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ent_chat_message_UserId",
                table: "ent_chat_message",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ent_chat_message");

            migrationBuilder.DropTable(
                name: "ent_room");

            migrationBuilder.DropTable(
                name: "ent_user");
        }
    }
}
