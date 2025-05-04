using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StarWarsPlanetsStats
{
    class Program
    {
        static async Task Main()
        {
            // 1. Load all planets from SWAPI
            var planets = await LoadAllPlanetsAsync();

            // 2. Print basic info: Name, Population, Diameter, Surface water
            Console.WriteLine("Loaded planets:\n");
            foreach (var p in planets)
            {
                Console.WriteLine(
                    $"{p.Name.PadRight(20)} " +
                    $"Pop: {p.PopulationRaw.PadLeft(12)}   " +
                    $"Dia: {p.DiameterRaw.PadLeft(5)}   " +
                    $"SurfWater: {p.SurfaceWaterRaw.PadLeft(5)}%");
            }

            // 3. Prompt user
            Console.WriteLine("\nThe statistics of which property would you like to see?");
            Console.WriteLine("population");
            Console.WriteLine("diameter");
            Console.WriteLine("surface water");
            Console.Write("> ");
            string? choice = Console.ReadLine()?.Trim().ToLower();

            // 4. Compute and print min/max
            switch (choice)
            {
                case "population":
                    PrintMinMax(
                        planets,
                        p => TryParseLong(p.PopulationRaw),
                        "population");
                    break;

                case "diameter":
                    PrintMinMax(
                        planets,
                        p => TryParseLong(p.DiameterRaw),
                        "diameter");
                    break;

                case "surface water":
                    PrintMinMax(
                        planets,
                        p => TryParseDouble(p.SurfaceWaterRaw),
                        "surface water");
                    break;

                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }

            Console.WriteLine("\nPress any key to close.");
            Console.ReadKey();
        }

        // Fetches all pages from SWAPI /planets endpoint
        static async Task<List<Planet>> LoadAllPlanetsAsync()
        {
            using var client = new HttpClient();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var all = new List<Planet>();
            string? url = "https://swapi.dev/api/planets";

            while (!string.IsNullOrEmpty(url))
            {
                string json = await client.GetStringAsync(url);
                var page = JsonSerializer.Deserialize<PlanetsPage>(json, options)
                           ?? throw new InvalidOperationException("Invalid SWAPI response");
                all.AddRange(page.Results);
                url = page.Next;
            }

            return all;
        }

        // Generic printer for long-valued properties
        static void PrintMinMax(
            List<Planet> planets,
            Func<Planet, long?> selector,
            string propertyName)
        {
            var valid = new List<(string Name, long Value)>();
            foreach (var p in planets)
            {
                var v = selector(p);
                if (v.HasValue)
                    valid.Add((p.Name, v.Value));
            }

            if (valid.Count == 0)
            {
                Console.WriteLine($"No valid {propertyName} data.");
                return;
            }

            var max = valid[0];
            var min = valid[0];
            foreach (var item in valid)
            {
                if (item.Value > max.Value) max = item;
                if (item.Value < min.Value) min = item;
            }

            Console.WriteLine($"Max {propertyName} is {max.Value} (planet: {max.Name})");
            Console.WriteLine($"Min {propertyName} is {min.Value} (planet: {min.Name})");
        }

        // Generic printer for double-valued properties
        static void PrintMinMax(
            List<Planet> planets,
            Func<Planet, double?> selector,
            string propertyName)
        {
            var valid = new List<(string Name, double Value)>();
            foreach (var p in planets)
            {
                var v = selector(p);
                if (v.HasValue)
                    valid.Add((p.Name, v.Value));
            }

            if (valid.Count == 0)
            {
                Console.WriteLine($"No valid {propertyName} data.");
                return;
            }

            var max = valid[0];
            var min = valid[0];
            foreach (var item in valid)
            {
                if (item.Value > max.Value) max = item;
                if (item.Value < min.Value) min = item;
            }

            Console.WriteLine($"Max {propertyName} is {max.Value} (planet: {max.Name})");
            Console.WriteLine($"Min {propertyName} is {min.Value} (planet: {min.Name})");
        }

        static long? TryParseLong(string raw)
            => long.TryParse(raw, out var v) ? v : null;

        static double? TryParseDouble(string raw)
            => double.TryParse(raw, out var d) ? d : null;

        // SWAPI page structure
        class PlanetsPage
        {
            [JsonPropertyName("next")]
            public string? Next { get; set; }

            [JsonPropertyName("results")]
            public List<Planet> Results { get; set; } = new();
        }

        // Planet model matching only needed fields
        class Planet
        {
            [JsonPropertyName("name")]
            public string Name { get; set; } = "";

            [JsonPropertyName("population")]
            public string PopulationRaw { get; set; } = "";

            [JsonPropertyName("diameter")]
            public string DiameterRaw { get; set; } = "";

            [JsonPropertyName("surface_water")]
            public string SurfaceWaterRaw { get; set; } = "";
        }
    }
}