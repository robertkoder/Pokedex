using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokedexApp
{
    public class PokedexApp
    {
        public async Task Run()
        {
            while (true)
            {
                Console.WriteLine("Enter the name, ID, or type of a Pokémon (or type 'exit' to quit):");
                string? input = Console.ReadLine()?.ToLower();

                if (string.IsNullOrEmpty(input))
                {
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    continue;
                }

                if (input == "exit")
                {
                    break;
                }

                Console.Clear();
                Console.WriteLine("\x1b[3J");
                Console.WriteLine("Searching...");

                if (int.TryParse(input, out int pokemonId))
                {
                    await PokemonService.FetchPokemonDataAsync(pokemonId.ToString());
                }
                else if (await PokemonService.IsTypeAsync(input))
                {
                    await PokemonService.FetchPokemonsByTypeAsync(input);
                }
                else
                {
                    await PokemonService.FetchPokemonDataAsync(input);
                }
            }
        }
    }
}
