using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Client
{
    public class IMPackage
    {
        public IMPackage()
        {

        }

        public byte Key { get; set; }

        /// <summary>
        /// 发送用户Id
        /// </summary>
        public long SId { get; set; }

        /// <summary>
        /// 接收用户Id
        /// </summary>
        public long RId { get; set; }

        public string Body { get; set; }
    }
}
