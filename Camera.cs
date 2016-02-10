using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lemonade
{
    public class Camera2D
    {
        private const float zoomUpperLimit = 1.5f;
        private const float zoomLowerLimit = .5f;

        private float _zoom;
        private Matrix _transform;
        private Vector2 _pos;
        private float _rotation;
        private int _viewportWidth;
        private int _viewportHeight;
        private int _worldWidth;
        private int _worldHeight;
        
        public Camera2D(Viewport viewport, Rectangle setWorldRect)
        {
            _zoom = 1f;
            _rotation = 0.0f;
            _pos = new Vector2(viewport.Bounds.X, viewport.Bounds.Y);
            _viewportWidth = viewport.Width;
            _viewportHeight = viewport.Height;
            _worldWidth = setWorldRect.Width;
            _worldHeight = setWorldRect.Height;
        }

        #region Properties

        public Matrix inverseTransform { get { return Matrix.Invert(GetTransformation()); } }

        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                if (_zoom < zoomLowerLimit)
                {
                    Logger.Log("Tried to set invalid camera zoom level - too low", true);
                    _zoom = zoomLowerLimit;
                }
                if (_zoom > zoomUpperLimit)
                {
                    Logger.Log("Tried to set invalid camera zoom level - too high", true);
                    _zoom = zoomUpperLimit;
                }
            }
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public Vector2 Pos
        {
            get { return _pos; }
            set
            {
                float leftBarrier = (float)_viewportWidth *
                       .5f / _zoom;
                float rightBarrier = _worldWidth -
                       (float)_viewportWidth * .5f / _zoom;
                float topBarrier = _worldHeight -
                           (float)_viewportHeight * .5f / _zoom;
                float bottomBarrier = (float)_viewportHeight *
                   .5f / _zoom;
                _pos = value;
                if (_pos.X < leftBarrier)
                    _pos.X = leftBarrier;
                if (_pos.X > rightBarrier)
                    _pos.X = rightBarrier;
                if (_pos.Y > topBarrier)
                    _pos.Y = topBarrier;
                if (_pos.Y < bottomBarrier)
                    _pos.Y = bottomBarrier;
            }
        }

        #endregion
        public void Move(Vector2 amount)
        {
            _pos += amount;
        }

        public void MoveTo(Vector2 toLocation, Vector2 speed, bool instant = false)
        {
            Vector2 dir = toLocation - Pos;
            if (dir != Vector2.Zero)
                dir.Normalize();

            Pos += dir * speed;

            if (instant)
                Pos = toLocation;

            //ClampCameraToWorldBounds();
        }

        private void ClampCameraToWorldBounds()
        {   //NOTE: probably won't work with zooming. Not sure if zoom will be implemented though
            float minimumX = (_viewportWidth / 2);
            float minimumY = (_viewportHeight / 2);
            Vector2 minimumPos = new Vector2(minimumX, minimumY);


            float maximumX = _worldWidth - (_viewportWidth / 2);
            float maximumY = _worldHeight - (_viewportWidth / 2);
            Vector2 maximumPos = new Vector2(maximumX, maximumY);

            _pos = Vector2.Clamp(_pos, minimumPos, maximumPos);
        }

        public Matrix GetTransformation()
        {
            _transform =
               Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
               Matrix.CreateRotationZ(Rotation) *
               Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
               Matrix.CreateTranslation(new Vector3(_viewportWidth * 0.5f,
                   _viewportHeight * 0.5f, 0));

            return _transform;
        }
    }
}
