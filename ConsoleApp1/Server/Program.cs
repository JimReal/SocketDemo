using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SuperSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            var host = SuperSocketHostBuilder.Create<IMPackage, IMPipelineFilter>();

            host.UsePackageDecoder<IMPackageDecoder>()
            .UsePackageHandler(async (session, package) =>
            {
                global::System.Console.WriteLine($"客户端消息：发送者 {package.SId} ，接受者 {package.RId}, 内容 { package.Body}");

                await session.SendAsync(new IMPackageEncoder(), new IMPackage() { Key = 1, RId = 100000001, SId = 100000001, Body = package.Body });
                return;
            });
            host.UseSessionHandler(async session =>
            {
                await session.SendAsync(new IMPackageEncoder(), new IMPackage() { Key = 1, RId = 100000001, SId = 100000001, Body = "已建立连接" });
            }, (s, c) =>
            {
                global::System.Console.WriteLine(c.Reason + " 会话断开" + s.SessionID);
                return new ValueTask();
            });

            //配置日志
            host.ConfigureLogging((ctx, loggingBuilder) =>
            {
                loggingBuilder.AddConsole();
            });
            //配置服务器信息
            host.ConfigureSuperSocket((option) =>
            {
                option.Name = "server one";
                option.SendTimeout = 30;
                option.ReceiveTimeout = 30;
                option.DefaultTextEncoding = Encoding.UTF8;
                option.Listeners = new List<ListenOptions>() {
            new ListenOptions()
            {
                Ip = "Any",
                Port = 12345
            }
        };
            });
            Console.WriteLine("server one is running");
            await host.Build().RunAsync();
        }
    }
}
