namespace SolveSudoku
{
    class SolveTechnique
    {
        private Sudoku sudoku;

        internal int[] CandidateNumbers { get; private set; }
        internal int[][][] CandidateGrid { get; private set; }

        public SolveTechnique()
        {
            sudoku = new Sudoku();

            CandidateNumbers = new int[Sudoku.RowLengthOfBlock * Sudoku.ColumnLengthOfBlock];
            for (int i = 0; i < CandidateNumbers.Length; i++)
            {
                CandidateNumbers[i] = i + 1;
            }

            CandidateGrid = new int[Sudoku.RowLengthOfGrid][][];
            for (int i = 0; i < Sudoku.RowLengthOfGrid; i++)
            {
                CandidateGrid[i] = new int[Sudoku.ColumnLengthOfGrid][];

                for (int j = 0; j < Sudoku.ColumnLengthOfGrid; j++)
                {
                    CandidateGrid[i][j] = new int[0];
                }
            }
        }

        public static int GetBlockNumberWithRowColumn(int rowNum, int columnNum)
        {
            return 3 * rowNum / 3 + columnNum / 3 + 1;
        }

        public int CountFilledCellsInRow(int[] row)
        {
            int filledCellsCount = 0;

            foreach (var cell in row)
            {
                if (cell != 0)
                    filledCellsCount++;
            }

            return filledCellsCount;
        }

        public int CountFilledCellsInRow(int rowNum)
        {
            int[] row = sudoku.GetRow(rowNum);

            return CountFilledCellsInRow(row);
        }

        public int CountUnfilledCellsInRow(int[] row)
        {
            return Sudoku.RowLengthOfGrid - CountFilledCellsInRow(row);
        }


        public int CountUnfilledCellsInRow(int rowNum)
        {
            return Sudoku.RowLengthOfGrid - CountFilledCellsInRow(rowNum);
        }

        public int CountFilledCellsInColumn(int[] column)
        {
            int filledCellsCount = 0;

            foreach (var cell in column)
            {
                if (cell != 0)
                    filledCellsCount++;
            }

            return filledCellsCount;
        }

        public int CountFilledCellsInColumn(int columnNum)
        {
            int[] column = sudoku.GetColumn(columnNum);

            return CountFilledCellsInColumn(column);
        }

        public int CountUnfilledCellsInColumn(int[] column)
        {
            return Sudoku.ColumnLengthOfGrid - CountFilledCellsInColumn(column);
        }

        public int CountUnfilledCellsInColumn(int columnNum)
        {
            return Sudoku.ColumnLengthOfGrid - CountFilledCellsInColumn(columnNum);
        }

        public int CountFilledCellsInBlock(int[][] block)
        {
            int filledCellsCount = 0;
            foreach (int[] row in block)
            {
                foreach (var cell in row)
                {
                    if (cell != 0)
                        filledCellsCount++;
                }
            }

            return filledCellsCount;
        }

        public int CountFilledCellsInBlock(int blockNum)
        {
            int[][] block = sudoku.GetBlock(blockNum);

            return CountFilledCellsInBlock(block);
        }

        public int CountUnfilledCellsInBlock(int[][] block)
        {
            return Sudoku.RowLengthOfBlock * Sudoku.ColumnLengthOfBlock - CountFilledCellsInBlock(block);
        }

        public int CountUnfilledCellsInBlock(int blockNum)
        {
            return Sudoku.RowLengthOfBlock * Sudoku.ColumnLengthOfBlock - CountFilledCellsInBlock(blockNum);
        }

        public int[] GetUsedNumbers(int[] numbers)
        {
            bool[] isUsedNumbers = new bool[numbers.Length];

            int notZeroCount = 0;
            foreach (var number in numbers)
            {
                if (number != 0)
                    notZeroCount++;
            }

            int[] usedNumbers = new int[notZeroCount];
            int index = 0;
            foreach (var number in numbers)
            {
                if (number != 0)
                {
                    usedNumbers[index] = number;
                    index++;
                }
            }

            return usedNumbers;
        }

        public int[] GetUsedNumbers(int[][] numbers)
        {
            bool[] isUsedNumbers = new bool[numbers.Length];

            int notZeroCount = 0;
            foreach (var row in numbers)
            {
                foreach (var number in row)
                {
                    if (number != 0)
                        notZeroCount++;
                }
            }

            int[] usedNumbers = new int[notZeroCount];
            int index = 0;
            foreach (var row in numbers)
            {
                foreach (var number in row)
                {
                    if (number != 0)
                    {
                        usedNumbers[index] = number;
                        index++;
                    }
                }
            }

            return usedNumbers;
        }

        public int[] GetUnusedNumbers(int[] numbers)
        {
            return CandidateNumbers.Except(GetUsedNumbers(numbers)).ToArray();
        }

        public int[] GetUnusedNumbers(int[][] numbers)
        {
            return CandidateNumbers.Except(GetUsedNumbers(numbers)).ToArray();
        }

        public int[] FindCandidatesOfCell(int[] row, int[] column, int[][] block)
        {
            int[] unusedOfRow = GetUnusedNumbers(row);
            int[] unusedOfColumn = GetUnusedNumbers(column);
            int[] unusedOfBlock = GetUnusedNumbers(block);

            return unusedOfRow.Union(unusedOfColumn).Union(unusedOfBlock).ToArray();
        }

        public int[] FindCandidatesOfCell(int rowNum, int columnNum)
        {
            int blockNum = GetBlockNumberWithRowColumn(rowNum, columnNum);

            int[] row = sudoku.GetRow(rowNum);
            int[] column = sudoku.GetColumn(columnNum);
            int[][] block = sudoku.GetBlock(blockNum);

            return FindCandidatesOfCell(row, column, block);
        }

        public int[] FindCandidatesOfCell(int rowNum, int columnNum, int blockNum)
        {
            int[] row = sudoku.GetRow(rowNum);
            int[] column = sudoku.GetColumn(columnNum);
            int[][] block = sudoku.GetBlock(blockNum);

            return FindCandidatesOfCell(row, column, block);
        }

        public void UpdateCellOfCandidates(int rowNum, int columnNum, int[] candidates)
        {
            CandidateGrid[rowNum][columnNum] = candidates;
        }
    }
}
