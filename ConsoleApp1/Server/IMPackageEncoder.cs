using SuperSocket.ProtoBase;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    /// <summary>
    /// 消息协议解析
    /// </summary>
    public class IMPackageEncoder : IPackageEncoder<IMPackage>
    {
        /// <summary>
        /// 编码消息,返回编码数据长度
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="pack"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int Encode(IBufferWriter<byte> writer, IMPackage pack)
        {
            var bodyBuffer = Encoding.UTF8.GetBytes(pack.Body);
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                ms.WriteByte(pack.Key);//命令字
                ms.Write(GetLittleBytes(pack.SId));//发送者
                ms.Write(GetLittleBytes(pack.RId));//接受者
                ms.Write(BitConverter.GetBytes(bodyBuffer.Length));//包体长度
                ms.Write(bodyBuffer);//包体
                buffer = ms.ToArray();
            }
            var str = Encoding.UTF8.GetString(buffer);
            var num = writer.Write(str, Encoding.UTF8);
            return num;
        }

        public byte[] GetLittleBytes(long ln)
        {
            var bytes = BitConverter.GetBytes(ln);
            if (!BitConverter.IsLittleEndian)
                bytes = bytes.Reverse().ToArray();
            return bytes;
        }
    }
}
