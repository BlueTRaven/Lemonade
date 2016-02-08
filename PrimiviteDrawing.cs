using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lemonade
{
    static public class PrimiviteDrawing
    {
        static public void DrawRectangle(Texture2D whitePixel, SpriteBatch batch, Rectangle area, int width, Color color)
        {
            if (whitePixel == null)
            {
                whitePixel = new Texture2D(batch.GraphicsDevice, 1, 1);
                whitePixel.SetData<Color>(new Color[] { color });
            }

            batch.Draw(whitePixel, new Rectangle(area.X, area.Y, area.Width, width), color);
            batch.Draw(whitePixel, new Rectangle(area.X, area.Y, width, area.Height), color);
            batch.Draw(whitePixel, new Rectangle(area.X + area.Width - width, area.Y, width, area.Height), color);
            batch.Draw(whitePixel, new Rectangle(area.X, area.Y + area.Height - width, area.Width, width), color);
        }
        static public void DrawRectangle(Texture2D whitePixel, SpriteBatch batch, Rectangle area, Color color)
        {
            if (whitePixel == null)
            {
                whitePixel = new Texture2D(batch.GraphicsDevice, 1, 1);
                whitePixel.SetData<Color>(new Color[] { color });
            }

            batch.Draw(whitePixel, area, color);

            //DrawRectangle(whitePixel, batch, area, 1, color);
        }

        static public void DrawRectangle(Texture2D whitePixel, SpriteBatch batch, Rectangle area, Color color, float rotation, Vector2 origin )
        {
            if (whitePixel == null)
            {
                whitePixel = new Texture2D(batch.GraphicsDevice, 1, 1);
                whitePixel.SetData<Color>(new Color[] { color });
            }

            batch.Draw(whitePixel, area, area, color, rotation, origin, SpriteEffects.None, 0f);

            //DrawRectangle(whitePixel, batch, area, 1, color);
        }
        static public void DrawGradientRectangle(Texture2D whitePixel, SpriteBatch batch, Rectangle area, Color color)
        {
            //Color c = Color.Lerp(color1, color2, 0.5f);

            if (whitePixel == null)
            {
                whitePixel = new Texture2D(batch.GraphicsDevice, area.Width, area.Height);
                whitePixel.SetData<Color>(GetGradientColors((uint)area.Width, (uint)area.Height));
            }

            batch.Draw(whitePixel, area, color);
        }
        private static Color[] GetGradientColors(uint width, uint height)
        {
            //Declare variables
            Color[] result;
            float
                increment;
            int color;

            //Determine that both height and width are greater than 0
            if (!(width > 0 && height > 0))
            //exit the function with a null color array
            { return null; }

            //Setup the result array
            result = new Color[width * height];

            //Calculate the increment values
            increment = (float)255 / (result.Length);

            //Loop through each color
            for (int i = 0; i < result.Length; i++)
            {
                color = (int)(increment * i);

                result[i] = new Color(
                    color,
                    color,
                    color);
            }

            //return the color
            return result;
        }

        public static void DrawCircle(Texture2D whitePixel, SpriteBatch batch, Vector2 center, float radius, Color color, int lineWidth = 2, int segments = 16)
        {
            if (whitePixel == null)
            {
                whitePixel = new Texture2D(batch.GraphicsDevice, 1, 1);
                whitePixel.SetData<Color>(new Color[] { color });
            }

            Vector2[] vertex = new Vector2[segments];

            double increment = Math.PI * 2.0 / segments;
            double theta = 0.0;

            for (int i = 0; i < segments; i++)
            {
                vertex[i] = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                theta += increment;
            }

            DrawPolygon(whitePixel, batch, vertex, segments, color, lineWidth);
        }
        public static void DrawPolygon(Texture2D whitePixel, SpriteBatch batch, Vector2[] vertex, int count, Color color, int lineWidth)
        {
            if (whitePixel == null)
            {
                whitePixel = new Texture2D(batch.GraphicsDevice, 1, 1);
                whitePixel.SetData<Color>(new Color[] { color });
            }

            if (count > 0)
            {
                for (int i = 0; i < count - 1; i++)
                {
                    DrawLineSegment(whitePixel, batch, vertex[i], vertex[i + 1], color, lineWidth);
                }
                DrawLineSegment(whitePixel, batch, vertex[count - 1], vertex[0], color, lineWidth);
            }
        }
        public static void DrawLineSegment(Texture2D whitePixel, SpriteBatch batch, Vector2 point1, Vector2 point2, Color color, int lineWidth)
        {
            if (whitePixel == null)
            {
                whitePixel = new Texture2D(batch.GraphicsDevice, 1, 1);
                whitePixel.SetData<Color>(new Color[] { color });
            }

            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(whitePixel, point1, null, color,
            angle, Vector2.Zero, new Vector2(length, lineWidth),
            SpriteEffects.None, 0f);
        }
    }
}
