namespace Kinect_v2
{
    partial class SampleRecordFrom
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtBox_fileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label_countdown = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(540, 423);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(558, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(552, 487);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(485, 450);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 39);
            this.button1.TabIndex = 2;
            this.button1.Text = "錄製";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtBox_fileName
            // 
            this.txtBox_fileName.Font = new System.Drawing.Font("新細明體", 12F);
            this.txtBox_fileName.Location = new System.Drawing.Point(200, 452);
            this.txtBox_fileName.Name = "txtBox_fileName";
            this.txtBox_fileName.Size = new System.Drawing.Size(189, 36);
            this.txtBox_fileName.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F);
            this.label1.Location = new System.Drawing.Point(64, 457);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 24);
            this.label1.TabIndex = 4;
            this.label1.Text = "檔案名稱：";
            // 
            // label_countdown
            // 
            this.label_countdown.AutoSize = true;
            this.label_countdown.Font = new System.Drawing.Font("新細明體", 16F);
            this.label_countdown.ForeColor = System.Drawing.Color.Red;
            this.label_countdown.Location = new System.Drawing.Point(809, 448);
            this.label_countdown.Name = "label_countdown";
            this.label_countdown.Size = new System.Drawing.Size(30, 32);
            this.label_countdown.TabIndex = 5;
            this.label_countdown.Text = "3";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(860, 452);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(221, 28);
            this.progressBar1.TabIndex = 6;
            // 
            // SampleRecordFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1122, 511);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label_countdown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBox_fileName);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "SampleRecordFrom";
            this.Text = "SampleRecordFrom";
            this.Load += new System.EventHandler(this.SampleRecordFrom_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtBox_fileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_countdown;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}