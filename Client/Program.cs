using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client
{

    class Program
    {
        static string remoteAddress = "127.0.0.1";
        static int remotePort;
        static int listenerPort;
        static string nickname;

        static void Main(string[] args)
        {
            try
            {
                Console.Write("Input the port for listerning: ");
                listenerPort = int.Parse(Console.ReadLine());

                Thread receiveThread = new Thread(new ThreadStart(ReceiveMsg));
                receiveThread.Start();
                Thread.Sleep(100);
                Console.Write("Input the for connecting: ");
                remotePort = Int32.Parse(Console.ReadLine());
                Console.Write("Input your nickname: ");
                nickname = Console.ReadLine();
                SendMsg();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void SendMsg()
        {
            UdpClient sender = new UdpClient();
            try
            {
                while (true)
                {
                    string message = Console.ReadLine();
                    byte[] data = Encoding.Unicode.GetBytes(nickname + ": " + message);
                    sender.Send(data, data.Length, remoteAddress, remotePort);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);            
            }
            finally
            {
                sender.Close();
            }
        }

        private static void ReceiveMsg()
        {
            UdpClient receiver;
            IPEndPoint remoteIp;
            int portCopy = listenerPort;
            while (true)
            {
                try
                {
                    receiver = new UdpClient(listenerPort);
                    remoteIp = null;
                    if(listenerPort != portCopy)
                        Console.WriteLine($"Your local port has been updated to { listenerPort }, because the prev port is busy");
                    break;
                }
                catch
                {
                    listenerPort++;
                }
            }
            
            try
            {
                while (true)
                {
                    byte[] data = receiver.Receive(ref remoteIp);
                    string message = Encoding.Unicode.GetString(data);
                    Console.WriteLine($"{ message }");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                receiver.Close();
            }
        }
    }
}
