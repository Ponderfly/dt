﻿#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2023-02-01 创建
******************************************************************************/
#endregion

#region 引用命名
#endregion

namespace Dt.MgrDemo
{
    /// <summary>
    /// demo服务的数据访问
    /// </summary>
    public partial class AtDemo : EntityAccess<AtDemo.Info>
    {
        public class Info : AccessInfo
        {
            public override string Name => "demo";
        }
    }
}