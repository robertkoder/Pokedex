using System;
using System.Net.Http;
using System.Threading.Tasks;
using SkiaSharp;

namespace PokedexApp
{
    public static class AsciiArtService
    {
        public static async Task PrintAsciiArtAsync(string spriteUrl, string infoBox)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    byte[] imageBytes = await client.GetByteArrayAsync(spriteUrl);
                    using (var image = SKBitmap.Decode(imageBytes))
                    {
                        var asciiArt = ConvertToAsciiArt(image);
                        var combinedOutput = CombineInfoAndArt(infoBox, asciiArt);
                        Console.WriteLine(combinedOutput);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error fetching sprite image: " + e.Message);
                }
            }
        }

        private static string[] ConvertToAsciiArt(SKBitmap image)
        {
            char[] asciiChars = { ' ', '.', ',', ':', ';', '+', '*', '?', '%', '$', '&', '#', '@' };

            int width = 80;
            int height = (int)((double)image.Height / image.Width * width / 2);

            image = image.Resize(new SKImageInfo(width, height), SKFilterQuality.Medium);

            string[] asciiArt = new string[height];
            for (int y = 0; y < height; y++)
            {
                asciiArt[y] = "";
                for (int x = 0; x < width; x++)
                {
                    var pixel = image.GetPixel(x, y);
                    var brightness = (0.299 * pixel.Red + 0.587 * pixel.Green + 0.114 * pixel.Blue) / 255.0;
                    var index = (int)Math.Round(brightness * (asciiChars.Length - 1));
                    asciiArt[y] += asciiChars[index];
                }
            }

            return asciiArt;
        }

        private static string CombineInfoAndArt(string infoBox, string[] asciiArt)
        {
            string[] infoBoxLines = infoBox.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            int maxHeight = Math.Max(infoBoxLines.Length, asciiArt.Length);
            int infoBoxPaddingTop = (maxHeight - infoBoxLines.Length) / 2;
            int asciiArtPaddingTop = (maxHeight - asciiArt.Length) / 2;

            string combined = "";

            for (int i = 0; i < maxHeight; i++)
            {
                string artLine = i >= asciiArtPaddingTop && i < asciiArtPaddingTop + asciiArt.Length
                    ? asciiArt[i - asciiArtPaddingTop]
                    : new string(' ', asciiArt[0].Length);

                string infoLine = i >= infoBoxPaddingTop && i < infoBoxPaddingTop + infoBoxLines.Length
                    ? infoBoxLines[i - infoBoxPaddingTop]
                    : new string(' ', infoBoxLines[0].Length);

                combined += artLine + " " + infoLine + Environment.NewLine;
            }

            var combinedLines = combined.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            combined = string.Join(Environment.NewLine, combinedLines.Skip(5).Take(combinedLines.Length - 10));

            return combined;
        }
    }
}
