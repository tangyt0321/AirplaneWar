using airplaneWar.GameLogic.Entities.Bullet.Core;
using System.Numerics;
using airplaneWar.GameLogic.Collision;
using airplaneWar.GameLogic.Entities.Enemies.Core;
using airplaneWar.Core.Manager;

namespace airplaneWar.GameLogic.Entities.Bullet
{
    // 普通子弹（直线运动）
    public class NormalBullet : IBullet, Projectile
    {
        private const double Speed = 0.8;
        private const int DAMAGE = 1;
        private static readonly Size bulletSize = new Size(10, 10);
        private CollisionBox hitbox = new CollisionBox();

        public RectangleF collision_src { get; set; }
        public RectangleF collision_dst { get; set; }

        public override int damage { get => DAMAGE; set; }
        public override CollisionBox Hitbox => hitbox;

        public NormalBullet()
        {
            collision_src = new RectangleF(Position.X, Position.Y, 10, 10);
            collision_dst = new RectangleF(Position.X, Position.Y, 10, 10);

            hitbox.objest_dst = this;
            hitbox.on_collide = () =>
            {
                if (hitbox.objest_src is IEnemy)
                {
                    Reset();
                    EventManager.score += 1;
                }
            };
        }

        public override void on_update(double delta)
        {
            if (!IsActive) return;
            this.Position += new Vector2(
                (float)(Math.Cos(Angle) * Speed * delta),
                (float)(Math.Sin(Angle) * Speed * delta)
                );
            collision_dst = new RectangleF(Position.X, Position.Y, bulletSize.Width, bulletSize.Height);
            collision_src = new RectangleF(Position.X, Position.Y, bulletSize.Width, bulletSize.Height);
        }

        public override void on_render(Graphics g)
        {
            //实心
            g.FillEllipse(Brushes.Red, Position.X, Position.Y, 10, 10);
        }

        public override void on_render(Graphics g, Vector2 position)
        {
            g.FillEllipse(Brushes.Red, Position.X - position.X, Position.Y - position.Y, 10, 10);
        }

    }

    ////穿透子弹
    //public class PenetrateBullet : IBullet
    //{
    //    private const double Speed = 0.8;
    //    private const int DAMAGE = 10;
    //    private CollisionBox hitbox = new CollisionBox();
    //    private Size bulletSize = new Size(10, 10);


    //    public override int damage => DAMAGE;
    //    public override CollisionBox Hitbox => hitbox;


    //    public PenetrateBullet()
    //    {
    //        hitbox.position = this.Position;
    //        hitbox.collision_dst = typeof(PenetrateBullet);
    //        hitbox.size_src = bulletSize;
    //        hitbox.size_dst = bulletSize;
    //        hitbox.on_collide = () =>
    //        {
    //            if (typeof(IEnemy).IsAssignableFrom(hitbox.collision_src))
    //            {
    //                EventManager.score += 1;
    //            }
    //        };
    //    }

    //    public override void on_update(double delta)
    //    {
    //        if (!IsActive) return;
    //        this.Position += new Vector2(
    //            (float)(Math.Cos(Angle) * Speed * delta),
    //            (float)(Math.Sin(Angle) * Speed * delta)
    //            );
    //        this.hitbox.position = this.Position;
    //    }

    //    public override void on_render(Graphics g)
    //    {
    //        //实心
    //        g.FillEllipse(Brushes.Red, Position.X, Position.Y, 10, 10);
    //    }

    //    public override void on_render(Graphics g, Vector2 position)
    //    {
    //        g.FillEllipse(Brushes.Red, Position.X - position.X, Position.Y - position.Y, 10, 10);
    //    }

    //}

    //// 追踪子弹（追踪目标位置）
    //public class HomingBullet : IBullet
    //{
    //    private const double Speed = 4f;
    //    private double targetX, targetY;

    //    public override CollisionBox hitbox => throw new NotImplementedException();

    //    public void SetTarget(double targetX, double targetY)
    //    {
    //        this.targetX = targetX;
    //        this.targetY = targetY;
    //    }

    //    public override void on_update(double delta)
    //    {
    //        if (!IsActive) return;

    //        double dx = targetX - this.Position.X;
    //        double dy = targetY - this.Position.Y;
    //        double distance = (double)Math.Sqrt(dx * dx + dy * dy);

    //        if (distance > 0)
    //        {
    //            this.Position += new Vector2((float)(dx / distance * Speed), (float)(dy / distance * Speed));
    //        }
    //    }

    //    public override void on_render(Graphics g) // Change Graphic to Graphics
    //    {
    //        g.FillEllipse(Brushes.Red, (int)Position.X, (int)Position.Y, 10, 10);
    //    }

    //    public override void on_render(Graphics g, Vector2 position)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //// 爆炸子弹（固定时间后爆炸）
    //public class ExplosiveBullet : IBullet
    //{
    //    private const int Speed = 3;
    //    private int timer = 60; // 60帧后爆炸

    //    public override CollisionBox hitbox => throw new NotImplementedException();

    //    public override void on_update(double delta)
    //    {
    //        if (!IsActive) return;

    //        this.Position = new Vector2((float)(Position.X + Math.Cos(Angle) * Speed), (float)(Position.Y + Math.Sin(Angle) * Speed));
    //        timer--;

    //        if (timer <= 0)
    //        {
    //            Reset();
    //        }
    //    }

    //    public override void on_render(Graphics g) // Change Graphic to Graphics
    //    {
    //        g.FillEllipse(Brushes.Red, (int)Position.X, (int)Position.Y, 10, 10);
    //    }

    //    public override void on_render(Graphics g, Vector2 position)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}


