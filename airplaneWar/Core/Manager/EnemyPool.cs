using airplaneWar.GameLogic.Entities.Enemies.Core;
using airplaneWar.GameLogic.Entities.Enemies;
using System;

namespace airplaneWar.Core.Manager
{
    public class EnemyPool
    {
        private Dictionary<EnemyType, Queue<IEnemy>> pools;
        private int maxPoolSize = 20;


        public EnemyPool()
        {
            pools = new Dictionary<EnemyType, Queue<IEnemy>>();
            foreach (EnemyType type in Enum.GetValues(typeof(EnemyType)))
            {
                pools[type] = new Queue<IEnemy>();
            }

            for (int i = 0; i < maxPoolSize; i++)
            {
                AddEnemyToPool(EnemyType.normal);
            }
        }

        private void AddEnemyToPool(EnemyType type)
        {
            IEnemy enemy = type switch
            {
                EnemyType.normal => new NormalEnemy(),
                _ => throw new ArgumentException("未知敌人类型")
            };
            pools[type].Enqueue(enemy);
        }

        public IEnemy GetEnemyType(EnemyType type)
        {
            if (pools[type].Count > 0)
            {
                return pools[type].Dequeue();
            }
            AddEnemyToPool(type);
            return pools[type].Dequeue();
        }
    }
}