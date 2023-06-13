using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RuxGymAPI.Migrations
{
    /// <inheritdoc />
    public partial class mig_ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OlimpiaWeeks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Week = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OlimpiaWeeks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PasswordCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodeKey = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerEnergies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerCurrentEnergy = table.Column<int>(type: "int", nullable: false),
                    StartEnergyTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndEnergyTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerEnergies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerGymItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DumbbellPressItem = table.Column<int>(type: "int", nullable: false),
                    AbsItem = table.Column<int>(type: "int", nullable: false),
                    SquatItem = table.Column<int>(type: "int", nullable: false),
                    DeadLiftItem = table.Column<int>(type: "int", nullable: false),
                    BenchPressItem = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerGymItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerPremia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    isPremium = table.Column<bool>(type: "bit", nullable: false),
                    EndPremiumDay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerPremia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    CreatedDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastConnectionDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    IsGuest = table.Column<bool>(type: "bit", nullable: false),
                    IsFacebookUser = table.Column<bool>(type: "bit", nullable: false),
                    FacebookId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerStats",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ALlPower = table.Column<float>(type: "real", nullable: false),
                    ArmPower = table.Column<float>(type: "real", nullable: false),
                    SixpackPower = table.Column<float>(type: "real", nullable: false),
                    BackPower = table.Column<float>(type: "real", nullable: false),
                    LegPower = table.Column<float>(type: "real", nullable: false),
                    ChestPower = table.Column<float>(type: "real", nullable: false),
                    ProteinItem = table.Column<int>(type: "int", nullable: false),
                    CreatinItem = table.Column<int>(type: "int", nullable: false),
                    EnergyItem = table.Column<int>(type: "int", nullable: false),
                    PlayerCash = table.Column<int>(type: "int", nullable: false),
                    PlayerDiamond = table.Column<int>(type: "int", nullable: false),
                    PlayerSpinCount = table.Column<int>(type: "int", nullable: false),
                    PlayerGoldTicket = table.Column<int>(type: "int", nullable: false),
                    OlimpiaWin = table.Column<int>(type: "int", nullable: false),
                    IsOlimpia = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerStats", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SpinDateTimes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedSpinTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpinDateTimes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OlimpiaWeeks");

            migrationBuilder.DropTable(
                name: "PasswordCodes");

            migrationBuilder.DropTable(
                name: "PlayerEnergies");

            migrationBuilder.DropTable(
                name: "PlayerGymItems");

            migrationBuilder.DropTable(
                name: "PlayerPremia");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "PlayerStats");

            migrationBuilder.DropTable(
                name: "SpinDateTimes");
        }
    }
}
