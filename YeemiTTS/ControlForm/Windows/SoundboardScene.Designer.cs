namespace YeemiTTS.ControlForm.Windows;

partial class SoundboardScene
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.controlLabel1 = new ControlLabel();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.ForeColor = System.Drawing.SystemColors.Control;
            this.textBox1.Location = new System.Drawing.Point(12, 250);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(305, 37);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "shut yo bitch ass the fuck up nigga i dont give a FUCK if you want or dont want i" +
    "t cuz I DO";
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(15, 38);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(293, 197);
            this.listView1.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView1.TabIndex = 2;
            this.listView1.TileSize = new System.Drawing.Size(100, 100);
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            // 
            // controlLabel1
            // 
            this.controlLabel1.AutoSize = true;
            this.controlLabel1.BackColor = System.Drawing.Color.Transparent;
            this.controlLabel1.ForeColor = System.Drawing.SystemColors.Control;
            this.controlLabel1.Location = new System.Drawing.Point(12, 9);
            this.controlLabel1.Name = "controlLabel1";
            this.controlLabel1.Size = new System.Drawing.Size(68, 13);
            this.controlLabel1.TabIndex = 0;
            this.controlLabel1.Text = "soundboards";
            // 
            // SoundboardScene
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.ClientSize = new System.Drawing.Size(343, 299);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.controlLabel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SoundboardScene";
            this.Text = "SoundboardScene";
            this.Activated += new System.EventHandler(this.SoundboardScene_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private ControlLabel controlLabel1;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.ListView listView1;
}