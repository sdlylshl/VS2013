namespace SyncChatServer
{
    partial class Main_Form
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.stateInfo = new System.Windows.Forms.GroupBox();
            this.rtb_State = new System.Windows.Forms.RichTextBox();
            this.btn_listener = new System.Windows.Forms.Button();
            this.btn_Stop = new System.Windows.Forms.Button();
            this.cmb_Address = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.stateInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // stateInfo
            // 
            this.stateInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stateInfo.Controls.Add(this.rtb_State);
            this.stateInfo.Location = new System.Drawing.Point(13, 41);
            this.stateInfo.Name = "stateInfo";
            this.stateInfo.Size = new System.Drawing.Size(422, 244);
            this.stateInfo.TabIndex = 1;
            this.stateInfo.TabStop = false;
            this.stateInfo.Text = "状态信息";
            // 
            // rtb_State
            // 
            this.rtb_State.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtb_State.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb_State.Location = new System.Drawing.Point(6, 20);
            this.rtb_State.Name = "rtb_State";
            this.rtb_State.Size = new System.Drawing.Size(410, 218);
            this.rtb_State.TabIndex = 1;
            this.rtb_State.Text = "";
            // 
            // btn_listener
            // 
            this.btn_listener.Location = new System.Drawing.Point(273, 12);
            this.btn_listener.Name = "btn_listener";
            this.btn_listener.Size = new System.Drawing.Size(75, 23);
            this.btn_listener.TabIndex = 2;
            this.btn_listener.Text = "启动监听";
            this.btn_listener.UseVisualStyleBackColor = true;
            this.btn_listener.Click += new System.EventHandler(this.btn_listener_Click);
            // 
            // btn_Stop
            // 
            this.btn_Stop.Location = new System.Drawing.Point(354, 12);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(75, 23);
            this.btn_Stop.TabIndex = 3;
            this.btn_Stop.Text = "停止监听";
            this.btn_Stop.UseVisualStyleBackColor = true;
            this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
            // 
            // cmb_Address
            // 
            this.cmb_Address.FormattingEnabled = true;
            this.cmb_Address.Location = new System.Drawing.Point(46, 12);
            this.cmb_Address.Name = "cmb_Address";
            this.cmb_Address.Size = new System.Drawing.Size(197, 20);
            this.cmb_Address.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "IP:";
            // 
            // Main_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 297);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmb_Address);
            this.Controls.Add(this.btn_Stop);
            this.Controls.Add(this.btn_listener);
            this.Controls.Add(this.stateInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Main_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "同步聊天程序服务端";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_Form_FormClosing);
            this.Load += new System.EventHandler(this.Main_Form_Load);
            this.stateInfo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox stateInfo;
        private System.Windows.Forms.RichTextBox rtb_State;
        private System.Windows.Forms.Button btn_listener;
        private System.Windows.Forms.Button btn_Stop;
        private System.Windows.Forms.ComboBox cmb_Address;
        private System.Windows.Forms.Label label1;
    }
}

