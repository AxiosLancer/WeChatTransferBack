using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeChatTransferBack.Model;
using Newtonsoft.Json;
using WeChatTransferBack.Common;

namespace WeChatTransferBack.DAL
{
    public class WeChatAccessTokenDAL
    {
        public  void UpdateLocalAccessToken(WxResponseResultModel  model)
        {
            string resString = JsonConvert.SerializeObject(model);
            File.WriteAllText(@"C:\Users\龙隐\source\repos\WeChatTransferBack\WeChatTransferBack\Access_Token\access_token.json", resString, Encoding.UTF8);
        }

        public WxResponseResultModel GetLocalAccessToken()
        {
            string resString = File.ReadAllText(@"C: \Users\龙隐\source\repos\WeChatTransferBack\WeChatTransferBack\Access_Token\access_token.json");
            WxResponseResultModel model = JsonConvert.DeserializeObject<WxResponseResultModel>(resString);
            return model;
        }
    }
}
