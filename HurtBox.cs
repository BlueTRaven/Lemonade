using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lemonade.entity;

namespace Lemonade
{
    public class HurtBox
    {
        Entity owner;

        public RotatedRectangle rect;

        private int duration, damage;

        public float rotation; //in radians?

        public bool active = true;

        public HurtBox(Entity owner, Rectangle setBounds, Vector2 origin, int setDamage, int setDuration, float setRotation)
        {
            this.owner = owner;
            rect = new RotatedRectangle(setBounds, origin, setRotation);
            damage = setDamage;
            duration = setDuration;

            rotation = setRotation;
        }

        public void Update()
        {
            duration--;

            if (duration <= 0)
                active = false;

            //Vector2 positionToRotation = Vector2.Transform(new Vector2(owner.center.X, owner.center.Y + 32) - owner.center, Matrix.CreateRotationZ(-rotation)) + owner.center;

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
