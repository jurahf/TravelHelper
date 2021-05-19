using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelHelperDb.Migrations
{
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategorySet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    NaviId = table.Column<string>(nullable: true),
                    Parent_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorySet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryCategory",
                        column: x => x.Parent_Id,
                        principalTable: "CategorySet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CitySet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    Lat = table.Column<decimal>(type: "decimal(18, 10)", nullable: false),
                    Lng = table.Column<decimal>(type: "decimal(18, 10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitySet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "geo",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    country_en = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    region_en = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    city_en = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    country = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    region = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    city = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    lat = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    lng = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    population = table.Column<int>(nullable: false, defaultValueSql: "('0')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_geo", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UserSettingsSet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SelectedTravelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettingsSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NaviAddressInfoSet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContainerAddress = table.Column<string>(nullable: true),
                    SelfAddress = table.Column<string>(nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(18, 10)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18, 10)", nullable: false),
                    Picture = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Category_Id = table.Column<int>(nullable: true),
                    City_Id = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaviAddressInfoSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryNaviAddressInfo",
                        column: x => x.Category_Id,
                        principalTable: "CategorySet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CityNaviAddressInfo",
                        column: x => x.City_Id,
                        principalTable: "CitySet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(nullable: false),
                    UserSettings_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSettingsUser",
                        column: x => x.UserSettings_Id,
                        principalTable: "UserSettingsSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TravelSet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    User_Id = table.Column<int>(nullable: false),
                    City_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CityTravel",
                        column: x => x.City_Id,
                        principalTable: "CitySet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTravel",
                        column: x => x.User_Id,
                        principalTable: "UserSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleSet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    TempPoint = table.Column<int>(nullable: false),
                    Travel_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TravelSchedule",
                        column: x => x.Travel_Id,
                        principalTable: "TravelSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TravelCategory",
                columns: table => new
                {
                    TravelCategory_Category_Id = table.Column<int>(nullable: false),
                    Categories_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelCategory", x => new { x.TravelCategory_Category_Id, x.Categories_Id });
                    table.ForeignKey(
                        name: "FK_TravelCategory_Category",
                        column: x => x.Categories_Id,
                        principalTable: "CategorySet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TravelCategory_Travel",
                        column: x => x.TravelCategory_Category_Id,
                        principalTable: "TravelSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlacePointSet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(nullable: false),
                    CustomName = table.Column<string>(nullable: false),
                    Time = table.Column<DateTime>(type: "datetime", nullable: false),
                    Schedule_Id = table.Column<int>(nullable: true),
                    NaviAddressInfo_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlacePointSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NaviAddressInfoPlacePoint",
                        column: x => x.NaviAddressInfo_Id,
                        principalTable: "NaviAddressInfoSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchedulePlacePoint",
                        column: x => x.Schedule_Id,
                        principalTable: "ScheduleSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FK_CategoryCategory",
                table: "CategorySet",
                column: "Parent_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FK_CategoryNaviAddressInfo",
                table: "NaviAddressInfoSet",
                column: "Category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FK_CityNaviAddressInfo",
                table: "NaviAddressInfoSet",
                column: "City_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FK_NaviAddressInfoPlacePoint",
                table: "PlacePointSet",
                column: "NaviAddressInfo_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FK_SchedulePlacePoint",
                table: "PlacePointSet",
                column: "Schedule_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FK_TravelSchedule",
                table: "ScheduleSet",
                column: "Travel_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FK_TravelCategory_Category",
                table: "TravelCategory",
                column: "Categories_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FK_CityTravel",
                table: "TravelSet",
                column: "City_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FK_UserTravel",
                table: "TravelSet",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FK_UserSettingsUser",
                table: "UserSet",
                column: "UserSettings_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "geo");

            migrationBuilder.DropTable(
                name: "PlacePointSet");

            migrationBuilder.DropTable(
                name: "TravelCategory");

            migrationBuilder.DropTable(
                name: "NaviAddressInfoSet");

            migrationBuilder.DropTable(
                name: "ScheduleSet");

            migrationBuilder.DropTable(
                name: "CategorySet");

            migrationBuilder.DropTable(
                name: "TravelSet");

            migrationBuilder.DropTable(
                name: "CitySet");

            migrationBuilder.DropTable(
                name: "UserSet");

            migrationBuilder.DropTable(
                name: "UserSettingsSet");
        }
    }
}
