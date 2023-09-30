using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace WindowAndControl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal enum MyKeys
        {
            None = 0,
            LButton = 1,
            RButton = 2,
            Cancel = 3,
            MButton = 4,
            XButton1 = 5,
            XButton2 = 6,
            Back = 8,
            Tab = 9,
            LineFeed = 10,
            Clear = 12,
            Return = 13,
            ShiftKey = 16,
            ControlKey = 17,
            Menu = 18,
            Pause = 19,
            Capital = 20,
            KanaMode = 21,
            JunjaMode = 23,
            FinalMode = 24,
            HanjaMode = 25,
            Escape = 27,
            IMEConvert = 28,
            IMENonconvert = 29,
            IMEAceept = 30,
            IMEModeChange = 31,
            Space = 32,
            PageUp = 33,
            Next = 34,
            End = 35,
            Home = 36,
            Left = 37,
            Up = 38,
            Right = 39,
            Down = 40,
            Select = 41,
            Print = 42,
            Execute = 43,
            PrintScreen = 44,
            Insert = 45,
            Delete = 46,
            Help = 47,
            D0 = 48,
            D1 = 49,
            D2 = 50,
            D3 = 51,
            D4 = 52,
            D5 = 53,
            D6 = 54,
            D7 = 55,
            D8 = 56,
            D9 = 57,
            A = 65,
            B = 66,
            C = 67,
            D = 68,
            E = 69,
            F = 70,
            G = 71,
            H = 72,
            I = 73,
            J = 74,
            K = 75,
            L = 76,
            M = 77,
            N = 78,
            O = 79,
            P = 80,
            Q = 81,
            R = 82,
            S = 83,
            T = 84,
            U = 85,
            V = 86,
            W = 87,
            X = 88,
            Y = 89,
            Z = 90,
            LWin = 91,
            RWin = 92,
            Apps = 93,
            Sleep = 95,
            NumPad0 = 96,
            NumPad1 = 97,
            NumPad2 = 98,
            NumPad3 = 99,
            NumPad4 = 100,
            NumPad5 = 101,
            NumPad6 = 102,
            NumPad7 = 103,
            NumPad8 = 104,
            NumPad9 = 105,
            Multiply = 106,
            Add = 107,
            Separator = 108,
            Subtract = 109,
            Decimal = 110,
            Divide = 111,
            F1 = 112,
            F2 = 113,
            F3 = 114,
            F4 = 115,
            F5 = 116,
            F6 = 117,
            F7 = 118,
            F8 = 119,
            F9 = 120,
            F10 = 121,
            F11 = 122,
            F12 = 123,
            F13 = 124,
            F14 = 125,
            F15 = 126,
            F16 = 127,
            F17 = 128,
            F18 = 129,
            F19 = 130,
            F20 = 131,
            F21 = 132,
            F22 = 133,
            F23 = 134,
            F24 = 135,
            NumLock = 144,
            Scroll = 145,
            LShiftKey = 160,
            RShiftKey = 161,
            LControlKey = 162,
            RControlKey = 163,
            LMenu = 164,
            RMenu = 165,
            BrowserBack = 166,
            BrowserForward = 167,
            BrowserRefresh = 168,
            BrowserStop = 169,
            BrowserSearch = 170,
            BrowserFavorites = 171,
            BrowserHome = 172,
            VolumeMute = 173,
            VolumeDown = 174,
            VolumeUp = 175,
            MediaNextTrack = 176,
            MediaPreviousTrack = 177,
            MediaStop = 178,
            MediaPlayPause = 179,
            LaunchMail = 180,
            SelectMedia = 181,
            LaunchApplication1 = 182,
            LaunchApplication2 = 183,
            Oem1 = 186,
            Oemplus = 187,
            Oemcomma = 188,
            OemMinus = 189,
            OemPeriod = 190,
            OemQuestion = 191,
            Oemtilde = 192,
            OemOpenBrackets = 219,
            Oem5 = 220,
            Oem6 = 221,
            Oem7 = 222,
            Oem8 = 223,
            OemBackslash = 226,
            ProcessKey = 229,
            Packet = 231,
            Attn = 246,
            Crsel = 247,
            Exsel = 248,
            EraseEof = 249,
            Play = 250,
            Zoom = 251,
            NoName = 252,
            Pa1 = 253,
            OemClear = 254,
            KeyCode = 65535,
            Shift = 65536,
            Control = 131072,
            Alt = 262144,
            Modifiers = -65536,
        }


        private const int WH_KEYBOARD_LL = 13;
        private const int WH_MOUSE_LL = 14;

        //keyboard events
        internal const int WM_KEYDOWN = 0x0100;
        internal const int WM_KEYUP = 0x0101;

        //mouse events
        internal const int WM_MOUSEMOVE = 0x00000200;
        internal const int WM_LBUTTONDOWN = 0x00000201;
        internal const int WM_LBUTTONUP = 0x00000202;
        internal const int WM_LBUTTONDBLCLK = 0x00000203;
        internal const int WM_RBUTTONDOWN = 0x00000204;
        internal const int WM_RBUTTONUP = 0x00000205;
        internal const int WM_RBUTTONDBLCLK = 0x00000206;
        internal const int WM_MBUTTONDOWN = 0x00000207;
        internal const int WM_MBUTTONUP = 0x00000208;
        internal const int WM_MBUTTONDBLCLK = 0x00000209;
        internal const int WM_MOUSEWHEEL = 0x0000020A;
        internal const int WM_XBUTTONDOWN = 0x0000020B;
        internal const int WM_XBUTTONUP = 0x0000020C;
        internal const int WM_XBUTTONDBLCLK = 0x0000020D;
        internal const int WM_MOUSEHWHEEL = 0x0000020E;

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT
        {
            public Int32 x;
            public Int32 y;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct CURSORINFO
        {
            public Int32 cbSize;        // Specifies the size, in bytes, of the structure.
                                        // The caller must set this to Marshal.SizeOf(typeof(CURSORINFO)).
            public Int32 flags;         // Specifies the cursor state. This parameter can be one of the following values:
                                        //    0             The cursor is hidden.
                                        //    CURSOR_SHOWING    The cursor is showing.
                                        //    CURSOR_SUPPRESSED    (Windows 8 and above.) The cursor is suppressed. This flag indicates that the system is not drawing the cursor because the user is providing input through touch or pen instead of the mouse.
            public IntPtr hCursor;          // Handle to the cursor.
            public POINT ptScreenPos;       // A POINT structure that receives the screen coordinates of the cursor.
            public CURSORINFO(Boolean? bb) : this()
            {
                cbSize = (Int32)(Marshal.SizeOf(typeof(CURSORINFO)));
            }
        }



        private delegate IntPtr LowLevelProc(int code, IntPtr wParam, IntPtr lParam);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr SetWindowsHookEx(int hookType, LowLevelProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        static extern bool GetCursorInfo(ref CURSORINFO pci);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool UnhookWindowsHookEx(IntPtr hhk);

        static LowLevelProc Keyboard_CallbackMethod = Keyboard_WriteToLog;
        static LowLevelProc Mouse_CallbackMethod = Mouse_WriteToLog;
        static IntPtr kb_HookHandle = IntPtr.Zero;
        static IntPtr mouse_HookHandle = IntPtr.Zero;

        static IntPtr Keyboard_WriteToLog(int ncode, IntPtr wParam, IntPtr lParam)
        {
            if (ncode >= 0)
            {
                if ((int)wParam == WM_KEYDOWN)
                {
                    int v = Marshal.ReadInt32(lParam);
                    WriteLog(v);
                }
            }

            return CallNextHookEx(kb_HookHandle, ncode, wParam, lParam);
        }

        static IntPtr Mouse_WriteToLog(int ncode, IntPtr wParam, IntPtr lParam)
        {
            if (ncode >= 0)
            {
                CURSORINFO cURSORINFO = new CURSORINFO(true);
                GetCursorInfo(ref cURSORINFO);
                if ((int)wParam == WM_LBUTTONUP)
                {
                    string content = "Left Mouse Up: " + cURSORINFO.ptScreenPos.x + "/" + cURSORINFO.ptScreenPos.y;
                    Console.WriteLine(content);
                    WriteLog(content);
                }
                else if ((int)wParam == WM_LBUTTONDOWN)
                {
                    string content = "Left Mouse Down: " + cURSORINFO.ptScreenPos.x + "/" + cURSORINFO.ptScreenPos.y;
                    Console.WriteLine(content);
                    WriteLog(content);
                }
            }

            return CallNextHookEx(mouse_HookHandle, ncode, wParam, lParam);
        }

        static private void WriteLog(int v)
        {
            StreamWriter streamWriter = new StreamWriter(LogFile, true);
            var s = ((MyKeys)v);
            Console.WriteLine(s);
            streamWriter.WriteLine(s);
            streamWriter.Close();
        }

        static private void WriteLog(string v)
        {
            StreamWriter streamWriter = new StreamWriter(LogFile, true);

            Console.WriteLine(v);
            streamWriter.WriteLine(v);
            streamWriter.Close();
        }

        static IntPtr SetKeyboardHook(LowLevelProc proc)
        {
            using (Process p = Process.GetCurrentProcess())
            {
                using (ProcessModule pm = p.MainModule)
                {
                    IntPtr pmHandle = GetModuleHandle(pm.ModuleName);
                    return SetWindowsHookEx(WH_KEYBOARD_LL, proc, pmHandle, 0);
                }
            }
        }

        static IntPtr SetMouseHook(LowLevelProc proc)
        {
            using (Process p = Process.GetCurrentProcess())
            {
                using (ProcessModule pm = p.MainModule)
                {
                    IntPtr pmHandle = GetModuleHandle(pm.ModuleName);
                    return SetWindowsHookEx(WH_MOUSE_LL, proc, pmHandle, 0);
                }
            }
        }

        internal static void StartKeyboardHook()
        {
            kb_HookHandle = SetKeyboardHook(Keyboard_CallbackMethod);
            //app.Run();
            //UnhookWindowsHookEx(kb_HookHandle);
        }

        internal static void StartMouseUpDownHook()
        {
            mouse_HookHandle = SetMouseHook(Mouse_CallbackMethod);
            //app.Run();
            //UnhookWindowsHookEx(mouse_HookHandle);
        }

        internal static string LogFile;

        internal static void UnhookWindowsHookEx()
        {
            UnhookWindowsHookEx(kb_HookHandle);
            UnhookWindowsHookEx(mouse_HookHandle);
        }

    }
}
