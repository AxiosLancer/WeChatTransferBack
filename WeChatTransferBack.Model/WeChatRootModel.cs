using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatTransferBack.Model
{
    public class WeChatRootModel
    {
        public List<WeChatTransferModel> data { get; set; }
        public int code { get; set; }
    }
}
