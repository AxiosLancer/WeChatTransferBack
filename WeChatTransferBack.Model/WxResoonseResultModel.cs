using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChatTransferBack.Model
{
    /// <summary>
    /// 响应结果返回类
    /// </summary>
    public class WxResponseResultModel
    {
        public string ErrCode { get; set; }
        public string ErrMsg { get; set; }
        public string Access_Token { get; set; }
        public DateTime CreateTime { get; set; }
    }
}