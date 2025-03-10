using System;
using airplaneWar.Panels.Core;
using System.Windows.Forms;
using airplaneWar.Core.Manager.PanelManager;
using airplaneWar.Panels.Start;


namespace airplaneWar.Panels.Result
{

    public partial class resultPanel : UserControl, IPanel
    {

        public void on_enter(object args)
        {
            this.BackColor = System.Drawing.Color.Black;
            this.TabIndex = 1;
            this.Focus();
            // 订阅 Panel 的 KeyDown 事件
            this.KeyDown += new KeyEventHandler(Key_down);
        }

        public void on_update(double delta)
        {

        }

        public void on_render(Graphics g)
        {
        }

        public void on_exit()
        {
            this.KeyDown -= new KeyEventHandler(Key_down);
        }

        public void on_return(object data)
        {
        }

        private void Key_down(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.R: PanelManager.Instance.GoBack(); break;
            }
        }
    }
}


