using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TranslateTray
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();            
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
    }
}
