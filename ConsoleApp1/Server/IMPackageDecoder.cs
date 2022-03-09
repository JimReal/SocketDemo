using SuperSocket.ProtoBase;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    /// <summary>
    /// 消息协议解析
    /// </summary>
    public class IMPackageDecoder : IPackageDecoder<IMPackage>
    {
        /// <summary>
        /// 解析二进制到实体类
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IMPackage Decode(ref ReadOnlySequence<byte> buffer, object context)
        {
            IMPackage data = new IMPackage();
            var reader = new SequenceReader<byte>(buffer);
            reader.TryRead(out byte key);

            reader.TryReadLittleEndian(out long sid);//发送者id
            data.SId = sid;

            reader.TryReadLittleEndian(out long rid);//接受者id
            data.RId = rid;

            reader.Advance(4);
            data.Key = key;
            data.Body = reader.ReadString();
            return data;
        }
    }
}
