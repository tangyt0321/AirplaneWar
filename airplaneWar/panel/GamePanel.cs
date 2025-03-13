using System;
using airplaneWar.Panels.Core;
using airplaneWar.Core.Manager.PanelManager;
//using airplaneWar.Core.Manager.CollisionManager;
using airplaneWar.GameLogic.Core;
using System.Numerics;
using airplaneWar.Core.Manager.InputManager;
using airplaneWar.Panels.Start;
using airplaneWar.Panels.Result;
using airplaneWar.Core.Manager;
//using airplaneWar.GameLogic.Core;

namespace airplaneWar.Panels.Game
{
    public partial class GamePanel : UserControl, IPanel
    {
        private double currentFPS = 0;
        private bool is_pause = false;
        private PictureBox miniMap;
        private static readonly Font DefaultFont = new Font("Arial", 12);

        public void on_enter(object args)
        {
            // 初始化游戏元素
            GameWorld.Instance.InitializeGameWorld();
            miniMap = new();
            // 初始化输入管理器
            InputManager.Instance.Initialize(this);
            // 绑定绘制事件
            this.Paint += new PaintEventHandler((sender, e) => on_render(e.Graphics));
            this.DoubleBuffered = true;
        }


        public void on_render(Graphics g)
        {
            GameWorld.Instance.RenderGameWorld(g);
            g.DrawString($"FPS: {currentFPS:F1}", DefaultFont, Brushes.Red, 10, 10);
            g.DrawString($"Score: {EventManager.score}", DefaultFont, Brushes.Red, 10, 70);
        }

        public void on_update(double deltaTime)
        {
            //计算试试帧率
            currentFPS = 1000 / deltaTime;
            //处理输入
            ProcessInput();
            InputManager.Instance.Update(deltaTime);
            //是否时停
            if (is_pause) { return; }
            // 更新游戏世界状态
            GameWorld.Instance.on_update(deltaTime);
            if (!GameWorld.Instance.Player.IsAlive)
            {
                is_pause = true;
                this.on_exit();
                PanelManager.Instance.NavigateTo<resultPanel>();
            }

            // 触发重绘
            Invalidate();
        }

        public void on_exit()
        {
            InputManager.Instance.Dispose();
            //GameWorld.Instance.;
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

            if (InputManager.Instance.GetKeyDown(Keys.Escape))
            {
                this.on_exit();
                PanelManager.Instance.NavigateTo<resultPanel>();
            }

            CameraMove();


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
            //GameWorld.Instance.camera.is_move = isMove;
            //GameWorld.Instance.camera.angle = moveDirection;
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
        private void CameraMove()
        {
            //鼠标移动到边缘
            bool isNearEdge = InputManager.Instance.MousePosition.X <= this.Width * 0.1 ||
                InputManager.Instance.MousePosition.X >= this.Width * 0.9 ||
                InputManager.Instance.MousePosition.Y <= this.Height * 0.1 ||
                InputManager.Instance.MousePosition.Y >= this.Height * 0.9;
            if (isNearEdge)
            {
                //GameWorld.Instance.camera.is_move = true;
                //GameWorld.Instance.camera.angle = Math.Atan2(InputManager.Instance.MousePosition.Y - this.Height / 2, InputManager.Instance.MousePosition.X - this.Width / 2);
            }
            else if (InputManager.Instance.GetMouseButton(MouseButtons.Right))//鼠标右键按下
            {
                GameWorld.Instance.camera.is_move = true;
                GameWorld.Instance.camera.angle = Math.Atan2(
                    InputManager.Instance.MousePosition.Y - this.Height / 2,
                    InputManager.Instance.MousePosition.X - this.Width / 2
                    );
            }
            else
            {
                GameWorld.Instance.camera.is_move = false;
            }
        }
    }
}
