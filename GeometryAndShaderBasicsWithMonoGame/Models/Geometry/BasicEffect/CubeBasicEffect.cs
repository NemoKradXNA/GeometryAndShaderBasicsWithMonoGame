using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GeometryAndShaderBasicsWithMonoGame.Models.Geometry
{
    /// <summary>
    /// Renders a Cube using the build in BasicEffect
    /// </summary>
    public class CubeBasicEffect : GeometryCubeBase<VertexPositionColorTexture>
    {
        public CubeBasicEffect(Game game) : base(game) { }

        protected override void LoadContent()
        {
            Effect = new BasicEffect(Game.GraphicsDevice);
            Colors = new List<Color>()
            {
                Color.Red, Color.Blue, Color.Green, Color.Yellow,
                Color.Red, Color.Blue, Color.Green, Color.Yellow,
                Color.Red, Color.Blue, Color.Green, Color.Yellow,
                Color.Red, Color.Blue, Color.Green, Color.Yellow,
                Color.Red, Color.Blue, Color.Green, Color.Yellow,
                Color.Red, Color.Blue, Color.Green, Color.Yellow
            };
            base.LoadContent();
        }

        public override void BuildData()
        {
            base.BuildData();

            int vCount = Vertices.Count;
            _vertexArray = new List<VertexPositionColorTexture>();

            for (int v = 0; v < vCount; v++)
                _vertexArray.Add(new VertexPositionColorTexture(Vertices[v], Colors[v], Texcoords[v]));
        }

        public override void SetEffect(GameTime gameTime)
        {
            ((BasicEffect)Effect).World = Transform.World;
            ((BasicEffect)Effect).View = _camera.View;
            ((BasicEffect)Effect).Projection = _camera.Projection;
            ((BasicEffect)Effect).VertexColorEnabled = true;
            ((BasicEffect)Effect).TextureEnabled = true;
            ((BasicEffect)Effect).Texture = Texture;
        }

        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                SetEffect(gameTime);

                Effect.CurrentTechnique.Passes[0].Apply();

                Game.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _vertexArray.ToArray(), 0, _vertexArray.Count, Indicies.ToArray(), 0, Indicies.Count / 3);
            }
        }
    }
}
