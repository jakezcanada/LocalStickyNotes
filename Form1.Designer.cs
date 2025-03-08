namespace LocalStickyNotes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtNote = new System.Windows.Forms.TextBox();
            this.titleBarPanel = new System.Windows.Forms.Panel();
            this.btnPin = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.titleBarPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtNote
            // 
            this.txtNote.Location = new System.Drawing.Point(0, 30);
            this.txtNote.Multiline = true;
            this.txtNote.Name = "txtNote";
            this.txtNote.Size = new System.Drawing.Size(284, 433);
            this.txtNote.TabIndex = 0;
            this.txtNote.TextChanged += new System.EventHandler(this.TxtNote_TextChanged);
            // 
            // titleBarPanel
            // 
            this.titleBarPanel.Controls.Add(this.btnPin);
            this.titleBarPanel.Controls.Add(this.btnClose);
            this.titleBarPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleBarPanel.Location = new System.Drawing.Point(0, 0);
            this.titleBarPanel.Name = "titleBarPanel";
            this.titleBarPanel.Size = new System.Drawing.Size(284, 30);
            this.titleBarPanel.TabIndex = 1;
            // 
            // btnPin
            // 
            this.btnPin.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPin.Location = new System.Drawing.Point(224, 0);
            this.btnPin.Name = "btnPin";
            this.btnPin.Size = new System.Drawing.Size(30, 30);
            this.btnPin.TabIndex = 1;
            this.btnPin.Text = "📌";
            this.btnPin.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.ForeColor = System.Drawing.Color.Red;
            this.btnClose.Location = new System.Drawing.Point(254, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(30, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 461);
            this.Controls.Add(this.titleBarPanel);
            this.Controls.Add(this.txtNote);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Sticky Note";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.titleBarPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.Panel titleBarPanel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnPin;
    }
}

