// 对象池管理敌人
using airplaneWar.GameLogic.Core;
using airplaneWar.GameLogic.Entities.Enemies.Core;
using airplaneWar.Panels.Core;
using System.Collections.Generic;
using System.Numerics;


namespace airplaneWar.GameLogic.Entities.Enemies
{
    public partial class NormalEnemy : IEnemy
    {
        public static readonly Size size = new Size(50, 50);
        public const double Speed = 0.1;
        public const int Health = 20;
        public const int Score = 10;

        public override void on_update(Vector2 playerPosition, double delta)
        {
            var x = playerPosition.X - Position.X;
            var y = playerPosition.Y - Position.Y;
            var distance = Math.Sqrt(x * x + y * y);
            Angle = Math.Atan2(y, x);
            //Console.WriteLine(Speed);

            if (distance < Speed)
            {
                Position = playerPosition;
                return;
            }
            //Position += new Vector2(
            //    (float)(Speed * x * delta / distance),
            //    (float)(Speed * y * delta / distance)
            //);
            this.Position += new Vector2(
                (float)(Math.Cos(Angle) * Speed * delta),
                (float)(Math.Sin(Angle) * Speed * delta)
            );
        }

        public override void on_render(Graphics g)
        {
            g.FillRectangle(Brushes.Blue, Position.X, Position.Y, size.Width, size.Height); // Fix for CS1003
        }

    }



    //public class EnemySpawner
    //{
    //    private Queue<Enemy1> _enemyPool = new Queue<Enemy1>();
    //    public int _maxEnemies { get; private set; } = 20;
    //    public int EnemyCount
    //    {
    //        get { return _enemyPool.Count; }
    //    }

    //    public void Initialize(int maxEnemies)
    //    {
    //        for (int i = 0; i < maxEnemies; i++)
    //        {
    //            _enemyPool.Enqueue(new Enemy1());
    //        }
    //    }

    //    public Enemy1 TrySpawnEnemy()
    //    {
    //        if (_enemyPool.Count > 0 && NeedSpawn())
    //        {
    //            var enemy = _enemyPool.Dequeue();
    //            GameWorld.Instance.ActiveEnemies.Add(enemy);
    //            return enemy;
    //        }
    //        return null;
    //    }


    //    private bool NeedSpawn()
    //    {
    //        return true;
    //    }


    //}
}