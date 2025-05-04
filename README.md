Star Wars Planets Stats

A .NET console application that fetches live planet data from the Star Wars API (SWAPI) and reports statistics—minimum and maximum—on population, diameter, or surface water of the planets. No mock data; every run uses real SWAPI data.

This implementation is provided “as‐is” and has not been tested. Use at your own discretion and consider writing tests before running in production.

Features

 Live Data
  Retrieves all pages of planets from the SWAPI endpoint:
  `https://swapi.dev/api/planets`
 Rich Console Output
  Displays each planet’s

   Name
   Population
   Diameter
   Surface water
 Interactive Statistics
  Prompts you to choose one of:

   `population`
   `diameter`
   `surface water`
 Min/Max Calculation
  Finds the planet with the highest and lowest value for your choice, ignoring `"unknown"` entries.
 Pure Live API
  No fallbacks or mock data—guaranteed up-to-date with SWAPI.



Getting Started

 Prerequisites

 .NET 9 SDK
 Internet access to reach `https://swapi.dev/api/planets`

Usage

1. On launch, the app fetches and prints a table of all SWAPI planets.

2. You’ll see a prompt:

   ```
   The statistics of which property would you like to see?
   population
   diameter
   surface water
   > 
   ```

3. Type one of the three choices (case-insensitive).

    Valid input e.g. `population`
     → Displays:

     ```
     Max population is 2000000000 (planet: Alderaan)
     Min population is 1000        (planet: Yavin IV)
     ```
    Invalid input
     → Prints `Invalid choice` and exits.

4. Press any key to close the console.


Security & Reliability

 HTTP timeouts and error handling are minimal—ensure your network permits calls to `swapi.dev`.
 SWAPI can be occasionally slow; expect a brief delay while fetching all pages.
 Untested: This code has not been validated end-to-end. Please write and run your own tests before relying on it.


Acknowledgement: This project was completed as part of the Udemy course ‘Complete C# Masterclass’ by Krystyna Ślusarczyk (link is provided below). Having significant prior experience with C#, my goal was to refresh my skills and familiarize myself with the latest updates, features, and best practices introduced in recent versions.

https://www.udemy.com/course/ultimate-csharp-masterclass/
