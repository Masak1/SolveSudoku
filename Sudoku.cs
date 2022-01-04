namespace SolveSudoku
{
    class Sudoku
    {
        private static readonly int RowLengthOfGrid = 9;
        private static readonly int ColumnLengthOfGrid = 9;
        private static readonly int RowLengthOfBlock = 3;
        private static readonly int ColumnLengthOfBlock = 3;

        private int[][] Grid { get; set; }

        public Sudoku()
        {
            Grid = new int[RowLengthOfGrid][];
            for (int i = 0; i < RowLengthOfGrid; i++)
            {
                Grid[i] = new int[ColumnLengthOfGrid];
            }
        }

        /// <summary>
        /// Gridの指定したセルを取得する。セルは何かしらの数字が入るマス1つのこと。
        /// 行や列番号はデフォルトで0~8で指定。
        /// Gridの一番上・左が0で、一番下・右が8。
        /// </summary>
        /// <param name="rowNum">行番号</param>
        /// <param name="columnNum">列番号</param>
        /// <param name="isMinIndexOne">行と列を数えるときのインデックスを1から9で数えるか</param>
        /// <returns>セルの数字</returns>
        public int GetCell(int rowNum, int columnNum, bool isMinIndexOne = false)
        {
            if (isMinIndexOne)
                return Grid[rowNum - 1][columnNum - 1];

            return Grid[rowNum][columnNum];
        }

        /// <summary>
        /// Gridの行を取得する。行は左から右の横方向。
        /// 行番号はデフォルトで0~8で指定。
        /// Gridの一番左が0で、一番右が8。
        /// </summary>
        /// <param name="rowNum">行番号</param>
        /// <param name="isMinIndexOne">行を数えるときのインデックスを1から9で数えるか</param>
        /// <returns>行の配列で、列のインデックスの小さい順</returns>
        public int[] GetRow(int rowNum, bool isMinIndexOne = false)
        {
            if (isMinIndexOne)
                return Grid[rowNum - 1];

            return Grid[rowNum];
        }

        /// <summary>
        /// Gridの列を取得する。列は上から下の縦方向。
        /// 列番号はデフォルトで0~8で指定。
        /// Gridの一番上が0で、一番下が8。
        /// </summary>
        /// <param name="columnNum">列番号</param>
        /// <param name="isMinIndexOne">列を数えるときのインデックスを1から9で数えるか</param>
        /// <returns>列の配列で、行のインデックスの小さい順</returns>
        public int[] GetColumn(int columnNum, bool isMinIndexOne = false)
        {
            int[] column = new int[ColumnLengthOfGrid];

            for (int i = 0; i < ColumnLengthOfGrid; i++)
            {
                if (isMinIndexOne)
                    column[i] = Grid[i][columnNum - 1];
                else
                    column[i] = Grid[i][columnNum];
            }

            return column;
        }

        /// <summary>
        /// Gridの指定ブロックを取得する。
        /// ブロックとは、一般的な9*9の問題で1~9の数字が1つずつおさまる3*3の区画のこと。
        /// ボックス(box)やregion、subgridなどとも呼ばれる。
        /// 下の図は3*3の四角になっているが、このひとマスをブロックとすると、9*9の問題では以下の数字を引数で指定する。<br/>
        /// -------------<br/>
        /// | 1 | 2 | 3 |<br/>
        /// |---|---|---|<br/>
        /// | 4 | 5 | 6 |<br/>
        /// |---|---|---|<br/>
        /// | 7 | 8 | 9 |<br/>
        /// -------------
        /// </summary>
        /// <param name="blockNum">ブロックの位置。場所の数字定義はsummary参照</param>
        /// <returns>ブロックサイズ*ブロックサイズのint型2次元配列</returns>
        public int[][] GetBlock(int blockNum)
        {
            int[][] block = new int[RowLengthOfBlock][];
            for (int i = 0; i < RowLengthOfBlock; i++)
            {
                block[i] = new int[ColumnLengthOfBlock];
            }

            for (int i = 0; i < RowLengthOfBlock; i++)
            {
                for (int j = 0; j < ColumnLengthOfBlock; j++)
                {
                    block[i][j] = Grid[i][j];
                }
            }

            return block;
        }

        internal void SetCell(int row, int column, int number)
        {
            Grid[row][column] = number;
        }

        /*public bool IsValidRow(int rowNum)
        {
            int[] row = Grid[rowNum,];
        }*/
    }
}