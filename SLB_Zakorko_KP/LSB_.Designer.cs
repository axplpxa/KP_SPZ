
using System.Windows.Forms;

namespace SLB_Zakorko_KP
{
    partial class LSB_
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LSB_));
            this.buttonwrite = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonread = new System.Windows.Forms.Button();
            this.textdev = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonwrite
            // 
            this.buttonwrite.Location = new System.Drawing.Point(12, 453);
            this.buttonwrite.Name = "buttonwrite";
            this.buttonwrite.Size = new System.Drawing.Size(115, 35);
            this.buttonwrite.TabIndex = 0;
            this.buttonwrite.Text = "Записати";
            this.buttonwrite.UseVisualStyleBackColor = true;
            this.buttonwrite.Click += new System.EventHandler(this.buttonwrite_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(162, 62);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(376, 376);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // buttonread
            // 
            this.buttonread.Location = new System.Drawing.Point(573, 453);
            this.buttonread.Name = "buttonread";
            this.buttonread.Size = new System.Drawing.Size(115, 35);
            this.buttonread.TabIndex = 2;
            this.buttonread.Text = "Прочитати";
            this.buttonread.UseVisualStyleBackColor = true;
            this.buttonread.Click += new System.EventHandler(this.buttonread_Click);
            // 
            // textdev
            // 
            this.textdev.Location = new System.Drawing.Point(221, 30);
            this.textdev.Name = "textdev";
            this.textdev.Size = new System.Drawing.Size(242, 20);
            this.textdev.TabIndex = 3;
            this.textdev.Text = "Закорко Сергій - Курсовий проект - СПЗ 2022 ";
            // 
            // LSB_
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(700, 500);
            this.Controls.Add(this.buttonread);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonwrite);
            this.Controls.Add(this.textdev);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LSB_";
            this.Text = "LSB - Stenography KP";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonwrite;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonread;
        private System.Windows.Forms.Label textdev;
    }
}

