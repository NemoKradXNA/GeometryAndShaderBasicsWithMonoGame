using GeometryAndShaderBasicsWithMonoGame.Interfaces;
using GeometryAndShaderBasicsWithMonoGame.Models.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GeometryAndShaderBasicsWithMonoGame
{
    /// <summary>
    /// Base abstract class for our examples.
    /// </summary>
    public abstract class BaseGame : Game
    {
        protected GraphicsDeviceManager _graphics;
        protected SpriteBatch _spriteBatch;

        /// <summary>
        /// Speed we can move the camera about.
        /// </summary>
        protected float _translateSpeed = .005f;
        
        /// <summary>
        /// Speed we can rotate the camera at.
        /// </summary>
        protected float _rotateSpeed = .001f;

        /// <summary>
        /// The camera
        /// </summary>
        protected ICameraService _camera;

        /// <summary>
        /// The starting rasteriser state
        /// </summary>
        protected RasterizerState _orgRasterizerState;

        protected SpriteFont _spriteFont;

        private float leftMargin = 8;
        private float topMargin = 8;
        private Vector2 shadowOffset = new Vector2(-2, 2);
        private int line = 0;

        protected bool _renderWireFrame = false;
        protected bool _cullingOff = false;

        protected List<string> instructionText;

        protected KeyboardState _keyboardState;
        protected KeyboardState _lastKeyboardState;

        protected bool _exitGameConfirmed = false;
        protected bool _eixtThisGameNow = false;

        protected string BlogUrl = "https://bedroom-coder.blogspot.com/";

        public BaseGame() : base()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _camera = new BaseCamera(this);
            _camera.Transform.Position = new Vector3(0, 0, 10);
        }

        protected override void Initialize()
        {
            instructionText = new List<string>()
            {
                Window.Title,
                "Camera Translation = WASD",
                "Camera Rotation = Arrow Keys",
                "F1 = Toggle Wire frame",
                "F2 = Toggle CullMode",
                "F12 = Goto Blog Page",
                "Exit = Esc",
            };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spriteFont = Content.Load<SpriteFont>("Fonts/font");

            base.LoadContent();

            _orgRasterizerState = GraphicsDevice.RasterizerState;
        }

        protected override void Update(GameTime gameTime)
        {
            _lastKeyboardState = _keyboardState;
            _keyboardState = Keyboard.GetState();

            if (_exitGameConfirmed)
            {
                Exit();
            }

            if (!_eixtThisGameNow)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || _keyboardState.IsKeyDown(Keys.Escape))
                {
                    _eixtThisGameNow = true;
                }

                if (_keyboardState.IsKeyDown(Keys.F12) && _lastKeyboardState.IsKeyUp(Keys.F12))
                {
                    GotoBlogUrl();
                }

                float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                // Camera controls.
                if (_keyboardState.IsKeyDown(Keys.W))
                    _camera.Transform.Position += Vector3.Transform(Vector3.Forward * _translateSpeed * deltaTime, Matrix.CreateFromQuaternion(_camera.Transform.Rotation));
                if (_keyboardState.IsKeyDown(Keys.A))
                    _camera.Transform.Position += Vector3.Transform(Vector3.Left * _translateSpeed * deltaTime, Matrix.CreateFromQuaternion(_camera.Transform.Rotation));
                if (_keyboardState.IsKeyDown(Keys.D))
                    _camera.Transform.Position += Vector3.Transform(Vector3.Right * _translateSpeed * deltaTime, Matrix.CreateFromQuaternion(_camera.Transform.Rotation));
                if (_keyboardState.IsKeyDown(Keys.S))
                    _camera.Transform.Position += Vector3.Transform(Vector3.Backward * _translateSpeed * deltaTime, Matrix.CreateFromQuaternion(_camera.Transform.Rotation));

                Vector3 axis;
                if (_keyboardState.IsKeyDown(Keys.Left))
                {
                    axis = Vector3.Transform(Vector3.Up, Matrix.CreateFromQuaternion(_camera.Transform.Rotation));
                    _camera.Transform.Rotation = Quaternion.Normalize(Quaternion.CreateFromAxisAngle(axis, _rotateSpeed * deltaTime) * _camera.Transform.Rotation);
                }

                if (_keyboardState.IsKeyDown(Keys.Right))
                {
                    axis = Vector3.Transform(Vector3.Down, Matrix.CreateFromQuaternion(_camera.Transform.Rotation));
                    _camera.Transform.Rotation = Quaternion.Normalize(Quaternion.CreateFromAxisAngle(axis, _rotateSpeed * deltaTime) * _camera.Transform.Rotation);
                }

                if (_keyboardState.IsKeyDown(Keys.Up))
                {
                    axis = Vector3.Transform(Vector3.Right, Matrix.CreateFromQuaternion(_camera.Transform.Rotation));
                    _camera.Transform.Rotation = Quaternion.Normalize(Quaternion.CreateFromAxisAngle(axis, _rotateSpeed * deltaTime) * _camera.Transform.Rotation);
                }

                if (_keyboardState.IsKeyDown(Keys.Down))
                {
                    axis = Vector3.Transform(Vector3.Left, Matrix.CreateFromQuaternion(_camera.Transform.Rotation));
                    _camera.Transform.Rotation = Quaternion.Normalize(Quaternion.CreateFromAxisAngle(axis, _rotateSpeed * deltaTime) * _camera.Transform.Rotation);
                }

                if (_keyboardState.IsKeyDown(Keys.F1) && _lastKeyboardState.IsKeyUp(Keys.F1))
                {
                    _renderWireFrame = !_renderWireFrame;
                    SetRasterizerState();
                }

                if (_keyboardState.IsKeyDown(Keys.F2) && _lastKeyboardState.IsKeyUp(Keys.F2))
                {
                    _cullingOff = !_cullingOff;
                    SetRasterizerState();
                }

            }
            else
            {
                if (_keyboardState.IsKeyDown(Keys.Y) && _lastKeyboardState.IsKeyUp(Keys.Y))
                {
                    _exitGameConfirmed = true;
                }

                if (_keyboardState.IsKeyDown(Keys.N) && _lastKeyboardState.IsKeyUp(Keys.N))
                {
                    _exitGameConfirmed = false;
                    _eixtThisGameNow = false;
                }
            }

            base.Update(gameTime);
        }

        protected void GotoBlogUrl()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                BlogUrl = BlogUrl.Replace("&", "^&");
                Process.Start(new ProcessStartInfo(BlogUrl) { UseShellExecute = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", BlogUrl);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", BlogUrl);
            }
        }

        public void SetRasterizerState() 
        {
            GraphicsDevice.RasterizerState = new RasterizerState() { FillMode = _renderWireFrame ? FillMode.WireFrame : FillMode.Solid, CullMode = _cullingOff ? CullMode.None : CullMode.CullCounterClockwiseFace };
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_camera != null ? _camera.ClearColor : Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        protected override void EndDraw()
        {
            line = 0;

            RasterizerState rasterizerState = GraphicsDevice.RasterizerState;
            BlendState blendState = GraphicsDevice.BlendState;
            DepthStencilState depthStencilState = GraphicsDevice.DepthStencilState;

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            foreach (string text in instructionText)
            {
                DrawString(text);
            }

            if (_eixtThisGameNow)
            {
                Texture2D bgT = new Texture2D(GraphicsDevice, 1, 1);
                bgT.SetData<Color>(new Color[] { Color.White });

                Vector2 vc = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height) * .5f;

                Point c = vc.ToPoint();
                Point s = new Point(400,200);
                Point h = new Point(200, 100);
                Point p = c - (h);

                _spriteBatch.Draw(bgT, new Rectangle(p.X,p.Y,s.X,s.Y), new Color(0, 0, 0, .5f));
                _spriteBatch.Draw(bgT, new Rectangle(p.X+4, p.Y+4, s.X-8, s.Y-8), new Color(0, 0, 0, .5f));

                string msg = "Are you sure you want to quit? ( Y / N )";
                Vector2 ts = _spriteFont.MeasureString(msg);
                Vector2 tp = vc - (ts * .5f);

                _spriteBatch.DrawString(_spriteFont, msg, tp - new Vector2(1,1), Color.Black);
                _spriteBatch.DrawString(_spriteFont, msg, tp, Color.White);
            }

            _spriteBatch.End();

            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.BlendState = blendState;
            GraphicsDevice.DepthStencilState = depthStencilState;

            base.EndDraw();
        }

        protected void DrawString(string text)
        {
            Vector2 pos = new Vector2(leftMargin, topMargin) + new Vector2(0, line);

            _spriteBatch.DrawString(_spriteFont, text, pos + shadowOffset, Color.Black);
            _spriteBatch.DrawString(_spriteFont, text, pos, Color.Gold);

            line += _spriteFont.LineSpacing;
        }

    }
}
