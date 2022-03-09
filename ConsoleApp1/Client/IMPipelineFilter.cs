using SuperSocket.ProtoBase;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    /// 消息处理管道
    /// </summary>
    public class IMPipelineFilter : FixedHeaderPipelineFilter<IMPackage>
    {
        public IMPipelineFilter() : base(21)//key + id + id + bodyLength = 1+8+8+4
        {
        }

        /// <summary>
        /// 实现获取包体长度的方法
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override int GetBodyLengthFromHeader(ref ReadOnlySequence<byte> buffer)
        {
            var reader = new SequenceReader<byte>(buffer);
            reader.Advance(17);
            reader.TryReadLittleEndian(out int bodyLength);
            return bodyLength;
        }

        protected override IMPackage DecodePackage(ref ReadOnlySequence<byte> buffer)
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
