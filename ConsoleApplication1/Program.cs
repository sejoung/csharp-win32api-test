using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace KillProcessUplus
{

    class Program
    {

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, String lParam);

        [DllImport("user32.dll")]
        static extern IntPtr GetDlgItem(IntPtr hDlg, int nIDDlgItem);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);


        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        const uint MOUSEEVENTF_LEFTUP = 0x0004;
        const Int32 WM_CHAR = 0x0102;
        const Int32 WM_KEYDOWN = 0x0100;
        const Int32 WM_KEYUP = 0x0101;
        const Int32 VK_RETURN = 0x0D;


        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);


        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }



        static void Main(string[] args)
        {
            Program a = new Program();
            a.autoLogin();

            Thread.Sleep(1000);

            a.killApp();

        }

        private void setCursorCenter(RECT rect)
        {
            SetCursorPos(rect.Left + (rect.Right - rect.Left) / 2, rect.Top + (rect.Bottom - rect.Top) / 2);
        }

        private void clickLeftMouseBtn()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(100);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            Thread.Sleep(100);
        }

        private void sendMsg(IntPtr obj, String msg)
        {

            foreach(char c in msg)
            {
               
                IntPtr val = new IntPtr(c);
                SendMessage(obj, WM_CHAR,val.ToInt32(),null);
            }


        }


        private void autoLogin()
        {
            IntPtr loginWindow = FindWindow(null, "Medialog");

            int idField = 0x000003F6;
            int pwField = 0x000003F9;
            int sendButton = 0x000003FC;

            IntPtr id =  GetDlgItem(loginWindow, idField);

            RECT idRct = new RECT();

            GetWindowRect(id,  out idRct);

            setCursorCenter(idRct);

            clickLeftMouseBtn();

            sendMsg(id, "aaaa");

            IntPtr pw = GetDlgItem(loginWindow, pwField);

            RECT pwRct = new RECT();

            GetWindowRect(pw, out pwRct);

            setCursorCenter(pwRct);

            clickLeftMouseBtn();

            sendMsg(pw, "aaa!");
            Thread.Sleep(100);

            IntPtr btn = GetDlgItem(loginWindow, sendButton);

            RECT btnRct = new RECT();

            GetWindowRect(btn, out btnRct);

            setCursorCenter(btnRct);

            clickLeftMouseBtn();
           
        }

        private void killApp()
        {
            String[] killProcessName = {"sdslogin", "sdsman", "comagent", "dscntsrv", "piagent",
                       "piprotectorns64", "pisupervisor", "patray", "pasvc", "secupc"};
            foreach (System.Diagnostics.Process myProc in System.Diagnostics.Process.GetProcesses())
            {
                foreach (String killName in killProcessName)
                {
                    String runName = myProc.ProcessName.ToLower();

                    if (runName == killName)
                    {
                        myProc.Kill();
                        Console.WriteLine(myProc.ProcessName + " kill");
                    }
                }

            }
        }

    }
}
