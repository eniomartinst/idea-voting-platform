using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CisApi.Src.Infrastructure.MySQL.Migrations
{
    /// <inheritdoc />
    public partial class AddIdeasTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Idea_topics_TopicId",
                table: "Idea");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Idea",
                table: "Idea");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Idea");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Idea");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Idea");

            migrationBuilder.DropColumn(
                name: "VoteCount",
                table: "Idea");

            migrationBuilder.RenameTable(
                name: "Idea",
                newName: "ideas");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ideas",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "VotedBy",
                table: "ideas",
                newName: "voted_by");

            migrationBuilder.RenameColumn(
                name: "TopicId",
                table: "ideas",
                newName: "topic_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ideas",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_Idea_TopicId",
                table: "ideas",
                newName: "IX_ideas_topic_id");

            migrationBuilder.AlterColumn<string>(
                name: "topic_id",
                table: "ideas",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "ideas",
                type: "datetime(0)",
                precision: 0,
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<string>(
                name: "content",
                table: "ideas",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "user_id",
                table: "ideas",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "votes_count",
                table: "ideas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ideas",
                table: "ideas",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ideas_topics_topic_id",
                table: "ideas",
                column: "topic_id",
                principalTable: "topics",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ideas_topics_topic_id",
                table: "ideas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ideas",
                table: "ideas");

            migrationBuilder.DropColumn(
                name: "content",
                table: "ideas");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "ideas");

            migrationBuilder.DropColumn(
                name: "votes_count",
                table: "ideas");

            migrationBuilder.RenameTable(
                name: "ideas",
                newName: "Idea");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Idea",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "voted_by",
                table: "Idea",
                newName: "VotedBy");

            migrationBuilder.RenameColumn(
                name: "topic_id",
                table: "Idea",
                newName: "TopicId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Idea",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_ideas_topic_id",
                table: "Idea",
                newName: "IX_Idea_TopicId");

            migrationBuilder.AlterColumn<string>(
                name: "TopicId",
                table: "Idea",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Idea",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(0)",
                oldPrecision: 0,
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Idea",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Idea",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Idea",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "VoteCount",
                table: "Idea",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Idea",
                table: "Idea",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Idea_topics_TopicId",
                table: "Idea",
                column: "TopicId",
                principalTable: "topics",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
