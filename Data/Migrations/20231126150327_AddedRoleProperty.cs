using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSD08_AppDev2Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedRoleProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Companys_JobCompanyId",
                table: "Jobs");

            migrationBuilder.DropTable(
                name: "ApplicationRoles");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_JobCompanyId",
                table: "Jobs");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ApplicationRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationRoles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_JobCompanyId",
                table: "Jobs",
                column: "JobCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Companys_JobCompanyId",
                table: "Jobs",
                column: "JobCompanyId",
                principalTable: "Companys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
