namespace XVModManager
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            installModToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            compileScriptsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            clearInstallationToolStripMenuItem = new ToolStripMenuItem();
            lvMods = new ListView();
            Mod_Name = new ColumnHeader();
            Mod_Author = new ColumnHeader();
            Mod_Type = new ColumnHeader();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, toolsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(841, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { installModToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // installModToolStripMenuItem
            // 
            installModToolStripMenuItem.Name = "installModToolStripMenuItem";
            installModToolStripMenuItem.Size = new Size(180, 22);
            installModToolStripMenuItem.Text = "Install Mod";
            installModToolStripMenuItem.Click += installModToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(177, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(180, 22);
            exitToolStripMenuItem.Text = "Exit";
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { compileScriptsToolStripMenuItem, toolStripSeparator2, clearInstallationToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(46, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // compileScriptsToolStripMenuItem
            // 
            compileScriptsToolStripMenuItem.Name = "compileScriptsToolStripMenuItem";
            compileScriptsToolStripMenuItem.Size = new Size(162, 22);
            compileScriptsToolStripMenuItem.Text = "Compile Scripts";
            compileScriptsToolStripMenuItem.Click += compileScriptsToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(159, 6);
            // 
            // clearInstallationToolStripMenuItem
            // 
            clearInstallationToolStripMenuItem.Name = "clearInstallationToolStripMenuItem";
            clearInstallationToolStripMenuItem.Size = new Size(162, 22);
            clearInstallationToolStripMenuItem.Text = "Clear Installation";
            clearInstallationToolStripMenuItem.Click += clearInstallationToolStripMenuItem_Click;
            // 
            // lvMods
            // 
            lvMods.Columns.AddRange(new ColumnHeader[] { Mod_Name, Mod_Author, Mod_Type });
            lvMods.Dock = DockStyle.Fill;
            lvMods.Location = new Point(0, 24);
            lvMods.Name = "lvMods";
            lvMods.Size = new Size(841, 407);
            lvMods.TabIndex = 1;
            lvMods.UseCompatibleStateImageBehavior = false;
            lvMods.View = View.Details;
            // 
            // Mod_Name
            // 
            Mod_Name.Text = "Name";
            Mod_Name.Width = 160;
            // 
            // Mod_Author
            // 
            Mod_Author.Text = "Author";
            Mod_Author.Width = 160;
            // 
            // Mod_Type
            // 
            Mod_Type.Text = "Type";
            Mod_Type.Width = 120;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(841, 431);
            Controls.Add(lvMods);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "XVModManager";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem installModToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem clearInstallationToolStripMenuItem;
        private ToolStripMenuItem compileScriptsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ListView lvMods;
        private ColumnHeader Mod_Name;
        private ColumnHeader Mod_Author;
        private ColumnHeader Mod_Type;
    }
}