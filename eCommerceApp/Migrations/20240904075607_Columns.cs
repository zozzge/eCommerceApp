using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommerceApp.Migrations
{
    /// <inheritdoc />
    public partial class Columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "SyncIds",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "SyncIds",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CartId",
                table: "SyncIds");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SyncIds");
        }
    }
}
