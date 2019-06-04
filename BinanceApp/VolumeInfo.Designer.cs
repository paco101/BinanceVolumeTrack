using System.ComponentModel;

namespace BinanceApp
{
    partial class VolumeInfo
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.coindatagridview = new System.Windows.Forms.DataGridView();
			this.alerticon = new System.Windows.Forms.NotifyIcon(this.components);
			this.pTop = new System.Windows.Forms.Panel();
			this.chkLiveTrading = new System.Windows.Forms.CheckBox();
			this.chkShowNowPercent = new System.Windows.Forms.CheckBox();
			this.RefreshButton = new System.Windows.Forms.Button();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.buttonStart = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.coindatagridview)).BeginInit();
			this.pTop.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
			// 
			// coindatagridview
			// 
			this.coindatagridview.AllowUserToAddRows = false;
			this.coindatagridview.AllowUserToDeleteRows = false;
			this.coindatagridview.AllowUserToOrderColumns = true;
			this.coindatagridview.AllowUserToResizeColumns = false;
			this.coindatagridview.AllowUserToResizeRows = false;
			this.coindatagridview.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.coindatagridview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.coindatagridview.DefaultCellStyle = dataGridViewCellStyle1;
			this.coindatagridview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.coindatagridview.Location = new System.Drawing.Point(0, 42);
			this.coindatagridview.Name = "coindatagridview";
			this.coindatagridview.ReadOnly = true;
			this.coindatagridview.RowHeadersVisible = false;
			this.coindatagridview.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.coindatagridview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.coindatagridview.Size = new System.Drawing.Size(927, 319);
			this.coindatagridview.TabIndex = 3;
			this.coindatagridview.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.coindatagridview_CellContentDoubleClick);
			this.coindatagridview.SelectionChanged += new System.EventHandler(this.coindatagridview_SelectionChanged);
			this.coindatagridview.Sorted += new System.EventHandler(this.coindatagridview_Sorted);
			// 
			// alerticon
			// 
			this.alerticon.Text = "notifyIcon1";
			this.alerticon.Visible = true;
			// 
			// pTop
			// 
			this.pTop.Controls.Add(this.buttonStart);
			this.pTop.Controls.Add(this.chkLiveTrading);
			this.pTop.Controls.Add(this.chkShowNowPercent);
			this.pTop.Controls.Add(this.RefreshButton);
			this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pTop.Location = new System.Drawing.Point(0, 0);
			this.pTop.Name = "pTop";
			this.pTop.Size = new System.Drawing.Size(927, 42);
			this.pTop.TabIndex = 5;
			// 
			// chkLiveTrading
			// 
			this.chkLiveTrading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkLiveTrading.AutoSize = true;
			this.chkLiveTrading.Location = new System.Drawing.Point(582, 12);
			this.chkLiveTrading.Name = "chkLiveTrading";
			this.chkLiveTrading.Size = new System.Drawing.Size(85, 17);
			this.chkLiveTrading.TabIndex = 7;
			this.chkLiveTrading.Text = "Live Trading";
			this.chkLiveTrading.UseVisualStyleBackColor = true;
			this.chkLiveTrading.CheckedChanged += new System.EventHandler(this.chkLiveTrading_CheckedChanged);
			// 
			// chkShowNowPercent
			// 
			this.chkShowNowPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkShowNowPercent.AutoSize = true;
			this.chkShowNowPercent.Location = new System.Drawing.Point(754, 12);
			this.chkShowNowPercent.Name = "chkShowNowPercent";
			this.chkShowNowPercent.Size = new System.Drawing.Size(161, 17);
			this.chkShowNowPercent.TabIndex = 6;
			this.chkShowNowPercent.Text = "Show only (NowPercent > 0)";
			this.chkShowNowPercent.UseVisualStyleBackColor = true;
			this.chkShowNowPercent.CheckedChanged += new System.EventHandler(this.chkShowNowPercent_CheckedChanged);
			// 
			// RefreshButton
			// 
			this.RefreshButton.ForeColor = System.Drawing.SystemColors.ControlText;
			this.RefreshButton.Location = new System.Drawing.Point(128, 3);
			this.RefreshButton.Name = "RefreshButton";
			this.RefreshButton.Size = new System.Drawing.Size(117, 36);
			this.RefreshButton.TabIndex = 5;
			this.RefreshButton.Text = "Refresh";
			this.RefreshButton.UseVisualStyleBackColor = true;
			this.RefreshButton.Visible = false;
			this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click_1);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Location = new System.Drawing.Point(0, 361);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(927, 22);
			this.statusStrip1.TabIndex = 6;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// buttonStart
			// 
			this.buttonStart.ForeColor = System.Drawing.SystemColors.ControlText;
			this.buttonStart.Location = new System.Drawing.Point(7, 3);
			this.buttonStart.Name = "buttonStart";
			this.buttonStart.Size = new System.Drawing.Size(117, 36);
			this.buttonStart.TabIndex = 8;
			this.buttonStart.Text = "Start";
			this.buttonStart.UseVisualStyleBackColor = true;
			this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
			// 
			// VolumeInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.ClientSize = new System.Drawing.Size(927, 383);
			this.Controls.Add(this.coindatagridview);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.pTop);
			this.Name = "VolumeInfo";
			this.Opacity = 0.8D;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Coin Volume";
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VolumeInfo_FormClosing);
			this.Load += new System.EventHandler(this.VolumeInfo_Load);
			((System.ComponentModel.ISupportInitialize)(this.coindatagridview)).EndInit();
			this.pTop.ResumeLayout(false);
			this.pTop.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.DataGridView coindatagridview;
        private System.Windows.Forms.NotifyIcon alerticon;
        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.CheckBox chkShowNowPercent;
        private System.Windows.Forms.CheckBox chkLiveTrading;
		private System.Windows.Forms.Button buttonStart;
	}
}

