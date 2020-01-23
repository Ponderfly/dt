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
using System.Collections.Generic;
using System.Collections.ObjectModel;
#endregion

namespace Dt.Cells.UndoRedo
{
    /// <summary>
    /// 
    /// </summary>
    public class AddFloatingObjectUndoAction : ActionBase, IUndo
    {
        private FloatingObject[] _floatingObjects;
        private SpreadChart[] _savedCharts;
        private FloatingObject[] _savedObjects;
        private Picture[] _savedPictures;
        private Worksheet _worksheet;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Dt.Cells.UndoRedo.MoveFloatingObjectUndoAction" /> class.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="floatingObjects">The floating objects.</param>
        public AddFloatingObjectUndoAction(Worksheet worksheet, FloatingObject[] floatingObjects)
        {
            this._worksheet = worksheet;
            this._floatingObjects = floatingObjects;
        }

        /// <summary>
        /// Defines the method that determines whether the action can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the action. If the action does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// <c>true</c> if this action can be executed; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Defines the method to be called when the action is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the action. If the action does not require data to be passed, this object can be set to null.</param>
        public override void Execute(object parameter)
        {
            if (this.CanExecute(parameter))
            {
                SheetView view = parameter as SheetView;
                try
                {
                    view.SuspendFloatingObjectsInvalidate();
                    this.SaveState();
                    if ((this._savedCharts != null) && (this._savedCharts.Length > 0))
                    {
                        this._worksheet.Charts.AddRange(this._savedCharts);
                    }
                    if ((this._savedPictures != null) && (-this._savedPictures.Length > 0))
                    {
                        this._worksheet.Pictures.AddRange(this._savedPictures);
                    }
                    if ((this._savedObjects != null) && (this._savedObjects.Length > 0))
                    {
                        this._worksheet.FloatingObjects.AddRange(this._savedObjects);
                    }
                }
                finally
                {
                    view.ResumeFloatingObjectsInvalidate();
                    ReadOnlyCollection<CellRange> selections = this._worksheet.Selections;
                    if (selections.Count != 0)
                    {
                        foreach (CellRange range in selections)
                        {
                            view.UpdateHeaderCellsState(range.Row, range.RowCount, range.Column, range.ColumnCount);
                        }
                    }
                }
                view.InvalidateFloatingObjectLayout();
            }
        }

        /// <summary>
        /// Saves the state for undoing the command or operation.
        /// </summary>
        public void SaveState()
        {
            List<SpreadChart> list = new List<SpreadChart>();
            List<FloatingObject> list2 = new List<FloatingObject>();
            List<Picture> list3 = new List<Picture>();
            foreach (FloatingObject obj2 in this._floatingObjects)
            {
                if (obj2 is SpreadChart)
                {
                    list.Add(obj2 as SpreadChart);
                }
                else if (obj2 is Picture)
                {
                    list3.Add(obj2 as Picture);
                }
                else
                {
                    list2.Add(obj2);
                }
            }
            this._savedCharts = list.ToArray();
            this._savedObjects = list2.ToArray();
            this._savedPictures = list3.ToArray();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return ResourceStrings.addFloatingObj;
        }

        /// <summary>
        /// Undoes the command or operation.
        /// </summary>
        /// <param name="parameter">The parameter to undo the command or operation.</param>
        /// <returns>
        /// <c>true</c> if an undo operation on the command or operation succeeds; otherwise, <c>false</c>.
        /// </returns>
        public bool Undo(object parameter)
        {
            SheetView view = parameter as SheetView;
            try
            {
                view.SuspendFloatingObjectsInvalidate();
                if ((this._savedCharts != null) && (this._savedCharts.Length > 0))
                {
                    foreach (SpreadChart chart in this._savedCharts)
                    {
                        this._worksheet.Charts.Remove(chart);
                    }
                }
                if ((this._savedPictures != null) && (this._savedPictures.Length > 0))
                {
                    foreach (Picture picture in this._savedPictures)
                    {
                        this._worksheet.Pictures.Remove(picture);
                    }
                }
                if ((this._savedObjects != null) && (this._savedObjects.Length > 0))
                {
                    foreach (FloatingObject obj2 in this._savedObjects)
                    {
                        this._worksheet.FloatingObjects.Remove(obj2);
                    }
                }
            }
            finally
            {
                view.ResumeFloatingObjectsInvalidate();
                ReadOnlyCollection<CellRange> selections = this._worksheet.Selections;
                if (selections.Count != 0)
                {
                    foreach (CellRange range in selections)
                    {
                        view.UpdateHeaderCellsState(range.Row, range.RowCount, range.Column, range.ColumnCount);
                    }
                }
            }
            view.InvalidateFloatingObjectLayout();
            return true;
        }

        /// <summary>
        /// Gets a value that indicates whether the command or operation can be undone.
        /// </summary>
        public bool CanUndo
        {
            get { return  true; }
        }
    }
}
