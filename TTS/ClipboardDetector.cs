using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TTS
{
    public class ClipboardDetector : Control
    {

        public MainWindow mainWindow;

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);
        private IntPtr _ClipboardViewerNext;

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            const int WM_DRAWCLIPBOARD = 0x308;

            switch (m.Msg)
            {
                case WM_DRAWCLIPBOARD:
                    //Clipboard is Change 
                    Debugger.Log(0, "debug", Environment.NewLine + "clipboard is changed" + Environment.NewLine);
                    mainWindow.DetectCopyAction();
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        public ClipboardDetector (MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

    }
}
