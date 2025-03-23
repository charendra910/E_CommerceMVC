using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_CommerceMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryToSliderImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "SliderImages",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "SliderImages");
        }
    }
}
