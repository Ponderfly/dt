﻿#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2019-12-10
******************************************************************************/
#endregion

#region 引用命名
using Dt.Base;
using Dt.Base.Docking;
using Dt.Base.FormView;
using Dt.Core;
using Dt.Core.Model;
using Dt.Kehu;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
#endregion

namespace Dt.Kehu
{

    public static class AtJieHu
    {
        public static void ShowDetail(long p_id)
        {
            AtApp.OpenWin(typeof(JieHuMain), p_params: p_id);
        }
    }
}