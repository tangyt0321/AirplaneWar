// GameWorld.cs (单例类)


namespace airplaneWar.GameLogic.Entities
{
    public interface Projectile
    {
        public int damage { get; set; }
        public void on_update(double delta);
        public RectangleF collision_src { get; set; }
        public RectangleF collision_dst { get; set; }
    }
}