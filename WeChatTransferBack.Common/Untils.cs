using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

using WeChatTransferBack.Model;
using Newtonsoft.Json;
using System.Net.Http;

namespace WeChatTransferBack.Common
{
    public static class Untils
    {
        #region 生成验证用的Token
        /// <summary>
        /// 生成验证用的Token
        /// </summary>
        /// <param name="loginTime">The login time.</param>
        /// <returns></returns>
        public static string GetTokenString()
        {
            string tempStr = AppConsts.WxOpenAppId;

            string md5Str = MD5Encrypt(tempStr);

            return md5Str;
        }
        #endregion

        #region 获取时间戳
        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string GetTimeStamp(DateTime time)
        {
            //DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            //int timeStamp = (int)(time - startTime).TotalMilliseconds; // 相差毫秒数
            //return timeStamp;
                //TimeSpan ts = time - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                //return (int)ts.TotalMilliseconds;

            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区

            string timeStamp = (string)(time - startTime).TotalMilliseconds.ToString(); // 相差毫秒数
            return timeStamp;
        }
        #endregion

        #region 时间戳转为C#格式时间
        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="jstimeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetTimeFromTimeStamp(long jstimeStamp)
        {

            //DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            //DateTime dt = startTime.AddSeconds(timeStamp);
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dt = startTime.AddMilliseconds(jstimeStamp);

            return dt;
        }
        #endregion

        #region MD5加密
        ///   <summary>
        ///   MD5加密
        ///   </summary>
        ///   <param   name="strText">待加密字符串</param>
        ///   <returns>加密后的字符串</returns>
        private static string MD5Encrypt(string strText)
        {
            string tempStr = string.Empty;

            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(strText));

                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                tempStr = sBuilder.ToString();
            }

            return tempStr;
        }
        #endregion

        #region HTTP请求（以GET方式）
        /// <summary>
        /// 发起一个HTTP请求（以GET方式）
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HttpGetStr(string url)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }
        #endregion

        #region POST请求

        public static string HttpPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            //request.CookieContainer = cookie;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
        #endregion

        #region 获取Json字符串某节点的值
        /// <summary>
        /// 获取Json字符串某节点的值
        /// </summary>
        public static string GetJsonValue(string jsonStr, string key)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(jsonStr))
            {
                key = "\"" + key.Trim('"') + "\"";
                int index = jsonStr.IndexOf(key) + key.Length + 1;
                if (index > key.Length + 1)
                {
                    //先截逗号，若是最后一个，截“｝”号，取最小值
                    int end = jsonStr.IndexOf(',', index);
                    if (end == -1)
                    {
                        end = jsonStr.IndexOf('}', index);
                    }

                    result = jsonStr.Substring(index, end - index);
                    result = result.Trim(new char[] { '"', ' ', '\'' }); //过滤引号或空格
                }
            }
            return result;
        }
        #endregion

        #region post请求url，不发送数据
        /// <summary>
        /// 请求url，不发送数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string RequestUrl(string url)
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            request.ContentType = "text/html";
            request.Headers.Add("charset", "utf-8");

            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream, Encoding.UTF8);
            //返回结果网页（html）代码
            string content = sr.ReadToEnd();
            return content;
        }
        #endregion

        #region 验证Token是否过期
        /// <summary>
        /// 验证Token是否过期
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static bool TokenExpired( WxResponseResultModel model)
        {
            double createTime = Convert.ToDouble(GetTimeStamp(model.CreateTime));//"1586837219832.68"
            double nowTime = Convert.ToDouble(GetTimeStamp(DateTime.Now));//"1586848867742.19"
            if (model.ErrCode == "42001" || (nowTime-createTime)/3600000>2)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 获取Token
        /// <summary>
        /// 获取Token
        /// </summary>
        public static string GetToken(string appid, string secret)
        {
            string strJson = HttpGetStr(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret));

            return strJson;
            //return GetJsonValue(strJson, "access_token");
        }
        #endregion
    }
}