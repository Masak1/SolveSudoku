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

        public static int GetBlockNumber(int rowNum, int columnNum)
        {
            return 3 * rowNum / 3 + columnNum / 3 + 1;
        }

        public int CountFilledCells(int[] numbers)
        {
            int filledCellsCount = 0;

            foreach (var cell in numbers)
            {
                if (cell != 0)
                    filledCellsCount++;
            }

            return filledCellsCount;
        }

        public int CountUnfilledCells(int[] numbers)
        {
            return numbers.Length - CountFilledCells(numbers);
        }

        public int CountFilledCellsInRow(int rowNum)
        {
            int[] row = sudoku.GetRow(rowNum);

            return CountFilledCells(row);
        }

        public int CountUnfilledCellsInRow(int rowNum)
        {
            return Sudoku.RowLengthOfGrid - CountFilledCellsInRow(rowNum);
        }

        public int CountFilledCellsInColumn(int columnNum)
        {
            int[] column = sudoku.GetColumn(columnNum);

            return CountFilledCells(column);
        }

        public int CountUnfilledCellsInColumn(int columnNum)
        {
            return Sudoku.ColumnLengthOfGrid - CountFilledCellsInColumn(columnNum);
        }

        public int CountFilledCells(int[][] numbers)
        {
            int filledCellsCount = 0;
            foreach (int[] row in numbers)
            {
                foreach (var cell in row)
                {
                    if (cell != 0)
                        filledCellsCount++;
                }
            }

            return filledCellsCount;
        }

        public int CountUnfilledCells(int[][] numbers)
        {
            return numbers.Length - CountFilledCells(numbers);
        }

        public int CountFilledCellsInBlock(int blockNum)
        {
            int[][] block = sudoku.GetBlock(blockNum);

            return CountFilledCells(block);
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
            int blockNum = GetBlockNumber(rowNum, columnNum);

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

        public int FindCellPosition(int[] numbers, int findingNumber)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] == findingNumber)
                    return i;
            }

            return -1;
        }

        public (int rowNum, int columnNum) FindCellPosition(int[][] numbers, int findingNumber)
        {
            for (int i = 0; i < numbers.GetLength(0); i++)
            {
                for (int j = 0; j < numbers.GetLength(1); j++)
                {
                    if (numbers[i][j] == findingNumber)
                        return (i, j);
                }
            }

            return (-1, -1);
        }

        public (int rowNum, int columnNum) GetCellPositionOnGrid(int blockNum, int rowNumInBlock, int columnNumInBlock)
        {
            int rowNum = (blockNum - 1) / 3 + rowNumInBlock;
            int columnNum = (blockNum - 1) % 3 * 3 + columnNumInBlock;

            return (rowNum, columnNum);
        }

        public (int number, int rowNum, int columnNum)? FullHouse(int blockNum)
        {
            int[][] block = sudoku.GetBlock(blockNum);
            int filledCellsCount = CountFilledCells(block);
            if (filledCellsCount < 8 || filledCellsCount >= 9)
                return null;

            int index = 0;
            foreach (var row in block)
            {
                foreach (var number in row)
                {
                    if (number != 0)
                        index++;
                    else
                        break;
                }
            }

            int unusedNumber = GetUnusedNumbers(block)[0];
            var cellPositionInBlock = FindCellPosition(block, unusedNumber);
            var cellPosition = GetCellPositionOnGrid(blockNum, cellPositionInBlock.rowNum, cellPositionInBlock.rowNum);

            return (unusedNumber, cellPosition.rowNum, cellPosition.columnNum);
        }
    }
}
