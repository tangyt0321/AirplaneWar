using airplaneWar.GameLogic.Entities;
using System;
using System.Numerics;

namespace airplaneWar.GameLogic.Collision
{

    public class CollisionBox
    {
        public Action on_collide { get; set; }
        public bool is_active { get; set; } = false;
        public Projectile objest_src { get; set; }
        public Projectile objest_dst { get; set; }
    }

    //public class CollisionInfo
    //{
    //    public object objest1 { get; set; }
    //    public object objest2 { get; set; }
    //}
}
