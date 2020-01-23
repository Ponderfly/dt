#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2014-07-03 创建
******************************************************************************/
#endregion

#region 引用命名
using Dt.Cells.Data;
using Dt.Cells.UI;
using System;
#endregion

namespace Dt.Cells.UndoRedo
{
    /// <summary>
    /// Represents a zoom undo action on the sheet.
    /// </summary>
    public class ZoomUndoAction : ActionBase, IUndo
    {
        private float prevZoomFactor;
        private Worksheet worksheet;
        private float zoomFactor;

        /// <summary>
        /// Creates a new instance of the <see cref="T:Dt.Cells.UndoRedo.ZoomUndoAction" /> class.
        /// </summary>
        /// <param name="sheet">The zoomed worksheet.</param>
        /// <param name="newZoomFactor">The new zoom factor on the worksheet.</param>
        public ZoomUndoAction(Worksheet sheet, float newZoomFactor)
        {
            this.worksheet = sheet;
            if (newZoomFactor < 0.1f)
            {
                newZoomFactor = 0.1f;
            }
            else if (newZoomFactor > 4f)
            {
                newZoomFactor = 4f;
            }
            this.zoomFactor = newZoomFactor;
            this.prevZoomFactor = -1f;
        }

        /// <summary>
        /// Defines the method that determines whether the action can be executed in its current state.
        /// </summary>
        /// <param name="parameter">Object on which the undo action occurred.</param>
        /// <returns>
        /// <c>true</c> if this action can be executed; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanExecute(object parameter)
        {
            SheetView view = parameter as SheetView;
            return (((view != null) && view.CanUserZoom) && (view.ZoomFactor != this.zoomFactor));
        }

        /// <summary>
        /// Executes the zoom action on the worksheet.
        /// </summary>
        /// <param name="parameter">Object on which the undo action occurred.</param>
        public override void Execute(object parameter)
        {
            this.SaveState();
            SheetView sheetView = parameter as SheetView;
            if (((sheetView == null) || !sheetView.CanUserZoom) || (sheetView.ZoomFactor == this.zoomFactor))
            {
                throw new ActionFailedException(this);
            }
            if (sheetView.IsEditing)
            {
                sheetView.StopCellEditing(false);
            }
            base.SuspendInvalidate(parameter);
            try
            {
                this.worksheet.ZoomFactor = this.zoomFactor;
            }
            finally
            {
                base.ResumeInvalidate(parameter);
            }
            this.RefreshUI(sheetView);
        }

        private void RefreshUI(object sheetView)
        {
            SpreadView view = sheetView as SpreadView;
            if (view != null)
            {
                view.InvalidateRange(-1, -1, -1, -1, SheetArea.Cells);
                view.InvalidateRange(-1, -1, -1, -1, SheetArea.ColumnHeader);
                view.InvalidateRange(-1, -1, -1, -1, SheetArea.CornerHeader | SheetArea.RowHeader);
                view.InvalidateFloatingObjects();
                view.InvalidateMeasure();
            }
        }

        /// <summary>
        /// Saves the state for undoing the action before executing the action.
        /// </summary>
        public void SaveState()
        {
            if (this.worksheet != null)
            {
                this.prevZoomFactor = this.worksheet.ZoomFactor;
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return ResourceStrings.undoActionZoom;
        }

        /// <summary>
        /// Undoes the zoom action on the worksheet.
        /// </summary>
        /// <param name="parameter">Object on which the undo action occurred.</param>
        /// <returns>
        /// <c>true</c> if undoing the action succeeds; otherwise, <c>false</c>.
        /// </returns>
        public bool Undo(object parameter)
        {
            if (this.worksheet == null)
            {
                return false;
            }
            base.SuspendInvalidate(parameter);
            try
            {
                this.worksheet.ZoomFactor = this.prevZoomFactor;
            }
            finally
            {
                base.ResumeInvalidate(parameter);
            }
            SheetView sheetView = parameter as SheetView;
            if (sheetView != null)
            {
                this.RefreshUI(sheetView);
            }
            return true;
        }

        /// <summary>
        /// Gets a value that indicates whether the action can be undone.
        /// </summary>
        public bool CanUndo
        {
            get { return  ((this.worksheet != null) && (this.prevZoomFactor > 0f)); }
        }
    }
}
