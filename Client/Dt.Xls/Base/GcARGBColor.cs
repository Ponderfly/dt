#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2014-07-01 创建
******************************************************************************/
#endregion

#region 引用命名
using System;
using System.Runtime.InteropServices;
#endregion

namespace Dt.Xls
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct GcARGBColor
    {
        public byte a;
        public byte r;
        public byte g;
        public byte b;
    }
}

