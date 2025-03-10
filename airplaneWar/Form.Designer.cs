
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            gameTimer = new System.Windows.Forms.Timer(components);
            shootTimer = new System.Windows.Forms.Timer(components);
            paintTime = new System.Windows.Forms.Timer(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            contextMenuStrip2 = new ContextMenuStrip(components);
            pnlContainer = new Panel();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(24, 24);
            contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(contextMenuStrip1, "contextMenuStrip1");
            // 
            // contextMenuStrip2
            // 
            contextMenuStrip2.ImageScalingSize = new Size(24, 24);
            contextMenuStrip2.Name = "contextMenuStrip2";
            resources.ApplyResources(contextMenuStrip2, "contextMenuStrip2");
            // 
            // pnlContainer
            // 
            resources.ApplyResources(pnlContainer, "pnlContainer");
            pnlContainer.Name = "pnlContainer";
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlContainer);
            KeyPreview = true;
            Name = "MainForm";
            ResumeLayout(false);
        }

        #endregion
        //private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.Timer shootTimer;
        private System.Windows.Forms.Timer paintTime;
        private ContextMenuStrip contextMenuStrip1;
        private ContextMenuStrip contextMenuStrip2;
        private Panel pnlContainer;
    }
}


