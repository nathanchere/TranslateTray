using System;
using System.Windows.Forms;

namespace TranslateTray
{
    // Poached from http://stackoverflow.com/a/11901709/243557

    public sealed class ClipboardNotification
    {
        public static event EventHandler ClipboardUpdate;

        private static NotificationForm _form = new NotificationForm();

        private static void OnClipboardUpdate(EventArgs e)
        {
            var handler = ClipboardUpdate;
            handler?.Invoke(null, e);
        }

        private class NotificationForm : Form
        {
            public NotificationForm()
            {
                NativeMethods.SetParent(Handle, NativeMethods.HWND_MESSAGE);
                NativeMethods.AddClipboardFormatListener(Handle);
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == NativeMethods.WM_CLIPBOARDUPDATE)
                {
                    OnClipboardUpdate(null);
                }
                base.WndProc(ref m);
            }
        }
    }
}