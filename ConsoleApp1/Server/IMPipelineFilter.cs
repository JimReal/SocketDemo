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
    }
}
