using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TravelHelperDb.Migrations
{
    public partial class CreateCustomSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategorySet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    NaviId = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorySet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategorySet_CategorySet_ParentId",
                        column: x => x.ParentId,
                        principalTable: "CategorySet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CitySet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Lat = table.Column<decimal>(nullable: false),
                    Lng = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitySet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Geo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryEn = table.Column<string>(nullable: true),
                    RegionEn = table.Column<string>(nullable: true),
                    CityEn = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Lat = table.Column<string>(nullable: true),
                    Lng = table.Column<string>(nullable: true),
                    Population = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Geo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSettingsSet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContainerAddress = table.Column<string>(nullable: true),
                    SelfAddress = table.Column<string>(nullable: true),
                    Latitude = table.Column<decimal>(nullable: false),
                    Longitude = table.Column<decimal>(nullable: false),
                    Picture = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CategoryId = table.Column<int>(nullable: true),
                    CityId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaviAddressInfoSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NaviAddressInfoSet_CategorySet_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CategorySet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NaviAddressInfoSet_CitySet_CityId",
                        column: x => x.CityId,
                        principalTable: "CitySet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Login = table.Column<string>(nullable: true),
                    UserSettingsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSet_UserSettingsSet_UserSettingsId",
                        column: x => x.UserSettingsId,
                        principalTable: "UserSettingsSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TravelSet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    CityId = table.Column<int>(nullable: false),
                    CurrentDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TravelSet_CitySet_CityId",
                        column: x => x.CityId,
                        principalTable: "CitySet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TravelSet_UserSet_UserId",
                        column: x => x.UserId,
                        principalTable: "UserSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleSet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    TempPoint = table.Column<int>(nullable: false),
                    TravelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleSet_TravelSet_TravelId",
                        column: x => x.TravelId,
                        principalTable: "TravelSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TravelCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TravelCategoryCategoryId = table.Column<int>(nullable: false),
                    CategoriesId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TravelCategory_CategorySet_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "CategorySet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TravelCategory_TravelSet_TravelCategoryCategoryId",
                        column: x => x.TravelCategoryCategoryId,
                        principalTable: "TravelSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlacePointSet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Order = table.Column<int>(nullable: false),
                    CustomName = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false),
                    ScheduleId = table.Column<int>(nullable: true),
                    NaviAddressInfoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlacePointSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlacePointSet_NaviAddressInfoSet_NaviAddressInfoId",
                        column: x => x.NaviAddressInfoId,
                        principalTable: "NaviAddressInfoSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlacePointSet_ScheduleSet_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "ScheduleSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategorySet_ParentId",
                table: "CategorySet",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_NaviAddressInfoSet_CategoryId",
                table: "NaviAddressInfoSet",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_NaviAddressInfoSet_CityId",
                table: "NaviAddressInfoSet",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_PlacePointSet_NaviAddressInfoId",
                table: "PlacePointSet",
                column: "NaviAddressInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_PlacePointSet_ScheduleId",
                table: "PlacePointSet",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleSet_TravelId",
                table: "ScheduleSet",
                column: "TravelId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelCategory_CategoriesId",
                table: "TravelCategory",
                column: "CategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelCategory_TravelCategoryCategoryId",
                table: "TravelCategory",
                column: "TravelCategoryCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelSet_CityId",
                table: "TravelSet",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelSet_UserId",
                table: "TravelSet",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSet_UserSettingsId",
                table: "UserSet",
                column: "UserSettingsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Geo");

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
