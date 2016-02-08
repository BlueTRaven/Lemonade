using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lemonade
{
    public class HurtBox
    {
        public RotatedRectangle rect;

        private int duration, damage;

        public float rotation; //in radians?

        public bool active = true;

        public HurtBox(Rectangle setBounds, int setDamage, int setDuration, float setRotation)
        {
            rect = new RotatedRectangle(setBounds, 0);
            damage = setDamage;
            duration = setDuration;

            rotation = setRotation;
        }

        public void Update()
        {
            duration--;

            if (duration <= 0)
                active = false;
        }

        public Vector2 RotatePoint(Vector2 p)
        {
            var m = Matrix.CreateRotationZ(rotation);

            var refToWorldOrig = p - new Vector2(rect.X, rect.Y);
            Vector2 rotatedVector = Vector2.Transform(refToWorldOrig, m);
            var backToSpriteOrig = rotatedVector + new Vector2(rect.X, rect.Y);
            return backToSpriteOrig;
        }
    }
}
