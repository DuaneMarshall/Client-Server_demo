using System;
using System.Net.Sockets;
using System.Text;


namespace Client_demo
{
    class ClientApp
    {
        static void Main(string[] args)
        {
            TcpClient client = null;
            try
            {
                string serverIp = "127.0.0.1"; // Server's IP address
                int port = 8001;

                client = new TcpClient(serverIp, port);
                Console.WriteLine("Connected to server.");
                //Console.WriteLine("Let's get ready to rumble!!!");


                NetworkStream stream = client.GetStream();

                while (true)
                {
                    
                    Console.Write("Enter message to send (or 'exit' to quit): ");
                    string message = Console.ReadLine();

                    if (message.ToLower() == "exit")
                        break;

                    byte[] data = Encoding.ASCII.GetBytes(message);
                    stream.Write(data, 0, data.Length);

                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received from server: {response}");
                }

                stream.Close();
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
            finally
            {
                client?.Close();
            }
            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }

    }
}
