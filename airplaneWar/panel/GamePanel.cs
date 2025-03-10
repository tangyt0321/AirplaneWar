using System;
using airplaneWar.Panels.Core;
using System.Diagnostics;
using Timer = System.Windows.Forms.Timer;
using airplaneWar.Core.Manager.PanelManager;
using airplaneWar.Core.Manager.CollisionManager;
using airplaneWar.GameLogic.Entities.Enemies;
using airplaneWar.GameLogic.Entities.Player;
using airplaneWar.GameLogic.Core;
using System.Numerics;
using airplaneWar.Core.Manager.InputManager;
//using airplaneWar.GameLogic.Core;

namespace airplaneWar.Panels.Game
{
    public partial class GamePanel : UserControl, IPanel
    {
        //private Timer _gameLoopTimer;
        private CollisionManager _collisionManager;
        private double currentFPS = 0;
        private bool is_pause = false;

        public void on_enter(object args)
        {
            // 初始化游戏元素
            GameWorld.Instance.InitializeGameWorld();

            // 初始化碰撞管理器
            _collisionManager = new CollisionManager();
            // 初始化输入管理器
            InputManager.Instance.Initialize(this);
            // 绑定绘制事件
            this.Paint += new PaintEventHandler((sender, e) => on_render(e.Graphics));
            this.DoubleBuffered = true;
        }


        public void on_render(Graphics g)
        {
            GameWorld.Instance.RenderGameWorld(g);
            g.DrawString($"FPS: {currentFPS:F1}", Font, Brushes.Black, 10, 10);
        }

        public void on_update(double deltaTime)
        {
            //Console.WriteLine(InputManager.Instance.GetKey(Keys.P));
            currentFPS = 1000 / deltaTime;
            ProcessInput();
            InputManager.Instance.Update(deltaTime);
            if (is_pause) { return; }

            // 更新游戏世界状态
            GameWorld.Instance.on_update(deltaTime);

            Invalidate(); // 触发重绘

            //_collisionManager.on_update(deltaTime);
        }

        public void on_exit()
        {
            //_gameLoopTimer.Stop();
            //_gameLoopTimer.Dispose();
        }

        public void on_return(object data)
        {
        }


        private void ProcessInput()
        {
            PlayerMove();
            PlayerShoot();

            if (InputManager.Instance.GetKeyDown(Keys.P))
            {
                is_pause = !is_pause;
            }


        }


        private void PlayerMove()
        {
            double moveDirection = double.NaN;
            bool isMove = false;
            if (InputManager.Instance.GetAction("MoveLeft"))
            {
                moveDirection = Math.PI;
                isMove = true;
                if (InputManager.Instance.GetAction("MoveUp"))
                {
                    moveDirection = Math.PI * 5 / 4;
                }
                else if (InputManager.Instance.GetAction("MoveDown"))
                {
                    moveDirection = Math.PI * 3 / 4;
                }
            }
            else if (InputManager.Instance.GetAction("MoveRight"))
            {
                moveDirection = 0;
                isMove = true;
                if (InputManager.Instance.GetAction("MoveUp"))
                {
                    moveDirection = Math.PI * 7 / 4;
                }
                else if (InputManager.Instance.GetAction("MoveDown"))
                {
                    moveDirection = Math.PI * 1 / 4;
                }
            }
            else if (InputManager.Instance.GetAction("MoveUp"))
            {
                moveDirection = Math.PI * 3 / 2;
                isMove = true;
            }
            else if (InputManager.Instance.GetAction("MoveDown"))
            {
                moveDirection = Math.PI * 1 / 2;
                isMove = true;
            }
            GameWorld.Instance.Player.IsMove = isMove;
            GameWorld.Instance.Player.Angle = moveDirection;
        }
        private void PlayerShoot()
        {
            if (InputManager.Instance.GetMouseButton(MouseButtons.Left))//鼠标左键按下
            {
                GameWorld.Instance.Player.IsShooting = true;
            }
            else
            {
                GameWorld.Instance.Player.IsShooting = false;
            }
        }
    }
}
