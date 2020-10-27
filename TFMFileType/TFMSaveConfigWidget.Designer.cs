namespace TFMFileType
{
    partial class TFMSaveConfigWidget
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
            this.PixelFormat = new System.Windows.Forms.ComboBox();
            this.GenerateMipmaps = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // PixelFormat
            // 
            this.PixelFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PixelFormat.FormattingEnabled = true;
            this.PixelFormat.Items.AddRange(new object[] {
            "R8G8B8 (256 color palette)",
            "R5G6B5",
            "R4G4B4A4",
            "R5G5B5A1"});
            this.PixelFormat.Location = new System.Drawing.Point(9, 12);
            this.PixelFormat.Name = "PixelFormat";
            this.PixelFormat.Size = new System.Drawing.Size(135, 21);
            this.PixelFormat.TabIndex = 1;
            this.PixelFormat.SelectedIndexChanged += new System.EventHandler(this.PixelFormat_SelectedIndexChanged);
            // 
            // GenerateMipmaps
            // 
            this.GenerateMipmaps.AutoSize = true;
            this.GenerateMipmaps.Location = new System.Drawing.Point(9, 48);
            this.GenerateMipmaps.Name = "GenerateMipmaps";
            this.GenerateMipmaps.Size = new System.Drawing.Size(114, 17);
            this.GenerateMipmaps.TabIndex = 2;
            this.GenerateMipmaps.Text = "Generate mipmaps";
            this.GenerateMipmaps.UseVisualStyleBackColor = true;
            this.GenerateMipmaps.CheckedChanged += new System.EventHandler(this.GenerateMipmaps_CheckedChanged);
            // 
            // TFMSaveConfigWidget
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GenerateMipmaps);
            this.Controls.Add(this.PixelFormat);
            this.Name = "TFMSaveConfigWidget";
            this.Size = new System.Drawing.Size(157, 77);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox PixelFormat;
        private System.Windows.Forms.CheckBox GenerateMipmaps;
    }
}