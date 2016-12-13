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
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnHandJoint
            // 
            this.btnHandJoint.Location = new System.Drawing.Point(128, 110);
            this.btnHandJoint.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnHandJoint.Name = "btnHandJoint";
            this.btnHandJoint.Size = new System.Drawing.Size(154, 34);
            this.btnHandJoint.TabIndex = 0;
            this.btnHandJoint.Text = "手部骨架偵測";
            this.btnHandJoint.UseVisualStyleBackColor = true;
            this.btnHandJoint.Click += new System.EventHandler(this.btnHandJoint_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(128, 207);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(154, 36);
            this.button1.TabIndex = 1;
            this.button1.Text = "錄製標準動作";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 392);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnHandJoint);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MenuForm";
            this.Text = "Menu";
            this.Load += new System.EventHandler(this.MenuForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnHandJoint;
        private System.Windows.Forms.Button button1;
    }
}