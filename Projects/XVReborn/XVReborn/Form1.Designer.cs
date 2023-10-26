using System;
using System.Windows.Forms;

namespace XVReborn
{
    partial class Form1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.installModToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compileScriptsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.clearInstallationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.Mods = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lvMods = new System.Windows.Forms.ListView();
            this.ch1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ch2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ch3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cbList = new System.Windows.Forms.ComboBox();
            this.cbLine = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtText = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtID = new System.Windows.Forms.TextBox();
            this.menuStrip3 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.charactersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.skillsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.supersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ultimatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.evasivesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.superInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ultimatesInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.evasivesInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtCSO1 = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.txtCSO2 = new System.Windows.Forms.TextBox();
            this.txtCSO4 = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.txtCSO3 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtCMS1 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtCMS2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCMS3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCMS4 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCMS5 = new System.Windows.Forms.TextBox();
            this.txtCMS6 = new System.Windows.Forms.TextBox();
            this.txtCMS7 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCSOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.editCMSFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editCSOFileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PSClstData = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label11 = new System.Windows.Forms.Label();
            this.PSCtxtName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.PSCtxtVal = new System.Windows.Forms.TextBox();
            this.menuStrip4 = new System.Windows.Forms.MenuStrip();
            this.editToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCSOFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.editCSOFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.UltLst1 = new System.Windows.Forms.ComboBox();
            this.SupLst1 = new System.Windows.Forms.ComboBox();
            this.label32 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.SupLst2 = new System.Windows.Forms.ComboBox();
            this.EvaLst = new System.Windows.Forms.ComboBox();
            this.label34 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.SupLst3 = new System.Windows.Forms.ComboBox();
            this.UltLst2 = new System.Windows.Forms.ComboBox();
            this.label36 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.SupLst4 = new System.Windows.Forms.ComboBox();
            this.label37 = new System.Windows.Forms.Label();
            this.menuStrip7 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem20 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem21 = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.editCUSFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.txtBLoop = new System.Windows.Forms.TextBox();
            this.txtBEnd = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txtHenshinEnd = new System.Windows.Forms.TextBox();
            this.txtKiCharge = new System.Windows.Forms.TextBox();
            this.txtBStart = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.cbAuraList = new System.Windows.Forms.ComboBox();
            this.txtkiMax = new System.Windows.Forms.TextBox();
            this.txtHenshinStart = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.cbAURChar = new System.Windows.Forms.ComboBox();
            this.chkInf = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtAURID = new System.Windows.Forms.TextBox();
            this.menuStrip5 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAURFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.addAuraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeAuraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.itemList = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMsgDesc = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.txtMsgName = new System.Windows.Forms.TextBox();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.txtModelID = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.txtEditValueb = new System.Windows.Forms.TextBox();
            this.txtEditNameb = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.txtSell = new System.Windows.Forms.TextBox();
            this.label39 = new System.Windows.Forms.Label();
            this.txtBuy = new System.Windows.Forms.TextBox();
            this.label40 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.txtDescID = new System.Windows.Forms.TextBox();
            this.label42 = new System.Windows.Forms.Label();
            this.txtNameID = new System.Windows.Forms.TextBox();
            this.cbStar = new System.Windows.Forms.ComboBox();
            this.label43 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lstvBasic = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.txtChance1 = new System.Windows.Forms.TextBox();
            this.label44 = new System.Windows.Forms.Label();
            this.txtEditValue1 = new System.Windows.Forms.TextBox();
            this.txtEditName1 = new System.Windows.Forms.TextBox();
            this.txtAVal1 = new System.Windows.Forms.TextBox();
            this.label45 = new System.Windows.Forms.Label();
            this.txtADelay1 = new System.Windows.Forms.TextBox();
            this.label46 = new System.Windows.Forms.Label();
            this.txtTimes1 = new System.Windows.Forms.TextBox();
            this.label47 = new System.Windows.Forms.Label();
            this.cbActive1 = new System.Windows.Forms.ComboBox();
            this.label48 = new System.Windows.Forms.Label();
            this.cbEffect1 = new System.Windows.Forms.ComboBox();
            this.label49 = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.lstvEffect1 = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage12 = new System.Windows.Forms.TabPage();
            this.txtChance2 = new System.Windows.Forms.TextBox();
            this.label51 = new System.Windows.Forms.Label();
            this.txtEditValue2 = new System.Windows.Forms.TextBox();
            this.txtEditName2 = new System.Windows.Forms.TextBox();
            this.txtAVal2 = new System.Windows.Forms.TextBox();
            this.label52 = new System.Windows.Forms.Label();
            this.txtADelay2 = new System.Windows.Forms.TextBox();
            this.label53 = new System.Windows.Forms.Label();
            this.txtTimes2 = new System.Windows.Forms.TextBox();
            this.label54 = new System.Windows.Forms.Label();
            this.cbActive2 = new System.Windows.Forms.ComboBox();
            this.label55 = new System.Windows.Forms.Label();
            this.cbEffect2 = new System.Windows.Forms.ComboBox();
            this.label56 = new System.Windows.Forms.Label();
            this.lstvEffect2 = new System.Windows.Forms.ListView();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip8 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.talismanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.skillToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.supersToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ultimatesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.evasivesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.accessoriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.battleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.costumesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.glovesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bottomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shoesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.materialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewZSoulToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.msgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.descriptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.nameToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.descriptionToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.menuStrip6 = new System.Windows.Forms.MenuStrip();
            this.editToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.editCSSFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ch = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.header = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.editPSCFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cbCharacter = new System.Windows.Forms.ComboBox();
            this.cbCostumes = new System.Windows.Forms.ComboBox();
            this.menuStrip1.SuspendLayout();
            this.Mods.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.menuStrip3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.menuStrip4.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.menuStrip7.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.menuStrip5.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.tabPage10.SuspendLayout();
            this.tabPage11.SuspendLayout();
            this.tabPage12.SuspendLayout();
            this.menuStrip8.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.menuStrip6.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(859, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.installModToolStripMenuItem,
            this.toolStripSeparator2,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // installModToolStripMenuItem
            // 
            this.installModToolStripMenuItem.Name = "installModToolStripMenuItem";
            this.installModToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.installModToolStripMenuItem.Text = "Install Mod";
            this.installModToolStripMenuItem.Click += new System.EventHandler(this.installmod);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(130, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.closeToolStripMenuItem.Text = "Exit";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compileScriptsToolStripMenuItem,
            this.toolStripSeparator1,
            this.clearInstallationToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // compileScriptsToolStripMenuItem
            // 
            this.compileScriptsToolStripMenuItem.Name = "compileScriptsToolStripMenuItem";
            this.compileScriptsToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.compileScriptsToolStripMenuItem.Text = "Compile Scripts";
            this.compileScriptsToolStripMenuItem.Click += new System.EventHandler(this.compileScriptsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(159, 6);
            // 
            // clearInstallationToolStripMenuItem
            // 
            this.clearInstallationToolStripMenuItem.Name = "clearInstallationToolStripMenuItem";
            this.clearInstallationToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.clearInstallationToolStripMenuItem.Text = "Clear Installation";
            this.clearInstallationToolStripMenuItem.Click += new System.EventHandler(this.clearInstallationToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Location = new System.Drawing.Point(0, 455);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Installed Mods: 0";
            // 
            // Mods
            // 
            this.Mods.Controls.Add(this.tabPage1);
            this.Mods.Controls.Add(this.tabPage3);
            this.Mods.Controls.Add(this.tabPage2);
            this.Mods.Controls.Add(this.tabPage4);
            this.Mods.Controls.Add(this.tabPage7);
            this.Mods.Controls.Add(this.tabPage5);
            this.Mods.Controls.Add(this.tabPage8);
            this.Mods.Controls.Add(this.tabPage6);
            this.Mods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Mods.Location = new System.Drawing.Point(0, 24);
            this.Mods.Name = "Mods";
            this.Mods.SelectedIndex = 0;
            this.Mods.Size = new System.Drawing.Size(859, 431);
            this.Mods.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.lvMods);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(851, 403);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Mods";
            // 
            // lvMods
            // 
            this.lvMods.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ch1,
            this.ch2,
            this.ch3});
            this.lvMods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvMods.HideSelection = false;
            this.lvMods.Location = new System.Drawing.Point(3, 3);
            this.lvMods.MultiSelect = false;
            this.lvMods.Name = "lvMods";
            this.lvMods.Size = new System.Drawing.Size(845, 397);
            this.lvMods.TabIndex = 4;
            this.lvMods.UseCompatibleStateImageBehavior = false;
            this.lvMods.View = System.Windows.Forms.View.Details;
            // 
            // ch1
            // 
            this.ch1.Text = "Name";
            this.ch1.Width = 116;
            // 
            // ch2
            // 
            this.ch2.Text = "Author";
            this.ch2.Width = 122;
            // 
            // ch3
            // 
            this.ch3.Text = "Type";
            this.ch3.Width = 591;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage3.Controls.Add(this.groupBox5);
            this.tabPage3.Controls.Add(this.menuStrip3);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(851, 403);
            this.tabPage3.TabIndex = 7;
            this.tabPage3.Text = "MSG";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cbList);
            this.groupBox5.Controls.Add(this.cbLine);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Controls.Add(this.txtText);
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.Controls.Add(this.txtName);
            this.groupBox5.Controls.Add(this.label15);
            this.groupBox5.Controls.Add(this.txtID);
            this.groupBox5.Location = new System.Drawing.Point(20, 71);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(277, 250);
            this.groupBox5.TabIndex = 18;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "MSG";
            // 
            // cbList
            // 
            this.cbList.FormattingEnabled = true;
            this.cbList.Location = new System.Drawing.Point(15, 33);
            this.cbList.Name = "cbList";
            this.cbList.Size = new System.Drawing.Size(162, 23);
            this.cbList.TabIndex = 9;
            this.cbList.SelectedIndexChanged += new System.EventHandler(this.cbList_SelectedIndexChanged);
            // 
            // cbLine
            // 
            this.cbLine.FormattingEnabled = true;
            this.cbLine.Location = new System.Drawing.Point(182, 33);
            this.cbLine.Name = "cbLine";
            this.cbLine.Size = new System.Drawing.Size(78, 23);
            this.cbLine.TabIndex = 10;
            this.cbLine.SelectedIndexChanged += new System.EventHandler(this.cbLine_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(13, 103);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(28, 15);
            this.label13.TabIndex = 16;
            this.label13.Text = "Text";
            // 
            // txtText
            // 
            this.txtText.Location = new System.Drawing.Point(15, 121);
            this.txtText.Multiline = true;
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(243, 115);
            this.txtText.TabIndex = 11;
            this.txtText.TextChanged += new System.EventHandler(this.txtText_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(179, 59);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(19, 15);
            this.label14.TabIndex = 15;
            this.label14.Text = "ID";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(15, 74);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(162, 21);
            this.txtName.TabIndex = 12;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(15, 56);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(41, 15);
            this.label15.TabIndex = 14;
            this.label15.Text = "Name";
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(182, 74);
            this.txtID.Name = "txtID";
            this.txtID.ReadOnly = true;
            this.txtID.Size = new System.Drawing.Size(78, 21);
            this.txtID.TabIndex = 13;
            // 
            // menuStrip3
            // 
            this.menuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem4,
            this.openToolStripMenuItem});
            this.menuStrip3.Location = new System.Drawing.Point(3, 3);
            this.menuStrip3.Name = "menuStrip3";
            this.menuStrip3.Padding = new System.Windows.Forms.Padding(7, 3, 0, 3);
            this.menuStrip3.Size = new System.Drawing.Size(845, 25);
            this.menuStrip3.TabIndex = 17;
            this.menuStrip3.Text = "menuStrip3";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(37, 19);
            this.toolStripMenuItem2.Text = "File";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(126, 22);
            this.toolStripMenuItem3.Text = "Save MSG";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem7,
            this.removeToolStripMenuItem});
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(39, 19);
            this.toolStripMenuItem4.Text = "Edit";
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(117, 22);
            this.toolStripMenuItem7.Text = "Add";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.toolStripMenuItem7_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.charactersToolStripMenuItem,
            this.skillsToolStripMenuItem});
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(48, 19);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // charactersToolStripMenuItem
            // 
            this.charactersToolStripMenuItem.Name = "charactersToolStripMenuItem";
            this.charactersToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.charactersToolStripMenuItem.Text = "Characters";
            this.charactersToolStripMenuItem.Click += new System.EventHandler(this.charactersToolStripMenuItem_Click);
            // 
            // skillsToolStripMenuItem
            // 
            this.skillsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.supersToolStripMenuItem,
            this.ultimatesToolStripMenuItem,
            this.evasivesToolStripMenuItem,
            this.superInfoToolStripMenuItem,
            this.ultimatesInfoToolStripMenuItem,
            this.evasivesInfoToolStripMenuItem});
            this.skillsToolStripMenuItem.Name = "skillsToolStripMenuItem";
            this.skillsToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.skillsToolStripMenuItem.Text = "Skills";
            // 
            // supersToolStripMenuItem
            // 
            this.supersToolStripMenuItem.Name = "supersToolStripMenuItem";
            this.supersToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.supersToolStripMenuItem.Text = "Supers";
            this.supersToolStripMenuItem.Click += new System.EventHandler(this.supersToolStripMenuItem_Click);
            // 
            // ultimatesToolStripMenuItem
            // 
            this.ultimatesToolStripMenuItem.Name = "ultimatesToolStripMenuItem";
            this.ultimatesToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.ultimatesToolStripMenuItem.Text = "Ultimates";
            this.ultimatesToolStripMenuItem.Click += new System.EventHandler(this.ultimatesToolStripMenuItem_Click);
            // 
            // evasivesToolStripMenuItem
            // 
            this.evasivesToolStripMenuItem.Name = "evasivesToolStripMenuItem";
            this.evasivesToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.evasivesToolStripMenuItem.Text = "Evasives";
            this.evasivesToolStripMenuItem.Click += new System.EventHandler(this.evasivesToolStripMenuItem_Click);
            // 
            // superInfoToolStripMenuItem
            // 
            this.superInfoToolStripMenuItem.Name = "superInfoToolStripMenuItem";
            this.superInfoToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.superInfoToolStripMenuItem.Text = "Super Info";
            this.superInfoToolStripMenuItem.Click += new System.EventHandler(this.superInfoToolStripMenuItem_Click);
            // 
            // ultimatesInfoToolStripMenuItem
            // 
            this.ultimatesInfoToolStripMenuItem.Name = "ultimatesInfoToolStripMenuItem";
            this.ultimatesInfoToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.ultimatesInfoToolStripMenuItem.Text = "Ultimates Info";
            this.ultimatesInfoToolStripMenuItem.Click += new System.EventHandler(this.ultimatesInfoToolStripMenuItem_Click);
            // 
            // evasivesInfoToolStripMenuItem
            // 
            this.evasivesInfoToolStripMenuItem.Name = "evasivesInfoToolStripMenuItem";
            this.evasivesInfoToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.evasivesInfoToolStripMenuItem.Text = "Evasives Info";
            this.evasivesInfoToolStripMenuItem.Click += new System.EventHandler(this.evasivesInfoToolStripMenuItem_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.menuStrip2);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(851, 403);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "CMS/CSO";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtCSO1);
            this.groupBox4.Controls.Add(this.label29);
            this.groupBox4.Controls.Add(this.label26);
            this.groupBox4.Controls.Add(this.txtCSO2);
            this.groupBox4.Controls.Add(this.txtCSO4);
            this.groupBox4.Controls.Add(this.label28);
            this.groupBox4.Controls.Add(this.label27);
            this.groupBox4.Controls.Add(this.txtCSO3);
            this.groupBox4.Location = new System.Drawing.Point(522, 65);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(237, 161);
            this.groupBox4.TabIndex = 53;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "CSO";
            // 
            // txtCSO1
            // 
            this.txtCSO1.Location = new System.Drawing.Point(41, 26);
            this.txtCSO1.Margin = new System.Windows.Forms.Padding(4);
            this.txtCSO1.Name = "txtCSO1";
            this.txtCSO1.Size = new System.Drawing.Size(159, 21);
            this.txtCSO1.TabIndex = 42;
            this.txtCSO1.TextChanged += new System.EventHandler(this.txtCSO1_TextChanged);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(13, 28);
            this.label29.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(14, 15);
            this.label29.TabIndex = 43;
            this.label29.Text = "1";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(13, 118);
            this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(14, 15);
            this.label26.TabIndex = 49;
            this.label26.Text = "4";
            // 
            // txtCSO2
            // 
            this.txtCSO2.Location = new System.Drawing.Point(41, 56);
            this.txtCSO2.Margin = new System.Windows.Forms.Padding(4);
            this.txtCSO2.Name = "txtCSO2";
            this.txtCSO2.Size = new System.Drawing.Size(159, 21);
            this.txtCSO2.TabIndex = 44;
            this.txtCSO2.TextChanged += new System.EventHandler(this.txtCSO2_TextChanged);
            // 
            // txtCSO4
            // 
            this.txtCSO4.Location = new System.Drawing.Point(41, 116);
            this.txtCSO4.Margin = new System.Windows.Forms.Padding(4);
            this.txtCSO4.Name = "txtCSO4";
            this.txtCSO4.Size = new System.Drawing.Size(159, 21);
            this.txtCSO4.TabIndex = 48;
            this.txtCSO4.TextChanged += new System.EventHandler(this.txtCSO4_TextChanged);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(13, 58);
            this.label28.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(14, 15);
            this.label28.TabIndex = 45;
            this.label28.Text = "2";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(13, 88);
            this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(14, 15);
            this.label27.TabIndex = 47;
            this.label27.Text = "3";
            // 
            // txtCSO3
            // 
            this.txtCSO3.Location = new System.Drawing.Point(41, 86);
            this.txtCSO3.Margin = new System.Windows.Forms.Padding(4);
            this.txtCSO3.Name = "txtCSO3";
            this.txtCSO3.Size = new System.Drawing.Size(159, 21);
            this.txtCSO3.TabIndex = 46;
            this.txtCSO3.TextChanged += new System.EventHandler(this.txtCSO3_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txtCMS1);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtCMS2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtCMS3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtCMS4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtCMS5);
            this.groupBox2.Controls.Add(this.txtCMS6);
            this.groupBox2.Controls.Add(this.txtCMS7);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(35, 65);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(301, 236);
            this.groupBox2.TabIndex = 52;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "CMS";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 176);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 15);
            this.label6.TabIndex = 31;
            this.label6.Text = "BCM";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 28);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 15);
            this.label10.TabIndex = 21;
            this.label10.Text = "BCS";
            // 
            // txtCMS1
            // 
            this.txtCMS1.Location = new System.Drawing.Point(122, 22);
            this.txtCMS1.Margin = new System.Windows.Forms.Padding(4);
            this.txtCMS1.Name = "txtCMS1";
            this.txtCMS1.Size = new System.Drawing.Size(169, 21);
            this.txtCMS1.TabIndex = 22;
            this.txtCMS1.TextChanged += new System.EventHandler(this.txtCMS1_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 58);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 15);
            this.label9.TabIndex = 23;
            this.label9.Text = "EAN";
            // 
            // txtCMS2
            // 
            this.txtCMS2.Location = new System.Drawing.Point(122, 52);
            this.txtCMS2.Margin = new System.Windows.Forms.Padding(4);
            this.txtCMS2.Name = "txtCMS2";
            this.txtCMS2.Size = new System.Drawing.Size(169, 21);
            this.txtCMS2.TabIndex = 24;
            this.txtCMS2.TextChanged += new System.EventHandler(this.txtCMS2_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 88);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 15);
            this.label3.TabIndex = 25;
            this.label3.Text = "FCE_EAN";
            // 
            // txtCMS3
            // 
            this.txtCMS3.Location = new System.Drawing.Point(122, 82);
            this.txtCMS3.Margin = new System.Windows.Forms.Padding(4);
            this.txtCMS3.Name = "txtCMS3";
            this.txtCMS3.Size = new System.Drawing.Size(169, 21);
            this.txtCMS3.TabIndex = 26;
            this.txtCMS3.TextChanged += new System.EventHandler(this.txtCMS3_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 118);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 15);
            this.label4.TabIndex = 27;
            this.label4.Text = "CAM_EAN";
            // 
            // txtCMS4
            // 
            this.txtCMS4.Location = new System.Drawing.Point(122, 112);
            this.txtCMS4.Margin = new System.Windows.Forms.Padding(4);
            this.txtCMS4.Name = "txtCMS4";
            this.txtCMS4.Size = new System.Drawing.Size(169, 21);
            this.txtCMS4.TabIndex = 28;
            this.txtCMS4.TextChanged += new System.EventHandler(this.txtCMS4_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 146);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 15);
            this.label5.TabIndex = 29;
            this.label5.Text = "BAC";
            // 
            // txtCMS5
            // 
            this.txtCMS5.Location = new System.Drawing.Point(122, 142);
            this.txtCMS5.Margin = new System.Windows.Forms.Padding(4);
            this.txtCMS5.Name = "txtCMS5";
            this.txtCMS5.Size = new System.Drawing.Size(169, 21);
            this.txtCMS5.TabIndex = 30;
            this.txtCMS5.TextChanged += new System.EventHandler(this.txtCMS5_TextChanged);
            // 
            // txtCMS6
            // 
            this.txtCMS6.Location = new System.Drawing.Point(122, 172);
            this.txtCMS6.Margin = new System.Windows.Forms.Padding(4);
            this.txtCMS6.Name = "txtCMS6";
            this.txtCMS6.Size = new System.Drawing.Size(169, 21);
            this.txtCMS6.TabIndex = 32;
            this.txtCMS6.TextChanged += new System.EventHandler(this.txtCMS6_TextChanged);
            // 
            // txtCMS7
            // 
            this.txtCMS7.Location = new System.Drawing.Point(122, 202);
            this.txtCMS7.Margin = new System.Windows.Forms.Padding(4);
            this.txtCMS7.Name = "txtCMS7";
            this.txtCMS7.Size = new System.Drawing.Size(169, 21);
            this.txtCMS7.TabIndex = 34;
            this.txtCMS7.TextChanged += new System.EventHandler(this.txtCMS7_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 206);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(25, 15);
            this.label7.TabIndex = 33;
            this.label7.Text = "BAI";
            // 
            // menuStrip2
            // 
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.editToolStripMenuItem2});
            this.menuStrip2.Location = new System.Drawing.Point(3, 3);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Padding = new System.Windows.Forms.Padding(7, 3, 0, 3);
            this.menuStrip2.Size = new System.Drawing.Size(845, 25);
            this.menuStrip2.TabIndex = 41;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.saveCSOToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 19);
            this.toolStripMenuItem1.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.saveToolStripMenuItem.Text = "Save CMS";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveCMSToolStripMenuItem_Click);
            // 
            // saveCSOToolStripMenuItem
            // 
            this.saveCSOToolStripMenuItem.Name = "saveCSOToolStripMenuItem";
            this.saveCSOToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.saveCSOToolStripMenuItem.Text = "Save CSO";
            this.saveCSOToolStripMenuItem.Click += new System.EventHandler(this.button1_Click);
            // 
            // editToolStripMenuItem2
            // 
            this.editToolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editCMSFileToolStripMenuItem,
            this.editCSOFileToolStripMenuItem1});
            this.editToolStripMenuItem2.Name = "editToolStripMenuItem2";
            this.editToolStripMenuItem2.Size = new System.Drawing.Size(39, 19);
            this.editToolStripMenuItem2.Text = "Edit";
            // 
            // editCMSFileToolStripMenuItem
            // 
            this.editCMSFileToolStripMenuItem.Name = "editCMSFileToolStripMenuItem";
            this.editCMSFileToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.editCMSFileToolStripMenuItem.Text = "Edit CMS File";
            this.editCMSFileToolStripMenuItem.Click += new System.EventHandler(this.editCMSFileToolStripMenuItem_Click);
            // 
            // editCSOFileToolStripMenuItem1
            // 
            this.editCSOFileToolStripMenuItem1.Name = "editCSOFileToolStripMenuItem1";
            this.editCSOFileToolStripMenuItem1.Size = new System.Drawing.Size(143, 22);
            this.editCSOFileToolStripMenuItem1.Text = "Edit CSO File";
            this.editCSOFileToolStripMenuItem1.Click += new System.EventHandler(this.editCSOFileToolStripMenuItem_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage4.Controls.Add(this.groupBox1);
            this.tabPage4.Controls.Add(this.menuStrip4);
            this.tabPage4.Location = new System.Drawing.Point(4, 24);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(851, 403);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "PSC";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.PSClstData);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.PSCtxtName);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.PSCtxtVal);
            this.groupBox1.Location = new System.Drawing.Point(17, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(388, 348);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PSC";
            // 
            // PSClstData
            // 
            this.PSClstData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.PSClstData.HideSelection = false;
            this.PSClstData.Location = new System.Drawing.Point(6, 70);
            this.PSClstData.Margin = new System.Windows.Forms.Padding(4);
            this.PSClstData.MultiSelect = false;
            this.PSClstData.Name = "PSClstData";
            this.PSClstData.Size = new System.Drawing.Size(367, 263);
            this.PSClstData.TabIndex = 14;
            this.PSClstData.TileSize = new System.Drawing.Size(200, 30);
            this.PSClstData.UseCompatibleStateImageBehavior = false;
            this.PSClstData.View = System.Windows.Forms.View.Tile;
            this.PSClstData.SelectedIndexChanged += new System.EventHandler(this.lstPSC_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(259, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(26, 15);
            this.label11.TabIndex = 16;
            this.label11.Text = "Val:";
            // 
            // PSCtxtName
            // 
            this.PSCtxtName.Location = new System.Drawing.Point(6, 41);
            this.PSCtxtName.Margin = new System.Windows.Forms.Padding(4);
            this.PSCtxtName.Name = "PSCtxtName";
            this.PSCtxtName.ReadOnly = true;
            this.PSCtxtName.Size = new System.Drawing.Size(249, 21);
            this.PSCtxtName.TabIndex = 12;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 15);
            this.label8.TabIndex = 15;
            this.label8.Text = "Name:";
            // 
            // PSCtxtVal
            // 
            this.PSCtxtVal.Location = new System.Drawing.Point(262, 41);
            this.PSCtxtVal.Margin = new System.Windows.Forms.Padding(4);
            this.PSCtxtVal.Name = "PSCtxtVal";
            this.PSCtxtVal.Size = new System.Drawing.Size(112, 21);
            this.PSCtxtVal.TabIndex = 13;
            this.PSCtxtVal.TextChanged += new System.EventHandler(this.txtPSCVal_TextChanged);
            // 
            // menuStrip4
            // 
            this.menuStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem1,
            this.editToolStripMenuItem3});
            this.menuStrip4.Location = new System.Drawing.Point(3, 3);
            this.menuStrip4.Name = "menuStrip4";
            this.menuStrip4.Padding = new System.Windows.Forms.Padding(7, 3, 0, 3);
            this.menuStrip4.Size = new System.Drawing.Size(845, 25);
            this.menuStrip4.TabIndex = 2;
            this.menuStrip4.Text = "menuStrip4";
            // 
            // editToolStripMenuItem1
            // 
            this.editToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveCSOFileToolStripMenuItem});
            this.editToolStripMenuItem1.Name = "editToolStripMenuItem1";
            this.editToolStripMenuItem1.Size = new System.Drawing.Size(37, 19);
            this.editToolStripMenuItem1.Text = "File";
            // 
            // saveCSOFileToolStripMenuItem
            // 
            this.saveCSOFileToolStripMenuItem.Name = "saveCSOFileToolStripMenuItem";
            this.saveCSOFileToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.saveCSOFileToolStripMenuItem.Text = "Save PSC File";
            this.saveCSOFileToolStripMenuItem.Click += new System.EventHandler(this.button4_Click);
            // 
            // editToolStripMenuItem3
            // 
            this.editToolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editCSOFileToolStripMenuItem});
            this.editToolStripMenuItem3.Name = "editToolStripMenuItem3";
            this.editToolStripMenuItem3.Size = new System.Drawing.Size(39, 19);
            this.editToolStripMenuItem3.Text = "Edit";
            // 
            // editCSOFileToolStripMenuItem
            // 
            this.editCSOFileToolStripMenuItem.Name = "editCSOFileToolStripMenuItem";
            this.editCSOFileToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.editCSOFileToolStripMenuItem.Text = "Edit PSC File";
            this.editCSOFileToolStripMenuItem.Click += new System.EventHandler(this.editPSCFileToolStripMenuItem_Click);
            // 
            // tabPage7
            // 
            this.tabPage7.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage7.Controls.Add(this.groupBox3);
            this.tabPage7.Controls.Add(this.menuStrip7);
            this.tabPage7.Location = new System.Drawing.Point(4, 24);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(851, 403);
            this.tabPage7.TabIndex = 6;
            this.tabPage7.Text = "CUS";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.UltLst1);
            this.groupBox3.Controls.Add(this.SupLst1);
            this.groupBox3.Controls.Add(this.label32);
            this.groupBox3.Controls.Add(this.label33);
            this.groupBox3.Controls.Add(this.SupLst2);
            this.groupBox3.Controls.Add(this.EvaLst);
            this.groupBox3.Controls.Add(this.label34);
            this.groupBox3.Controls.Add(this.label35);
            this.groupBox3.Controls.Add(this.SupLst3);
            this.groupBox3.Controls.Add(this.UltLst2);
            this.groupBox3.Controls.Add(this.label36);
            this.groupBox3.Controls.Add(this.label38);
            this.groupBox3.Controls.Add(this.SupLst4);
            this.groupBox3.Controls.Add(this.label37);
            this.groupBox3.Location = new System.Drawing.Point(24, 64);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(461, 266);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "CUS";
            // 
            // UltLst1
            // 
            this.UltLst1.FormattingEnabled = true;
            this.UltLst1.Location = new System.Drawing.Point(98, 153);
            this.UltLst1.Margin = new System.Windows.Forms.Padding(4);
            this.UltLst1.Name = "UltLst1";
            this.UltLst1.Size = new System.Drawing.Size(344, 23);
            this.UltLst1.TabIndex = 9;
            this.UltLst1.SelectedIndexChanged += new System.EventHandler(this.cbUlt1_SelectedIndexChanged);
            // 
            // SupLst1
            // 
            this.SupLst1.FormattingEnabled = true;
            this.SupLst1.Location = new System.Drawing.Point(98, 26);
            this.SupLst1.Margin = new System.Windows.Forms.Padding(4);
            this.SupLst1.Name = "SupLst1";
            this.SupLst1.Size = new System.Drawing.Size(344, 23);
            this.SupLst1.TabIndex = 1;
            this.SupLst1.SelectedIndexChanged += new System.EventHandler(this.cbSuper1_SelectedIndexChanged);
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(20, 29);
            this.label32.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(57, 15);
            this.label32.TabIndex = 2;
            this.label32.Text = "Super #1";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(20, 218);
            this.label33.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(66, 15);
            this.label33.TabIndex = 14;
            this.label33.Text = "Evasive #1";
            // 
            // SupLst2
            // 
            this.SupLst2.FormattingEnabled = true;
            this.SupLst2.Location = new System.Drawing.Point(98, 56);
            this.SupLst2.Margin = new System.Windows.Forms.Padding(4);
            this.SupLst2.Name = "SupLst2";
            this.SupLst2.Size = new System.Drawing.Size(344, 23);
            this.SupLst2.TabIndex = 3;
            this.SupLst2.SelectedIndexChanged += new System.EventHandler(this.cbSuper2_SelectedIndexChanged);
            // 
            // EvaLst
            // 
            this.EvaLst.FormattingEnabled = true;
            this.EvaLst.Location = new System.Drawing.Point(98, 214);
            this.EvaLst.Margin = new System.Windows.Forms.Padding(4);
            this.EvaLst.Name = "EvaLst";
            this.EvaLst.Size = new System.Drawing.Size(344, 23);
            this.EvaLst.TabIndex = 13;
            this.EvaLst.SelectedIndexChanged += new System.EventHandler(this.cbEva_SelectedIndexChanged);
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(20, 60);
            this.label34.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(57, 15);
            this.label34.TabIndex = 4;
            this.label34.Text = "Super #2";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(20, 188);
            this.label35.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(70, 15);
            this.label35.TabIndex = 12;
            this.label35.Text = "Ultimate #2";
            // 
            // SupLst3
            // 
            this.SupLst3.FormattingEnabled = true;
            this.SupLst3.Location = new System.Drawing.Point(98, 87);
            this.SupLst3.Margin = new System.Windows.Forms.Padding(4);
            this.SupLst3.Name = "SupLst3";
            this.SupLst3.Size = new System.Drawing.Size(344, 23);
            this.SupLst3.TabIndex = 5;
            this.SupLst3.SelectedIndexChanged += new System.EventHandler(this.cbSuper3_SelectedIndexChanged);
            // 
            // UltLst2
            // 
            this.UltLst2.FormattingEnabled = true;
            this.UltLst2.Location = new System.Drawing.Point(98, 184);
            this.UltLst2.Margin = new System.Windows.Forms.Padding(4);
            this.UltLst2.Name = "UltLst2";
            this.UltLst2.Size = new System.Drawing.Size(344, 23);
            this.UltLst2.TabIndex = 11;
            this.UltLst2.SelectedIndexChanged += new System.EventHandler(this.cbUlt2_SelectedIndexChanged);
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(20, 91);
            this.label36.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(57, 15);
            this.label36.TabIndex = 6;
            this.label36.Text = "Super #3";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(20, 156);
            this.label38.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(70, 15);
            this.label38.TabIndex = 10;
            this.label38.Text = "Ultimate #1";
            // 
            // SupLst4
            // 
            this.SupLst4.FormattingEnabled = true;
            this.SupLst4.Location = new System.Drawing.Point(98, 119);
            this.SupLst4.Margin = new System.Windows.Forms.Padding(4);
            this.SupLst4.Name = "SupLst4";
            this.SupLst4.Size = new System.Drawing.Size(344, 23);
            this.SupLst4.TabIndex = 7;
            this.SupLst4.SelectedIndexChanged += new System.EventHandler(this.cbSuper4_SelectedIndexChanged);
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(20, 123);
            this.label37.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(57, 15);
            this.label37.TabIndex = 8;
            this.label37.Text = "Super #4";
            // 
            // menuStrip7
            // 
            this.menuStrip7.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem20,
            this.editToolStripMenuItem6});
            this.menuStrip7.Location = new System.Drawing.Point(0, 0);
            this.menuStrip7.Name = "menuStrip7";
            this.menuStrip7.Padding = new System.Windows.Forms.Padding(7, 3, 0, 3);
            this.menuStrip7.Size = new System.Drawing.Size(851, 25);
            this.menuStrip7.TabIndex = 1;
            this.menuStrip7.Text = "menuStrip7";
            // 
            // toolStripMenuItem20
            // 
            this.toolStripMenuItem20.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem21});
            this.toolStripMenuItem20.Name = "toolStripMenuItem20";
            this.toolStripMenuItem20.Size = new System.Drawing.Size(37, 19);
            this.toolStripMenuItem20.Text = "File";
            // 
            // toolStripMenuItem21
            // 
            this.toolStripMenuItem21.Name = "toolStripMenuItem21";
            this.toolStripMenuItem21.Size = new System.Drawing.Size(123, 22);
            this.toolStripMenuItem21.Text = "Save CUS";
            this.toolStripMenuItem21.Click += new System.EventHandler(this.button3_Click);
            // 
            // editToolStripMenuItem6
            // 
            this.editToolStripMenuItem6.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editCUSFileToolStripMenuItem});
            this.editToolStripMenuItem6.Name = "editToolStripMenuItem6";
            this.editToolStripMenuItem6.Size = new System.Drawing.Size(39, 19);
            this.editToolStripMenuItem6.Text = "Edit";
            // 
            // editCUSFileToolStripMenuItem
            // 
            this.editCUSFileToolStripMenuItem.Name = "editCUSFileToolStripMenuItem";
            this.editCUSFileToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.editCUSFileToolStripMenuItem.Text = "Edit CUS File";
            this.editCUSFileToolStripMenuItem.Click += new System.EventHandler(this.editCUSFileToolStripMenuItem_Click);
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage5.Controls.Add(this.groupBox6);
            this.tabPage5.Controls.Add(this.menuStrip5);
            this.tabPage5.Location = new System.Drawing.Point(4, 24);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(851, 403);
            this.tabPage5.TabIndex = 8;
            this.tabPage5.Text = "AUR";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.groupBox8);
            this.groupBox6.Controls.Add(this.groupBox7);
            this.groupBox6.Location = new System.Drawing.Point(7, 27);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(827, 369);
            this.groupBox6.TabIndex = 8;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "AUR";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.txtBLoop);
            this.groupBox8.Controls.Add(this.txtBEnd);
            this.groupBox8.Controls.Add(this.label17);
            this.groupBox8.Controls.Add(this.label18);
            this.groupBox8.Controls.Add(this.txtHenshinEnd);
            this.groupBox8.Controls.Add(this.txtKiCharge);
            this.groupBox8.Controls.Add(this.txtBStart);
            this.groupBox8.Controls.Add(this.label19);
            this.groupBox8.Controls.Add(this.label20);
            this.groupBox8.Controls.Add(this.label21);
            this.groupBox8.Controls.Add(this.cbAuraList);
            this.groupBox8.Controls.Add(this.txtkiMax);
            this.groupBox8.Controls.Add(this.txtHenshinStart);
            this.groupBox8.Controls.Add(this.label22);
            this.groupBox8.Controls.Add(this.label23);
            this.groupBox8.Controls.Add(this.label24);
            this.groupBox8.Location = new System.Drawing.Point(420, 21);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(398, 334);
            this.groupBox8.TabIndex = 10;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Aura Editor";
            // 
            // txtBLoop
            // 
            this.txtBLoop.Location = new System.Drawing.Point(244, 91);
            this.txtBLoop.Name = "txtBLoop";
            this.txtBLoop.Size = new System.Drawing.Size(126, 21);
            this.txtBLoop.TabIndex = 23;
            this.txtBLoop.TextChanged += new System.EventHandler(this.txtBLoop_TextChanged);
            // 
            // txtBEnd
            // 
            this.txtBEnd.Location = new System.Drawing.Point(28, 137);
            this.txtBEnd.Name = "txtBEnd";
            this.txtBEnd.Size = new System.Drawing.Size(126, 21);
            this.txtBEnd.TabIndex = 25;
            this.txtBEnd.TextChanged += new System.EventHandler(this.txtBEnd_TextChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Location = new System.Drawing.Point(270, 119);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(62, 15);
            this.label17.TabIndex = 26;
            this.label17.Text = "Ki Charge";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Location = new System.Drawing.Point(52, 119);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(64, 15);
            this.label18.TabIndex = 24;
            this.label18.Text = "Boost End";
            // 
            // txtHenshinEnd
            // 
            this.txtHenshinEnd.Location = new System.Drawing.Point(141, 236);
            this.txtHenshinEnd.Name = "txtHenshinEnd";
            this.txtHenshinEnd.Size = new System.Drawing.Size(126, 21);
            this.txtHenshinEnd.TabIndex = 33;
            this.txtHenshinEnd.TextChanged += new System.EventHandler(this.txtHenshinEnd_TextChanged);
            // 
            // txtKiCharge
            // 
            this.txtKiCharge.Location = new System.Drawing.Point(244, 137);
            this.txtKiCharge.Name = "txtKiCharge";
            this.txtKiCharge.Size = new System.Drawing.Size(126, 21);
            this.txtKiCharge.TabIndex = 27;
            this.txtKiCharge.TextChanged += new System.EventHandler(this.txtKiCharge_TextChanged);
            // 
            // txtBStart
            // 
            this.txtBStart.Location = new System.Drawing.Point(28, 91);
            this.txtBStart.Name = "txtBStart";
            this.txtBStart.Size = new System.Drawing.Size(126, 21);
            this.txtBStart.TabIndex = 21;
            this.txtBStart.TextChanged += new System.EventHandler(this.txtBStart_TextChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.Location = new System.Drawing.Point(64, 168);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(42, 15);
            this.label19.TabIndex = 28;
            this.label19.Text = "Ki Max";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BackColor = System.Drawing.Color.Transparent;
            this.label20.Location = new System.Drawing.Point(158, 219);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(89, 15);
            this.label20.TabIndex = 32;
            this.label20.Text = "Transform End";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.BackColor = System.Drawing.Color.Transparent;
            this.label21.Location = new System.Drawing.Point(270, 73);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(70, 15);
            this.label21.TabIndex = 22;
            this.label21.Text = "Boost Loop";
            // 
            // cbAuraList
            // 
            this.cbAuraList.FormattingEnabled = true;
            this.cbAuraList.Location = new System.Drawing.Point(28, 47);
            this.cbAuraList.Name = "cbAuraList";
            this.cbAuraList.Size = new System.Drawing.Size(126, 23);
            this.cbAuraList.TabIndex = 18;
            this.cbAuraList.SelectedIndexChanged += new System.EventHandler(this.cbAuraList_SelectedIndexChanged);
            // 
            // txtkiMax
            // 
            this.txtkiMax.Location = new System.Drawing.Point(28, 185);
            this.txtkiMax.Name = "txtkiMax";
            this.txtkiMax.Size = new System.Drawing.Size(126, 21);
            this.txtkiMax.TabIndex = 29;
            this.txtkiMax.TextChanged += new System.EventHandler(this.txtkiMax_TextChanged);
            // 
            // txtHenshinStart
            // 
            this.txtHenshinStart.Location = new System.Drawing.Point(244, 185);
            this.txtHenshinStart.Name = "txtHenshinStart";
            this.txtHenshinStart.Size = new System.Drawing.Size(126, 21);
            this.txtHenshinStart.TabIndex = 31;
            this.txtHenshinStart.TextChanged += new System.EventHandler(this.txtHenshinStart_TextChanged);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.Location = new System.Drawing.Point(52, 73);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(67, 15);
            this.label22.TabIndex = 20;
            this.label22.Text = "Boost Start";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Location = new System.Drawing.Point(25, 29);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(32, 15);
            this.label23.TabIndex = 19;
            this.label23.Text = "Aura";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.BackColor = System.Drawing.Color.Transparent;
            this.label24.Location = new System.Drawing.Point(258, 168);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(92, 15);
            this.label24.TabIndex = 30;
            this.label24.Text = "Transform Start";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.cbAURChar);
            this.groupBox7.Controls.Add(this.chkInf);
            this.groupBox7.Controls.Add(this.label16);
            this.groupBox7.Controls.Add(this.label12);
            this.groupBox7.Controls.Add(this.txtAURID);
            this.groupBox7.Location = new System.Drawing.Point(15, 21);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(399, 334);
            this.groupBox7.TabIndex = 9;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Character Auras";
            // 
            // cbAURChar
            // 
            this.cbAURChar.FormattingEnabled = true;
            this.cbAURChar.Location = new System.Drawing.Point(97, 44);
            this.cbAURChar.Name = "cbAURChar";
            this.cbAURChar.Size = new System.Drawing.Size(256, 23);
            this.cbAURChar.TabIndex = 6;
            this.cbAURChar.SelectedIndexChanged += new System.EventHandler(this.cbChar_SelectedIndexChanged);
            // 
            // chkInf
            // 
            this.chkInf.AutoSize = true;
            this.chkInf.Location = new System.Drawing.Point(238, 91);
            this.chkInf.Name = "chkInf";
            this.chkInf.Size = new System.Drawing.Size(89, 19);
            this.chkInf.TabIndex = 5;
            this.chkInf.Text = "Infinite Flag";
            this.chkInf.UseVisualStyleBackColor = true;
            this.chkInf.CheckedChanged += new System.EventHandler(this.chkInf_CheckedChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(24, 47);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(64, 15);
            this.label16.TabIndex = 7;
            this.label16.Text = "Character:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(39, 91);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(50, 15);
            this.label12.TabIndex = 4;
            this.label12.Text = "Aura ID:";
            // 
            // txtAURID
            // 
            this.txtAURID.Location = new System.Drawing.Point(97, 88);
            this.txtAURID.Name = "txtAURID";
            this.txtAURID.Size = new System.Drawing.Size(126, 21);
            this.txtAURID.TabIndex = 3;
            this.txtAURID.TextChanged += new System.EventHandler(this.txtAURID_TextChanged);
            // 
            // menuStrip5
            // 
            this.menuStrip5.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.toolStripMenuItem10});
            this.menuStrip5.Location = new System.Drawing.Point(3, 3);
            this.menuStrip5.Name = "menuStrip5";
            this.menuStrip5.Padding = new System.Windows.Forms.Padding(7, 3, 0, 3);
            this.menuStrip5.Size = new System.Drawing.Size(845, 25);
            this.menuStrip5.TabIndex = 2;
            this.menuStrip5.Text = "menuStrip5";
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAURFileToolStripMenuItem});
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(37, 19);
            this.fileToolStripMenuItem1.Text = "File";
            // 
            // saveAURFileToolStripMenuItem
            // 
            this.saveAURFileToolStripMenuItem.Name = "saveAURFileToolStripMenuItem";
            this.saveAURFileToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.saveAURFileToolStripMenuItem.Text = "Save AUR";
            this.saveAURFileToolStripMenuItem.Click += new System.EventHandler(this.saveAURFileToolStripMenuItem_Click);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addAuraToolStripMenuItem,
            this.removeAuraToolStripMenuItem,
            this.toolStripSeparator3,
            this.toolStripMenuItem11});
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(39, 19);
            this.toolStripMenuItem10.Text = "Edit";
            // 
            // addAuraToolStripMenuItem
            // 
            this.addAuraToolStripMenuItem.Name = "addAuraToolStripMenuItem";
            this.addAuraToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.addAuraToolStripMenuItem.Text = "Add Aura";
            this.addAuraToolStripMenuItem.Click += new System.EventHandler(this.addAuraToolStripMenuItem_Click);
            // 
            // removeAuraToolStripMenuItem
            // 
            this.removeAuraToolStripMenuItem.Name = "removeAuraToolStripMenuItem";
            this.removeAuraToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.removeAuraToolStripMenuItem.Text = "Remove Aura";
            this.removeAuraToolStripMenuItem.Click += new System.EventHandler(this.removeAuraToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(142, 6);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem11.Text = "Edit AUR File";
            this.toolStripMenuItem11.Click += new System.EventHandler(this.editAURFileToolStripMenuItem_Click);
            // 
            // tabPage8
            // 
            this.tabPage8.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage8.Controls.Add(this.itemList);
            this.tabPage8.Controls.Add(this.tabControl1);
            this.tabPage8.Controls.Add(this.menuStrip8);
            this.tabPage8.Location = new System.Drawing.Point(4, 24);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(851, 403);
            this.tabPage8.TabIndex = 10;
            this.tabPage8.Text = "IDB";
            // 
            // itemList
            // 
            this.itemList.FormattingEnabled = true;
            this.itemList.Location = new System.Drawing.Point(10, 43);
            this.itemList.Name = "itemList";
            this.itemList.Size = new System.Drawing.Size(455, 23);
            this.itemList.TabIndex = 5;
            this.itemList.SelectedIndexChanged += new System.EventHandler(this.itemList_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage9);
            this.tabControl1.Controls.Add(this.tabPage10);
            this.tabControl1.Controls.Add(this.tabPage11);
            this.tabControl1.Controls.Add(this.tabPage12);
            this.tabControl1.Location = new System.Drawing.Point(6, 84);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(463, 424);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage9
            // 
            this.tabPage9.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage9.Controls.Add(this.label2);
            this.tabPage9.Controls.Add(this.txtMsgDesc);
            this.tabPage9.Controls.Add(this.label25);
            this.tabPage9.Controls.Add(this.txtMsgName);
            this.tabPage9.Location = new System.Drawing.Point(4, 24);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(455, 396);
            this.tabPage9.TabIndex = 0;
            this.tabPage9.Text = "Msg Details";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 15);
            this.label2.TabIndex = 15;
            this.label2.Text = "Description";
            // 
            // txtMsgDesc
            // 
            this.txtMsgDesc.Location = new System.Drawing.Point(6, 70);
            this.txtMsgDesc.Multiline = true;
            this.txtMsgDesc.Name = "txtMsgDesc";
            this.txtMsgDesc.Size = new System.Drawing.Size(443, 320);
            this.txtMsgDesc.TabIndex = 14;
            this.txtMsgDesc.TextChanged += new System.EventHandler(this.txtMsgDesc_TextChanged);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(6, 3);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(41, 15);
            this.label25.TabIndex = 13;
            this.label25.Text = "Name";
            // 
            // txtMsgName
            // 
            this.txtMsgName.Location = new System.Drawing.Point(6, 22);
            this.txtMsgName.Name = "txtMsgName";
            this.txtMsgName.Size = new System.Drawing.Size(443, 21);
            this.txtMsgName.TabIndex = 12;
            this.txtMsgName.TextChanged += new System.EventHandler(this.txtMsgName_TextChanged);
            // 
            // tabPage10
            // 
            this.tabPage10.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage10.Controls.Add(this.txtModelID);
            this.tabPage10.Controls.Add(this.label30);
            this.tabPage10.Controls.Add(this.txtEditValueb);
            this.tabPage10.Controls.Add(this.txtEditNameb);
            this.tabPage10.Controls.Add(this.label31);
            this.tabPage10.Controls.Add(this.txtSell);
            this.tabPage10.Controls.Add(this.label39);
            this.tabPage10.Controls.Add(this.txtBuy);
            this.tabPage10.Controls.Add(this.label40);
            this.tabPage10.Controls.Add(this.label41);
            this.tabPage10.Controls.Add(this.txtDescID);
            this.tabPage10.Controls.Add(this.label42);
            this.tabPage10.Controls.Add(this.txtNameID);
            this.tabPage10.Controls.Add(this.cbStar);
            this.tabPage10.Controls.Add(this.label43);
            this.tabPage10.Controls.Add(this.textBox1);
            this.tabPage10.Controls.Add(this.lstvBasic);
            this.tabPage10.Location = new System.Drawing.Point(4, 24);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage10.Size = new System.Drawing.Size(455, 396);
            this.tabPage10.TabIndex = 1;
            this.tabPage10.Text = "Basic Details";
            // 
            // txtModelID
            // 
            this.txtModelID.Location = new System.Drawing.Point(221, 118);
            this.txtModelID.Name = "txtModelID";
            this.txtModelID.Size = new System.Drawing.Size(100, 21);
            this.txtModelID.TabIndex = 18;
            this.txtModelID.TextChanged += new System.EventHandler(this.txtModelID_TextChanged);
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(158, 121);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(55, 15);
            this.label30.TabIndex = 17;
            this.label30.Text = "Model ID";
            // 
            // txtEditValueb
            // 
            this.txtEditValueb.Location = new System.Drawing.Point(221, 144);
            this.txtEditValueb.Name = "txtEditValueb";
            this.txtEditValueb.Size = new System.Drawing.Size(188, 21);
            this.txtEditValueb.TabIndex = 16;
            this.txtEditValueb.TextChanged += new System.EventHandler(this.txtEditValueb_TextChanged);
            // 
            // txtEditNameb
            // 
            this.txtEditNameb.Location = new System.Drawing.Point(27, 144);
            this.txtEditNameb.Name = "txtEditNameb";
            this.txtEditNameb.ReadOnly = true;
            this.txtEditNameb.Size = new System.Drawing.Size(188, 21);
            this.txtEditNameb.TabIndex = 14;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(221, 73);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(61, 15);
            this.label31.TabIndex = 13;
            this.label31.Text = "Sell Value";
            // 
            // txtSell
            // 
            this.txtSell.Location = new System.Drawing.Point(221, 92);
            this.txtSell.Name = "txtSell";
            this.txtSell.Size = new System.Drawing.Size(188, 21);
            this.txtSell.TabIndex = 12;
            this.txtSell.TextChanged += new System.EventHandler(this.txtSell_TextChanged);
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(27, 73);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(155, 15);
            this.label39.TabIndex = 11;
            this.label39.Text = "Buy Value (0 = Not in Store)";
            // 
            // txtBuy
            // 
            this.txtBuy.Location = new System.Drawing.Point(27, 92);
            this.txtBuy.Name = "txtBuy";
            this.txtBuy.Size = new System.Drawing.Size(188, 21);
            this.txtBuy.TabIndex = 10;
            this.txtBuy.TextChanged += new System.EventHandler(this.txtBuy_TextChanged);
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(121, 25);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(36, 15);
            this.label40.TabIndex = 9;
            this.label40.Text = "Stars";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(318, 25);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(111, 15);
            this.label41.TabIndex = 8;
            this.label41.Text = "Description Msg ID";
            // 
            // txtDescID
            // 
            this.txtDescID.Location = new System.Drawing.Point(318, 44);
            this.txtDescID.Name = "txtDescID";
            this.txtDescID.Size = new System.Drawing.Size(91, 21);
            this.txtDescID.TabIndex = 7;
            this.txtDescID.TextChanged += new System.EventHandler(this.txtDescID_TextChanged);
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(221, 25);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(82, 15);
            this.label42.TabIndex = 6;
            this.label42.Text = "Name Msg ID";
            // 
            // txtNameID
            // 
            this.txtNameID.Location = new System.Drawing.Point(221, 44);
            this.txtNameID.Name = "txtNameID";
            this.txtNameID.Size = new System.Drawing.Size(91, 21);
            this.txtNameID.TabIndex = 5;
            this.txtNameID.TextChanged += new System.EventHandler(this.txtNameID_TextChanged);
            // 
            // cbStar
            // 
            this.cbStar.FormattingEnabled = true;
            this.cbStar.Location = new System.Drawing.Point(124, 44);
            this.cbStar.Name = "cbStar";
            this.cbStar.Size = new System.Drawing.Size(91, 23);
            this.cbStar.TabIndex = 4;
            this.cbStar.SelectedIndexChanged += new System.EventHandler(this.cbStar_SelectedIndexChanged);
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(27, 25);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(19, 15);
            this.label43.TabIndex = 3;
            this.label43.Text = "ID";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(27, 44);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(91, 21);
            this.textBox1.TabIndex = 2;
            // 
            // lstvBasic
            // 
            this.lstvBasic.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader9});
            this.lstvBasic.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstvBasic.HideSelection = false;
            this.lstvBasic.Location = new System.Drawing.Point(5, 183);
            this.lstvBasic.MultiSelect = false;
            this.lstvBasic.Name = "lstvBasic";
            this.lstvBasic.Size = new System.Drawing.Size(445, 207);
            this.lstvBasic.TabIndex = 1;
            this.lstvBasic.TileSize = new System.Drawing.Size(200, 30);
            this.lstvBasic.UseCompatibleStateImageBehavior = false;
            this.lstvBasic.View = System.Windows.Forms.View.Tile;
            this.lstvBasic.SelectedIndexChanged += new System.EventHandler(this.lstvBasic_SelectedIndexChanged);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Name";
            this.columnHeader3.Width = 215;
            // 
            // tabPage11
            // 
            this.tabPage11.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage11.Controls.Add(this.txtChance1);
            this.tabPage11.Controls.Add(this.label44);
            this.tabPage11.Controls.Add(this.txtEditValue1);
            this.tabPage11.Controls.Add(this.txtEditName1);
            this.tabPage11.Controls.Add(this.txtAVal1);
            this.tabPage11.Controls.Add(this.label45);
            this.tabPage11.Controls.Add(this.txtADelay1);
            this.tabPage11.Controls.Add(this.label46);
            this.tabPage11.Controls.Add(this.txtTimes1);
            this.tabPage11.Controls.Add(this.label47);
            this.tabPage11.Controls.Add(this.cbActive1);
            this.tabPage11.Controls.Add(this.label48);
            this.tabPage11.Controls.Add(this.cbEffect1);
            this.tabPage11.Controls.Add(this.label49);
            this.tabPage11.Controls.Add(this.label50);
            this.tabPage11.Controls.Add(this.lstvEffect1);
            this.tabPage11.Location = new System.Drawing.Point(4, 24);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage11.Size = new System.Drawing.Size(455, 396);
            this.tabPage11.TabIndex = 2;
            this.tabPage11.Text = "Effect 1 Details";
            // 
            // txtChance1
            // 
            this.txtChance1.Location = new System.Drawing.Point(19, 159);
            this.txtChance1.Name = "txtChance1";
            this.txtChance1.Size = new System.Drawing.Size(163, 21);
            this.txtChance1.TabIndex = 31;
            this.txtChance1.TextChanged += new System.EventHandler(this.txtChance1_TextChanged);
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(16, 142);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(148, 15);
            this.label44.TabIndex = 30;
            this.label44.Text = "Chance of being activated";
            // 
            // txtEditValue1
            // 
            this.txtEditValue1.Location = new System.Drawing.Point(227, 274);
            this.txtEditValue1.Name = "txtEditValue1";
            this.txtEditValue1.Size = new System.Drawing.Size(188, 21);
            this.txtEditValue1.TabIndex = 29;
            this.txtEditValue1.TextChanged += new System.EventHandler(this.txtEditValue1_TextChanged);
            // 
            // txtEditName1
            // 
            this.txtEditName1.Location = new System.Drawing.Point(33, 274);
            this.txtEditName1.Name = "txtEditName1";
            this.txtEditName1.ReadOnly = true;
            this.txtEditName1.Size = new System.Drawing.Size(188, 21);
            this.txtEditName1.TabIndex = 28;
            // 
            // txtAVal1
            // 
            this.txtAVal1.Location = new System.Drawing.Point(303, 114);
            this.txtAVal1.Name = "txtAVal1";
            this.txtAVal1.Size = new System.Drawing.Size(130, 21);
            this.txtAVal1.TabIndex = 27;
            this.txtAVal1.TextChanged += new System.EventHandler(this.txtAVal1_TextChanged);
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(300, 97);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(91, 15);
            this.label45.TabIndex = 26;
            this.label45.Text = "Activation Value";
            // 
            // txtADelay1
            // 
            this.txtADelay1.Location = new System.Drawing.Point(188, 114);
            this.txtADelay1.Name = "txtADelay1";
            this.txtADelay1.Size = new System.Drawing.Size(109, 21);
            this.txtADelay1.TabIndex = 25;
            this.txtADelay1.TextChanged += new System.EventHandler(this.txtADelay1_TextChanged);
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(188, 97);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(89, 15);
            this.label46.TabIndex = 24;
            this.label46.Text = "Activation Time";
            // 
            // txtTimes1
            // 
            this.txtTimes1.Location = new System.Drawing.Point(19, 114);
            this.txtTimes1.Name = "txtTimes1";
            this.txtTimes1.Size = new System.Drawing.Size(163, 21);
            this.txtTimes1.TabIndex = 23;
            this.txtTimes1.TextChanged += new System.EventHandler(this.txtTimes1_TextChanged);
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(16, 97);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(187, 15);
            this.label47.TabIndex = 22;
            this.label47.Text = "how many times can be Activated";
            // 
            // cbActive1
            // 
            this.cbActive1.FormattingEnabled = true;
            this.cbActive1.Location = new System.Drawing.Point(19, 73);
            this.cbActive1.Name = "cbActive1";
            this.cbActive1.Size = new System.Drawing.Size(414, 23);
            this.cbActive1.TabIndex = 21;
            this.cbActive1.SelectedIndexChanged += new System.EventHandler(this.cbActive1_SelectedIndexChanged);
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(16, 57);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(148, 15);
            this.label48.TabIndex = 20;
            this.label48.Text = "How the Effect is Activated";
            // 
            // cbEffect1
            // 
            this.cbEffect1.FormattingEnabled = true;
            this.cbEffect1.Location = new System.Drawing.Point(19, 28);
            this.cbEffect1.Name = "cbEffect1";
            this.cbEffect1.Size = new System.Drawing.Size(414, 23);
            this.cbEffect1.TabIndex = 19;
            this.cbEffect1.SelectedIndexChanged += new System.EventHandler(this.cbEffect1_SelectedIndexChanged);
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(16, 12);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(37, 15);
            this.label49.TabIndex = 18;
            this.label49.Text = "Effect";
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(4, -173);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(70, 15);
            this.label50.TabIndex = 17;
            this.label50.Text = "Description";
            // 
            // lstvEffect1
            // 
            this.lstvEffect1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5});
            this.lstvEffect1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstvEffect1.HideSelection = false;
            this.lstvEffect1.Location = new System.Drawing.Point(5, 300);
            this.lstvEffect1.MultiSelect = false;
            this.lstvEffect1.Name = "lstvEffect1";
            this.lstvEffect1.Size = new System.Drawing.Size(445, 90);
            this.lstvEffect1.TabIndex = 2;
            this.lstvEffect1.TileSize = new System.Drawing.Size(200, 30);
            this.lstvEffect1.UseCompatibleStateImageBehavior = false;
            this.lstvEffect1.View = System.Windows.Forms.View.Tile;
            this.lstvEffect1.SelectedIndexChanged += new System.EventHandler(this.lstvEffect1_SelectedIndexChanged);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Name";
            this.columnHeader4.Width = 215;
            // 
            // tabPage12
            // 
            this.tabPage12.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage12.Controls.Add(this.txtChance2);
            this.tabPage12.Controls.Add(this.label51);
            this.tabPage12.Controls.Add(this.txtEditValue2);
            this.tabPage12.Controls.Add(this.txtEditName2);
            this.tabPage12.Controls.Add(this.txtAVal2);
            this.tabPage12.Controls.Add(this.label52);
            this.tabPage12.Controls.Add(this.txtADelay2);
            this.tabPage12.Controls.Add(this.label53);
            this.tabPage12.Controls.Add(this.txtTimes2);
            this.tabPage12.Controls.Add(this.label54);
            this.tabPage12.Controls.Add(this.cbActive2);
            this.tabPage12.Controls.Add(this.label55);
            this.tabPage12.Controls.Add(this.cbEffect2);
            this.tabPage12.Controls.Add(this.label56);
            this.tabPage12.Controls.Add(this.lstvEffect2);
            this.tabPage12.Location = new System.Drawing.Point(4, 24);
            this.tabPage12.Name = "tabPage12";
            this.tabPage12.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage12.Size = new System.Drawing.Size(455, 396);
            this.tabPage12.TabIndex = 3;
            this.tabPage12.Text = "Effect 2 Details";
            // 
            // txtChance2
            // 
            this.txtChance2.Location = new System.Drawing.Point(19, 158);
            this.txtChance2.Name = "txtChance2";
            this.txtChance2.Size = new System.Drawing.Size(163, 21);
            this.txtChance2.TabIndex = 44;
            this.txtChance2.TextChanged += new System.EventHandler(this.txtChance2_TextChanged);
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(16, 141);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(148, 15);
            this.label51.TabIndex = 43;
            this.label51.Text = "Chance of being activated";
            // 
            // txtEditValue2
            // 
            this.txtEditValue2.Location = new System.Drawing.Point(227, 274);
            this.txtEditValue2.Name = "txtEditValue2";
            this.txtEditValue2.Size = new System.Drawing.Size(188, 21);
            this.txtEditValue2.TabIndex = 42;
            this.txtEditValue2.TextChanged += new System.EventHandler(this.txtEditValue2_TextChanged);
            // 
            // txtEditName2
            // 
            this.txtEditName2.Location = new System.Drawing.Point(33, 274);
            this.txtEditName2.Name = "txtEditName2";
            this.txtEditName2.ReadOnly = true;
            this.txtEditName2.Size = new System.Drawing.Size(188, 21);
            this.txtEditName2.TabIndex = 41;
            // 
            // txtAVal2
            // 
            this.txtAVal2.Location = new System.Drawing.Point(303, 114);
            this.txtAVal2.Name = "txtAVal2";
            this.txtAVal2.Size = new System.Drawing.Size(130, 21);
            this.txtAVal2.TabIndex = 40;
            this.txtAVal2.TextChanged += new System.EventHandler(this.txtAVal2_TextChanged);
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(300, 97);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(91, 15);
            this.label52.TabIndex = 39;
            this.label52.Text = "Activation Value";
            // 
            // txtADelay2
            // 
            this.txtADelay2.Location = new System.Drawing.Point(188, 114);
            this.txtADelay2.Name = "txtADelay2";
            this.txtADelay2.Size = new System.Drawing.Size(109, 21);
            this.txtADelay2.TabIndex = 38;
            this.txtADelay2.TextChanged += new System.EventHandler(this.txtADelay2_TextChanged);
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(188, 97);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(89, 15);
            this.label53.TabIndex = 37;
            this.label53.Text = "Activation Time";
            // 
            // txtTimes2
            // 
            this.txtTimes2.Location = new System.Drawing.Point(19, 114);
            this.txtTimes2.Name = "txtTimes2";
            this.txtTimes2.Size = new System.Drawing.Size(163, 21);
            this.txtTimes2.TabIndex = 36;
            this.txtTimes2.TextChanged += new System.EventHandler(this.txtTimes2_TextChanged);
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(16, 97);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(187, 15);
            this.label54.TabIndex = 35;
            this.label54.Text = "how many times can be Activated";
            // 
            // cbActive2
            // 
            this.cbActive2.FormattingEnabled = true;
            this.cbActive2.Location = new System.Drawing.Point(19, 73);
            this.cbActive2.Name = "cbActive2";
            this.cbActive2.Size = new System.Drawing.Size(414, 23);
            this.cbActive2.TabIndex = 34;
            this.cbActive2.SelectedIndexChanged += new System.EventHandler(this.cbActive2_SelectedIndexChanged);
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(16, 57);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(148, 15);
            this.label55.TabIndex = 33;
            this.label55.Text = "How the Effect is Activated";
            // 
            // cbEffect2
            // 
            this.cbEffect2.FormattingEnabled = true;
            this.cbEffect2.Location = new System.Drawing.Point(19, 28);
            this.cbEffect2.Name = "cbEffect2";
            this.cbEffect2.Size = new System.Drawing.Size(414, 23);
            this.cbEffect2.TabIndex = 32;
            this.cbEffect2.SelectedIndexChanged += new System.EventHandler(this.cbEffect2_SelectedIndexChanged);
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(16, 12);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(37, 15);
            this.label56.TabIndex = 31;
            this.label56.Text = "Effect";
            // 
            // lstvEffect2
            // 
            this.lstvEffect2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7});
            this.lstvEffect2.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstvEffect2.HideSelection = false;
            this.lstvEffect2.Location = new System.Drawing.Point(5, 300);
            this.lstvEffect2.MultiSelect = false;
            this.lstvEffect2.Name = "lstvEffect2";
            this.lstvEffect2.Size = new System.Drawing.Size(445, 90);
            this.lstvEffect2.TabIndex = 30;
            this.lstvEffect2.TileSize = new System.Drawing.Size(200, 30);
            this.lstvEffect2.UseCompatibleStateImageBehavior = false;
            this.lstvEffect2.View = System.Windows.Forms.View.Tile;
            this.lstvEffect2.SelectedIndexChanged += new System.EventHandler(this.lstvEffect2_SelectedIndexChanged);
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Name";
            this.columnHeader6.Width = 215;
            // 
            // menuStrip8
            // 
            this.menuStrip8.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem8,
            this.addNewZSoulToolStripMenuItem,
            this.msgToolStripMenuItem});
            this.menuStrip8.Location = new System.Drawing.Point(3, 3);
            this.menuStrip8.Name = "menuStrip8";
            this.menuStrip8.Size = new System.Drawing.Size(845, 24);
            this.menuStrip8.TabIndex = 3;
            this.menuStrip8.Text = "menuStrip8";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem1,
            this.toolStripMenuItem9});
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem8.Text = "File";
            // 
            // openToolStripMenuItem1
            // 
            this.openToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.talismanToolStripMenuItem,
            this.skillToolStripMenuItem,
            this.accessoriesToolStripMenuItem,
            this.battleToolStripMenuItem,
            this.costumesToolStripMenuItem,
            this.extraToolStripMenuItem,
            this.materialToolStripMenuItem});
            this.openToolStripMenuItem1.Name = "openToolStripMenuItem1";
            this.openToolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            this.openToolStripMenuItem1.Text = "Load IDB";
            // 
            // talismanToolStripMenuItem
            // 
            this.talismanToolStripMenuItem.Name = "talismanToolStripMenuItem";
            this.talismanToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.talismanToolStripMenuItem.Text = "Talisman";
            this.talismanToolStripMenuItem.Click += new System.EventHandler(this.talismanToolStripMenuItem_Click);
            // 
            // skillToolStripMenuItem
            // 
            this.skillToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.supersToolStripMenuItem1,
            this.ultimatesToolStripMenuItem1,
            this.evasivesToolStripMenuItem1});
            this.skillToolStripMenuItem.Name = "skillToolStripMenuItem";
            this.skillToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.skillToolStripMenuItem.Text = "Skills";
            // 
            // supersToolStripMenuItem1
            // 
            this.supersToolStripMenuItem1.Name = "supersToolStripMenuItem1";
            this.supersToolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.supersToolStripMenuItem1.Text = "Supers";
            this.supersToolStripMenuItem1.Click += new System.EventHandler(this.supersToolStripMenuItem1_Click);
            // 
            // ultimatesToolStripMenuItem1
            // 
            this.ultimatesToolStripMenuItem1.Name = "ultimatesToolStripMenuItem1";
            this.ultimatesToolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.ultimatesToolStripMenuItem1.Text = "Ultimates";
            this.ultimatesToolStripMenuItem1.Click += new System.EventHandler(this.ultimatesToolStripMenuItem1_Click);
            // 
            // evasivesToolStripMenuItem1
            // 
            this.evasivesToolStripMenuItem1.Name = "evasivesToolStripMenuItem1";
            this.evasivesToolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.evasivesToolStripMenuItem1.Text = "Evasives";
            this.evasivesToolStripMenuItem1.Click += new System.EventHandler(this.evasivesToolStripMenuItem1_Click);
            // 
            // accessoriesToolStripMenuItem
            // 
            this.accessoriesToolStripMenuItem.Name = "accessoriesToolStripMenuItem";
            this.accessoriesToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.accessoriesToolStripMenuItem.Text = "Accessories";
            this.accessoriesToolStripMenuItem.Click += new System.EventHandler(this.accessoriesToolStripMenuItem_Click);
            // 
            // battleToolStripMenuItem
            // 
            this.battleToolStripMenuItem.Name = "battleToolStripMenuItem";
            this.battleToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.battleToolStripMenuItem.Text = "Battle";
            this.battleToolStripMenuItem.Click += new System.EventHandler(this.battleToolStripMenuItem_Click);
            // 
            // costumesToolStripMenuItem
            // 
            this.costumesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.topToolStripMenuItem,
            this.glovesToolStripMenuItem,
            this.bottomToolStripMenuItem,
            this.shoesToolStripMenuItem});
            this.costumesToolStripMenuItem.Name = "costumesToolStripMenuItem";
            this.costumesToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.costumesToolStripMenuItem.Text = "Costumes";
            // 
            // topToolStripMenuItem
            // 
            this.topToolStripMenuItem.Name = "topToolStripMenuItem";
            this.topToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.topToolStripMenuItem.Text = "Top";
            this.topToolStripMenuItem.Click += new System.EventHandler(this.topToolStripMenuItem_Click);
            // 
            // glovesToolStripMenuItem
            // 
            this.glovesToolStripMenuItem.Name = "glovesToolStripMenuItem";
            this.glovesToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.glovesToolStripMenuItem.Text = "Gloves";
            this.glovesToolStripMenuItem.Click += new System.EventHandler(this.glovesToolStripMenuItem_Click);
            // 
            // bottomToolStripMenuItem
            // 
            this.bottomToolStripMenuItem.Name = "bottomToolStripMenuItem";
            this.bottomToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.bottomToolStripMenuItem.Text = "Bottom";
            this.bottomToolStripMenuItem.Click += new System.EventHandler(this.bottomToolStripMenuItem_Click);
            // 
            // shoesToolStripMenuItem
            // 
            this.shoesToolStripMenuItem.Name = "shoesToolStripMenuItem";
            this.shoesToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.shoesToolStripMenuItem.Text = "Shoes";
            this.shoesToolStripMenuItem.Click += new System.EventHandler(this.shoesToolStripMenuItem_Click);
            // 
            // extraToolStripMenuItem
            // 
            this.extraToolStripMenuItem.Name = "extraToolStripMenuItem";
            this.extraToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.extraToolStripMenuItem.Text = "Extra";
            this.extraToolStripMenuItem.Click += new System.EventHandler(this.extraToolStripMenuItem_Click);
            // 
            // materialToolStripMenuItem
            // 
            this.materialToolStripMenuItem.Name = "materialToolStripMenuItem";
            this.materialToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.materialToolStripMenuItem.Text = "Material";
            this.materialToolStripMenuItem.Click += new System.EventHandler(this.materialToolStripMenuItem_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(121, 22);
            this.toolStripMenuItem9.Text = "Save IDB";
            this.toolStripMenuItem9.Click += new System.EventHandler(this.toolStripMenuItem9_Click);
            // 
            // addNewZSoulToolStripMenuItem
            // 
            this.addNewZSoulToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.toolStripMenuItem12,
            this.replaceImportToolStripMenuItem,
            this.exportToolStripMenuItem1});
            this.addNewZSoulToolStripMenuItem.Name = "addNewZSoulToolStripMenuItem";
            this.addNewZSoulToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.addNewZSoulToolStripMenuItem.Text = "IDB";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.addToolStripMenuItem.Text = "Add/Import";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.IDBaddToolStripMenuItem_Click);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(156, 22);
            this.toolStripMenuItem12.Text = "Remove";
            this.toolStripMenuItem12.Click += new System.EventHandler(this.IDBremoveToolStripMenuItem_Click);
            // 
            // replaceImportToolStripMenuItem
            // 
            this.replaceImportToolStripMenuItem.Name = "replaceImportToolStripMenuItem";
            this.replaceImportToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.replaceImportToolStripMenuItem.Text = "Replace/Import";
            this.replaceImportToolStripMenuItem.Click += new System.EventHandler(this.replaceImportToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem1
            // 
            this.exportToolStripMenuItem1.Name = "exportToolStripMenuItem1";
            this.exportToolStripMenuItem1.Size = new System.Drawing.Size(156, 22);
            this.exportToolStripMenuItem1.Text = "Export";
            this.exportToolStripMenuItem1.Click += new System.EventHandler(this.exportToolStripMenuItem1_Click);
            // 
            // msgToolStripMenuItem
            // 
            this.msgToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewToolStripMenuItem,
            this.removeToolStripMenuItem1});
            this.msgToolStripMenuItem.Name = "msgToolStripMenuItem";
            this.msgToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.msgToolStripMenuItem.Text = "Msg";
            // 
            // addNewToolStripMenuItem
            // 
            this.addNewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nameToolStripMenuItem,
            this.descriptionToolStripMenuItem});
            this.addNewToolStripMenuItem.Name = "addNewToolStripMenuItem";
            this.addNewToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.addNewToolStripMenuItem.Text = "Name";
            // 
            // nameToolStripMenuItem
            // 
            this.nameToolStripMenuItem.Name = "nameToolStripMenuItem";
            this.nameToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.nameToolStripMenuItem.Text = "Add";
            this.nameToolStripMenuItem.Click += new System.EventHandler(this.nameToolStripMenuItem_Click);
            // 
            // descriptionToolStripMenuItem
            // 
            this.descriptionToolStripMenuItem.Name = "descriptionToolStripMenuItem";
            this.descriptionToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.descriptionToolStripMenuItem.Text = "Remove";
            this.descriptionToolStripMenuItem.Click += new System.EventHandler(this.descriptionToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem1
            // 
            this.removeToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nameToolStripMenuItem1,
            this.descriptionToolStripMenuItem1});
            this.removeToolStripMenuItem1.Name = "removeToolStripMenuItem1";
            this.removeToolStripMenuItem1.Size = new System.Drawing.Size(134, 22);
            this.removeToolStripMenuItem1.Text = "Description";
            // 
            // nameToolStripMenuItem1
            // 
            this.nameToolStripMenuItem1.Name = "nameToolStripMenuItem1";
            this.nameToolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
            this.nameToolStripMenuItem1.Text = "Add";
            this.nameToolStripMenuItem1.Click += new System.EventHandler(this.nameToolStripMenuItem1_Click);
            // 
            // descriptionToolStripMenuItem1
            // 
            this.descriptionToolStripMenuItem1.Name = "descriptionToolStripMenuItem1";
            this.descriptionToolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
            this.descriptionToolStripMenuItem1.Text = "Remove";
            this.descriptionToolStripMenuItem1.Click += new System.EventHandler(this.descriptionToolStripMenuItem1_Click);
            // 
            // tabPage6
            // 
            this.tabPage6.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage6.Controls.Add(this.menuStrip6);
            this.tabPage6.Location = new System.Drawing.Point(4, 24);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(851, 403);
            this.tabPage6.TabIndex = 9;
            this.tabPage6.Text = "CSS";
            // 
            // menuStrip6
            // 
            this.menuStrip6.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem4});
            this.menuStrip6.Location = new System.Drawing.Point(3, 3);
            this.menuStrip6.Name = "menuStrip6";
            this.menuStrip6.Size = new System.Drawing.Size(845, 24);
            this.menuStrip6.TabIndex = 1;
            this.menuStrip6.Text = "menuStrip6";
            // 
            // editToolStripMenuItem4
            // 
            this.editToolStripMenuItem4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editCSSFileToolStripMenuItem});
            this.editToolStripMenuItem4.Name = "editToolStripMenuItem4";
            this.editToolStripMenuItem4.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem4.Text = "Edit";
            // 
            // editCSSFileToolStripMenuItem
            // 
            this.editCSSFileToolStripMenuItem.Name = "editCSSFileToolStripMenuItem";
            this.editCSSFileToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.editCSSFileToolStripMenuItem.Text = "Edit CSS File";
            this.editCSSFileToolStripMenuItem.Click += new System.EventHandler(this.editCSSFileToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem6});
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(37, 19);
            this.toolStripMenuItem5.Text = "File";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(98, 22);
            this.toolStripMenuItem6.Text = "Save";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.button4_Click);
            // 
            // editToolStripMenuItem5
            // 
            this.editToolStripMenuItem5.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editPSCFileToolStripMenuItem});
            this.editToolStripMenuItem5.Name = "editToolStripMenuItem5";
            this.editToolStripMenuItem5.Size = new System.Drawing.Size(39, 19);
            this.editToolStripMenuItem5.Text = "Edit";
            // 
            // editPSCFileToolStripMenuItem
            // 
            this.editPSCFileToolStripMenuItem.Name = "editPSCFileToolStripMenuItem";
            this.editPSCFileToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.editPSCFileToolStripMenuItem.Text = "Edit .PSC File";
            this.editPSCFileToolStripMenuItem.Click += new System.EventHandler(this.editPSCFileToolStripMenuItem_Click);
            // 
            // cbCharacter
            // 
            this.cbCharacter.Dock = System.Windows.Forms.DockStyle.Right;
            this.cbCharacter.FormattingEnabled = true;
            this.cbCharacter.Location = new System.Drawing.Point(689, 24);
            this.cbCharacter.Name = "cbCharacter";
            this.cbCharacter.Size = new System.Drawing.Size(170, 23);
            this.cbCharacter.TabIndex = 6;
            this.cbCharacter.SelectedIndexChanged += new System.EventHandler(this.cbCharacter_SelectedIndexChanged);
            // 
            // cbCostumes
            // 
            this.cbCostumes.Dock = System.Windows.Forms.DockStyle.Right;
            this.cbCostumes.FormattingEnabled = true;
            this.cbCostumes.Location = new System.Drawing.Point(519, 24);
            this.cbCostumes.Name = "cbCostumes";
            this.cbCostumes.Size = new System.Drawing.Size(170, 23);
            this.cbCostumes.TabIndex = 7;
            this.cbCostumes.SelectedIndexChanged += new System.EventHandler(this.cbCostumes_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 470);
            this.Controls.Add(this.cbCostumes);
            this.Controls.Add(this.cbCharacter);
            this.Controls.Add(this.Mods);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Form1";
            this.Text = "XVReborn";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.Mods.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.menuStrip3.ResumeLayout(false);
            this.menuStrip3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip4.ResumeLayout(false);
            this.menuStrip4.PerformLayout();
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.menuStrip7.ResumeLayout(false);
            this.menuStrip7.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.menuStrip5.ResumeLayout(false);
            this.menuStrip5.PerformLayout();
            this.tabPage8.ResumeLayout(false);
            this.tabPage8.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage9.ResumeLayout(false);
            this.tabPage9.PerformLayout();
            this.tabPage10.ResumeLayout(false);
            this.tabPage10.PerformLayout();
            this.tabPage11.ResumeLayout(false);
            this.tabPage11.PerformLayout();
            this.tabPage12.ResumeLayout(false);
            this.tabPage12.PerformLayout();
            this.menuStrip8.ResumeLayout(false);
            this.menuStrip8.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.menuStrip6.ResumeLayout(false);
            this.menuStrip6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem installModToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl Mods;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtCMS7;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtCMS6;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCMS5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCMS4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCMS3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCMS2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtCMS1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox UltLst1;
        private System.Windows.Forms.ComboBox SupLst1;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.ComboBox SupLst2;
        private System.Windows.Forms.ComboBox EvaLst;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.ComboBox SupLst3;
        private System.Windows.Forms.ComboBox UltLst2;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.ComboBox SupLst4;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.ToolStripMenuItem editCMSFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem clearInstallationToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ListView PSClstData;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.TextBox PSCtxtVal;
        private System.Windows.Forms.TextBox PSCtxtName;
        private System.Windows.Forms.MenuStrip menuStrip4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem editPSCFileToolStripMenuItem;
        private System.Windows.Forms.ComboBox cbCharacter;
        private System.Windows.Forms.ComboBox cbCostumes;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox txtCSO4;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox txtCSO3;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox txtCSO2;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox txtCSO1;
        private System.Windows.Forms.ToolStripMenuItem saveCSOToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editCSOFileToolStripMenuItem1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveCSOFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem editCSOFileToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtText;
        private System.Windows.Forms.ComboBox cbLine;
        public System.Windows.Forms.ComboBox cbList;
        private System.Windows.Forms.MenuStrip menuStrip3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem charactersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem skillsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem supersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ultimatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem evasivesToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.MenuStrip menuStrip5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem11;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.MenuStrip menuStrip7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem20;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem21;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem editCUSFileToolStripMenuItem;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtAURID;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveAURFileToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkInf;
        private System.Windows.Forms.ComboBox cbAURChar;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox txtBLoop;
        private System.Windows.Forms.TextBox txtBEnd;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtHenshinEnd;
        private System.Windows.Forms.TextBox txtKiCharge;
        private System.Windows.Forms.TextBox txtBStart;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.ComboBox cbAuraList;
        private System.Windows.Forms.TextBox txtkiMax;
        private System.Windows.Forms.TextBox txtHenshinStart;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.ToolStripMenuItem addAuraToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeAuraToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem compileScriptsToolStripMenuItem;
        private System.Windows.Forms.ListView lvMods;
        private System.Windows.Forms.ColumnHeader ch1;
        private System.Windows.Forms.ColumnHeader ch2;
        private System.Windows.Forms.ColumnHeader ch3;
        private System.Windows.Forms.ColumnHeader header;
        private System.Windows.Forms.ColumnHeader ch;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.MenuStrip menuStrip6;
        private TabPage tabPage8;
        private ComboBox itemList;
        private TabControl tabControl1;
        private TabPage tabPage9;
        private Label label2;
        private TextBox txtMsgDesc;
        private Label label25;
        private TextBox txtMsgName;
        private TabPage tabPage10;
        private TextBox txtModelID;
        private Label label30;
        private TextBox txtEditValueb;
        private TextBox txtEditNameb;
        private Label label31;
        private TextBox txtSell;
        private Label label39;
        private TextBox txtBuy;
        private Label label40;
        private Label label41;
        private TextBox txtDescID;
        private Label label42;
        private TextBox txtNameID;
        private ComboBox cbStar;
        private Label label43;
        private TextBox textBox1;
        private ListView lstvBasic;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader9;
        private TabPage tabPage11;
        private TextBox txtChance1;
        private Label label44;
        private TextBox txtEditValue1;
        private TextBox txtEditName1;
        private TextBox txtAVal1;
        private Label label45;
        private TextBox txtADelay1;
        private Label label46;
        private TextBox txtTimes1;
        private Label label47;
        private ComboBox cbActive1;
        private Label label48;
        private ComboBox cbEffect1;
        private Label label49;
        private Label label50;
        private ListView lstvEffect1;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private TabPage tabPage12;
        private TextBox txtChance2;
        private Label label51;
        private TextBox txtEditValue2;
        private TextBox txtEditName2;
        private TextBox txtAVal2;
        private Label label52;
        private TextBox txtADelay2;
        private Label label53;
        private TextBox txtTimes2;
        private Label label54;
        private ComboBox cbActive2;
        private Label label55;
        private ComboBox cbEffect2;
        private Label label56;
        private ListView lstvEffect2;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader7;
        private MenuStrip menuStrip8;
        private ToolStripMenuItem toolStripMenuItem8;
        private ToolStripMenuItem toolStripMenuItem9;
        private ToolStripMenuItem addNewZSoulToolStripMenuItem;
        private ToolStripMenuItem addToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem12;
        private ToolStripMenuItem replaceImportToolStripMenuItem;
        private ToolStripMenuItem exportToolStripMenuItem1;
        private ToolStripMenuItem msgToolStripMenuItem;
        private ToolStripMenuItem addNewToolStripMenuItem;
        private ToolStripMenuItem nameToolStripMenuItem;
        private ToolStripMenuItem descriptionToolStripMenuItem;
        private ToolStripMenuItem removeToolStripMenuItem1;
        private ToolStripMenuItem nameToolStripMenuItem1;
        private ToolStripMenuItem descriptionToolStripMenuItem1;
        private ToolStripMenuItem openToolStripMenuItem1;
        private ToolStripMenuItem skillToolStripMenuItem;
        private ToolStripMenuItem talismanToolStripMenuItem;
        private ToolStripMenuItem accessoriesToolStripMenuItem;
        private ToolStripMenuItem battleToolStripMenuItem;
        private ToolStripMenuItem costumesToolStripMenuItem;
        private ToolStripMenuItem extraToolStripMenuItem;
        private ToolStripMenuItem materialToolStripMenuItem;
        private ToolStripMenuItem supersToolStripMenuItem1;
        private ToolStripMenuItem ultimatesToolStripMenuItem1;
        private ToolStripMenuItem evasivesToolStripMenuItem1;
        private ToolStripMenuItem topToolStripMenuItem;
        private ToolStripMenuItem glovesToolStripMenuItem;
        private ToolStripMenuItem bottomToolStripMenuItem;
        private ToolStripMenuItem shoesToolStripMenuItem;
        private ToolStripMenuItem superInfoToolStripMenuItem;
        private ToolStripMenuItem ultimatesInfoToolStripMenuItem;
        private ToolStripMenuItem evasivesInfoToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem4;
        private ToolStripMenuItem editCSSFileToolStripMenuItem;
    }
}

