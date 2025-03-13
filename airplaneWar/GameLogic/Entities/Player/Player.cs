using airplaneWar.GameLogic.Collision;
using airplaneWar.GameLogic.Entities.Enemies.Core;
using System;
using System.Numerics;


namespace airplaneWar.GameLogic.Entities.Player
{
    // 游戏元素类

    public class Player : Projectile
    {

        public Vector2 Position;
        public Size size { get; set; } = new Size(50, 50);

        public double Speed { get; set; } = 0.3;
        public int HP { get; set; } = 10;
        public bool IsAlive { get; set; } = true;

        public bool IsShooting { get; set; } = false;
        public bool IsMove { get; set; }

        public double Angle { get; set; } = 0;
        //double last_shoot_time = 0;
        double last_hurt_time = 0;
        double Invincible_time = 0.5 * 1000;
        //public double ShootInterval { get; set; } = 0.5;
        int Projectile.damage { get; set; } = 0;


        public RectangleF collision_src { get; set; }
        public RectangleF collision_dst { get; set; }

        public CollisionBox hitbox = new CollisionBox();

        public Player()
        {
            hitbox.objest_dst = this;
            collision_src = new RectangleF(Position.X, Position.Y, size.Width, size.Height);
            collision_dst = new RectangleF(Position.X, Position.Y, size.Width, size.Height);

            size = new Size(50, 50);
            Position = new Vector2(0, 0);
            hitbox.on_collide = () =>
            {
                if (hitbox.objest_src is IEnemy)
                {
                    var x = (hitbox.objest_src.collision_dst.X + hitbox.objest_dst.collision_dst.X + hitbox.objest_src.collision_dst.Width / 2 + hitbox.objest_dst.collision_dst.Width / 2) / 2;
                    var y = (hitbox.objest_src.collision_dst.Y + hitbox.objest_dst.collision_dst.Y + hitbox.objest_src.collision_dst.Height / 2 + hitbox.objest_dst.collision_dst.Height / 2) / 2;
                    var angle = Math.Atan2(y - (hitbox.objest_dst.collision_dst.Y + hitbox.objest_dst.collision_dst.Height / 2), x - (hitbox.objest_dst.collision_dst.X + hitbox.objest_dst.collision_dst.Width / 2));
                    this.Position -= new Vector2((float)(Math.Cos(angle) * Speed * 10), (float)(Math.Sin(angle) * Speed * 10));

                    if (last_hurt_time > Invincible_time)
                    {
                        Console.WriteLine("1");
                        hurt(hitbox.objest_src.damage);
                        last_hurt_time = 0;
                    }

                }
            };
        }


        public void hurt(int demage)
        {
            HP -= demage;
            if (HP <= 0)
                IsAlive = false;
        }

        public void on_render(Graphics g)
        {
            if (IsAlive)
            {
                g.FillRectangle(Brushes.Green, Position.X, Position.Y, size.Width, size.Height);
            }
        }
        public void on_render(Graphics g, Vector2 position)
        {
            if (IsAlive)
            {
                g.FillRectangle(Brushes.Green, Position.X - position.X, Position.Y - position.Y, size.Width, size.Height);
            }
        }

        public void on_update(double delta)
        {

            collision_dst = new RectangleF(Position.X, Position.Y, size.Width, size.Height);
            collision_src = new RectangleF(Position.X, Position.Y, size.Width, size.Height);
            last_hurt_time += delta;

            if (!IsMove)
            {
                return;
            }
            Position += new Vector2(
                (float)(Math.Cos(Angle) * Speed * delta),
                (float)(Math.Sin(Angle) * Speed * delta)
            );
        }


    }


}

