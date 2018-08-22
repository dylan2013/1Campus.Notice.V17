namespace _1campus.notice.v17
{
    partial class LogViewForm
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
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.lbTime = new DevComponents.DotNetBar.LabelX();
            this.lbUser = new DevComponents.DotNetBar.LabelX();
            this.lbTitle = new DevComponents.DotNetBar.LabelX();
            this.lbContent = new DevComponents.DotNetBar.LabelX();
            this.lbSendToUser = new DevComponents.DotNetBar.LabelX();
            this.cbParent = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.cbStudent = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(707, 605);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 25);
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExit.TabIndex = 8;
            this.btnExit.Text = "離開";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lbTime
            // 
            this.lbTime.AutoSize = true;
            this.lbTime.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbTime.BackgroundStyle.Class = "";
            this.lbTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbTime.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbTime.Location = new System.Drawing.Point(12, 12);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(90, 26);
            this.lbTime.TabIndex = 0;
            this.lbTime.Text = "發送時間：";
            // 
            // lbUser
            // 
            this.lbUser.AutoSize = true;
            this.lbUser.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbUser.BackgroundStyle.Class = "";
            this.lbUser.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbUser.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbUser.Location = new System.Drawing.Point(291, 12);
            this.lbUser.Name = "lbUser";
            this.lbUser.Size = new System.Drawing.Size(90, 26);
            this.lbUser.TabIndex = 1;
            this.lbUser.Text = "發送人員：";
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbTitle.BackgroundStyle.Class = "";
            this.lbTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbTitle.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbTitle.Location = new System.Drawing.Point(12, 55);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(90, 26);
            this.lbTitle.TabIndex = 5;
            this.lbTitle.Text = "標　　題：";
            // 
            // lbContent
            // 
            this.lbContent.AutoSize = true;
            this.lbContent.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbContent.BackgroundStyle.Class = "";
            this.lbContent.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbContent.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbContent.Location = new System.Drawing.Point(12, 98);
            this.lbContent.Name = "lbContent";
            this.lbContent.Size = new System.Drawing.Size(90, 26);
            this.lbContent.TabIndex = 6;
            this.lbContent.Text = "訊息內容：";
            // 
            // lbSendToUser
            // 
            this.lbSendToUser.AutoSize = true;
            this.lbSendToUser.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbSendToUser.BackgroundStyle.Class = "";
            this.lbSendToUser.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbSendToUser.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbSendToUser.Location = new System.Drawing.Point(538, 12);
            this.lbSendToUser.Name = "lbSendToUser";
            this.lbSendToUser.Size = new System.Drawing.Size(90, 26);
            this.lbSendToUser.TabIndex = 2;
            this.lbSendToUser.Text = "發送對象：";
            // 
            // cbParent
            // 
            this.cbParent.AutoSize = true;
            this.cbParent.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.cbParent.BackgroundStyle.Class = "";
            this.cbParent.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.cbParent.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbParent.Location = new System.Drawing.Point(636, 12);
            this.cbParent.Name = "cbParent";
            this.cbParent.Size = new System.Drawing.Size(61, 26);
            this.cbParent.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbParent.TabIndex = 3;
            this.cbParent.Text = "家長";
            // 
            // cbStudent
            // 
            this.cbStudent.AutoSize = true;
            this.cbStudent.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.cbStudent.BackgroundStyle.Class = "";
            this.cbStudent.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.cbStudent.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbStudent.Location = new System.Drawing.Point(715, 12);
            this.cbStudent.Name = "cbStudent";
            this.cbStudent.Size = new System.Drawing.Size(61, 26);
            this.cbStudent.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbStudent.TabIndex = 4;
            this.cbStudent.Text = "學生";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Location = new System.Drawing.Point(12, 136);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(772, 456);
            this.panel1.TabIndex = 7;
            // 
            // LogViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 638);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cbStudent);
            this.Controls.Add(this.cbParent);
            this.Controls.Add(this.lbSendToUser);
            this.Controls.Add(this.lbContent);
            this.Controls.Add(this.lbTitle);
            this.Controls.Add(this.lbUser);
            this.Controls.Add(this.lbTime);
            this.Controls.Add(this.btnExit);
            this.DoubleBuffered = true;
            this.MaximizeBox = true;
            this.Name = "LogViewForm";
            this.Text = "詳細內容";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.LabelX lbTime;
        private DevComponents.DotNetBar.LabelX lbUser;
        private DevComponents.DotNetBar.LabelX lbTitle;
        private DevComponents.DotNetBar.LabelX lbContent;
        private DevComponents.DotNetBar.LabelX lbSendToUser;
        private DevComponents.DotNetBar.Controls.CheckBoxX cbParent;
        private DevComponents.DotNetBar.Controls.CheckBoxX cbStudent;
        private System.Windows.Forms.Panel panel1;
    }
}