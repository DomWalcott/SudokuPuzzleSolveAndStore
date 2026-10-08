namespace SudokuPuzzleSolveAndStore.Solver
{
    public class SudokuSolver
    {
        public int PuzzleId { get; set; }
        public int[,] UnsolvedPuzzle { get; set; }
        public int[,] SolvedPuzzle { get; set; }

        static public bool SolvePuzzle(int[,] board)
        {
            (int row, int col) = GetEmpty(board);
            if (row < 0 && col < 0) { return true; }

            for (int i = 1; i <= 9; i++)
            {
                if (IsCorrect(board, row, col, i))
                {
                    board[row, col] = i;
                    if (SolvePuzzle(board)) { return true; }
                    else { board[row, col] = 0; }
                }
            }
            return false;
        }
        static (int, int) GetEmpty(int[,] board)
        {
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (board[r, c] == 0) { return (r, c); }
                }
            }
            return (-1, -1);
        }
        static bool IsCorrect(int[,] board, int row, int col, int num)
        {
            for (int i = 0; i < 9; i++)
            {
                if (board[row, i] == num) { return false; }
                if (board[i, col] == num) { return false; }
            }
            int rs = row - row % 3;
            int cs = col - col % 3;
            for (int r = rs; r < rs + 3; r++)
            {
                for (int c = cs; c < cs + 3; c++)
                {
                    if (board[r, c] == num) { return false; }
                }
            }
            return true;
        }
        static void Print(int[,] board)
        {
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    Console.Write($"{board[r, c]}");
                }
                Console.WriteLine();
            }
        }
    }

    }

