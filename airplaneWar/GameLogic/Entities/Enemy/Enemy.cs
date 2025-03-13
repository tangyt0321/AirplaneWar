// 对象池管理敌人
using airplaneWar.GameLogic.Collision;
using airplaneWar.GameLogic.Entities.Bullet.Core;
using airplaneWar.GameLogic.Entities.Enemies.Core;
using airplaneWar.GameLogic.Entities.Player;
using System.Numerics;

//Player = airplaneWar.GameLogic.Entities.Player.Player;

namespace airplaneWar.GameLogic.Entities.Enemies
{
    public partial class NormalEnemy : IEnemy, Projectile
    {
        public static readonly Size size = new Size(50, 50);
        public const double Speed = 0.1;
        public const int Health = 20;
        public const int Score = 10;


        public int HP { get; private set; } = Health;
        public int damage { get; set; } = 1;
        public CollisionBox hitbox { get; set; } = new();

        public RectangleF collision_src { get; set; }
        public RectangleF collision_dst { get; set; }

        public NormalEnemy()
        {
            hitbox.objest_dst = this;
            collision_dst = new RectangleF(Position.X, Position.Y, size.Width, size.Height);
            collision_src = new RectangleF(Position.X, Position.Y, size.Width, size.Height);

            hitbox.on_collide = () =>
            {
                if (hitbox.objest_src is IBullet)
                {
                    Hurt(hitbox.objest_src.damage);
                }
                if (hitbox.objest_src is IEnemy || hitbox.objest_src is Player.Player)
                {
                    var x = (
                        hitbox.objest_src.collision_dst.X + hitbox.objest_src.collision_dst.Width / 2 +
                        hitbox.objest_dst.collision_dst.X + hitbox.objest_dst.collision_dst.Width / 2) / 2;
                    var y = (
                        hitbox.objest_src.collision_dst.Y + hitbox.objest_src.collision_dst.Height / 2 +
                        hitbox.objest_dst.collision_dst.Y + hitbox.objest_dst.collision_dst.Height / 2) / 2;
                    var angle = Math.Atan2(
                        y - (hitbox.objest_dst.collision_dst.Y + hitbox.objest_dst.collision_dst.Height / 2),
                        x - (hitbox.objest_dst.collision_dst.X + hitbox.objest_dst.collision_dst.Width / 2));
                    this.Position -= new Vector2(
                        (float)(Math.Cos(angle) * Speed * 10),
                        (float)(Math.Sin(angle) * Speed * 10));
                }
            };
        }

        public override CollisionBox Hitbox => hitbox;

        public override void on_update(double delta)
        {
            this.Position += new Vector2(
                (float)(Math.Cos(Angle) * Speed * delta),
                (float)(Math.Sin(Angle) * Speed * delta)
            );
            collision_dst = new RectangleF(Position.X, Position.Y, size.Width, size.Height);
            collision_src = new RectangleF(Position.X, Position.Y, size.Width, size.Height);
        }

        public override void Hurt(int damage)
        {
            HP -= damage;
            if (HP <= 0)
            {
                IsAlive = false;
            }
        }

        public override void on_render(Graphics g)
        {
            g.FillRectangle(Brushes.Blue, Position.X, Position.Y, size.Width, size.Height);
        }

        public override void on_render(Graphics g, Vector2 position)
        {
            g.FillRectangle(Brushes.Blue, Position.X - position.X, Position.Y - position.Y, size.Width, size.Height);
            g.FillRectangle(Brushes.Black, Position.X - position.X, Position.Y - position.Y, size.Width, 5);
            g.FillRectangle(Brushes.Red, Position.X - position.X, Position.Y - position.Y + 1, size.Width * HP / Health, 3);

        }
    }




}