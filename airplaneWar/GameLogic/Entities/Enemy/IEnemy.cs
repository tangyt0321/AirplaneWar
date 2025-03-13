using airplaneWar.GameLogic.Collision;
using System;
using System.Numerics;


namespace airplaneWar.GameLogic.Entities.Enemies.Core
{
    public enum EnemyType
    {
        normal,
        Medium,
        Big
    }

    public abstract class IEnemy
    {
        public Vector2 Position { get; set; }
        public Size Size { get; set; }
        private double HP { get; set; }
        public bool IsAlive { get; set; } = true;
        public double Angle { get; set; } = double.NaN;
        public abstract CollisionBox Hitbox { get; }
        public abstract void Hurt(int damage);
        public abstract void on_update(double delta);
        public abstract void on_render(Graphics g);
        public abstract void on_render(Graphics g, Vector2 position);
    }

}