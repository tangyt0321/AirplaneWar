namespace airplaneWar
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            gamePanel = new Panel();
            gameTimer = new System.Windows.Forms.Timer(components);
            shootTimer = new System.Windows.Forms.Timer(components);
            paintTime = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // gamePanel
            // 
            gamePanel.Location = new Point(12, 12);
            gamePanel.Name = "gamePanel";
            gamePanel.Size = new Size(444, 353);
            gamePanel.TabIndex = 0;
            gamePanel.Paint += GamePanel_Paint;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(716, 533);
            Controls.Add(gamePanel);
            KeyPreview = true;
            Name = "MainForm";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Panel gamePanel;
        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.Timer shootTimer;
        private System.Windows.Forms.Timer paintTime;
    }
}
