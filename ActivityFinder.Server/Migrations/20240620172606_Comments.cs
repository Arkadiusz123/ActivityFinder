using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ActivityFinder.Server.Migrations
{
    /// <inheritdoc />
    public partial class Comments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_Addresses_AddressId",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_Activity_AspNetUsers_CreatorId",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityApplicationUser_Activity_JoinedActivitiesActivityId",
                table: "ActivityApplicationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activity",
                table: "Activity");

            migrationBuilder.RenameTable(
                name: "Activity",
                newName: "Activities");

            migrationBuilder.RenameIndex(
                name: "IX_Activity_CreatorId",
                table: "Activities",
                newName: "IX_Activities_CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Activity_AddressId",
                table: "Activities",
                newName: "IX_Activities_AddressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activities",
                table: "Activities",
                column: "ActivityId");

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ActivityId = table.Column<int>(type: "integer", nullable: false),
                    DbProperties_Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DbProperties_Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    DbProperties_Edited = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "ActivityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ActivityId",
                table: "Comments",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Addresses_AddressId",
                table: "Activities",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_AspNetUsers_CreatorId",
                table: "Activities",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityApplicationUser_Activities_JoinedActivitiesActivity~",
                table: "ActivityApplicationUser",
                column: "JoinedActivitiesActivityId",
                principalTable: "Activities",
                principalColumn: "ActivityId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Addresses_AddressId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_AspNetUsers_CreatorId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityApplicationUser_Activities_JoinedActivitiesActivity~",
                table: "ActivityApplicationUser");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activities",
                table: "Activities");

            migrationBuilder.RenameTable(
                name: "Activities",
                newName: "Activity");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_CreatorId",
                table: "Activity",
                newName: "IX_Activity_CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_AddressId",
                table: "Activity",
                newName: "IX_Activity_AddressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activity",
                table: "Activity",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_Addresses_AddressId",
                table: "Activity",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_AspNetUsers_CreatorId",
                table: "Activity",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityApplicationUser_Activity_JoinedActivitiesActivityId",
                table: "ActivityApplicationUser",
                column: "JoinedActivitiesActivityId",
                principalTable: "Activity",
                principalColumn: "ActivityId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
