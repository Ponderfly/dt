#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2014-07-03 创建
******************************************************************************/
#endregion

#region 引用命名
using System;
using System.Runtime.CompilerServices;
#endregion

namespace Dt.Cells.UI
{
    internal static class DoubleExtension
    {
        public static bool IsZero(this double value)
        {
            return (Math.Abs((double) (value - 0.0)) < 1E-08);
        }
    }
}

