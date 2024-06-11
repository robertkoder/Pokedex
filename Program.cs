using System;

namespace PokedexApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new PokedexApp().Run();
        }
    }
}
