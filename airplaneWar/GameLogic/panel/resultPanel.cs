using System;
using System.Windows.Forms;
using airplaneWar.Core.Manager.PanelManager;
using airplaneWar.Core.Manager.InputManager;
using airplaneWar.GameLogic.panel;
using airplaneWar.Core.Manager;


namespace airplaneWar.GameLogic.panel
{

    public partial class resultPanel : UserControl, IPanel
    {

        public void on_enter(object args)
        {
            Paint += new PaintEventHandler((sender, e) => on_render(e.Graphics));
            InputManager.Instance.Initialize(this);
            DoubleBuffered = true;
        }

        public void on_update(double delta)
        {
            ProcessInput();
            InputManager.Instance.Update(delta);
        }

        public void on_render(Graphics g)
        {
            g.Clear(Color.Black);
            g.DrawString("游戏结束", new Font("微软雅黑", 30), Brushes.Red, new PointF(200, 100));
            g.DrawString("得分：" + EventManager.score, new Font("微软雅黑", 20), Brushes.Red, new PointF(200, 200));
            Invalidate();
        }

        public void on_exit()
        {
            InputManager.Instance.Dispose();
        }

        public void on_return(object data)
        {
        }

        private void ProcessInput()
        {
            // 处理玩家输入
            if (InputManager.Instance.GetKeyDown(Keys.Escape))
            {
                on_exit();
                PanelManager.Instance.NavigateTo<startPanel>();
            }
        }
    }
}


