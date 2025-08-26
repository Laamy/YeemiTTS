namespace YeemiTTS.ControlForm.Windows;

partial class HomeScene
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
            this.controlLabel1 = new ControlLabel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // controlLabel1
            // 
            this.controlLabel1.AutoSize = true;
            this.controlLabel1.BackColor = System.Drawing.Color.Transparent;
            this.controlLabel1.ForeColor = System.Drawing.SystemColors.Control;
            this.controlLabel1.Location = new System.Drawing.Point(12, 9);
            this.controlLabel1.Name = "controlLabel1";
            this.controlLabel1.Size = new System.Drawing.Size(65, 13);
            this.controlLabel1.TabIndex = 0;
            this.controlLabel1.Text = "home scene";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.SystemColors.Control;
            this.textBox1.Location = new System.Drawing.Point(15, 43);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(174, 153);
            this.textBox1.TabIndex = 1;
            // 
            // HomeScene
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.ClientSize = new System.Drawing.Size(343, 299);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.controlLabel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HomeScene";
            this.Text = "HomeScene";
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private ControlLabel controlLabel1;
    private System.Windows.Forms.TextBox textBox1;
}