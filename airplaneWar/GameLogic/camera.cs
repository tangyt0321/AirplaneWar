using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Numerics;
using System.Reflection.Metadata;

namespace airplaneWar.GameLogic.Core
{
    public class Camera
    {
        public Vector2 position { get; set; }
        public double speed { get; set; } = 1.0;
        public Size size { get; set; }
        //public Vector2 tergetPosition { get; set; }
        public double angle { get; set; }
        public bool is_move { get; set; }

        public void on_update(double delta)
        {
            //var x = tergetPosition.X - this.position.X;
            //var y = tergetPosition.Y - this.position.Y;
            //var distance = Math.Sqrt(x * x + y * y);
            //angle = Math.Atan2(y, x);


            //if (distance < speed)
            //{
            //    this.position = tergetPosition;
            //    return;
            //}
            if (is_move)
                this.position += new Vector2(
                    (float)(Math.Cos(angle) * speed * delta),
                    (float)(Math.Sin(angle) * speed * delta)
                );
        }
        public void setPosition(Vector2 position)
        {
            this.position = position;
        }


    }
}
