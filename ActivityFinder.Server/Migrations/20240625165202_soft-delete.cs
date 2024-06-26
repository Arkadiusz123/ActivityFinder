using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActivityFinder.Server.Migrations
{
    /// <inheritdoc />
    public partial class softdelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DbProperties_DeleteTime",
                table: "Comments",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DbProperties_Created",
                table: "AspNetUsers",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DbProperties_DeleteTime",
                table: "AspNetUsers",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DbProperties_Deleted",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DbProperties_Edited",
                table: "AspNetUsers",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DbProperties_DeleteTime",
                table: "Activities",
                type: "timestamp without time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DbProperties_DeleteTime",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "DbProperties_Created",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DbProperties_DeleteTime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DbProperties_Deleted",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DbProperties_Edited",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DbProperties_DeleteTime",
                table: "Activities");
        }
    }
}
