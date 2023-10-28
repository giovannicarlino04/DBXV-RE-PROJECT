namespace XVReplacerCreator
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
            label1 = new Label();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            buildXVModFileToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            sToolStripMenuItem = new ToolStripMenuItem();
            txtFolder = new TextBox();
            btnFolder = new Button();
            label2 = new Label();
            txtAuthor = new TextBox();
            label3 = new Label();
            txtName = new TextBox();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(43, 120);
            label1.Name = "label1";
            label1.Size = new Size(71, 15);
            label1.TabIndex = 0;
            label1.Text = "Mod Folder:";
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(376, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { buildXVModFileToolStripMenuItem, toolStripSeparator1, sToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // buildXVModFileToolStripMenuItem
            // 
            buildXVModFileToolStripMenuItem.Name = "buildXVModFileToolStripMenuItem";
            buildXVModFileToolStripMenuItem.Size = new Size(180, 22);
            buildXVModFileToolStripMenuItem.Text = "Build XVMod File";
            buildXVModFileToolStripMenuItem.Click += buildXVModFileToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(177, 6);
            // 
            // sToolStripMenuItem
            // 
            sToolStripMenuItem.Name = "sToolStripMenuItem";
            sToolStripMenuItem.Size = new Size(180, 22);
            sToolStripMenuItem.Text = "Exit";
            // 
            // txtFolder
            // 
            txtFolder.Location = new Point(125, 116);
            txtFolder.Name = "txtFolder";
            txtFolder.Size = new Size(170, 23);
            txtFolder.TabIndex = 2;
            // 
            // btnFolder
            // 
            btnFolder.Location = new Point(301, 116);
            btnFolder.Name = "btnFolder";
            btnFolder.Size = new Size(33, 23);
            btnFolder.TabIndex = 3;
            btnFolder.Text = "...";
            btnFolder.UseVisualStyleBackColor = true;
            btnFolder.Click += btnFolder_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(42, 80);
            label2.Name = "label2";
            label2.Size = new Size(75, 15);
            label2.TabIndex = 0;
            label2.Text = "Mod Author:";
            // 
            // txtAuthor
            // 
            txtAuthor.Location = new Point(125, 77);
            txtAuthor.Name = "txtAuthor";
            txtAuthor.Size = new Size(170, 23);
            txtAuthor.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(42, 44);
            label3.Name = "label3";
            label3.Size = new Size(70, 15);
            label3.TabIndex = 0;
            label3.Text = "Mod Name:";
            // 
            // txtName
            // 
            txtName.Location = new Point(125, 41);
            txtName.Name = "txtName";
            txtName.Size = new Size(170, 23);
            txtName.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(376, 179);
            Controls.Add(btnFolder);
            Controls.Add(txtName);
            Controls.Add(txtAuthor);
            Controls.Add(txtFolder);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            Name = "Form1";
            Text = "XVReplacerCreator";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem buildXVModFileToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem sToolStripMenuItem;
        private TextBox txtFolder;
        private Button btnFolder;
        private Label label2;
        private TextBox txtAuthor;
        private Label label3;
        private TextBox txtName;
    }
}