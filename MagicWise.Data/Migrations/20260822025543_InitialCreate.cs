using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicWise.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TagCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsVisible = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_TagCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "TagCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DestinationTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DestinationId = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    TagId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DestinationTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DestinationTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TagCategories",
                columns: new[] { "Id", "DisplayOrder", "IsVisible", "Name" },
                values: new object[,]
                {
                    { 1, 1, true, "Company" },
                    { 2, 2, true, "Region" },
                    { 3, 3, true, "Country" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CategoryId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Disney" },
                    { 2, 1, "Universal" },
                    { 3, 1, "Six Flags" },
                    { 4, 1, "SeaWorld Parks & Entertainment" },
                    { 5, 1, "Merlin Entertainments" },
                    { 6, 1, "Cedar Fair / Paramount" },
                    { 7, 1, "Herschend Family Entertainment" },
                    { 8, 1, "Parques Reunidos" },
                    { 9, 1, "Efteling" },
                    { 10, 1, "Europa-Park" },
                    { 11, 2, "North America" },
                    { 12, 2, "Europe" },
                    { 13, 2, "Asia-Pacific" },
                    { 14, 2, "Middle East" },
                    { 15, 2, "Latin America" },
                    { 16, 3, "United States" },
                    { 17, 3, "United Kingdom" },
                    { 18, 3, "France" },
                    { 19, 3, "Japan" },
                    { 20, 3, "China" },
                    { 21, 3, "Canada" },
                    { 22, 3, "Australia" },
                    { 23, 3, "United Arab Emirates" },
                    { 24, 3, "Netherlands" },
                    { 25, 3, "Germany" },
                    { 26, 3, "Spain" },
                    { 27, 3, "Belgium" },
                    { 28, 3, "Denmark" },
                    { 29, 3, "Sweden" },
                    { 30, 3, "Hong Kong" }
                });

            migrationBuilder.InsertData(
                table: "DestinationTags",
                columns: new[] { "Id", "DestinationId", "TagId" },
                values: new object[,]
                {
                    { 1, "waltdisneyworld", 1 },
                    { 2, "waltdisneyworld", 11 },
                    { 3, "waltdisneyworld", 16 },
                    { 4, "disneyland", 1 },
                    { 5, "disneyland", 11 },
                    { 6, "disneyland", 16 },
                    { 7, "disneylandparis", 1 },
                    { 8, "disneylandparis", 12 },
                    { 9, "disneylandparis", 18 },
                    { 10, "tokyodisney", 1 },
                    { 11, "tokyodisney", 13 },
                    { 12, "tokyodisney", 19 },
                    { 13, "hongkongdisneyland", 1 },
                    { 14, "hongkongdisneyland", 13 },
                    { 15, "hongkongdisneyland", 30 },
                    { 16, "shanghaidisney", 1 },
                    { 17, "shanghaidisney", 13 },
                    { 18, "shanghaidisney", 20 },
                    { 19, "universalorlando", 2 },
                    { 20, "universalorlando", 11 },
                    { 21, "universalorlando", 16 },
                    { 22, "universalhollywood", 2 },
                    { 23, "universalhollywood", 11 },
                    { 24, "universalhollywood", 16 },
                    { 25, "universaljapan", 2 },
                    { 26, "universaljapan", 13 },
                    { 27, "universaljapan", 19 },
                    { 28, "universalsingapore", 2 },
                    { 29, "universalsingapore", 13 },
                    { 30, "sixflagsmagicmountain", 3 },
                    { 31, "sixflagsmagicmountain", 11 },
                    { 32, "sixflagsmagicmountain", 16 },
                    { 33, "sixflagsgreatadventure", 3 },
                    { 34, "sixflagsgreatadventure", 11 },
                    { 35, "sixflagsgreatadventure", 16 },
                    { 36, "sixflagsovergeorgia", 3 },
                    { 37, "sixflagsovergeorgia", 11 },
                    { 38, "sixflagsovergeorgia", 16 },
                    { 39, "sixflagsovertexas", 3 },
                    { 40, "sixflagsovertexas", 11 },
                    { 41, "sixflagsovertexas", 16 },
                    { 42, "sixflagsgreatamerica", 3 },
                    { 43, "sixflagsgreatamerica", 11 },
                    { 44, "sixflagsgreatamerica", 16 },
                    { 45, "seaworldorlando", 4 },
                    { 46, "seaworldorlando", 11 },
                    { 47, "seaworldorlando", 16 },
                    { 48, "seaworldsandiego", 4 },
                    { 49, "seaworldsandiego", 11 },
                    { 50, "seaworldsandiego", 16 },
                    { 51, "seaworldsanantonio", 4 },
                    { 52, "seaworldsanantonio", 11 },
                    { 53, "seaworldsanantonio", 16 },
                    { 54, "buschgardenstampa", 4 },
                    { 55, "buschgardenstampa", 11 },
                    { 56, "buschgardenstampa", 16 },
                    { 57, "buschgardenswilliamsburg", 4 },
                    { 58, "buschgardenswilliamsburg", 11 },
                    { 59, "buschgardenswilliamsburg", 16 },
                    { 60, "legolandcalifornia", 5 },
                    { 61, "legolandcalifornia", 11 },
                    { 62, "legolandcalifornia", 16 },
                    { 63, "legolandflorida", 5 },
                    { 64, "legolandflorida", 11 },
                    { 65, "legolandflorida", 16 },
                    { 66, "legolandwindsor", 5 },
                    { 67, "legolandwindsor", 12 },
                    { 68, "legolandwindsor", 17 },
                    { 69, "legolanddeutschland", 5 },
                    { 70, "legolanddeutschland", 12 },
                    { 71, "legolanddeutschland", 25 },
                    { 72, "altontowers", 5 },
                    { 73, "altontowers", 12 },
                    { 74, "altontowers", 17 },
                    { 75, "thorpepark", 5 },
                    { 76, "thorpepark", 12 },
                    { 77, "thorpepark", 17 },
                    { 78, "chessingtonwoa", 5 },
                    { 79, "chessingtonwoa", 12 },
                    { 80, "chessingtonwoa", 17 },
                    { 81, "gardaland", 5 },
                    { 82, "gardaland", 12 },
                    { 83, "efteling", 9 },
                    { 84, "efteling", 12 },
                    { 85, "efteling", 24 },
                    { 86, "europapark", 10 },
                    { 87, "europapark", 12 },
                    { 88, "europapark", 25 },
                    { 89, "phantasialand", 12 },
                    { 90, "phantasialand", 25 },
                    { 91, "tivoligardens", 12 },
                    { 92, "tivoligardens", 28 },
                    { 93, "liseberg", 12 },
                    { 94, "liseberg", 29 },
                    { 95, "portaventuraworld", 8 },
                    { 96, "portaventuraworld", 12 },
                    { 97, "portaventuraworld", 26 },
                    { 98, "plopsaland", 8 },
                    { 99, "plopsaland", 12 },
                    { 100, "plopsaland", 27 },
                    { 101, "imgworldsofadventure", 14 },
                    { 102, "imgworldsofadventure", 23 },
                    { 103, "ferrariworldabudhabi", 14 },
                    { 104, "ferrariworldabudhabi", 23 },
                    { 105, "warnerbrosworld", 14 },
                    { 106, "warnerbrosworld", 23 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DestinationTags_DestinationId_TagId",
                table: "DestinationTags",
                columns: new[] { "DestinationId", "TagId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DestinationTags_TagId",
                table: "DestinationTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_TagCategories_Name",
                table: "TagCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_CategoryId_Name",
                table: "Tags",
                columns: new[] { "CategoryId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DestinationTags");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "TagCategories");
        }
    }
}
