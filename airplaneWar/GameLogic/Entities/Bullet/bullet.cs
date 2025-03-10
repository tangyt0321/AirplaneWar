using airplaneWar.GameLogic.Core;
using airplaneWar.GameLogic.Entities.Bullet.Core;
using DocumentFormat.OpenXml.Drawing;
using System.Drawing;
using System.Numerics;

namespace airplaneWar.GameLogic.Entities.Bullet
{
    // 普通子弹（直线运动）
    public class NormalBullet : IBullet
    {
        private const double Speed = 0.8;

        public override void on_update(double delta)
        {
            if (!IsActive) return;
            this.Position += new Vector2(
                (float)(Math.Cos(Angle) * Speed * delta),
                (float)(Math.Sin(Angle) * Speed * delta)
                );
        }

        public override void on_render(Graphics g) // Change Graphic to Graphics
        {
            //实心
            g.FillEllipse(Brushes.Red, (int)Position.X, (int)Position.Y, 10, 10);
        }
    }

    // 追踪子弹（追踪目标位置）
    public class HomingBullet : IBullet
    {
        private const double Speed = 4f;
        private double targetX, targetY;

        public void SetTarget(double targetX, double targetY)
        {
            this.targetX = targetX;
            this.targetY = targetY;
        }

        public override void on_update(double delta)
        {
            if (!IsActive) return;

            double dx = targetX - this.Position.X;
            double dy = targetY - this.Position.Y;
            double distance = (double)Math.Sqrt(dx * dx + dy * dy);

            if (distance > 0)
            {
                this.Position += new Vector2((float)(dx / distance * Speed), (float)(dy / distance * Speed));
            }
        }

        public override void on_render(Graphics g) // Change Graphic to Graphics
        {
            g.FillEllipse(Brushes.Red, (int)Position.X, (int)Position.Y, 10, 10);
        }
    }

    // 爆炸子弹（固定时间后爆炸）
    public class ExplosiveBullet : IBullet
    {
        private const int Speed = 3;
        private int timer = 60; // 60帧后爆炸

        public override void on_update(double delta)
        {
            if (!IsActive) return;

            this.Position = new Vector2((float)(Position.X + Math.Cos(Angle) * Speed), (float)(Position.Y + Math.Sin(Angle) * Speed));
            timer--;

            if (timer <= 0)
            {
                Reset();
            }
        }

        public override void on_render(Graphics g) // Change Graphic to Graphics
        {
            g.FillEllipse(Brushes.Red, (int)Position.X, (int)Position.Y, 10, 10);
        }
    }
}


