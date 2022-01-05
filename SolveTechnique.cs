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

        public int CountUnfilledCells(int[] numbers)
        {
            return numbers.Length - CountFilledCells(numbers);
        }

        public int CountUnfilledCells(int[][] numbers)
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

        public int[] FindCandidatesOfCell(int rowNum, int columnNum, int blockNum)
        {
            int[] row = sudoku.GetRow(rowNum);
            int[] column = sudoku.GetColumn(columnNum);
            int[][] block = sudoku.GetBlock(blockNum);

            return FindCandidatesOfCell(row, column, block);
        }

        public int[] FindCandidatesOfCell(int rowNum, int columnNum)
        {
            int blockNum = GetBlockNumber(rowNum, columnNum);

            return FindCandidatesOfCell(rowNum, columnNum, blockNum);
        }

        public int[] GetCandidateCell(int rowNum, int columnNum)
        {
            return CandidateGrid[rowNum][columnNum];
        }

        public int[][] GetCandidateGridRow(int rowNum)
        {
            return CandidateGrid[rowNum];
        }

        public int[][] GetCandidateGridColumn(int columnNum)
        {
            int[][] column = new int[Sudoku.ColumnLengthOfGrid][];

            for (int i = 0; i < column.GetLength(0); i++)
            {
                column[i] = CandidateGrid[i][columnNum];
            }

            return column;
        }

        public int[][][] GetCandidateGridBlock(int blockNum)
        {
            int[][][] block = new int[Sudoku.RowLengthOfBlock][][];
            for (int i = 0; i < block.GetLength(0); i++)
            {
                block[i] = new int[Sudoku.ColumnLengthOfBlock][];
            }

            var upperLeftCellPosition = Sudoku.GetCellPositionOnGrid(blockNum, 0, 0);

            for (int i = upperLeftCellPosition.rowNum; i < block.GetLength(0); i++)
            {
                for (int j = upperLeftCellPosition.columnNum; j < block.GetLength(1); j++)
                {
                    block[i][j] = CandidateGrid[i][j];
                }
            }

            return block;
        }

        public void SetCandidateGridCell(int rowNum, int columnNum, int[] candidates)
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

        public (int number, int rowNum, int columnNum)? FullHouse(int blockNum)
        {
            int[][] block = sudoku.GetBlock(blockNum);
            int filledCellsCount = CountFilledCells(block);
            if (filledCellsCount < 8 || filledCellsCount >= 9)
                return null;

            int unusedNumber = GetUnusedNumbers(block)[0];
            var cellPositionInBlock = FindCellPosition(block, unusedNumber);
            var cellPosition = Sudoku.GetCellPositionOnGrid(blockNum, cellPositionInBlock.rowNum, cellPositionInBlock.rowNum);

            return (unusedNumber, cellPosition.rowNum, cellPosition.columnNum);
        }

        public (int number, int rowNum, int columnNum)? LastDigit(int indexOfNums, bool isColumnMode)
        {
            int[] numbers = isColumnMode ? sudoku.GetColumn(indexOfNums) : sudoku.GetRow(indexOfNums);
            int filledCellsCount = CountFilledCells(numbers);
            if (filledCellsCount < 8 || filledCellsCount >= 9)
                return null;

            int unusedNumber = GetUnusedNumbers(numbers)[0];
            int index = FindCellPosition(numbers, unusedNumber);
            int rowNum, columnNum;
            if (isColumnMode)
            {
                rowNum = index;
                columnNum = indexOfNums;
            }
            else
            {
                rowNum = indexOfNums;
                columnNum = index;
            }

            return (unusedNumber, rowNum, columnNum);
        }
    }
}
