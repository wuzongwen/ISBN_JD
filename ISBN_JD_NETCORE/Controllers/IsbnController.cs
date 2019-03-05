using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Common;
using Microsoft.AspNetCore.Cors;

namespace ISBN_JD.Controllers
{
    //启用跨域
    [EnableCors("AllowSameDomain")]
    [Route("api/[controller]")]
    public class IsbnController : Controller
    {
        // GET api/Isbn/Isbnstr
        [HttpGet("{Isbn}")]
        public JsonResult GetIsbnimg(string Isbn)
        {
            string code = string.Empty;
            string msg = string.Empty;
            string pic = string.Empty;
            try
            {
                if (Isbn != null)
                {
                    if (HtmlHelper.RemoveNotNumber(Isbn).Length == 13)
                    {
                        //获取网页数据,页面编码格式(GB2312:Encoding.Default;UTF-8:Encoding.UTF8)
                        string HtmlData = HtmlHelper.DownloadHtml("https://search.jd.com/Search?keyword=" + HtmlHelper.RemoveNotNumber(Isbn), Encoding.UTF8);

                        //获取商品部分Html
                        string Html = HtmlHelper.GetValue(HtmlData, "<ul class=\"gl-warp clearfix\" data-tpl=\".*?\">", "</ul>");

                        //获取所有商品图片
                        var ImgList = HtmlHelper.GetHtmlImageUrlList(Html);

                        if (ImgList.Length > 0)
                        {
                            pic = ImgList[0];
                            msg = "获取成功";
                            code = "10001";
                        }
                        else
                        {
                            msg = "未查询到书籍信息";
                            code = "10002";
                        }
                    }
                    else
                    {
                        msg = "未查询到书籍信息";
                        code = "10002";
                    }
                }
                else
                {
                    msg = "ISBN错误";
                    code = "10003";
                }
            }
            catch (Exception ex)
            {
                msg = "服务器错误";
                code = "10004";
            }
            return Json(new { code, msg, pic });
        }
    }
}
