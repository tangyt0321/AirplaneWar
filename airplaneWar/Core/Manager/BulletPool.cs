using System.Collections.Generic;
using System;
using airplaneWar.GameLogic.Entities.Bullet;
using airplaneWar.GameLogic.Entities.Bullet.Core;

namespace airplaneWar.Core.Manager
{
    public class BulletPool
    {
        private Dictionary<BulletType, Queue<IBullet>> pools = new();
        private int maxPoolSize = 100;

        public BulletPool()
        {
            // 初始化各类型子弹池
            foreach (BulletType type in Enum.GetValues(typeof(BulletType)))
            {
                pools[type] = new Queue<IBullet>();
            }

            // 预创建100发子弹（按需分配类型）
            for (int i = 0; i < maxPoolSize; i++)
            {
                AddBulletToPool(BulletType.Normal);
                //AddBulletToPool(BulletType.Penetrate);
                //AddBulletToPool(BulletType.Homing);
                //AddBulletToPool(BulletType.Explosive);
            }
        }

        private void AddBulletToPool(BulletType type)
        {
            IBullet bullet = type switch
            {
                BulletType.Normal => new NormalBullet(),
                //BulletType.Penetrate => new PenetrateBullet(),
                //BulletType.Homing => new HomingBullet(),
                //BulletType.Explosive => new ExplosiveBullet(),
                _ => throw new ArgumentException("未知子弹类型")
            };
            bullet.Reset();
            pools[type].Enqueue(bullet);
        }

        // 从池中获取子弹
        public IBullet GetBullet(BulletType type)
        {
            if (pools[type].Count > 0)
            {
                return pools[type].Dequeue();
            }
            // 池为空时动态创建（可选限制最大数量）
            AddBulletToPool(type);
            return pools[type].Dequeue();
        }

        // 回收子弹
        public void ReturnBullet(IBullet bullet)
        {
            if (bullet.IsActive) bullet.Reset();
            BulletType type = bullet switch
            {
                NormalBullet => BulletType.Normal,
                //PenetrateBullet => BulletType.Penetrate,
                //HomingBullet => BulletType.Homing,
                //ExplosiveBullet => BulletType.Explosive,
                _ => throw new InvalidOperationException()
            };
            pools[type].Enqueue(bullet);
        }



        //private bool NeedSpawn(double deltaTime)
        //{
        //    //每5秒生成一个子弹

        //    if (spawnTime >= 1000)
        //    {
        //        spawnTime = 0;
        //        return true;
        //    }

        //    spawnTime += deltaTime;

        //    return false;
        //}
    }
}