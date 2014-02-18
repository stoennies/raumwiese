namespace RaumfeldNET_TestProject
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.LogWriterTestButton = new System.Windows.Forms.Button();
            this.RetrieveZones = new System.Windows.Forms.Button();
            this.InitController = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LogWriterTestButton
            // 
            this.LogWriterTestButton.Location = new System.Drawing.Point(12, 12);
            this.LogWriterTestButton.Name = "LogWriterTestButton";
            this.LogWriterTestButton.Size = new System.Drawing.Size(201, 23);
            this.LogWriterTestButton.TabIndex = 0;
            this.LogWriterTestButton.Text = "LogWriter Test";
            this.LogWriterTestButton.UseVisualStyleBackColor = true;
            this.LogWriterTestButton.Click += new System.EventHandler(this.LogWriterTestButton_Click);
            // 
            // RetrieveZones
            // 
            this.RetrieveZones.Location = new System.Drawing.Point(12, 41);
            this.RetrieveZones.Name = "RetrieveZones";
            this.RetrieveZones.Size = new System.Drawing.Size(201, 23);
            this.RetrieveZones.TabIndex = 1;
            this.RetrieveZones.Text = "RetrieveZones Test";
            this.RetrieveZones.UseVisualStyleBackColor = true;
            this.RetrieveZones.Click += new System.EventHandler(this.RetrieveZones_Click);
            // 
            // InitController
            // 
            this.InitController.Location = new System.Drawing.Point(12, 70);
            this.InitController.Name = "InitController";
            this.InitController.Size = new System.Drawing.Size(201, 23);
            this.InitController.TabIndex = 2;
            this.InitController.Text = "InitController Test";
            this.InitController.UseVisualStyleBackColor = true;
            this.InitController.Click += new System.EventHandler(this.InitController_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 302);
            this.Controls.Add(this.InitController);
            this.Controls.Add(this.RetrieveZones);
            this.Controls.Add(this.LogWriterTestButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button LogWriterTestButton;
        private System.Windows.Forms.Button RetrieveZones;
        private System.Windows.Forms.Button InitController;
    }
}

