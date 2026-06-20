using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CisApi.Src.Infrastructure.MySQL.Migrations
{
    /// <inheritdoc />
    public partial class RefactorTopicUserRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Idea_Topics_TopicId",
                table: "Idea");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Topics",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Topics");

            migrationBuilder.RenameTable(
                name: "Topics",
                newName: "topics");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "topics",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "topics",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "topics",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "TotalIdeaCount",
                table: "topics",
                newName: "total_idea_count");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "topics",
                newName: "created_at");

            migrationBuilder.AddColumn<string>(
                name: "user_id",
                table: "topics",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_topics",
                table: "topics",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Idea_topics_TopicId",
                table: "Idea",
                column: "TopicId",
                principalTable: "topics",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Idea_topics_TopicId",
                table: "Idea");

            migrationBuilder.DropPrimaryKey(
                name: "PK_topics",
                table: "topics");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "topics");

            migrationBuilder.RenameTable(
                name: "topics",
                newName: "Topics");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Topics",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Topics",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Topics",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "total_idea_count",
                table: "Topics",
                newName: "TotalIdeaCount");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Topics",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Topics",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Topics",
                table: "Topics",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Idea_Topics_TopicId",
                table: "Idea",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
