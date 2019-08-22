namespace UnityTestOutputRig
{
    partial class Form1
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.X_label1 = new System.Windows.Forms.Label();
            this.XtextBox = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.Ylabel = new System.Windows.Forms.Label();
            this.YtextBox = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.Z_label = new System.Windows.Forms.Label();
            this.ZtextBox = new System.Windows.Forms.TextBox();
            this.UpdateButton = new System.Windows.Forms.Button();
            this.ConsoleBufferflowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SendAsJsonCheckBox = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel2);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel3);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel4);
            this.flowLayoutPanel1.Controls.Add(this.UpdateButton);
            this.flowLayoutPanel1.Controls.Add(this.ConsoleBufferflowLayoutPanel);
            this.flowLayoutPanel1.Controls.Add(this.SendAsJsonCheckBox);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(836, 450);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.X_label1);
            this.flowLayoutPanel2.Controls.Add(this.XtextBox);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(200, 36);
            this.flowLayoutPanel2.TabIndex = 0;
            // 
            // X_label1
            // 
            this.X_label1.AutoSize = true;
            this.X_label1.Location = new System.Drawing.Point(3, 0);
            this.X_label1.Name = "X_label1";
            this.X_label1.Size = new System.Drawing.Size(23, 13);
            this.X_label1.TabIndex = 0;
            this.X_label1.Text = "X : ";
            // 
            // XtextBox
            // 
            this.XtextBox.Location = new System.Drawing.Point(32, 3);
            this.XtextBox.Name = "XtextBox";
            this.XtextBox.Size = new System.Drawing.Size(157, 20);
            this.XtextBox.TabIndex = 1;
            this.XtextBox.Text = "0";
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.Ylabel);
            this.flowLayoutPanel3.Controls.Add(this.YtextBox);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(209, 3);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(200, 36);
            this.flowLayoutPanel3.TabIndex = 1;
            // 
            // Ylabel
            // 
            this.Ylabel.AutoSize = true;
            this.Ylabel.Location = new System.Drawing.Point(3, 0);
            this.Ylabel.Name = "Ylabel";
            this.Ylabel.Size = new System.Drawing.Size(23, 13);
            this.Ylabel.TabIndex = 0;
            this.Ylabel.Text = "Y : ";
            // 
            // YtextBox
            // 
            this.YtextBox.Location = new System.Drawing.Point(32, 3);
            this.YtextBox.Name = "YtextBox";
            this.YtextBox.Size = new System.Drawing.Size(149, 20);
            this.YtextBox.TabIndex = 1;
            this.YtextBox.Text = "0";
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.Controls.Add(this.Z_label);
            this.flowLayoutPanel4.Controls.Add(this.ZtextBox);
            this.flowLayoutPanel4.Location = new System.Drawing.Point(415, 3);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(200, 36);
            this.flowLayoutPanel4.TabIndex = 2;
            // 
            // Z_label
            // 
            this.Z_label.AutoSize = true;
            this.Z_label.Location = new System.Drawing.Point(3, 0);
            this.Z_label.Name = "Z_label";
            this.Z_label.Size = new System.Drawing.Size(23, 13);
            this.Z_label.TabIndex = 0;
            this.Z_label.Text = "Z : ";
            // 
            // ZtextBox
            // 
            this.ZtextBox.Location = new System.Drawing.Point(32, 3);
            this.ZtextBox.Name = "ZtextBox";
            this.ZtextBox.Size = new System.Drawing.Size(157, 20);
            this.ZtextBox.TabIndex = 1;
            this.ZtextBox.Text = "0";
            // 
            // UpdateButton
            // 
            this.UpdateButton.Location = new System.Drawing.Point(621, 3);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(167, 36);
            this.UpdateButton.TabIndex = 4;
            this.UpdateButton.Text = "Update";
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // ConsoleBufferflowLayoutPanel
            // 
            this.ConsoleBufferflowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.ConsoleBufferflowLayoutPanel.Location = new System.Drawing.Point(3, 45);
            this.ConsoleBufferflowLayoutPanel.Name = "ConsoleBufferflowLayoutPanel";
            this.ConsoleBufferflowLayoutPanel.Size = new System.Drawing.Size(200, 346);
            this.ConsoleBufferflowLayoutPanel.TabIndex = 3;
            // 
            // SendAsJsonCheckBox
            // 
            this.SendAsJsonCheckBox.AutoSize = true;
            this.SendAsJsonCheckBox.Checked = true;
            this.SendAsJsonCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SendAsJsonCheckBox.Location = new System.Drawing.Point(209, 45);
            this.SendAsJsonCheckBox.Name = "SendAsJsonCheckBox";
            this.SendAsJsonCheckBox.Size = new System.Drawing.Size(85, 17);
            this.SendAsJsonCheckBox.TabIndex = 6;
            this.SendAsJsonCheckBox.Text = "SendAsJson";
            this.SendAsJsonCheckBox.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 450);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.flowLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label X_label1;
        private System.Windows.Forms.TextBox XtextBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Label Ylabel;
        private System.Windows.Forms.TextBox YtextBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.Label Z_label;
        private System.Windows.Forms.TextBox ZtextBox;
        private System.Windows.Forms.FlowLayoutPanel ConsoleBufferflowLayoutPanel;
        private System.Windows.Forms.Button UpdateButton;
        private System.Windows.Forms.CheckBox SendAsJsonCheckBox;
    }
}

