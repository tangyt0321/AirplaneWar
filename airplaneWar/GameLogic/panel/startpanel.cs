using System;
using airplaneWar.Core.Manager.InputManager;
using airplaneWar.Core.Manager.PanelManager;
using airplaneWar.GameLogic;

namespace airplaneWar.GameLogic.panel
{

    public partial class startPanel : UserControl, IPanel
    {
        private Image startMenuImage;
        public void on_enter(object args)
        {
            startMenuImage = Image.FromFile("D:\\code\\project\\airplaneWar\\airplaneWar\\Resources\\menu_bkg.png");

            InputManager.Instance.Initialize(this);
            Paint += new PaintEventHandler((sender, e) => on_render(e.Graphics));
            DoubleBuffered = true;
        }

        public void on_update(double delta)
        {
            ProcessInput();
            InputManager.Instance.Update(delta);
        }


        public void on_render(Graphics g)
        {
            g.DrawImage(startMenuImage, 0, 0, Size.Width, Size.Height);
            Invalidate();
        }

        public void on_exit()
        {
        }

        public void on_return(object data)
        {
            InputManager.Instance.Dispose();
        }

        private void ProcessInput()
        {
            // 处理玩家输入
            if (InputManager.Instance.GetKeyDown(Keys.Space))
            {
                // 进入游戏
                on_exit();
                PanelManager.Instance.NavigateTo<GamePanel>();
                //Console.WriteLine("进入游戏");
            }
        }
    }
}

