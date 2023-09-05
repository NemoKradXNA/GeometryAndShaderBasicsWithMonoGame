using GeometryAndShaderBasicsWithMonoGame.Models.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GeometryAndShaderBasicsWithMonoGame
{
    public class Game_Chapter_2 : BaseGame
    {
        protected TriangleBasicEffect triangle;
        protected QuadBasicEffect quad;
        protected CubeBasicEffect cube;

        public Game_Chapter_2() : base()
        {

            triangle = new TriangleBasicEffect(this);
            triangle.Transform.Position = new Vector3(-.5f, 0, 0);

            quad = new QuadBasicEffect(this);
            quad.Transform.Position = new Vector3(.5f, 0, 0);

            cube = new CubeBasicEffect(this);
            cube.Transform.Position = new Vector3(0, 0, -5);

            Components.Add(triangle);
            Components.Add(quad);
            Components.Add(cube);

            Window.Title = "Chapter 2";
            BlogUrl = "https://bedroom-coder.blogspot.com/2023/08/geometry-shader-basics-geometry-in-code.html";
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            instructionText.Add("F3 Toggle Triangle Visible");
            instructionText.Add("F4 Toggle Quad Visible");
            instructionText.Add("F5 Toggle Cube Visible");

            //triangle.Texture = Content.Load<Texture2D>("Textures/Illuminati");
            //quad.Texture = Content.Load<Texture2D>("Textures/Illuminati");
            //cube.Texture = Content.Load<Texture2D>("Textures/Illuminati");
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!_eixtThisGameNow)
            {
                if (Keyboard.GetState().IsKeyUp(Keys.F3) && _lastKeyboardState.IsKeyDown(Keys.F3))
                {
                    triangle.Visible = !triangle.Visible;
                }

                if (Keyboard.GetState().IsKeyUp(Keys.F4) && _lastKeyboardState.IsKeyDown(Keys.F4))
                {
                    quad.Visible = !quad.Visible;
                }

                if (Keyboard.GetState().IsKeyUp(Keys.F5) && _lastKeyboardState.IsKeyDown(Keys.F5))
                {
                    cube.Visible = !cube.Visible;
                }
            }
        }
    }
}