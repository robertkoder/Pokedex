using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace PokedexApp
{
    public static class Utility
    {
        public static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return char.ToUpper(input[0]) + input.Substring(1);
        }

        public static string GeneratePokemonInfoBox(JObject pokemonData)
        {
            string name = CapitalizeFirstLetter(pokemonData["name"]?.ToString() ?? "N/A");
            string id = pokemonData["id"]?.ToString() ?? "N/A";
            string height = pokemonData["height"]?.ToString() ?? "N/A";
            string weight = pokemonData["weight"]?.ToString() ?? "N/A";
            string types = string.Join(", ", pokemonData["types"]?.Select(t => CapitalizeFirstLetter(t["type"]?["name"]?.ToString() ?? "N/A")) ?? new List<string>());

            int maxLength = new[] { $"Name: {name}", $"ID: {id}", $"Height: {height}", $"Weight: {weight}", $"Types: {types}" }
                .Max(line => line.Length);

            string horizontalBorder = $"+-{new string('-', maxLength)}-+";

            string infoBox = $@"
{horizontalBorder}
| Name: {name.PadRight(maxLength - 6)} |
| ID: {id.PadRight(maxLength - 4)} |
| Height: {height.PadRight(maxLength - 8)} |
| Weight: {weight.PadRight(maxLength - 8)} |
| Types: {types.PadRight(maxLength - 7)} |
{horizontalBorder}";

            return infoBox.Trim('\n');
        }
    }
}
