namespace GeoTiffCoordRef.Forms
{
    partial class GeoTiffCoordRef_Form
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeoTiffCoordRef_Form));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxTiffFolder = new System.Windows.Forms.TextBox();
            this.btnTiffFolder = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.dgvStatus = new System.Windows.Forms.DataGridView();
            this.clmThreadID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmTimeStamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblNetworkOffline = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numOfProcesses = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.cboProjection = new System.Windows.Forms.ComboBox();
            this.cboEPSG = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnHoverDam = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkUpdateMetadataTags = new System.Windows.Forms.CheckBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOfProcesses)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(474, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "A&bout";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tiff Folder";
            // 
            // tbxTiffFolder
            // 
            this.tbxTiffFolder.AllowDrop = true;
            this.tbxTiffFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxTiffFolder.Location = new System.Drawing.Point(79, 29);
            this.tbxTiffFolder.Name = "tbxTiffFolder";
            this.tbxTiffFolder.Size = new System.Drawing.Size(357, 21);
            this.tbxTiffFolder.TabIndex = 2;
            this.tbxTiffFolder.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbxTiffFolder_DragDrop);
            this.tbxTiffFolder.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbxTiffFolder_DragEnter);
            // 
            // btnTiffFolder
            // 
            this.btnTiffFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTiffFolder.ImageIndex = 0;
            this.btnTiffFolder.ImageList = this.imageList1;
            this.btnTiffFolder.Location = new System.Drawing.Point(439, 27);
            this.btnTiffFolder.Name = "btnTiffFolder";
            this.btnTiffFolder.Size = new System.Drawing.Size(23, 23);
            this.btnTiffFolder.TabIndex = 3;
            this.btnTiffFolder.UseVisualStyleBackColor = true;
            this.btnTiffFolder.Click += new System.EventHandler(this.btnTiffFolder_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList1.Images.SetKeyName(0, "browse.bmp");
            // 
            // dgvStatus
            // 
            this.dgvStatus.AllowUserToAddRows = false;
            this.dgvStatus.AllowUserToDeleteRows = false;
            this.dgvStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStatus.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmThreadID,
            this.clmName,
            this.clmStatus,
            this.clmTimeStamp});
            this.dgvStatus.Location = new System.Drawing.Point(8, 148);
            this.dgvStatus.Name = "dgvStatus";
            this.dgvStatus.ReadOnly = true;
            this.dgvStatus.Size = new System.Drawing.Size(458, 294);
            this.dgvStatus.TabIndex = 13;
            // 
            // clmThreadID
            // 
            this.clmThreadID.HeaderText = "ThreadID";
            this.clmThreadID.Name = "clmThreadID";
            this.clmThreadID.ReadOnly = true;
            this.clmThreadID.Visible = false;
            // 
            // clmName
            // 
            this.clmName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.clmName.HeaderText = "File Name";
            this.clmName.Name = "clmName";
            this.clmName.ReadOnly = true;
            // 
            // clmStatus
            // 
            this.clmStatus.HeaderText = "Status";
            this.clmStatus.Name = "clmStatus";
            this.clmStatus.ReadOnly = true;
            // 
            // clmTimeStamp
            // 
            this.clmTimeStamp.HeaderText = "Time Stamp";
            this.clmTimeStamp.Name = "clmTimeStamp";
            this.clmTimeStamp.ReadOnly = true;
            // 
            // btnStop
            // 
            this.btnStop.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnStop.Enabled = false;
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.ForeColor = System.Drawing.Color.Red;
            this.btnStop.Location = new System.Drawing.Point(243, 451);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 32);
            this.btnStop.TabIndex = 15;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.ForeColor = System.Drawing.Color.Green;
            this.btnStart.Location = new System.Drawing.Point(148, 451);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 32);
            this.btnStart.TabIndex = 14;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(356, 468);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(0, 15);
            this.lblVersion.TabIndex = 16;
            // 
            // lblNetworkOffline
            // 
            this.lblNetworkOffline.AutoSize = true;
            this.lblNetworkOffline.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNetworkOffline.ForeColor = System.Drawing.Color.Red;
            this.lblNetworkOffline.Location = new System.Drawing.Point(115, 234);
            this.lblNetworkOffline.Name = "lblNetworkOffline";
            this.lblNetworkOffline.Size = new System.Drawing.Size(241, 24);
            this.lblNetworkOffline.TabIndex = 17;
            this.lblNetworkOffline.Text = "Searching For Directory...";
            this.lblNetworkOffline.Visible = false;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(209, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 15);
            this.label2.TabIndex = 18;
            this.label2.Text = "Number of concurrent updates";
            // 
            // numOfProcesses
            // 
            this.numOfProcesses.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numOfProcesses.Location = new System.Drawing.Point(388, 121);
            this.numOfProcesses.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numOfProcesses.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numOfProcesses.Name = "numOfProcesses";
            this.numOfProcesses.Size = new System.Drawing.Size(78, 21);
            this.numOfProcesses.TabIndex = 19;
            this.numOfProcesses.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numOfProcesses.ValueChanged += new System.EventHandler(this.numOfProcesses_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 15);
            this.label3.TabIndex = 20;
            this.label3.Text = "Projection";
            // 
            // cboProjection
            // 
            this.cboProjection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboProjection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProjection.DropDownWidth = 500;
            this.cboProjection.FormattingEnabled = true;
            this.cboProjection.Location = new System.Drawing.Point(81, 59);
            this.cboProjection.Name = "cboProjection";
            this.cboProjection.Size = new System.Drawing.Size(355, 23);
            this.cboProjection.Sorted = true;
            this.cboProjection.TabIndex = 21;
            this.cboProjection.SelectedIndexChanged += new System.EventHandler(this.cboProjection_SelectedIndexChanged);
            // 
            // cboEPSG
            // 
            this.cboEPSG.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboEPSG.DropDownWidth = 142;
            this.cboEPSG.FormattingEnabled = true;
            this.cboEPSG.Location = new System.Drawing.Point(81, 88);
            this.cboEPSG.Name = "cboEPSG";
            this.cboEPSG.Size = new System.Drawing.Size(142, 23);
            this.cboEPSG.Sorted = true;
            this.cboEPSG.TabIndex = 23;
            this.cboEPSG.SelectedIndexChanged += new System.EventHandler(this.cboEPSG_SelectedIndexChanged);
            this.cboEPSG.TextUpdate += new System.EventHandler(this.cboEPSG_TextUpdate);
            this.cboEPSG.Validating += new System.ComponentModel.CancelEventHandler(this.cboEPSG_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 15);
            this.label4.TabIndex = 22;
            this.label4.Text = "EPSG";
            // 
            // btnHoverDam
            // 
            this.btnHoverDam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHoverDam.BackColor = System.Drawing.SystemColors.Info;
            this.btnHoverDam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHoverDam.Location = new System.Drawing.Point(439, 59);
            this.btnHoverDam.Name = "btnHoverDam";
            this.btnHoverDam.Size = new System.Drawing.Size(27, 23);
            this.btnHoverDam.TabIndex = 24;
            this.btnHoverDam.Text = "...";
            this.toolTip1.SetToolTip(this.btnHoverDam, "Spatial Reference Definition");
            this.btnHoverDam.UseVisualStyleBackColor = false;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.BackColor = System.Drawing.Color.White;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // chkUpdateMetadataTags
            // 
            this.chkUpdateMetadataTags.AutoSize = true;
            this.chkUpdateMetadataTags.Checked = true;
            this.chkUpdateMetadataTags.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUpdateMetadataTags.Location = new System.Drawing.Point(16, 123);
            this.chkUpdateMetadataTags.Name = "chkUpdateMetadataTags";
            this.chkUpdateMetadataTags.Size = new System.Drawing.Size(151, 19);
            this.chkUpdateMetadataTags.TabIndex = 25;
            this.chkUpdateMetadataTags.Text = "Update Metadata Tags";
            this.chkUpdateMetadataTags.UseVisualStyleBackColor = true;
            // 
            // GeoTiffCoordRef_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 492);
            this.Controls.Add(this.chkUpdateMetadataTags);
            this.Controls.Add(this.btnHoverDam);
            this.Controls.Add(this.cboEPSG);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboProjection);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numOfProcesses);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblNetworkOffline);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.dgvStatus);
            this.Controls.Add(this.btnTiffFolder);
            this.Controls.Add(this.tbxTiffFolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GeoTiffCoordRef_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GeoTiff Spatial Reference Update";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GeoTiffCoordRef_Form_FormClosing);
            this.Load += new System.EventHandler(this.GeoTiffCoordRef_Form_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOfProcesses)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxTiffFolder;
        private System.Windows.Forms.Button btnTiffFolder;
        public System.Windows.Forms.DataGridView dgvStatus;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblVersion;
        public System.Windows.Forms.Label lblNetworkOffline;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numOfProcesses;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmThreadID;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmName;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmTimeStamp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboProjection;
        private System.Windows.Forms.ComboBox cboEPSG;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnHoverDam;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chkUpdateMetadataTags;

    }
}

