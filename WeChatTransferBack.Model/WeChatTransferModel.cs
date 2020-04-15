using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatTransferBack.Model
{
    public  class WeChatTransferModel
    {
        public string _id { get; set; }
        public string _openid { get; set; }
        public bool check { get; set; }
        public string depnumber { get; set; }
        public float price { get; set; }
        public string size { get; set; }
        public int status { get; set; }
        public string telnumber { get; set; }
        public string time { get; set; }
        public string transfername { get; set; }
        public string transfernum { get; set; }
        public string type { get; set; }
        public string typeid { get; set; }
    }
}
