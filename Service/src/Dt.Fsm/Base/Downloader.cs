﻿#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2019-09-12 创建
******************************************************************************/
#endregion

#region 引用命名
using Dt.Core;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;
using System.Threading.Tasks;
#endregion

namespace Dt.Fsm
{
    /// <summary>
    /// 处理文件下载
    /// </summary>
    public class Downloader
    {
        HttpContext _context;

        public Downloader(HttpContext p_context)
        {
            _context = p_context;
        }

        public async Task Handle()
        {
            // 截取路径
            string path = _context.Request.Path.Value.ToLower().Substring(4);
            FileInfo fileInfo = new FileInfo(Path.Combine(Cfg.Root, path));
            if (!fileInfo.Exists)
            {
                _context.Response.Headers["error"] = WebUtility.UrlEncode("😢下载失败，文件不存在！");
                return;
            }

            await new Db().Exec("增加下载次数", new { path = path });
            var response = _context.Response;
            response.Headers["Content-Type"] = "application/octet-stream";
            response.Headers["Content-Transfer-Encoding"] = "binary";
            response.Headers["Content-Length"] = fileInfo.Length.ToString();
            // 不以附件形式下载
            //response.Headers["Content-Disposition"] = "attachment;filename=" + path.Substring(path.LastIndexOf('/') + 1);

            try
            {
                await response.SendFileAsync(Path.Combine(Cfg.Root, path), _context.RequestAborted);
            }
            catch { }
        }
    }
}
