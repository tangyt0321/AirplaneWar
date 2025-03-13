using DocumentFormat.OpenXml.Office2010.CustomUI;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime;

namespace airplaneWar.GameLogic
{
    public class Camera
    {
        public Vector2 position { get; set; }
        public double speed { get; set; } = 1.0;
        public Size size { get; set; }
        public Vector2 tergetPosition { get; set; }
        public double angle { get; set; }
        public bool is_move { get; set; }
        public bool is_follow { get; set; }

        public void on_update(double delta)
        {
            if (is_move)
            {
                this.position += new Vector2(
                    (float)(Math.Cos(angle) * speed * delta),
                    (float)(Math.Sin(angle) * speed * delta)
                );
            }
        }
        public void setPosition(Vector2 position)
        {
            this.position = position;
        }


    }
}
