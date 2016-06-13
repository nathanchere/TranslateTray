using System;
using System.Windows.Forms;
using TranslateTray.Core;

namespace TranslateTray
{
    public partial class frmMain : Form
    {
        private readonly ITranslationClient client = new TranslationClient();

        public frmMain()
        {
            InitializeComponent();

            ClipboardNotification.ClipboardUpdate += OnClipboardUpdated;
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            SetWindowVisibility(WindowState != FormWindowState.Minimized);
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            SetWindowVisibility(true);
        }

        private void SetWindowVisibility(bool isVisible)
        {
            WindowState = isVisible
                ? FormWindowState.Normal
                : FormWindowState.Minimized;
            ShowInTaskbar = isVisible;

            if (isVisible)
            {
                Show();
                BringToFront();
            }
        }

        private void OnClipboardUpdated(object sender, EventArgs eventArgs)
        {
            var text = Clipboard.GetText(TextDataFormat.Text)?.Trim();
            if (string.IsNullOrEmpty(text)) return;

            try
            {
                var newText = client.Translate(text);
                notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon.BalloonTipTitle = $"Translation for: {(text.Length > 30 ? (text.Substring(0, 30) + '…') : text)}";
                notifyIcon.BalloonTipText = newText;
            }
            catch (Exception ex)
            {
                notifyIcon.BalloonTipIcon = ToolTipIcon.Error;
                notifyIcon.BalloonTipTitle = "Error";
                notifyIcon.BalloonTipText = ex.Message;
            }

            notifyIcon.ShowBalloonTip(8000);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            SetWindowVisibility(false);
        }
    }
}
