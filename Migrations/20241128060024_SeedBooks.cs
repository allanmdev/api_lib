using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_lib.Migrations
{
    /// <inheritdoc />
    public partial class SeedBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
               table: "Books",
               columns: new[] { "Title", "Author", "Year", "Quantity", "CreatedAt" },
               values: new object[,]
               {
                    { "Dom Casmurro", "Machado de Assis", 1899, 2, DateTime.Now },
                    { "Memórias Póstumas de Brás Cubas", "Machado de Assis", 1881, 3, DateTime.Now },
                    { "Grande Sertão: Veredas", "João Guimarães Rosa", 1956, 4, DateTime.Now },
                    { "O Cortiço", "Aluísio Azevedo", 1890, 4, DateTime.Now },
                    { "Iracema", "José de Alencar", 1865, 1, DateTime.Now },
                    { "Macunaíma", "Mário de Andrade", 1928, 11, DateTime.Now },
                    { "Capitães da Areia", "Jorge Amado", 1937, 2, DateTime.Now },
                    { "Vidas Secas", "Graciliano Ramos", 1938, 9, DateTime.Now },
                    { "A Moreninha", "Joaquim Manuel de Macedo", 1844, 2, DateTime.Now },
                    { "O Tempo e o Vento", "Erico Verissimo", 1949, 1, DateTime.Now },
                    { "A Hora da Estrela", "Clarice Lispector", 1977, 1, DateTime.Now },
                    { "O Quinze", "Rachel de Queiroz", 1930, 1, DateTime.Now },
                    { "Menino do Engenho", "José Lins do Rego", 1932, 5, DateTime.Now },
                    { "Sagarana", "João Guimarães Rosa", 1946, 3, DateTime.Now },
                    { "Fogo Morto", "José Lins do Rego", 1943, 1, DateTime.Now }
               });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Books WHERE Title IN (" +
               "'Dom Casmurro', " +
               "'Memórias Póstumas de Brás Cubas', " +
               "'Grande Sertão: Veredas', " +
               "'O Cortiço', " +
               "'Iracema', " +
               "'Macunaíma', " +
               "'Capitães da Areia', " +
               "'Vidas Secas', " +
               "'A Moreninha', " +
               "'O Tempo e o Vento', " +
               "'A Hora da Estrela', " +
               "'O Quinze', " +
               "'Menino do Engenho', " +
               "'Sagarana', " +
               "'Fogo Morto'" +
               ");");

        }
    }
}
