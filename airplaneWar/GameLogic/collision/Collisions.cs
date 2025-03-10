//using System;

//namespace airplaneWar.GameLogic.Collision
//{
//    internal static class Collision
//    {
//        //public static void CheckCollisions(Player player, List<Enemy> enemies, List<stdBullet> bullets) // 检测碰撞
//        //{
//        //    // 子弹与敌人碰撞检测
//        //    foreach (var bullet in bullets.ToArray())
//        //    {
//        //        foreach (var enemy in enemies.ToArray())
//        //        {
//        //            if (bullet.Bounds.IntersectsWith(enemy.Bounds))
//        //            {
//        //                enemy.Hurt();
//        //                bullets.Remove(bullet);
//        //                break;
//        //            }
//        //        }
//        //    }
//        //    // 玩家与敌人碰撞检测
//        //    foreach (var enemy in enemies.ToArray())
//        //    {
//        //        if (player.Bounds.IntersectsWith(enemy.Bounds))
//        //        {
//        //            player.Hurt();
//        //            enemies.Remove(enemy);
//        //            return;
//        //        }
//        //    }
//        //}
//        public static bool CheckCollision(Rectangle rect1, Rectangle rect2)
//        {
//            return rect1.IntersectsWith(rect2);
//        }
//        public static bool CheckCollision(Rectangle rect, Vector2 point)
//        {
//            return rect.Contains(point);
//        }
//        public static bool CheckCollision(Rectangle rect, int x, int y)
//        {
//            return rect.Contains(x, y);
//        }
//    }
//}
