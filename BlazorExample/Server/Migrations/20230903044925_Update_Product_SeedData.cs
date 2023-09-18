using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorExample.Server.Migrations
{
    /// <inheritdoc />
    public partial class Update_Product_SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CategoryId", "Description", "ImageUrl" },
                values: new object[] { 2, "Nineteen Eighty-Four, also known as 1984, is a 1984 dystopian drama film written and directed by Michael Radford, based upon George Orwell's 1949 novel of the same name. Starring John Hurt, Richard Burton, Suzanna Hamilton, and Cyril Cusack, the film follows the life of Winston Smith (Hurt), a low-ranking civil servant in a war-torn London ruled by Oceania, a totalitarian superstate.[6] Smith struggles to maintain his sanity and his grip on reality as the regime's overwhelming power and influence persecutes individualism and individual thinking on both a political and personal level.", "https://upload.wikimedia.org/wikipedia/en/thumb/c/c4/Nineteen_Eighty_Four.jpg/330px-Nineteen_Eighty_Four.jpg" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CategoryId", "Description", "ImageUrl" },
                values: new object[] { 1, "Nineteen Eighty-Four (also stylised as 1984) is a dystopian social science fiction novel and cautionary tale written by English writer George Orwell. It was published on 8 June 1949 by Secker & Warburg as Orwell's ninth and final book completed in his lifetime. Thematically, it centres on the consequences of totalitarianism, mass surveillance and repressive regimentation of people and behaviours within society.[2][3] Orwell, a democratic socialist, modelled the totalitarian government in the novel after Stalinist Russia and Nazi Germany.[2][3][4] More broadly, the novel examines the role of truth and facts within politics and the ways in which they are manipulated.", "https://upload.wikimedia.org/wikipedia/commons/c/c3/1984first.jpg" });
        }
    }
}
