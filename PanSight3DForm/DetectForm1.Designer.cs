namespace PanSight3DForm
{
    partial class DetectForm1
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.localIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cameraIp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cameraType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cameraStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_connect = new System.Windows.Forms.Button();
            this.btn_scan = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 236F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(817, 359);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.localIP,
            this.cameraIp,
            this.cameraType,
            this.cameraStatus});
            this.tableLayoutPanel1.SetColumnSpan(this.dataGridView1, 2);
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(4, 4);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(809, 228);
            this.dataGridView1.TabIndex = 0;
            // 
            // localIP
            // 
            this.localIP.HeaderText = "本地IP";
            this.localIP.MinimumWidth = 6;
            this.localIP.Name = "localIP";
            this.localIP.ReadOnly = true;
            this.localIP.Width = 125;
            // 
            // cameraIp
            // 
            this.cameraIp.HeaderText = "相机IP";
            this.cameraIp.MinimumWidth = 6;
            this.cameraIp.Name = "cameraIp";
            this.cameraIp.ReadOnly = true;
            this.cameraIp.Width = 125;
            // 
            // cameraType
            // 
            this.cameraType.HeaderText = "相机型号";
            this.cameraType.MinimumWidth = 6;
            this.cameraType.Name = "cameraType";
            this.cameraType.ReadOnly = true;
            this.cameraType.Width = 125;
            // 
            // cameraStatus
            // 
            this.cameraStatus.HeaderText = "连接状态";
            this.cameraStatus.MinimumWidth = 6;
            this.cameraStatus.Name = "cameraStatus";
            this.cameraStatus.ReadOnly = true;
            this.cameraStatus.Width = 125;
            // 
            // panel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.btn_connect);
            this.panel1.Controls.Add(this.btn_scan);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(4, 240);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(809, 115);
            this.panel1.TabIndex = 1;
            // 
            // btn_connect
            // 
            this.btn_connect.Location = new System.Drawing.Point(670, 32);
            this.btn_connect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(111, 48);
            this.btn_connect.TabIndex = 1;
            this.btn_connect.Text = "连接";
            this.btn_connect.UseVisualStyleBackColor = true;
            this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
            // 
            // btn_scan
            // 
            this.btn_scan.Location = new System.Drawing.Point(12, 32);
            this.btn_scan.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_scan.Name = "btn_scan";
            this.btn_scan.Size = new System.Drawing.Size(111, 48);
            this.btn_scan.TabIndex = 0;
            this.btn_scan.Text = "搜索";
            this.btn_scan.UseVisualStyleBackColor = true;
            this.btn_scan.Click += new System.EventHandler(this.btn_scan_Click);
            // 
            // DetectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 359);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "DetectForm";
            this.Text = "DetectForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DetectForm_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn localIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn cameraIp;
        private System.Windows.Forms.DataGridViewTextBoxColumn cameraType;
        private System.Windows.Forms.DataGridViewTextBoxColumn cameraStatus;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_connect;
        private System.Windows.Forms.Button btn_scan;
    }
}