Sudoku Puzzle Solver

This is a learning project. I built it to practise how a browser front end sends data to a C# back end through an API, and how the back end sends a result back.

The Sudoku-solving code is in C#. The browser page uses HTML, CSS, and JavaScript to collect the numbers, send them to the API, and show the answer.

## Run it on your computer

You need the .NET 8 SDK, or Visual Studio with the **ASP.NET and web development** workload.

In Visual Studio, open `SudokuPuzzleSolveAndStore.sln` and run the project with the HTTPS profile. The local address is usually:

[https://localhost:7224](https://localhost:7224)

Or, from the folder containing `SudokuPuzzleSolveAndStore.csproj`, run:

```sh
dotnet run --launch-profile https
```

Swagger, a page for trying the API, is available in Development mode at [https://localhost:7224/swagger](https://localhost:7224/swagger).

## How the app works

1. Enter a Sudoku puzzle in the browser. Use `0` for empty squares.
2. The page sends the 9 × 9 puzzle to `POST /api/sudokusolver` as JSON.
3. The C# API calls the Sudoku solver and returns the puzzle and its solution.
4. The page displays the returned solution.

Other API routes:

| Method | Route | What it does |
|---|---|---|
| GET | `/api/sudokusolver` | List saved puzzles |
| GET | `/api/sudokusolver/{id}` | Get one puzzle |
| PUT | `/api/sudokusolver/{id}` | Solve and replace a puzzle |
| DELETE | `/api/sudokusolver/{id}` | Delete a puzzle |

A submitted grid must have nine rows with nine numbers in each row. If the clues cannot be solved, the API returns `422`. If the grid is not 9 × 9, it returns `400`.

## Saving puzzles

Puzzles are currently kept in memory while the app is running. They are cleared when the app restarts. The project includes Entity Framework packages, but it does not use a database yet.

## Hosting

GitHub stores this project’s code, but GitHub Pages cannot run the C# API. The full app needs a host that can run ASP.NET Core. No public demo is set up yet, so run it locally using the instructions above.
