using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace practice_for_wms.Migrations
{
    /// <inheritdoc />
    public partial class MyTaskCascadeOnUserDelete2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyTasks_Users_AssignedToId",
                table: "MyTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_MyTasks_Users_AssignedToId",
                table: "MyTasks",
                column: "AssignedToId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyTasks_Users_AssignedToId",
                table: "MyTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_MyTasks_Users_AssignedToId",
                table: "MyTasks",
                column: "AssignedToId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
