using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActivityFinder.Server.Migrations
{
    /// <inheritdoc />
    public partial class usersLimit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsersLimit",
                table: "Activity",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsersLimit",
                table: "Activity");
        }
    }
}
