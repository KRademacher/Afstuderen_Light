using System;
using System.Threading.Tasks;
using Grpc.Core;
using Weatherstation;

//A test gRPC Receiver to test the gRPC server with. Will not be used in the final testing.
namespace Test_gRPC_Receiver
{
    class Receiver : WeatherDataSender.WeatherDataSenderBase
    {
        //Override to determine what to do on a received package.
        public override Task<DataReply> SendData(DataRequest request, ServerCallContext context)
        {
            return Task.FromResult(new DataReply {
                Message = "ACK " + request.Time
            });
        }
    }

    class Program
    {
        const int port = 50051;

        static void Main(string[] args)
        {
            //Create a server and listen for incoming packages.
            Grpc.Core.Server server = new Grpc.Core.Server
            {
                Services = { WeatherDataSender.BindService(new Receiver()) },
                Ports = { new ServerPort("localhost", port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("Receiver server listening on port " + port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}
