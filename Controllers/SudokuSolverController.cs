using Microsoft.AspNetCore.Mvc;
using SudokuPuzzleSolveAndStore.Solver;
using SudokuPuzzleSolveAndStore.Models;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SudokuPuzzleSolveAndStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SudokuSolverController : ControllerBase
    {

        public static List<SudokuSolver> puzzles = new List<SudokuSolver>
        { new SudokuSolver
        { PuzzleId = 1,
            UnsolvedPuzzle = new int[,]
            {
                {0,5,0,4,0,1,0,0,6},
                {1,0,0,9,5,0,8,0,0},
                {9,0,4,0,6,0,0,0,1},
                { 6,2,0,0,0,5,3,0,4},
                { 0,9,0,0,7,0,2,0,5},
                { 5,0,7,0,0,0,0,8,9},
                { 8,0,0,5,1,9,0,0,2},
                { 2,3,0,0,0,6,5,0,8},
                { 4,1,0,2,0,8,6,0,0}
            },
            SolvedPuzzle = new int[,]
            {
                 {7,5,3,4,8,1,9,2,6},
                 {1,6,2,9,5,7,8,4,3},
                 {9,8,4,3,6,2,7,5,1},
                 {6,2,1,8,9,5,3,7,4},
                 {3,9,8,1,7,4,2,6,5},
                 {5,4,7,6,2,3,1,8,9},
                 {8,7,6,5,1,9,4,3,2},
                 {2,3,9,7,4,6,5,1,8},
                 {4,1,5,2,3,8,6,9,7}
            }}
            
        };


        // GET: api/<SudokuSolverController>
        [HttpGet]
        public ActionResult<IEnumerable<PuzzleResponse>> Get()
        {
            var responses = puzzles.Select(ToResponse).ToList();
            return Ok(responses);
        }

        // GET api/<SudokuSolverController>/5
        [HttpGet("{id}")]
        public ActionResult<PuzzleResponse> Get(int id)
        {
            var puzzle = puzzles.FirstOrDefault(p => p.PuzzleId == id);
            if (puzzle == null)
            {
                return NotFound();
            }

            return Ok(ToResponse(puzzle));
        }

        // POST api/<SudokuSolverController>
        [HttpPost]
        public ActionResult<PuzzleResponse> Post([FromBody] SolvePuzzleRequest request)
        {
            if (!IsValidGrid(request?.UnsolvedPuzzle))
            {
                return BadRequest("the puzzle must be a 9x9 grid");
            }
            var unsolved = ToRectangular(request.UnsolvedPuzzle);
            var solved = (int[,])unsolved.Clone();

            if(!SudokuSolver.SolvePuzzle(solved))
            {
                return UnprocessableEntity("The puzzle is unsolveable");
            }

            var puzzle = new SudokuSolver
            {
                PuzzleId = puzzles.Count == 0 ? 1 : puzzles.Max(p => p.PuzzleId) + 1,
                UnsolvedPuzzle = unsolved,
                SolvedPuzzle = solved
            };

            puzzles.Add(puzzle);

            var response = ToResponse(puzzle);
            return CreatedAtAction(nameof(Get), new { id = puzzle.PuzzleId }, response);

        }

        // PUT api/<SudokuSolverController>/5
        [HttpPut("{id}")]
        public ActionResult<PuzzleResponse> SolveExisting(int id, [FromBody]SolvePuzzleRequest request)
        {
            var puzzle = puzzles.FirstOrDefault(p => p.PuzzleId == id);
            if (puzzle == null)
            {
                return NotFound();
            }
           
            if(!IsValidGrid(request?.UnsolvedPuzzle))
            {
                return BadRequest("the grid must be 9x9");
            }

            var unsolved = ToRectangular(request.UnsolvedPuzzle);
            var solved = (int[,]) unsolved.Clone();

            if(!SudokuSolver.SolvePuzzle(solved))
            {
                return UnprocessableEntity("This puzzle is unsolveable");
            }
            puzzle.UnsolvedPuzzle = unsolved;
            puzzle.SolvedPuzzle = solved;


            return Ok(ToResponse(puzzle));
        }

        // DELETE api/<SudokuSolverController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var puzzle = puzzles.FirstOrDefault(p => p.PuzzleId == id);
            if (puzzle == null)
            {
                return NotFound("The puzzle was not found");
            }
            puzzles.Remove(puzzle);
            return NoContent();
        }



        /*
        [HttpPost("Solve")]
        public ActionResult<SudokuSolver> Solve([FromBody] SolvePuzzleRequest request)
        {
            if (!IsValidGrid(request?.UnsolvedPuzzle))
            {
                return BadRequest("The puzzle must be in 9 x 9");
            }
            var unsolved = ToRectangular(request.UnsolvedPuzzle);
            var solved = (int[,])unsolved.Clone();

            if(!SudokuSolver.SolvePuzzle(solved))
            {
                return UnprocessableEntity("This puzzle is unsolveable");
            }
            var puzzle = new SudokuSolver
            {
                PuzzleId = puzzles.Count == 0 ? 1 : puzzles.Max(p => p.PuzzleId) + 1,
                UnsolvedPuzzle = unsolved,
                SolvedPuzzle = solved
            };
            puzzles.Add(puzzle);

            var response = new PuzzleResponse
            {
                PuzzleId = puzzle.PuzzleId,
                UnsolvedPuzzle = ToJagged(puzzle.UnsolvedPuzzle),
                SolvedPuzzle = ToJagged(puzzle.SolvedPuzzle)
            };

            return CreatedAtAction(nameof(Get), new { id = puzzle.PuzzleId }, response);
        }
        */


        private static int[,] ToRectangular(int[][] rows)
        {
            var board = new int[9, 9];

            for (int r = 0; r<9; r++)
            {
                for(int c = 0; c<9; c++)
                {
                    board[r, c] = rows[r][c];
                }
            }
            return board;
        }
        private static int[][] ToJagged(int[,] board)
        {
            var rows = new int[9][];
            for (int r = 0; r<9; r++)
            {
                rows[r] = new int[9];
                for(int c = 0; c<9; c++)
                {
                    rows[r][c] = board[r, c];
                }
            }
            return rows;
        }
        private static bool IsValidGrid(int[][]? rows) =>rows is { Length: 9 } && rows.All(row => row is { Length: 9 });

        private static PuzzleResponse ToResponse(SudokuSolver puzzle)
        {
            return new PuzzleResponse
            {
                PuzzleId = puzzle.PuzzleId,
                UnsolvedPuzzle = ToJagged(puzzle.UnsolvedPuzzle),
                SolvedPuzzle = ToJagged(puzzle.SolvedPuzzle)
            };
        }
    }
}
