using SuperSocket.Client;
using SuperSocket.Channel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class TestClient
    {
        private bool _conn;
        private IPEndPoint _url;
        private readonly IEasyClient<IMPackage, IMPackage> _client;
        private IMPackageEncoder _encoder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip">服务器IP</param>
        /// <param name="port">服务器端口</param>
        public TestClient(string ip, int port)
        {
            _url = new IPEndPoint(IPAddress.Parse(ip), port);
            _encoder = new IMPackageEncoder();
            var client = new EasyClient<IMPackage, IMPackage>(new IMPipelineFilter(), _encoder, new ChannelOptions()
            {
                SendTimeout = 30//...
            });
            _client = client;
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public async ValueTask Closeing()
        {
            await _client.CloseAsync();
            _conn = false;
        }

        /// <summary>
        /// 开始连接
        /// </summary>
        public async ValueTask Recevice()
        {
            _conn = await _client.ConnectAsync(_url);
            while (_conn)
            {
                var msg = await _client.ReceiveAsync();
                if (msg != null)
                    Console.WriteLine($"服务端消息：发送者 {msg.SId} ，接受者 {msg.RId}, 内容 { msg.Body}");
            }
        }

        public async ValueTask Send(IMPackage model)
        {
            if (_conn)
            {
                await _client.SendAsync(model);
            }
        }
    }
}
