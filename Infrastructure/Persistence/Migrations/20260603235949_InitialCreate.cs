using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StructAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KnowledgeEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    User = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ReplacesEntryId = table.Column<int>(type: "integer", nullable: true),
                    Embedding = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KnowledgeEntryLifecycleLogs",
                columns: table => new
                {
                    KnowledgeEntryId = table.Column<int>(type: "integer", nullable: false),
                    OccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    OldStatus = table.Column<string>(type: "text", nullable: false),
                    NewStatus = table.Column<string>(type: "text", nullable: false),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    User = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeEntryLifecycleLogs", x => new { x.KnowledgeEntryId, x.OccurredAt });
                });

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeEntries_CreatedAt",
                table: "KnowledgeEntries",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeEntries_Status",
                table: "KnowledgeEntries",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeEntryLifecycleLogs_NewStatus",
                table: "KnowledgeEntryLifecycleLogs",
                column: "NewStatus");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeEntryLifecycleLogs_OccurredAt",
                table: "KnowledgeEntryLifecycleLogs",
                column: "OccurredAt");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeEntryLifecycleLogs_OldStatus",
                table: "KnowledgeEntryLifecycleLogs",
                column: "OldStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KnowledgeEntries");

            migrationBuilder.DropTable(
                name: "KnowledgeEntryLifecycleLogs");
        }
    }
}
