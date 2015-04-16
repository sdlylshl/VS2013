namespace SocketHelperDemo
{
    partial class FrmClient
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
            this.BtnCon = new System.Windows.Forms.Button();
            this.TxtIp = new System.Windows.Forms.TextBox();
            this.TxtPort = new System.Windows.Forms.TextBox();
            this.MsgInfomationList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtSendMsg = new System.Windows.Forms.TextBox();
            this.BtnSend = new System.Windows.Forms.Button();
            this.StateInfoList = new System.Windows.Forms.ListBox();
            this.ErrorMsgList = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnCon
            // 
            this.BtnCon.Location = new System.Drawing.Point(292, 14);
            this.BtnCon.Name = "BtnCon";
            this.BtnCon.Size = new System.Drawing.Size(75, 23);
            this.BtnCon.TabIndex = 0;
            this.BtnCon.Text = "连接服务器";
            this.BtnCon.UseVisualStyleBackColor = true;
            this.BtnCon.Click += new System.EventHandler(this.BtnConClick);
            // 
            // TxtIp
            // 
            this.TxtIp.Location = new System.Drawing.Point(59, 16);
            this.TxtIp.Name = "TxtIp";
            this.TxtIp.Size = new System.Drawing.Size(100, 21);
            this.TxtIp.TabIndex = 1;
            this.TxtIp.Text = "127.0.0.1";
            // 
            // TxtPort
            // 
            this.TxtPort.Location = new System.Drawing.Point(199, 16);
            this.TxtPort.Name = "TxtPort";
            this.TxtPort.Size = new System.Drawing.Size(78, 21);
            this.TxtPort.TabIndex = 1;
            this.TxtPort.Text = "5100";
            // 
            // MsgInfomationList
            // 
            this.MsgInfomationList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MsgInfomationList.FormattingEnabled = true;
            this.MsgInfomationList.ItemHeight = 12;
            this.MsgInfomationList.Location = new System.Drawing.Point(199, 57);
            this.MsgInfomationList.Name = "MsgInfomationList";
            this.MsgInfomationList.Size = new System.Drawing.Size(385, 352);
            this.MsgInfomationList.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "服务IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(165, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "端口";
            // 
            // TxtSendMsg
            // 
            this.TxtSendMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtSendMsg.Location = new System.Drawing.Point(11, 430);
            this.TxtSendMsg.Name = "TxtSendMsg";
            this.TxtSendMsg.Size = new System.Drawing.Size(492, 21);
            this.TxtSendMsg.TabIndex = 5;
            // 
            // BtnSend
            // 
            this.BtnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSend.Location = new System.Drawing.Point(509, 428);
            this.BtnSend.Name = "BtnSend";
            this.BtnSend.Size = new System.Drawing.Size(75, 23);
            this.BtnSend.TabIndex = 0;
            this.BtnSend.Text = "发送数据";
            this.BtnSend.UseVisualStyleBackColor = true;
            this.BtnSend.Click += new System.EventHandler(this.BtnSendClick);
            // 
            // StateInfoList
            // 
            this.StateInfoList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.StateInfoList.FormattingEnabled = true;
            this.StateInfoList.ItemHeight = 12;
            this.StateInfoList.Location = new System.Drawing.Point(14, 57);
            this.StateInfoList.Name = "StateInfoList";
            this.StateInfoList.Size = new System.Drawing.Size(179, 220);
            this.StateInfoList.TabIndex = 2;
            // 
            // ErrorMsgList
            // 
            this.ErrorMsgList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ErrorMsgList.FormattingEnabled = true;
            this.ErrorMsgList.ItemHeight = 12;
            this.ErrorMsgList.Location = new System.Drawing.Point(14, 297);
            this.ErrorMsgList.Name = "ErrorMsgList";
            this.ErrorMsgList.Size = new System.Drawing.Size(179, 112);
            this.ErrorMsgList.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "状态信息列表";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 281);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "错误信息列表";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(373, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "断开连接";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.BtnCloseTcpClientClick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(454, 14);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "压力测试";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FrmClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 461);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TxtSendMsg);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ErrorMsgList);
            this.Controls.Add(this.StateInfoList);
            this.Controls.Add(this.MsgInfomationList);
            this.Controls.Add(this.TxtPort);
            this.Controls.Add(this.TxtIp);
            this.Controls.Add(this.BtnSend);
            this.Controls.Add(this.BtnCon);
            this.Name = "FrmClient";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnCon;
        private System.Windows.Forms.TextBox TxtIp;
        private System.Windows.Forms.TextBox TxtPort;
        private System.Windows.Forms.ListBox MsgInfomationList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtSendMsg;
        private System.Windows.Forms.Button BtnSend;
        private System.Windows.Forms.ListBox StateInfoList;
        private System.Windows.Forms.ListBox ErrorMsgList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

