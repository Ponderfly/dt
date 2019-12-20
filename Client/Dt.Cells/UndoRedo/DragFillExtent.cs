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
#endregion

namespace Dt.Cells.UndoRedo
{
    /// <summary>
    /// Represents a drag fill action extent to support dragging and filling a range on the sheet.
    /// </summary>
    public class DragFillExtent
    {
        private Dt.Cells.Data.AutoFillType _autoFillType;
        private Dt.Cells.Data.FillDirection _fillDirection = Dt.Cells.Data.FillDirection.Down;
        private CellRange _fillRange;
        private CellRange _startRange;

        /// <summary>
        /// Creates a new instance of the <see cref="T:Dt.Cells.UndoRedo.DragFillExtent" /> class.
        /// </summary>
        /// <param name="startRange">The drag fill start range.</param>
        /// <param name="fillRange">The drag fill end range.</param>
        /// <param name="autoFillType">The <see cref="T:GrapeCity.Windows.SpreadSheet.UI.AutoFillType" /> indicates the fill type.</param>
        /// <param name="fillDirection">The drag fill direction.</param>
        public DragFillExtent(CellRange startRange, CellRange fillRange, Dt.Cells.Data.AutoFillType autoFillType, Dt.Cells.Data.FillDirection fillDirection)
        {
            this._startRange = startRange;
            this._fillRange = fillRange;
            this._autoFillType = autoFillType;
            this._fillDirection = fillDirection;
        }

        /// <summary>
        /// Gets the type of the drag fill.
        /// </summary>
        public Dt.Cells.Data.AutoFillType AutoFillType
        {
            get { return  this._autoFillType; }
        }

        /// <summary>
        /// Gets the drag fill direction.
        /// </summary>
        public Dt.Cells.Data.FillDirection FillDirection
        {
            get { return  this._fillDirection; }
        }

        /// <summary>
        /// Gets the drag fill end range.
        /// </summary>
        public CellRange FillRange
        {
            get { return  this._fillRange; }
        }

        /// <summary>
        /// Gets the drag fill start range.
        /// </summary>
        public CellRange StartRange
        {
            get { return  this._startRange; }
        }
    }
}

