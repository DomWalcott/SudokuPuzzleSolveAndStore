namespace SudokuPuzzleSolveAndStore.Models
{
    public sealed class PuzzleResponse
    {
        public int PuzzleId { get; set; }
        public int[][] UnsolvedPuzzle { get; set; } = [];
        public int[][] SolvedPuzzle { get; set; } = [];
    }
}
