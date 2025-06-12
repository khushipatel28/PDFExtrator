using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pdf1.Migrations
{
    /// <inheritdoc />
    public partial class Error : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Skillgroups_SgId",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_SgId",
                table: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skillgroups",
                table: "Skillgroups");

            migrationBuilder.RenameTable(
                name: "Skillgroups",
                newName: "Skillgroup");

            migrationBuilder.AddColumn<int>(
                name: "SkillgroupId",
                table: "Skills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skillgroup",
                table: "Skillgroup",
                column: "SkillgroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_SkillgroupId",
                table: "Skills",
                column: "SkillgroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Skillgroup_SkillgroupId",
                table: "Skills",
                column: "SkillgroupId",
                principalTable: "Skillgroup",
                principalColumn: "SkillgroupId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Skillgroup_SkillgroupId",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_SkillgroupId",
                table: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skillgroup",
                table: "Skillgroup");

            migrationBuilder.DropColumn(
                name: "SkillgroupId",
                table: "Skills");

            migrationBuilder.RenameTable(
                name: "Skillgroup",
                newName: "Skillgroups");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skillgroups",
                table: "Skillgroups",
                column: "SkillgroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_SgId",
                table: "Skills",
                column: "SgId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Skillgroups_SgId",
                table: "Skills",
                column: "SgId",
                principalTable: "Skillgroups",
                principalColumn: "SkillgroupId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
