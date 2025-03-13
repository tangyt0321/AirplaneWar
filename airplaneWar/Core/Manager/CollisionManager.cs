using airplaneWar.GameLogic.Collision;
using System.Numerics;

namespace airplaneWar.Core.Manager.CollisionManager
{
    public sealed class CollisionManager
    {
        private static CollisionManager _instance;
        public static CollisionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CollisionManager();
                }
                return _instance;
            }
        }
        private List<CollisionBox> CollisionList = new List<CollisionBox>();

        public void add_collision(CollisionBox collisionBox)
        {
            CollisionList.Add(collisionBox);
            collisionBox.is_active = true;
        }

        public void remove_collision(CollisionBox collisionBox)
        {
            CollisionList.Remove(collisionBox);
            collisionBox.is_active = false;
        }

        public void on_collide(double deltaTime)
        {

            foreach (CollisionBox collisionBoxSrc in CollisionList)
            {
                if (!collisionBoxSrc.is_active) { continue; }
                foreach (CollisionBox collisionBoxDst in CollisionList)
                {
                    if (!collisionBoxDst.is_active) { continue; }
                    if (collisionBoxSrc == collisionBoxDst) { continue; }

                    //Rectangle rectangle_src = new Rectangle(
                    //    (int)collisionBoxSrc.position.X, (int)collisionBoxSrc.position.Y,
                    //    collisionBoxSrc.size_src.Width, collisionBoxSrc.size_src.Height
                    //    );
                    //Rectangle rectangle_dst = new Rectangle(
                    //    (int)collisionBoxSrc.position.X, (int)collisionBoxSrc.position.Y,
                    //    collisionBoxDst.size_dst.Width, collisionBoxDst.size_dst.Height
                    //    );

                    //if (collisionBoxSrc.objest_dst.collision_src.IntersectsWith(collisionBoxSrc.objest_dst.collision_dst))

                    if (collisionBoxSrc.objest_dst.collision_src.Left < collisionBoxDst.objest_dst.collision_dst.Right &&
                        collisionBoxSrc.objest_dst.collision_src.Right > collisionBoxDst.objest_dst.collision_dst.Left &&
                        collisionBoxSrc.objest_dst.collision_src.Top < collisionBoxDst.objest_dst.collision_dst.Bottom &&
                        collisionBoxSrc.objest_dst.collision_src.Bottom > collisionBoxDst.objest_dst.collision_dst.Top)



                    //if (collisionBoxSrc.position.X + collisionBoxSrc.size_src.Width > collisionBoxDst.position.X &&
                    //    collisionBoxSrc.position.X < collisionBoxDst.position.X + collisionBoxDst.size_dst.Width &&
                    //    collisionBoxSrc.position.Y + collisionBoxSrc.size_src.Height > collisionBoxDst.position.Y &&
                    //    collisionBoxSrc.position.Y < collisionBoxDst.position.Y + collisionBoxDst.size_dst.Height)
                    {
                        //Console.WriteLine("collision");
                        collisionBoxSrc.objest_src = collisionBoxDst.objest_dst;
                        collisionBoxDst.on_collide?.Invoke();
                    }
                }
            }
        }
    }

}

