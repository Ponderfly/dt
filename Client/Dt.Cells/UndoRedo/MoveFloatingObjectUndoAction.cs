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
using Windows.Foundation;
#endregion

namespace Dt.Cells.UndoRedo
{
    /// <summary>
    /// 
    /// </summary>
    public class MoveFloatingObjectUndoAction : ActionBase, IUndo
    {
        private MoveFloatingObjectExtent _movingExtent;
        private Worksheet _worksheet;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Dt.Cells.UndoRedo.MoveFloatingObjectUndoAction" /> class.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="extent">The extent.</param>
        public MoveFloatingObjectUndoAction(Worksheet worksheet, MoveFloatingObjectExtent extent)
        {
            this._worksheet = worksheet;
            this._movingExtent = extent;
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
                    foreach (string str in this._movingExtent.Names)
                    {
                        FloatingObject obj2 = this._worksheet.FindChart(str);
                        if (obj2 == null)
                        {
                            obj2 = this._worksheet.FindPicture(str);
                        }
                        if (obj2 == null)
                        {
                            obj2 = this._worksheet.FindFloatingObject(str);
                        }
                        if (obj2 != null)
                        {
                            obj2.Location = new Windows.Foundation.Point(obj2.Location.X + this._movingExtent.OffsetX, obj2.Location.Y + this._movingExtent.OffsetY);
                        }
                    }
                }
                finally
                {
                    view.ResumeFloatingObjectsInvalidate();
                }
                view.InvalidateFloatingObjectLayout();
            }
        }

        /// <summary>
        /// Saves the state for undoing the command or operation.
        /// </summary>
        public void SaveState()
        {
        }

        /// <summary>
        /// Returns a <see cref="T:System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return ResourceStrings.moveFloatingObj;
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
                this.SaveState();
                foreach (string str in this._movingExtent.Names)
                {
                    FloatingObject obj2 = this._worksheet.FindChart(str);
                    if (obj2 == null)
                    {
                        obj2 = this._worksheet.FindPicture(str);
                    }
                    if (obj2 == null)
                    {
                        obj2 = this._worksheet.FindFloatingObject(str);
                    }
                    if (obj2 != null)
                    {
                        obj2.Location = new Windows.Foundation.Point(obj2.Location.X - this._movingExtent.OffsetX, obj2.Location.Y - this._movingExtent.OffsetY);
                    }
                }
            }
            finally
            {
                view.ResumeFloatingObjectsInvalidate();
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
