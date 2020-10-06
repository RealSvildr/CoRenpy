namespace CoRenpy
{
    partial class Main
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.processText = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.processPath = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cMod = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 89);
            this.textBox1.MaxLength = 9999999;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(822, 459);
            this.textBox1.TabIndex = 0;
            // 
            // processText
            // 
            this.processText.Location = new System.Drawing.Point(706, 559);
            this.processText.Name = "processText";
            this.processText.Size = new System.Drawing.Size(128, 23);
            this.processText.TabIndex = 1;
            this.processText.Text = "Process Text";
            this.processText.UseVisualStyleBackColor = true;
            this.processText.Click += new System.EventHandler(this.processText_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Multiselect = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 27);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(621, 20);
            this.textBox2.TabIndex = 2;
            // 
            // processPath
            // 
            this.processPath.Location = new System.Drawing.Point(639, 25);
            this.processPath.Name = "processPath";
            this.processPath.Size = new System.Drawing.Size(93, 23);
            this.processPath.TabIndex = 3;
            this.processPath.Text = "Process Path";
            this.processPath.UseVisualStyleBackColor = true;
            this.processPath.Click += new System.EventHandler(this.processPath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(236, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Process All files inside a folder and it\'s sub-folder:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Process Inserted Text";
            // 
            // cMod
            // 
            this.cMod.AutoSize = true;
            this.cMod.Location = new System.Drawing.Point(15, 554);
            this.cMod.Name = "cMod";
            this.cMod.Size = new System.Drawing.Size(148, 17);
            this.cMod.TabIndex = 6;
            this.cMod.Text = "Add \'_mod\' tag on Images";
            this.cMod.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 594);
            this.Controls.Add(this.cMod);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.processPath);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.processText);
            this.Controls.Add(this.textBox1);
            this.Name = "Main";
            this.Text = "CoRenpy";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button processText;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button processPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cMod;
    }
}

