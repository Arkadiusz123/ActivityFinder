using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActivityFinder.Server.Migrations
{
    /// <inheritdoc />
    public partial class addressrelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Activity",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OtherInfo",
                table: "Activity",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ActivityApplicationUser",
                columns: table => new
                {
                    JoinedActivitiesActivityId = table.Column<int>(type: "integer", nullable: false),
                    JoinedUsersId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityApplicationUser", x => new { x.JoinedActivitiesActivityId, x.JoinedUsersId });
                    table.ForeignKey(
                        name: "FK_ActivityApplicationUser_Activity_JoinedActivitiesActivityId",
                        column: x => x.JoinedActivitiesActivityId,
                        principalTable: "Activity",
                        principalColumn: "ActivityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityApplicationUser_AspNetUsers_JoinedUsersId",
                        column: x => x.JoinedUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_OsmId",
                table: "Addresses",
                column: "OsmId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activity_CreatorId",
                table: "Activity",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityApplicationUser_JoinedUsersId",
                table: "ActivityApplicationUser",
                column: "JoinedUsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_AspNetUsers_CreatorId",
                table: "Activity",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_AspNetUsers_CreatorId",
                table: "Activity");

            migrationBuilder.DropTable(
                name: "ActivityApplicationUser");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_OsmId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Activity_CreatorId",
                table: "Activity");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Activity");

            migrationBuilder.DropColumn(
                name: "OtherInfo",
                table: "Activity");
        }
    }
}
