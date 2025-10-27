using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Com.Coppel.Web.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "criteria",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    version = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    category = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    severity = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_criteria", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "revisions",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    key = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    uploaded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    spec_version = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    spec_date = table.Column<DateOnly>(type: "date", nullable: true),
                    spec_author = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    checksum = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_revisions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "evaluations",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    revision_id = table.Column<string>(type: "text", nullable: false),
                    idempotency = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    status = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    overall_status = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    model = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    prompt_version = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    retries = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    score_pass = table.Column<int>(type: "integer", nullable: false),
                    score_fail = table.Column<int>(type: "integer", nullable: false),
                    score_na = table.Column<int>(type: "integer", nullable: false),
                    score_total = table.Column<int>(type: "integer", nullable: false),
                    crit_pass = table.Column<int>(type: "integer", nullable: false),
                    crit_fail = table.Column<int>(type: "integer", nullable: false),
                    crit_na = table.Column<int>(type: "integer", nullable: false),
                    gate_passed = table.Column<bool>(type: "boolean", nullable: false),
                    gate_reason = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    raw_result = table.Column<JsonDocument>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_evaluations", x => x.id);
                    table.ForeignKey(
                        name: "FK_evaluations_revisions_revision_id",
                        column: x => x.revision_id,
                        principalTable: "revisions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "evaluation_criteria",
                columns: table => new
                {
                    evaluation_id = table.Column<string>(type: "text", nullable: false),
                    criterion_id = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    severity = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    evidence = table.Column<string>(type: "text", nullable: true),
                    comments = table.Column<string>(type: "text", nullable: true),
                    id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_evaluation_criteria", x => new { x.evaluation_id, x.criterion_id });
                    table.ForeignKey(
                        name: "FK_evaluation_criteria_criteria_criterion_id",
                        column: x => x.criterion_id,
                        principalTable: "criteria",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_evaluation_criteria_evaluations_evaluation_id",
                        column: x => x.evaluation_id,
                        principalTable: "evaluations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_evaluation_criteria_criterion_id",
                table: "evaluation_criteria",
                column: "criterion_id");

            migrationBuilder.CreateIndex(
                name: "IX_evaluation_criteria_severity",
                table: "evaluation_criteria",
                column: "severity");

            migrationBuilder.CreateIndex(
                name: "IX_evaluation_criteria_status",
                table: "evaluation_criteria",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_evaluations_created_at",
                table: "evaluations",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_evaluations_overall_status",
                table: "evaluations",
                column: "overall_status");

            migrationBuilder.CreateIndex(
                name: "IX_evaluations_revision_id",
                table: "evaluations",
                column: "revision_id");

            migrationBuilder.CreateIndex(
                name: "IX_revisions_key_number",
                table: "revisions",
                columns: new[] { "key", "number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_revisions_type",
                table: "revisions",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "IX_revisions_uploaded_at",
                table: "revisions",
                column: "uploaded_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "evaluation_criteria");

            migrationBuilder.DropTable(
                name: "criteria");

            migrationBuilder.DropTable(
                name: "evaluations");

            migrationBuilder.DropTable(
                name: "revisions");
        }
    }
}
