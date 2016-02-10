using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lemonade
{
    [DataContract(Name = "TileStatic")]
    public abstract class Tile
    {
        public Texture2D texture;
        public Texture2D blend;

        [DataMember]
        public Rectangle rect;
        public Vector2 position { get { return new Vector2(rect.X, rect.Y); } }
        public Vector2 center { get { return new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2); } }

        public Vector2 scale { get; set; }

        public Vector2 topLeft { get { return new Vector2(rect.X, rect.Y); } }
        public Vector2 topRight { get { return new Vector2(rect.X + rect.Width, rect.Y); } }
        public Vector2 bottomLeft { get { return new Vector2(rect.X, rect.Y + rect.Height); } }
        public Vector2 bottomRight { get { return new Vector2(rect.X + rect.Width, rect.Y + rect.Height); } }

        [DataMember]
        public Directions facing;
        public float facingAngle { get { return MathHelper.ToRadians((int)facing * 90); } }

        public float rotation;

        [DataMember]
        public int id, layer;
        public int index = 0;
        public bool solid, draw, wall;
        public float mass, inv_mass, restitution;

        public abstract void Initialize(ContentManager content);

        public abstract void Update(World world);

        public abstract void Draw(SpriteBatch batch, Camera2D camera);
        //public abstract void DrawShadow(SpriteBatch batch, Camera2d camera);
    }

}
