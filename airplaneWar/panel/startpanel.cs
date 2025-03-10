using System;
using airplaneWar.Core.Manager.InputManager;
using airplaneWar.Core.Manager.PanelManager;
using airplaneWar.GameLogic.Core;
using airplaneWar.Panels.Core;
using airplaneWar.Panels.Game;

namespace airplaneWar.Panels.Start
{

    public partial class startPanel : UserControl, IPanel
    {
        public void on_enter(object args)
        {
            InputManager.Instance.Initialize(this);
        }

        public void on_update(double delta)
        {
            ProcessInput();
        }


        public void on_render(Graphics g)
        {
            this.Invalidate();
        }

        public void on_exit()
        {
            //this.KeyDown -= new KeyEventHandler(Key_down);
            //this.TabIndex = 0;
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
                PanelManager.Instance.NavigateTo<GamePanel>();
                Console.WriteLine("进入游戏");
            }
        }
    }
}

