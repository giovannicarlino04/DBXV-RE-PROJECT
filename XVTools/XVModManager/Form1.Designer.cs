namespace XVModManager
{
    partial class XVModManager
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XVModManager));
            clbMods = new CheckedListBox();
            fileToolStripMenuItem = new ToolStripMenuItem();
            installModToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            uninstallModToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem1 = new ToolStripMenuItem();
            clearInstallationToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1 = new MenuStrip();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // clbMods
            // 
            clbMods.BackColor = SystemColors.Control;
            clbMods.Dock = DockStyle.Fill;
            clbMods.ForeColor = SystemColors.WindowText;
            clbMods.FormattingEnabled = true;
            clbMods.Location = new Point(0, 24);
            clbMods.Name = "clbMods";
            clbMods.Size = new Size(800, 426);
            clbMods.TabIndex = 2;
            clbMods.ItemCheck += clbMods_ItemCheck;
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.BackColor = SystemColors.Control;
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { installModToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
            fileToolStripMenuItem.ForeColor = SystemColors.ControlText;
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // installModToolStripMenuItem
            // 
            installModToolStripMenuItem.BackColor = SystemColors.Control;
            installModToolStripMenuItem.Name = "installModToolStripMenuItem";
            installModToolStripMenuItem.Size = new Size(133, 22);
            installModToolStripMenuItem.Text = "Install Mod";
            installModToolStripMenuItem.Click += installModToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(130, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.BackColor = SystemColors.Control;
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(133, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.BackColor = SystemColors.Control;
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { uninstallModToolStripMenuItem });
            toolsToolStripMenuItem.ForeColor = SystemColors.Desktop;
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(39, 20);
            toolsToolStripMenuItem.Text = "Edit";
            // 
            // uninstallModToolStripMenuItem
            // 
            uninstallModToolStripMenuItem.BackColor = SystemColors.Control;
            uninstallModToolStripMenuItem.Name = "uninstallModToolStripMenuItem";
            uninstallModToolStripMenuItem.Size = new Size(148, 22);
            uninstallModToolStripMenuItem.Text = "Uninstall Mod";
            uninstallModToolStripMenuItem.Click += uninstallModToolStripMenuItem_Click;
            // 
            // toolsToolStripMenuItem1
            // 
            toolsToolStripMenuItem1.BackColor = SystemColors.Control;
            toolsToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { clearInstallationToolStripMenuItem });
            toolsToolStripMenuItem1.ForeColor = SystemColors.Desktop;
            toolsToolStripMenuItem1.Name = "toolsToolStripMenuItem1";
            toolsToolStripMenuItem1.Size = new Size(46, 20);
            toolsToolStripMenuItem1.Text = "Tools";
            // 
            // clearInstallationToolStripMenuItem
            // 
            clearInstallationToolStripMenuItem.BackColor = SystemColors.Control;
            clearInstallationToolStripMenuItem.Name = "clearInstallationToolStripMenuItem";
            clearInstallationToolStripMenuItem.Size = new Size(162, 22);
            clearInstallationToolStripMenuItem.Text = "Clear Installation";
            clearInstallationToolStripMenuItem.Click += clearInstallationToolStripMenuItem_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = SystemColors.Control;
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, toolsToolStripMenuItem, toolsToolStripMenuItem1 });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // XVModManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(800, 450);
            Controls.Add(clbMods);
            Controls.Add(menuStrip1);
            ForeColor = SystemColors.WindowText;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "XVModManager";
            Text = "XVModManager";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private CheckedListBox clbMods;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem installModToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem uninstallModToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem1;
        private ToolStripMenuItem clearInstallationToolStripMenuItem;
        private MenuStrip menuStrip1;
        private ToolStripSeparator toolStripSeparator1;
    }
}