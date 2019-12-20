#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2014-07-03 创建
******************************************************************************/
#endregion

#region 引用命名
using System;
using System.Collections;
using System.Collections.Generic;
#endregion

namespace Dt.Cells.UI
{
    /// <summary>
    /// 
    /// </summary>
    internal class CellLayoutModel : IEnumerable<CellLayout>, IEnumerable
    {
        private List<CellLayout> innerList = new List<CellLayout>();
        private Dictionary<int, List<CellLayout>> rowMappingCache = new Dictionary<int, List<CellLayout>>();

        public void Add(CellLayout item)
        {
            this.innerList.Add(item);
            this.AddToRowMapping(item);
        }

        private void AddToRowMapping(CellLayout item)
        {
            for (int i = 0; i < item.RowCount; i++)
            {
                int num2 = item.Row + i;
                List<CellLayout> list = null;
                if (!this.rowMappingCache.TryGetValue(num2, out list))
                {
                    list = new List<CellLayout>();
                    this.rowMappingCache[num2] = list;
                }
                if (list != null)
                {
                    list.Add(item);
                }
            }
        }

        public CellLayout FindCell(int row, int column)
        {
            List<CellLayout> list = null;
            if (this.rowMappingCache.TryGetValue(row, out list) && (list != null))
            {
                int num = list.Count;
                for (int i = 0; i < num; i++)
                {
                    CellLayout layout = list[i];
                    if (layout.ContainsCell(row, column))
                    {
                        return layout;
                    }
                }
            }
            return null;
        }

        public CellLayout FindPoint(double x, double y)
        {
            int num = this.innerList.Count;
            for (int i = 0; i < num; i++)
            {
                CellLayout layout = this.innerList[i];
                if (layout.ContainsPoint(x, y))
                {
                    return layout;
                }
            }
            return null;
        }

        public IEnumerator<CellLayout> GetEnumerator()
        {
            return (IEnumerator<CellLayout>) this.innerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.innerList.GetEnumerator();
        }
    }
}

