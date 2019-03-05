using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;

namespace ISBN_JD_MVC.Controllers
{
    public class IsbnController : ApiController
    {
        [Route("api/Isbn/{Isbn}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public void GetIsbnimg(string Isbn)
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
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];//获取传统context
            HttpRequestBase request = context.Request;//定义传统request对象
            HttpContext.Current.Response.Charset = "GB2312";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(new { code, msg, pic }));
            HttpContext.Current.Response.End();
        }
    }
}
