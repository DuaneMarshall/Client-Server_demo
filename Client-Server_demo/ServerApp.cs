using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;



namespace Client_Server_demo
{
    class ServerApp
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                IPAddress localAddr = IPAddress.Parse("127.0.0.1"); // Use your server's IP
                int port = 8001;

                server = new TcpListener(localAddr, port);
                server.Start();

                

                Console.WriteLine("Server started. Waiting for connections...");
                //Console.WriteLine(theIntro);

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Client connected!");

                    // Handle client communication in a separate thread
                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.Start();
                    
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server?.Stop();
            }
            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }

        static void HandleClient(TcpClient client)
        {

            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            //Send Intro to client
            string theIntro = "Welcome to the server app.  Get ready for your game experience!!!";
            byte[] intro = Encoding.ASCII.GetBytes($"Server received: {theIntro}");
            stream.Write(intro, 0, intro.Length);

            try
            {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received from client: {data}");

                    // Send a response back to the client
                    byte[] msg = Encoding.ASCII.GetBytes($"Server received: {data}");
                    stream.Write(msg, 0, msg.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error handling client: {e.Message}");
            }
            finally
            {
                stream.Close();
                client.Close();
                Console.WriteLine("Client disconnected.");
            }
        }
        

    }
}
