using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatTransferBack.Common;
using WeChatTransferBack.DAL;
using WeChatTransferBack.Model;
using Newtonsoft.Json;
using System.IO;

namespace WeChatTransferBack.BLL
{
    public class WeChatAccessTokenBLL
    {
        #region 更新AccessToken到本地
        ///// <summary>
        ///// 将获取的access_token写入本地文件
        ///// </summary>
        public void UpdateAccessToken(WxResponseResultModel model)
        {
            
            model.CreateTime =DateTime.Now;
            WeChatAccessTokenDAL dal = new WeChatAccessTokenDAL();
            if (model.ErrCode == "45009")
            {
                throw new Exception("当天接口调用次数达到上限" + model.ErrMsg);
            }
            dal.UpdateLocalAccessToken(model);
        }
        #endregion

        #region 获取access_token
        /// <summary>
        /// 获取access_token
        /// </summary>
        public string GetAccessToken()
        {
            string access_token = string.Empty;
            string resString;
            WeChatAccessTokenDAL dal = new WeChatAccessTokenDAL();
            WxResponseResultModel model = dal.GetLocalAccessToken();
            if (string.IsNullOrWhiteSpace(model.Access_Token)) //尚未保存过access_token
            {
                resString = Untils.GetToken(AppConsts.WxOpenAppId, AppConsts.WxOpenAppSecret);
                WxResponseResultModel newModel = JsonConvert.DeserializeObject<WxResponseResultModel>(resString);
                UpdateAccessToken(newModel);
                access_token = newModel.Access_Token;
            }
            else
            {
                if (Untils.TokenExpired(model)) //access_token过期
                {
                    resString = Untils.GetToken(AppConsts.WxOpenAppId, AppConsts.WxOpenAppSecret);
                    WxResponseResultModel newModel = JsonConvert.DeserializeObject<WxResponseResultModel>(resString);
                    UpdateAccessToken(newModel);
                    access_token = newModel.Access_Token;
                }
                else
                {
                    return model.Access_Token;
                }
            }
            return access_token;
        }
        #endregion


    }
}
