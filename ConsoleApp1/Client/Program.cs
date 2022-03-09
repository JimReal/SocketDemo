using System;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            TestClient client = new TestClient("127.0.0.1", 12345);
            await Task.Factory.StartNew(() => { client.Recevice(); }, TaskCreationOptions.LongRunning);

            while (true)
            {
                var data = Console.ReadLine();
                await client.Send(new IMPackage() { Key = 1, RId = 100000001, SId = 100000001, Body = data });
            }
        }
    }
}
