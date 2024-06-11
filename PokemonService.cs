using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PokedexApp
{
    public static class PokemonService
    {
        public static async Task<bool> IsTypeAsync(string type)
        {
            string typeApiUrl = $"https://pokeapi.co/api/v2/type/{type}";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(typeApiUrl);
                    response.EnsureSuccessStatusCode();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static async Task FetchPokemonsByTypeAsync(string type)
        {
            string apiUrl = $"https://pokeapi.co/api/v2/type/{type}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject? typeData = JObject.Parse(responseBody);

                    if (typeData != null)
                    {
                        string formattedType = Utility.CapitalizeFirstLetter(type);
                        Console.WriteLine($"{formattedType} type Pokémon:");
                        var pokemons = typeData["pokemon"];
                        if (pokemons != null)
                        {
                            foreach (var pokemon in pokemons)
                            {
                                Console.WriteLine($"- {Utility.CapitalizeFirstLetter(pokemon["pokemon"]?["name"]?.ToString() ?? "N/A")}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data found for the specified type.");
                    }
                }
                catch (HttpRequestException)
                {
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    Console.WriteLine("Type not found.");
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    Console.WriteLine("Error: " + e.Message);
                }
            }
        }

        public static async Task FetchPokemonDataAsync(string identifier)
        {
            string apiUrl = $"https://pokeapi.co/api/v2/pokemon/{identifier}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject? pokemonData = JObject.Parse(responseBody);

                    if (pokemonData != null)
                    {
                        var infoBox = Utility.GeneratePokemonInfoBox(pokemonData);

                        string? spriteUrl = pokemonData["sprites"]?["front_default"]?.ToString();
                        if (!string.IsNullOrEmpty(spriteUrl))
                        {
                            await AsciiArtService.PrintAsciiArtAsync(spriteUrl, infoBox);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data found for the specified Pokémon.");
                    }
                }
                catch (HttpRequestException)
                {
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Pokémon not found.");
                    Console.ResetColor();
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    Console.WriteLine("Error: " + e.Message);
                }
            }
        }
    }
}
