using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace KeystrokeMod {

    public partial class Keystroke : Form {

        private static char chForward = 'W';
        private static char chLeft = 'A';
        private static char chBackward = 'S';
        private static char chRight = 'D';

        public static new IntPtr Handle;

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(Keys key);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string IpClassName, string IpWindowName);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT IpRect);

        public static RECT rect;

        public struct RECT {
            public int left, top, right, bottom;
        }

        public Keystroke() {
            InitializeComponent();
        }

        private void Keystroke_Load(object sender, EventArgs e) {
            ActiveControl = pnlKeystroke;

            btnForward.Text = chForward.ToString();
            btnLeft.Text = chLeft.ToString();
            btnBackward.Text = chBackward.ToString();
            btnRight.Text = chRight.ToString();

            CheckForIllegalCrossThreadCalls = false;
            SetInvisible(this);

            Thread thKeystroke = new Thread(() => KeystrokeMod(1, "Minecraft", this)) { IsBackground = true };
            thKeystroke.Start();
        }

        private void SetInvisible(Form form) {
            form.BackColor = Color.Wheat;
            form.TransparencyKey = Color.Wheat;
            form.TopMost = true;
            form.FormBorderStyle = FormBorderStyle.None;

            int initialSize = GetWindowLong(Handle, -20);
            SetWindowLong(Handle, -20, initialSize | 0x8000 | 0x20);
        }

        private void KeystrokeMod(int frequency, string windowName, Form form) {
            while (true) {
                Handle = FindWindow(null, windowName);
                GetWindowRect(Handle, out rect);
                form.Size = new Size(rect.right - rect.left, rect.bottom - rect.top);
                form.Left = rect.left;
                form.Top = rect.top;

                if (GetAsyncKeyState((Keys)32) < 0) {
                    btnSpace.BackColor = Color.Red;
                } else if (GetAsyncKeyState((Keys)32) == 0) {
                    btnSpace.BackColor = Color.White;
                }

                if (GetAsyncKeyState((Keys)65) < 0) {
                    btnLeft.BackColor = Color.Red;
                } else if (GetAsyncKeyState((Keys)65) == 0) {
                    btnLeft.BackColor = Color.White;
                }

                if (GetAsyncKeyState((Keys)68) < 0) {
                    btnRight.BackColor = Color.Red;
                } else if (GetAsyncKeyState((Keys)68) == 0) {
                    btnRight.BackColor = Color.White;
                }

                if (GetAsyncKeyState(Keys.LButton | Keys.R) < 0) {
                    btnBackward.BackColor = Color.Red;
                } else if (GetAsyncKeyState(Keys.LButton | Keys.R) == 0) {
                    btnBackward.BackColor = Color.White;
                }

                if (GetAsyncKeyState(Keys.LButton | Keys.V) < 0) {
                    btnForward.BackColor = Color.Red;
                } else if (GetAsyncKeyState(Keys.LButton | Keys.V) == 0) {
                    btnForward.BackColor = Color.White;
                }

                if (GetAsyncKeyState((Keys)1) < 0){
                    btnLeftClick.BackColor = Color.Red; }
                else if (GetAsyncKeyState((Keys)1) == 0) {
                    btnLeftClick.BackColor = Color.White;
                }

                if (GetAsyncKeyState((Keys)2) < 0) {
                    btnRightClick.BackColor = Color.Red;
                } else if (GetAsyncKeyState((Keys)2) == 0) {
                    btnRightClick.BackColor = Color.White;
                }

                Thread.Sleep(frequency);
            }
        }
    }

}