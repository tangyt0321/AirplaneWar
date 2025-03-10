using System;
using System.Numerics;


namespace airplaneWar.GameLogic.Entities.Player
{
    // 游戏元素类

    public class Player
    {
        public Player()
        {
            Size = new Size(50, 50);
            Position = new Vector2(0, 0);
        }

        public Player(Vector2 position, Size size)
        {
            Position = position;
            Size = size;
        }

        public Vector2 Position;
        public Size Size { get; set; } = new Size(50, 50);
        public double Speed { get; set; } = 0.3;
        public bool IsAlive { get; set; } = true;
        public bool IsShooting { get; set; } = false;
        public bool IsMove { get; set; }
        public double Angle { get; set; } = 0;
        public int HP { get; set; } = 10;
        public void hurt()
        {
            HP--;
            if (HP <= 0)
                IsAlive = false;
        }
        public void on_render(Graphics g)
        {
            if (IsAlive)
            {
                g.FillRectangle(Brushes.Green, Position.X, Position.Y, Size.Width, Size.Height);
            }

        }

        public void on_move(double delta)
        {

            if (!IsMove)
            {
                return;
            }
            Position += new Vector2(
                (float)(Math.Cos(Angle) * Speed * delta),
                (float)(Math.Sin(Angle) * Speed * delta)
            );
            //Console.WriteLine(Speed * delta);
        }
    }


}

