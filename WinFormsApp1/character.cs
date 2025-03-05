using System;

namespace Character
{

    class Character
    {
        public virtual Point Position { get; set; }
        public virtual Size Size { get; set; }
        public virtual int Speed { get; set; }
        public virtual Rectangle Bounds => new Rectangle(Position, Size);
        public virtual bool IsAlive { get; set; } = true;
        public virtual int Health { get; set; } = 100;

        public virtual void Hurt()
        {
            Health -= 10;
            if (Health <= 0)
            {
                IsAlive = false;
            }
        }
    }
    // 游戏元素类
    class Player : Character
    {
        public new Point Position { get; set; }
        public new Size Size { get; set; }
        public new int Speed { get; set; }
        public new Rectangle Bounds => new Rectangle(Position, Size);

        public override bool IsAlive { get; set; } = true;
        public override int Health { get; set; } = 10;

        public override void Hurt()
        {
            base.Hurt();
        }
    }

    class Enemy : Character
    {
        public new Point Position { get; set; }
        public new Size Size { get; set; }
        public new int Speed { get; set; }
        public new Rectangle Bounds => new Rectangle(Position, Size);

        public override bool IsAlive { get; set; } = true;
        public override int Health { get; set; } = 20;

        public override void Hurt()
        {
            base.Hurt();
        }
    }



}
