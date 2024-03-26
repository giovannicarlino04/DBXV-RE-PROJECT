namespace XVCMSTool
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
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            toolToolStripMenuItem = new ToolStripMenuItem();
            addEntryToolStripMenuItem = new ToolStripMenuItem();
            removeEntryToolStripMenuItem = new ToolStripMenuItem();
            comboBox1 = new ComboBox();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            textBox5 = new TextBox();
            textBox6 = new TextBox();
            textBox7 = new TextBox();
            label1 = new Label();
            label2 = new Label();
            numericUpDown1 = new NumericUpDown();
            label3 = new Label();
            textBox8 = new TextBox();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, toolToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(360, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, saveToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(103, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(103, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(100, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(103, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // toolToolStripMenuItem
            // 
            toolToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { addEntryToolStripMenuItem, removeEntryToolStripMenuItem });
            toolToolStripMenuItem.Name = "toolToolStripMenuItem";
            toolToolStripMenuItem.Size = new Size(46, 20);
            toolToolStripMenuItem.Text = "Tools";
            // 
            // addEntryToolStripMenuItem
            // 
            addEntryToolStripMenuItem.Name = "addEntryToolStripMenuItem";
            addEntryToolStripMenuItem.Size = new Size(302, 22);
            addEntryToolStripMenuItem.Text = "Add Entry (Copies Data from Current Entry)";
            addEntryToolStripMenuItem.Click += addEntryToolStripMenuItem_Click;
            // 
            // removeEntryToolStripMenuItem
            // 
            removeEntryToolStripMenuItem.Name = "removeEntryToolStripMenuItem";
            removeEntryToolStripMenuItem.Size = new Size(302, 22);
            removeEntryToolStripMenuItem.Text = "Remove Entry";
            removeEntryToolStripMenuItem.Click += removeEntryToolStripMenuItem_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(12, 48);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(107, 23);
            comboBox1.TabIndex = 1;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 90);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(336, 23);
            textBox1.TabIndex = 2;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(12, 119);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(336, 23);
            textBox2.TabIndex = 3;
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(12, 148);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(336, 23);
            textBox3.TabIndex = 4;
            textBox3.TextChanged += textBox3_TextChanged;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(12, 177);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(336, 23);
            textBox4.TabIndex = 5;
            textBox4.TextChanged += textBox4_TextChanged;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(12, 206);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(336, 23);
            textBox5.TabIndex = 6;
            textBox5.TextChanged += textBox5_TextChanged;
            // 
            // textBox6
            // 
            textBox6.Location = new Point(12, 235);
            textBox6.Name = "textBox6";
            textBox6.Size = new Size(336, 23);
            textBox6.TabIndex = 7;
            textBox6.TextChanged += textBox6_TextChanged;
            // 
            // textBox7
            // 
            textBox7.Location = new Point(12, 264);
            textBox7.Name = "textBox7";
            textBox7.Size = new Size(336, 23);
            textBox7.TabIndex = 8;
            textBox7.TextChanged += textBox7_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 30);
            label1.Name = "label1";
            label1.Size = new Size(61, 15);
            label1.TabIndex = 9;
            label1.Text = "Character:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(225, 30);
            label2.Name = "label2";
            label2.Size = new Size(75, 15);
            label2.TabIndex = 10;
            label2.Text = "Character ID:";
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(225, 48);
            numericUpDown1.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(120, 23);
            numericUpDown1.TabIndex = 12;
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(125, 30);
            label3.Name = "label3";
            label3.Size = new Size(70, 15);
            label3.TabIndex = 13;
            label3.Text = "ShortName:";
            // 
            // textBox8
            // 
            textBox8.Location = new Point(125, 47);
            textBox8.Name = "textBox8";
            textBox8.Size = new Size(94, 23);
            textBox8.TabIndex = 14;
            textBox8.TextChanged += textBox8_TextChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(360, 309);
            Controls.Add(textBox8);
            Controls.Add(label3);
            Controls.Add(numericUpDown1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBox7);
            Controls.Add(textBox6);
            Controls.Add(textBox5);
            Controls.Add(textBox4);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(comboBox1);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "XVCMSTool";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem toolToolStripMenuItem;
        private ToolStripMenuItem addEntryToolStripMenuItem;
        private ToolStripMenuItem removeEntryToolStripMenuItem;
        private ComboBox comboBox1;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private TextBox textBox5;
        private TextBox textBox6;
        private TextBox textBox7;
        private Label label1;
        private Label label2;
        private NumericUpDown numericUpDown1;
        private Label label3;
        private TextBox textBox8;
    }
}
