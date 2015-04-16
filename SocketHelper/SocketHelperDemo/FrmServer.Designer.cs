namespace SocketHelperDemo
{
    partial class FrmServer
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
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtSendMsg = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ErrorMsgList = new System.Windows.Forms.ListBox();
            this.StateInfoList = new System.Windows.Forms.ListBox();
            this.MsgInfomationList = new System.Windows.Forms.ListBox();
            this.TxtPort = new System.Windows.Forms.TextBox();
            this.TxtIp = new System.Windows.Forms.TextBox();
            this.BtnSend = new System.Windows.Forms.Button();
            this.BtnCon = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.ClientList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(223, 255);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 17;
            this.label4.Text = "错误信息列表";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(223, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 18;
            this.label3.Text = "状态信息列表";
            // 
            // TxtSendMsg
            // 
            this.TxtSendMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtSendMsg.Location = new System.Drawing.Point(225, 424);
            this.TxtSendMsg.Name = "TxtSendMsg";
            this.TxtSendMsg.Size = new System.Drawing.Size(508, 21);
            this.TxtSendMsg.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 15;
            this.label2.Text = "端口";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(466, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "服务IP";
            this.label1.Visible = false;
            // 
            // ErrorMsgList
            // 
            this.ErrorMsgList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ErrorMsgList.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ErrorMsgList.FormattingEnabled = true;
            this.ErrorMsgList.ItemHeight = 17;
            this.ErrorMsgList.Location = new System.Drawing.Point(225, 271);
            this.ErrorMsgList.Name = "ErrorMsgList";
            this.ErrorMsgList.Size = new System.Drawing.Size(319, 140);
            this.ErrorMsgList.TabIndex = 11;
            // 
            // StateInfoList
            // 
            this.StateInfoList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.StateInfoList.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.StateInfoList.FormattingEnabled = true;
            this.StateInfoList.ItemHeight = 17;
            this.StateInfoList.Location = new System.Drawing.Point(225, 48);
            this.StateInfoList.Name = "StateInfoList";
            this.StateInfoList.Size = new System.Drawing.Size(319, 191);
            this.StateInfoList.TabIndex = 12;
            // 
            // MsgInfomationList
            // 
            this.MsgInfomationList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MsgInfomationList.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MsgInfomationList.FormattingEnabled = true;
            this.MsgInfomationList.ItemHeight = 17;
            this.MsgInfomationList.Location = new System.Drawing.Point(550, 50);
            this.MsgInfomationList.Name = "MsgInfomationList";
            this.MsgInfomationList.Size = new System.Drawing.Size(261, 361);
            this.MsgInfomationList.TabIndex = 13;
            // 
            // TxtPort
            // 
            this.TxtPort.Location = new System.Drawing.Point(51, 7);
            this.TxtPort.Name = "TxtPort";
            this.TxtPort.Size = new System.Drawing.Size(78, 21);
            this.TxtPort.TabIndex = 9;
            this.TxtPort.Text = "5100";
            // 
            // TxtIp
            // 
            this.TxtIp.Location = new System.Drawing.Point(513, 9);
            this.TxtIp.Name = "TxtIp";
            this.TxtIp.Size = new System.Drawing.Size(100, 21);
            this.TxtIp.TabIndex = 10;
            this.TxtIp.Text = "127.0.0.1";
            this.TxtIp.Visible = false;
            // 
            // BtnSend
            // 
            this.BtnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSend.Location = new System.Drawing.Point(739, 422);
            this.BtnSend.Name = "BtnSend";
            this.BtnSend.Size = new System.Drawing.Size(75, 23);
            this.BtnSend.TabIndex = 7;
            this.BtnSend.Text = "发送数据";
            this.BtnSend.UseVisualStyleBackColor = true;
            this.BtnSend.Click += new System.EventHandler(this.BtnSend_Click);
            // 
            // BtnCon
            // 
            this.BtnCon.Location = new System.Drawing.Point(144, 7);
            this.BtnCon.Name = "BtnCon";
            this.BtnCon.Size = new System.Drawing.Size(75, 23);
            this.BtnCon.TabIndex = 8;
            this.BtnCon.Text = "启动监听";
            this.BtnCon.UseVisualStyleBackColor = true;
            this.BtnCon.Click += new System.EventHandler(this.BtnCon_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 18;
            this.label5.Text = "客户端列表";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(225, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 20;
            this.button1.Text = "启动客户端";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ClientList
            // 
            this.ClientList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ClientList.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ClientList.FormattingEnabled = true;
            this.ClientList.ItemHeight = 17;
            this.ClientList.Location = new System.Drawing.Point(14, 50);
            this.ClientList.Name = "ClientList";
            this.ClientList.Size = new System.Drawing.Size(205, 395);
            this.ClientList.TabIndex = 12;
            // 
            // FrmServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 455);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TxtSendMsg);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ErrorMsgList);
            this.Controls.Add(this.ClientList);
            this.Controls.Add(this.StateInfoList);
            this.Controls.Add(this.MsgInfomationList);
            this.Controls.Add(this.TxtPort);
            this.Controls.Add(this.TxtIp);
            this.Controls.Add(this.BtnSend);
            this.Controls.Add(this.BtnCon);
            this.Name = "FrmServer";
            this.Text = "FrmServer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmServer_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmServer_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtSendMsg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox ErrorMsgList;
        private System.Windows.Forms.ListBox StateInfoList;
        private System.Windows.Forms.ListBox MsgInfomationList;
        private System.Windows.Forms.TextBox TxtPort;
        private System.Windows.Forms.TextBox TxtIp;
        private System.Windows.Forms.Button BtnSend;
        private System.Windows.Forms.Button BtnCon;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox ClientList;
    }
}