namespace SudokuPuzzleSolveAndStore.Models
{
    public sealed class UpdatePuzzleRequest
    {
        public int[][] UnsolvedPuzzle { get; set; } = [];
        public int[][] SolvedPuzzle { get; set; } = [];
    }
}
