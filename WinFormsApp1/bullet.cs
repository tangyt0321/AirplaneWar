using System;

namespace Bullets
{
    class Bullet
    {
        public virtual Point Position { get; set; }
        public virtual Size Size { get; set; }
        public virtual int Speed { get; set; }
        public virtual Rectangle Bounds => new Rectangle(Position, Size);
    }

    class stdBullet : Bullet
    {
        public new Point Position { get; set; }
        public new Size Size { get; set; }
        public new int Speed { get; set; }
        public new Rectangle Bounds => new Rectangle(Position, Size);
    }
}