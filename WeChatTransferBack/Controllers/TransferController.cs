using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Script.Serialization;
using WeChatTransferBack.BLL;
using WeChatTransferBack.Common;
using WeChatTransferBack.Controllers;
using WeChatTransferBack.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeChatTransferBack.WebApi.Controllers
{
    public class TransferController : BaseApiController
    {
        [HttpGet]
        public HttpResponseMessage GetTransferList()
        {
            WeChatAccessTokenBLL bll = new WeChatAccessTokenBLL();
            String access_token = bll.GetAccessToken();
            //return access_token;
            string json = new JavaScriptSerializer().Serialize(new
            {
                db = "transfer", //指定操作的数据表
            });
            string cloudfuncurl = string.Format("https://api.weixin.qq.com/tcb/invokecloudfunction?access_token={0}&env={1}&name=GetTransfer", access_token, AppConsts.ENV);
            string resString=Untils.HttpPost(cloudfuncurl, json);

            WeChatTransferRootModel res = JsonConvert.DeserializeObject<WeChatTransferRootModel>(resString);
            WeChatRootModel model = JsonConvert.DeserializeObject<WeChatRootModel>(res.resp_data.ToString());
            model.code = 20000;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string str = serializer.Serialize(model);
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }
    }

}
