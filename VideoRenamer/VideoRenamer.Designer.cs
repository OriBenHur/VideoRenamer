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
            this.listView = new System.Windows.Forms.ListView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.All_checkBox = new System.Windows.Forms.CheckBox();
            this.Rename_button = new System.Windows.Forms.Button();
            this.CheckForUpdates = new System.ComponentModel.BackgroundWorker();
            this.AndroidStyleToggleSwitch = new JCS.ToggleSwitch();
            this.AutoMode = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView.Location = new System.Drawing.Point(0, 28);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(617, 350);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView_ItemChecked);
            this.listView.Resize += new System.EventHandler(this.listView_Resize);
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
            // All_checkBox
            // 
            this.All_checkBox.AutoSize = true;
            this.All_checkBox.Location = new System.Drawing.Point(6, 36);
            this.All_checkBox.Name = "All_checkBox";
            this.All_checkBox.Size = new System.Drawing.Size(15, 14);
            this.All_checkBox.TabIndex = 2;
            this.All_checkBox.UseVisualStyleBackColor = true;
            this.All_checkBox.CheckedChanged += new System.EventHandler(this.All_checkBox_CheckedChanged);
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
            // AndroidStyleToggleSwitch
            // 
            this.AndroidStyleToggleSwitch.Size = new System.Drawing.Size(78, 23);
            this.AndroidStyleToggleSwitch.Style = JCS.ToggleSwitch.ToggleSwitchStyle.Android;
            this.AndroidStyleToggleSwitch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AndroidStyleToggleSwitch.Location = new System.Drawing.Point(92, 384);
            this.AndroidStyleToggleSwitch.Name = "AndroidStyleToggleSwitch";
            this.AndroidStyleToggleSwitch.OffForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(123)))), ((int)(((byte)(141)))));
            this.AndroidStyleToggleSwitch.OffText = "OFF";
            this.AndroidStyleToggleSwitch.OnFont = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.AndroidStyleToggleSwitch.OnForeColor = System.Drawing.Color.White;
            this.AndroidStyleToggleSwitch.OnText = "ON";
            
            this.AndroidStyleToggleSwitch.TabIndex = 4;
            this.AndroidStyleToggleSwitch.UseAnimation = true; //Default
            this.AndroidStyleToggleSwitch.AnimationInterval = 10; //Default
            this.AndroidStyleToggleSwitch.AnimationStep = 3; //Default
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
            // VideoRenamer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 417);
            this.Controls.Add(this.AutoMode);
            this.Controls.Add(this.AndroidStyleToggleSwitch);
            this.Controls.Add(this.Rename_button);
            this.Controls.Add(this.All_checkBox);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "VideoRenamer";
            this.Text = "VideoRenamer";
            this.Load += new System.EventHandler(this.VideoRenamer_Load);
            this.Shown += new System.EventHandler(this.VideoRenamer_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.CheckBox All_checkBox;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.Button Rename_button;
        private System.ComponentModel.BackgroundWorker CheckForUpdates;
        private JCS.ToggleSwitch AndroidStyleToggleSwitch;
        private System.Windows.Forms.Label AutoMode;
    }
}

