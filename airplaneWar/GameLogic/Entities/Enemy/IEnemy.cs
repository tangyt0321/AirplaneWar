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
        public void Hurt()
        {
            HP--;
            if (HP <= 0)
                IsAlive = false;
        }
        public abstract void on_update(Vector2 Position, double delta);
        public abstract void on_render(Graphics g);

    }

}