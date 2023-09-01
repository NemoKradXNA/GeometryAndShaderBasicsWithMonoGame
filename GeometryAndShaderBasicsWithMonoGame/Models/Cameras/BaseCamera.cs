using GeometryAndShaderBasicsWithMonoGame.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryAndShaderBasicsWithMonoGame.Models.Cameras
{
    /// <summary>
    /// A basic camera.
    /// </summary>
    public class BaseCamera : GameComponent, ICameraService, IHasTransform
    {
        /// <summary>
        /// The cameras aspect ratio
        /// </summary>
        public float AspectRatio { get { return (float)_viewport.Width / (float)_viewport.Height; } }

        /// <summary>
        /// The objects transform
        /// </summary>
        public ITransform Transform { get; set; }

        /// <summary>
        /// The cameras View Matrix (View Space)
        /// </summary>
        public Matrix View { get { return Matrix.Invert(Transform.World); } }

        /// <summary>
        /// The cameras Projection Matrix (Projection Space)
        /// </summary>
        public Matrix Projection { get { return Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, Viewport.MinDepth, Viewport.MaxDepth); } }

        /// <summary>
        /// The current viewport we are rendering to
        /// </summary>
        protected Viewport _viewport;

        /// <summary>
        /// The current viewport we are rendering to
        /// </summary>
        public Viewport Viewport { get { return _viewport; } set { _viewport = value; } }

        /// <summary>
        /// The cameras field of view
        /// </summary>
        public float FieldOfView { get; set; }

        /// <summary>
        /// The cameras near clip plane distance, anything closer than this value to the camera will not be drawn.
        /// </summary>
        public float NearClipPlane { get; set; }

        /// <summary>
        /// The cameras far clip plane distance, anything further than this value will not be drawn
        /// </summary>
        public float FarClipPlane { get; set; }

        /// <summary>
        /// The color used to clear the graphics device before each draw call.
        /// </summary>
        public Color ClearColor { get; set; } = Color.CornflowerBlue;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="game"></param>
        /// <param name="fov"></param>
        /// <param name="nearClipPlane"></param>
        /// <param name="farClipPlane"></param>
        public BaseCamera(Game game, float fov = .7583982f, float nearClipPlane = .01f, float farClipPlane = 10000f) : base(game)
        {
            // Make sure we are not trying to add the same type
            if (game.Services.GetService(typeof(ICameraService)) != null)
            {
                Game.Services.RemoveService(typeof(ICameraService));
                Game.Components.Remove(this);
            }

            game.Services.AddService(typeof(ICameraService), this);

            Game.Components.Add(this);

            NearClipPlane = nearClipPlane;
            FarClipPlane = farClipPlane;
            FieldOfView = fov;

            Transform = new Transform();
            Transform.Scale = Vector3.One;
            Transform.Rotation = Quaternion.Identity;
        }

        /// <summary>
        /// Initialize method.
        /// </summary>
        public override void Initialize()
        {
            _viewport = Game.GraphicsDevice.Viewport;
            base.Initialize();
        }

        /// <summary>
        /// Update method
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _viewport.MinDepth = NearClipPlane;
            _viewport.MaxDepth = FarClipPlane;
        }
    }
}
