using DocumentFormat.OpenXml.Drawing;
using System;
using System.Numerics;


namespace airplaneWar.GameLogic.Entities.Bullet.Core
{
    // 子弹类型枚举
    public enum BulletType { Normal, Homing, Explosive }

    // 子弹基类
    public abstract class IBullet
    {
        public Vector2 Position { get; set; }
        public double Angle { get; protected set; }
        public bool IsActive { get; set; } // 是否处于激活状态

        // 初始化方法（供对象池调用）
        public virtual void Initialize(Vector2 position, double angle)
        {
            this.Position = position;
            Angle = angle;
            IsActive = true;
        }

        // 重置方法（回收时调用）
        public virtual void Reset()
        {
            IsActive = false;
        }

        // 更新逻辑（子类实现多态）
        public abstract void on_update(double delta);
        public abstract void on_render(Graphics g);
    }



}