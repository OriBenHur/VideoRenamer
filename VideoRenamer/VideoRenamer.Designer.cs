namespace VideoRenamer
{
    partial class VideoRenamer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoRenamer));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Rename_button = new System.Windows.Forms.Button();
            this.CheckForUpdates = new System.ComponentModel.BackgroundWorker();
            this.AutoMode = new System.Windows.Forms.Label();
            this.AndroidStyleToggleSwitch = new JCS.ToggleSwitch();
            this.olv1 = new BrightIdeasSoftware.ObjectListView();
            this.checkBox = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.index = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.oldName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.outputName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olv1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(618, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFolderToolStripMenuItem,
            this.openFileToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openFolderToolStripMenuItem
            // 
            this.openFolderToolStripMenuItem.Name = "openFolderToolStripMenuItem";
            this.openFolderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openFolderToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.openFolderToolStripMenuItem.Text = "Open Folder";
            this.openFolderToolStripMenuItem.Click += new System.EventHandler(this.openFolderToolStripMenuItem_Click);
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.openFileToolStripMenuItem.Text = "Open File";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkForUpdatesToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.settingsToolStripMenuItem.Text = "About";
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.checkForUpdatesToolStripMenuItem.Text = "Check For Updates";
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
            // 
            // Rename_button
            // 
            this.Rename_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Rename_button.Location = new System.Drawing.Point(533, 384);
            this.Rename_button.Name = "Rename_button";
            this.Rename_button.Size = new System.Drawing.Size(75, 23);
            this.Rename_button.TabIndex = 3;
            this.Rename_button.Text = "Rename";
            this.Rename_button.UseVisualStyleBackColor = true;
            this.Rename_button.Click += new System.EventHandler(this.Rename_button_Click);
            // 
            // CheckForUpdates
            // 
            this.CheckForUpdates.DoWork += new System.ComponentModel.DoWorkEventHandler(this.CheckForUpdates_DoWork);
            // 
            // AutoMode
            // 
            this.AutoMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AutoMode.AutoSize = true;
            this.AutoMode.Location = new System.Drawing.Point(13, 389);
            this.AutoMode.Name = "AutoMode";
            this.AutoMode.Size = new System.Drawing.Size(79, 13);
            this.AutoMode.TabIndex = 5;
            this.AutoMode.Text = "Assume Latest:";
            // 
            // AndroidStyleToggleSwitch
            // 
            this.AndroidStyleToggleSwitch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AndroidStyleToggleSwitch.AnimationInterval = 10;
            this.AndroidStyleToggleSwitch.AnimationStep = 3;
            this.AndroidStyleToggleSwitch.Location = new System.Drawing.Point(92, 384);
            this.AndroidStyleToggleSwitch.Name = "AndroidStyleToggleSwitch";
            this.AndroidStyleToggleSwitch.OffFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AndroidStyleToggleSwitch.OffForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(123)))), ((int)(((byte)(141)))));
            this.AndroidStyleToggleSwitch.OffText = "OFF";
            this.AndroidStyleToggleSwitch.OnFont = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.AndroidStyleToggleSwitch.OnForeColor = System.Drawing.Color.White;
            this.AndroidStyleToggleSwitch.OnText = "ON";
            this.AndroidStyleToggleSwitch.Size = new System.Drawing.Size(78, 23);
            this.AndroidStyleToggleSwitch.Style = JCS.ToggleSwitch.ToggleSwitchStyle.Android;
            this.AndroidStyleToggleSwitch.TabIndex = 4;
            // 
            // olv1
            // 
            this.olv1.AllColumns.Add(this.checkBox);
            this.olv1.AllColumns.Add(this.index);
            this.olv1.AllColumns.Add(this.oldName);
            this.olv1.AllColumns.Add(this.outputName);
            this.olv1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olv1.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.olv1.CellEditUseWholeCell = false;
            this.olv1.CheckBoxes = true;
            this.olv1.CheckedAspectName = "";
            this.olv1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.checkBox,
            this.index,
            this.oldName,
            this.outputName});
            this.olv1.Cursor = System.Windows.Forms.Cursors.Default;
            this.olv1.FullRowSelect = true;
            this.olv1.HasCollapsibleGroups = false;
            this.olv1.Location = new System.Drawing.Point(0, 27);
            this.olv1.Name = "olv1";
            this.olv1.ShowGroups = false;
            this.olv1.Size = new System.Drawing.Size(618, 351);
            this.olv1.TabIndex = 6;
            this.olv1.UseCellFormatEvents = true;
            this.olv1.UseCompatibleStateImageBehavior = false;
            this.olv1.View = System.Windows.Forms.View.Details;
            this.olv1.CellEditFinished += new BrightIdeasSoftware.CellEditEventHandler(this.olv1_CellEditFinished);
            this.olv1.HeaderCheckBoxChanging += new System.EventHandler<BrightIdeasSoftware.HeaderCheckBoxChangingEventArgs>(this.olv1_HeaderCheckBoxChanging);
            this.olv1.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.olv1_ItemChecked);
            // 
            // checkBox
            // 
            this.checkBox.AspectName = "CheckBox";
            this.checkBox.ButtonSizing = BrightIdeasSoftware.OLVColumn.ButtonSizingMode.CellBounds;
            this.checkBox.HeaderCheckBox = true;
            this.checkBox.MaximumWidth = 26;
            this.checkBox.MinimumWidth = 26;
            this.checkBox.ShowTextInHeader = false;
            this.checkBox.Text = "";
            this.checkBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.checkBox.Width = 26;
            // 
            // index
            // 
            this.index.AspectName = "Index";
            this.index.CellEditUseWholeCell = false;
            this.index.FillsFreeSpace = true;
            this.index.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.index.IsEditable = false;
            this.index.MaximumWidth = 24;
            this.index.MinimumWidth = 40;
            this.index.Sortable = false;
            this.index.Text = "#";
            this.index.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.index.UseFiltering = false;
            this.index.Width = 40;
            // 
            // oldName
            // 
            this.oldName.AspectName = "OldName";
            this.oldName.IsEditable = false;
            this.oldName.Sortable = false;
            this.oldName.Text = "Original Name";
            this.oldName.Width = 150;
            // 
            // outputName
            // 
            this.outputName.AspectName = "OutputName";
            this.outputName.Sortable = false;
            this.outputName.Text = "New Name";
            this.outputName.Width = 150;
            // 
            // VideoRenamer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 417);
            this.Controls.Add(this.olv1);
            this.Controls.Add(this.AutoMode);
            this.Controls.Add(this.AndroidStyleToggleSwitch);
            this.Controls.Add(this.Rename_button);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "VideoRenamer";
            this.Text = "VideoRenamer";
            this.Shown += new System.EventHandler(this.VideoRenamer_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olv1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.Button Rename_button;
        private System.ComponentModel.BackgroundWorker CheckForUpdates;
        private JCS.ToggleSwitch AndroidStyleToggleSwitch;
        private System.Windows.Forms.Label AutoMode;
        private BrightIdeasSoftware.ObjectListView olv1;
        private BrightIdeasSoftware.OLVColumn index;
        private BrightIdeasSoftware.OLVColumn oldName;
        private BrightIdeasSoftware.OLVColumn outputName;
        private BrightIdeasSoftware.OLVColumn checkBox;
    }
}

