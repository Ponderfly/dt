#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2014-07-03 创建
******************************************************************************/
#endregion

#region 引用命名
using Dt.Cells.Data;
using System;
using System.Runtime.CompilerServices;
#endregion

namespace Dt.Cells.UI
{
    /// <summary>
    /// Represents the event data for the CellClick events for the GcSpread component; occurs when the user clicks the mouse button with the pointer on a cell. 
    /// </summary>
    public class CellClickEventArgs : EventArgs
    {
        internal CellClickEventArgs(Dt.Cells.Data.SheetArea sheetArea, int row, int column, MouseButtonType button)
        {
            this.SheetArea = sheetArea;
            this.Row = row;
            this.Column = column;
            this.ButtonType = button;
        }

        /// <summary>
        /// Gets the mouse button that is clicked.
        /// </summary>
        /// <value>The mouse button that is clicked.</value>
        public MouseButtonType ButtonType { get; private set; }

        /// <summary>
        /// Gets the column index of the clicked cell.
        /// </summary>
        /// <value>The column index of the clicked cell.</value>
        public int Column { get; private set; }

        /// <summary>
        /// Gets the row index of the clicked cell.
        /// </summary>
        /// <value>The row index of the clicked cell.</value>
        public int Row { get; private set; }

        /// <summary>
        /// Gets the area the clicked cell is in.
        /// </summary>
        /// <value>The area the clicked cell is in.</value>
        public Dt.Cells.Data.SheetArea SheetArea { get; private set; }
    }
}

