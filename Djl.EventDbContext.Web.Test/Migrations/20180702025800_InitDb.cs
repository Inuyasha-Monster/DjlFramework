using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Djl.EventDbContext.Web.Test.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MqEventMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatiedDateTime = table.Column<DateTime>(nullable: false),
                    SeedTime = table.Column<DateTime>(nullable: true),
                    AssemblyName = table.Column<string>(maxLength: 100, nullable: false),
                    ClassFullName = table.Column<string>(maxLength: 100, nullable: false),
                    JsonBody = table.Column<string>(maxLength: 4000, nullable: false),
                    PublishErrorMsg = table.Column<string>(maxLength: 1000, nullable: true),
                    RetryCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MqEventMessages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MqEventMessages");
        }
    }
}
