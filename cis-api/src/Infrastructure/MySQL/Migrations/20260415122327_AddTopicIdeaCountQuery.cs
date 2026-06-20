using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CisApi.Src.Infrastructure.MySQL.Migrations
{
    /// <inheritdoc />
    public partial class AddTopicIdeaCountQuery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "total_idea_count",
                table: "topics");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "total_idea_count",
                table: "topics",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
