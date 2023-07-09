using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RuxGymAPI.Migrations
{
    /// <inheritdoc />
    public partial class mig_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isPremium",
                table: "PlayerPremia",
                newName: "IsPremium");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPremium",
                table: "PlayerPremia",
                newName: "isPremium");
        }
    }
}
