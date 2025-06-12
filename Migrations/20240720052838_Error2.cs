using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pdf1.Migrations
{
    /// <inheritdoc />
    public partial class Error2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Skillgroup_SkillgroupId",
                table: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skillgroup",
                table: "Skillgroup");

            migrationBuilder.DropColumn(
                name: "SgId",
                table: "Skills");

            migrationBuilder.RenameTable(
                name: "Skillgroup",
                newName: "Skillgroups");

            migrationBuilder.AlterColumn<int>(
                name: "SkillId",
                table: "Resourcesofskill",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ResId",
                table: "Resourcesofskill",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skillgroups",
                table: "Skillgroups",
                column: "SkillgroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Skillgroups_SkillgroupId",
                table: "Skills",
                column: "SkillgroupId",
                principalTable: "Skillgroups",
                principalColumn: "SkillgroupId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Skillgroups_SkillgroupId",
                table: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skillgroups",
                table: "Skillgroups");

            migrationBuilder.RenameTable(
                name: "Skillgroups",
                newName: "Skillgroup");

            migrationBuilder.AddColumn<int>(
                name: "SgId",
                table: "Skills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SkillId",
                table: "Resourcesofskill",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ResId",
                table: "Resourcesofskill",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skillgroup",
                table: "Skillgroup",
                column: "SkillgroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Skillgroup_SkillgroupId",
                table: "Skills",
                column: "SkillgroupId",
                principalTable: "Skillgroup",
                principalColumn: "SkillgroupId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
