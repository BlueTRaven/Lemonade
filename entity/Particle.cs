using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lemonade.entity
{
    public class Particle : Entity
    {
        /// <summary>
        /// ENUM AITYPE:
        /// enum of ai types.
        /// BASIC: moves in velocity direction, slows down.
        /// </summary>
        public enum AiType
        {
            BASIC
        }

        AiType ai;
        int timeLeft;
        int maxTimeLeft;
        Color color;

        bool timeFade = false;
        float fadeAlpha { get { return 1 * ((float)timeLeft / (float)maxTimeLeft); } }

        public Particle(Vector2 setPosition, Vector2 setVelocity, AiType setAiType, Color setColor)
        {
            position = setPosition;
            velocity = setVelocity;
            ai = setAiType;
            color = setColor;
            
            if (ai == AiType.BASIC)
            {
                timeLeft = 200;
                maxTimeLeft = 200;
            }
        }

        public Particle(Vector2 setPosition, Vector2 setVelocity, AiType setAiType, int setTimeLeft, Color setColor)
        {
            position = setPosition;
            velocity = setVelocity;
            ai = setAiType;
            timeLeft = setTimeLeft;
            maxTimeLeft = setTimeLeft;
            color = setColor;
        }

        public override void Initialize()
        {
            Name = "Particle";
        }

        public override void Update()
        {
            //Console.WriteLine(fadeAlpha);
            timeLeft--;
            if (ai == AiType.BASIC)
            {
                position += velocity;

                velocity *= 0.95f;
            }

            if (timeLeft <= 0)
            {
                dead = true;
            }
        }

        public void collide(Entity entityHit)
        {
            if (entityHit is Player)
            {

            }
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, camera.GetTransformation());

            if (!timeFade)
            {
                PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle((int)position.X, (int)position.Y, 2, 2), color);
            }
            else
            {
                PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle((int)position.X, (int)position.Y, 2, 2), new Color(1, 0, 0, fadeAlpha));
            }
            batch.End();
        }

        /// <summary>
        /// Creates a new particle.
        /// </summary>
        /// <param name="position">Position where the particle is created.</param>
        /// <param name="velocity">Initial velocity of particle.</param>
        /// <param name="ai">AI type of particle. SEE: Particle.AiType for individual explanations.</param>
        /// <param name="color">Color of the particle.</param>
        /// <param name="timeLeft">NULLABLE. How long the particle lasts (60 = 1s)</param>
        /// <returns>instance of created particle.</returns>
        public static Particle createParticle(Vector2 position, Vector2 velocity, Particle.AiType ai, Color color, int? timeLeft = null)
        {
            Particle p;

            if (timeLeft == null)
            {
                p = new Particle(position, velocity, ai, color);
            }
            else
            {
                p = new Particle(position, velocity, ai, (int)timeLeft, color);
            }

            p.Initialize();

            World.particles.Add(p);

            return p;
        }
    }
}
