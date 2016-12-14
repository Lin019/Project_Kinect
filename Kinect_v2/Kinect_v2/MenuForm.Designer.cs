namespace Kinect_v2
{
    partial class MenuForm
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
            this.btnHandJoint = new System.Windows.Forms.Button();
            this.btnRecordSample = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnHandJoint
            // 
            this.btnHandJoint.Location = new System.Drawing.Point(128, 110);
            this.btnHandJoint.Margin = new System.Windows.Forms.Padding(4);
            this.btnHandJoint.Name = "btnHandJoint";
            this.btnHandJoint.Size = new System.Drawing.Size(154, 34);
            this.btnHandJoint.TabIndex = 0;
            this.btnHandJoint.Text = "手部骨架偵測";
            this.btnHandJoint.UseVisualStyleBackColor = true;
            this.btnHandJoint.Click += new System.EventHandler(this.btnHandJoint_Click);
            // 
            // btnRecordSample
            // 
            this.btnRecordSample.Location = new System.Drawing.Point(128, 207);
            this.btnRecordSample.Name = "btnRecordSample";
            this.btnRecordSample.Size = new System.Drawing.Size(154, 36);
            this.btnRecordSample.TabIndex = 1;
            this.btnRecordSample.Text = "錄製標準動作";
            this.btnRecordSample.UseVisualStyleBackColor = true;
            this.btnRecordSample.Click += new System.EventHandler(this.btnRecordSample_Click);
            // 
            // MenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 392);
            this.Controls.Add(this.btnRecordSample);
            this.Controls.Add(this.btnHandJoint);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MenuForm";
            this.Text = "Menu";
            this.Load += new System.EventHandler(this.MenuForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnHandJoint;
        private System.Windows.Forms.Button btnRecordSample;
    }
}