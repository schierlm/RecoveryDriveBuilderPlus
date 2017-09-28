using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

// http://blogs.msdn.com/b/knom/archive/2007/03/12/command_5f00_link.aspx
// http://stackoverflow.com/a/9071742/90203
namespace RecoveryDriveBuilderPlus
{
    public class CommandLink : Button
    {
        const int BS_COMMANDLINK = 0x0000000E;
        const uint BCM_SETNOTE = 0x1609;

        private string note = null;

        public CommandLink()
        {
            this.FlatStyle = FlatStyle.System;
        }


        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cParams = base.CreateParams;
                cParams.Style |= BS_COMMANDLINK;
                return cParams;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        public string NoteText
        {
            get { return note; }
            set
            {
                note = value;
                SendMessage(new HandleRef(this, this.Handle), BCM_SETNOTE, IntPtr.Zero, note);
            }
        }

        [DllImport("user32.dll", EntryPoint = "SendMessageW")]
        private static extern IntPtr SendMessage(HandleRef hWnd, UInt32 Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
    }
}
