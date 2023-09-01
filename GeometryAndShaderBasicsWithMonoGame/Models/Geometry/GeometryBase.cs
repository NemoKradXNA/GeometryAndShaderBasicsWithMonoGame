using GeometryAndShaderBasicsWithMonoGame.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GeometryAndShaderBasicsWithMonoGame.Models.Geometry
{
    /// <summary>
    /// The base class we are using to render all of our custom geometry
    /// </summary>
    /// <typeparam name="T">The IVertextType to be used.</typeparam>
    public abstract class GeometryBase<T> : DrawableGameComponent, IHasTransform where T : IVertexType
    {
        /// <summary>
        /// All the position data four our vertices's
        /// </summary>
        public List<Vector3> Vertices { get; protected set; }

        /// <summary>
        /// All the normals for our vertices
        /// </summary>
        public List<Vector3> Normals { get; protected set; }

        /// <summary>
        /// All the texture coordinates for our vertices
        /// </summary>
        public List<Vector2> Texcoords { get; protected set; }

        /// <summary>
        /// All the tangents for our vertices
        /// </summary>
        public List<Vector3> Tangents { get; protected set; }

        /// <summary>
        /// All the colours for our vertices
        /// </summary>
        public List<Color> Colors { get; protected set; }

        /// <summary>
        /// The index or draw order used to draw our vertices
        /// </summary>
        public List<int> Indicies { get; protected set; }

        #region Texture Asset Names
        protected string _textureAsset;
        protected string _normalMapAsset;
        protected string _heightMapAsset;
        protected string _occlusionAsset;
        protected string _specularAsset;
        protected string _environmentMapAsset;
        protected string _effectAsset;
        #endregion

        #region Texture Assets
        protected Texture2D _texture;
        protected Texture2D _normalMapTexture;
        protected Texture2D _occlusionTexture;
        protected Texture2D _specularTexture;
        protected Texture2D _heigntMapTexture;
        protected TextureCube _environmentMap;
        #endregion

        #region Lighting
        public Vector3 LightDirection = new Vector3(-.5f, .5f, .5f);

        public float AmbientIntensity { get; set; } = .25f;
        public Color AmbientColor { get; set; } = Color.CornflowerBlue;
        public float AmbientPower { get; set; } = .25f;
        #endregion

        public float UVMultiplier { get; set; }

        protected ICameraService _camera { get { return Game.Services.GetService<ICameraService>(); } }

        /// <summary>
        /// The objects transform
        /// </summary>
        public ITransform Transform { get; set; }

        /// <summary>
        /// Vertex array for the given IVertexType
        /// </summary>
        protected List<T> _vertexArray { get; set; }

        /// <summary>
        /// Diffuse texture for geometry
        /// </summary>
        public Texture2D Texture
        {
            get
            {
                if (_texture == null)
                {
                    _texture = new Texture2D(Game.GraphicsDevice, 1, 1);
                    _texture.SetData(new Color[] { Color.White });
                }

                return _texture;
            }

            set
            {
                _texture = value;
            }
        }

        /// <summary>
        /// Normal map texture for geometry
        /// </summary>
        public Texture2D NormalMap
        {
            get
            {
                if (_normalMapTexture == null)
                {
                    _normalMapTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
                    _normalMapTexture.SetData(new Color[] { new Color(128, 128, 255) });
                }

                return _normalMapTexture;
            }

            set
            {
                _normalMapTexture = value;
            }
        }

        /// <summary>
        /// Heigh map texture for geometry
        /// </summary>
        public Texture2D HeightMap
        {
            get
            {
                if (_heigntMapTexture == null)
                {
                    _heigntMapTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
                    _heigntMapTexture.SetData(new Color[] { Color.Transparent });
                }

                return _heigntMapTexture;
            }

            set
            {
                _heigntMapTexture = value;
            }
        }

        /// <summary>
        /// Occlusion map texture for geometry
        /// </summary>
        public Texture2D OcclusionMap
        {
            get
            {
                if (_occlusionTexture == null)
                {
                    _occlusionTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
                    _occlusionTexture.SetData(new Color[] { Color.White });
                }

                return _occlusionTexture;
            }

            set
            {
                _occlusionTexture = value;
            }
        }

        /// <summary>
        /// Specular map texture for geometry
        /// </summary>
        public Texture2D SpecularMap
        {
            get
            {
                if (_specularTexture == null)
                {
                    _specularTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
                    _specularTexture.SetData(new Color[] { Color.Black });
                }

                return _specularTexture;
            }

            set
            {
                _specularTexture = value;
            }
        }

        /// <summary>
        /// The compiles shader to be used
        /// </summary>
        public Effect Effect { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="game"></param>
        public GeometryBase(Game game) : base(game)
        {
            Transform = new Transform();
        }

        /// <summary>
        /// If true, normals will be calculated for us.
        /// </summary>
        public bool AutoCalculateNormals { get; set; } = false;

        /// <summary>
        /// If true, tangents will be calculated for us.
        /// </summary>
        public bool AutoCalculateTangents { get; set; } = false;

        /// <summary>
        /// MEthod to set the shader values
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void SetEffect(GameTime gameTime)
        {
            if (Effect != null)
            {
                if (Effect.Parameters["WorldViewProjection"] != null)
                    Effect.Parameters["WorldViewProjection"].SetValue(Transform.World * _camera.View * _camera.Projection);

                if (Effect.Parameters["World"] != null)
                    Effect.Parameters["World"].SetValue(Transform.World);

                if (Effect.Parameters["View"] != null)
                    Effect.Parameters["View"].SetValue(_camera.View);

                if (Effect.Parameters["Projection"] != null)
                    Effect.Parameters["Projection"].SetValue(_camera.Projection);

                if (Effect.Parameters["textureMap"] != null)
                    Effect.Parameters["textureMap"].SetValue(Texture);

                if (Effect.Parameters["normalMap"] != null)
                    Effect.Parameters["normalMap"].SetValue(_normalMapTexture);

                if (Effect.Parameters["occlusionMap"] != null)
                    Effect.Parameters["occlusionMap"].SetValue(_occlusionTexture);

                if (Effect.Parameters["specularMap"] != null)
                    Effect.Parameters["specularMap"].SetValue(_specularTexture);

                if (Effect.Parameters["lightDirection"] != null)
                    Effect.Parameters["lightDirection"].SetValue(LightDirection);

                if (Effect.Parameters["CameraPosition"] != null)
                    Effect.Parameters["CameraPosition"].SetValue(_camera.Transform.Position);

                if (Effect.Parameters["uvMultiplier"] != null)
                    Effect.Parameters["uvMultiplier"].SetValue(UVMultiplier);

                if (Effect.Parameters["AmbientColor"] != null)
                    Effect.Parameters["AmbientColor"].SetValue(AmbientColor.ToVector4());

                if (Effect.Parameters["AmbientPower"] != null)
                    Effect.Parameters["AmbientPower"].SetValue(AmbientPower);

                if (Effect.Parameters["EnvironmentMap"] != null)
                    Effect.Parameters["EnvironmentMap"].SetValue(_environmentMap);

                if (Effect.Parameters["ViewInverse"] != null)
                    Effect.Parameters["ViewInverse"].SetValue(Matrix.Invert(_camera.View));

                if (Effect.Parameters["Time"] != null)
                    Effect.Parameters["Time"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);

                if (Effect.Parameters["heightMap"] != null)
                    Effect.Parameters["heightMap"].SetValue(_heigntMapTexture);
            }
        }

        /// <summary>
        /// LoadContext
        /// </summary>
        protected override void LoadContent()
        {
            if (!string.IsNullOrEmpty(_textureAsset))
                Texture = Game.Content.Load<Texture2D>(_textureAsset);

            if (!string.IsNullOrEmpty(_environmentMapAsset))
                _environmentMap = Game.Content.Load<TextureCube>(_environmentMapAsset);

            if (!string.IsNullOrEmpty(_normalMapAsset))
                _normalMapTexture = Game.Content.Load<Texture2D>(_normalMapAsset);
            else
            {
                _normalMapTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
                _normalMapTexture.SetData(new Color[] { new Color(128, 128, 255) });
            }

            if (!string.IsNullOrEmpty(_heightMapAsset))
                _heigntMapTexture = Game.Content.Load<Texture2D>(_heightMapAsset);
            else
            {
                _heigntMapTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
                _heigntMapTexture.SetData(new Color[] { Color.White });
            }

            if (!string.IsNullOrEmpty(_occlusionAsset))
                _occlusionTexture = Game.Content.Load<Texture2D>(_occlusionAsset);
            else
            {
                _occlusionTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
                _occlusionTexture.SetData(new Color[] { Color.White });
            }

            if (!string.IsNullOrEmpty(_specularAsset))
                _specularTexture = Game.Content.Load<Texture2D>(_specularAsset);
            else
            {
                _specularTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
                _specularTexture.SetData(new Color[] { Color.Black });
            }

            if (_effectAsset != null)
                Effect = Game.Content.Load<Effect>(_effectAsset);

            BuildData();
        }

        /// <summary>
        /// MEthod to load the vertex data into the vertex array.
        /// </summary>
        public virtual void BuildData()
        {
            if (AutoCalculateNormals)
            {
                CalculateNormals();
            }

            if (AutoCalculateTangents)
            {
                CalculateTangents();
            }
        }

        /// <summary>
        /// Some very useful functions I found here, for calculating tangent and normals data from the vertex and texture coordinate lists.
        /// https://gamedev.stackexchange.com/questions/68612/how-to-compute-tangent-and-bitangent-vectors
        /// http://foundationsofgameenginedev.com/FGED2-sample.pdf 
        /// </summary>
        public virtual void CalculateTangents()
        {
            int triangleCount = Indicies.Count;
            int vertexCount = Vertices.Count;

            Vector3[] tan1 = new Vector3[vertexCount];

            for (int i = 0; i < triangleCount; i += 3)
            {
                // Get the index for this triangle.
                int i1 = Indicies[i + 0];
                int i2 = Indicies[i + 1];
                int i3 = Indicies[i + 2];

                // Get the positions of each vertex.
                Vector3 v1 = Vertices[i1];
                Vector3 v2 = Vertices[i2];
                Vector3 v3 = Vertices[i3];

                // Get the texture coordinates of each vertex.
                Vector2 w1 = Texcoords[i1];
                Vector2 w2 = Texcoords[i2];
                Vector2 w3 = Texcoords[i3];

                // Calculate the vertex directions
                Vector3 vd1 = v2 - v1;
                Vector3 vd2 = v3 - v1;

                // Calculate the texture coordinates directions.
                Vector2 td1 = w2 - w1;
                Vector2 td2 = w3 - w1;

                // Calculate final direction.
                Vector3 dir = ((vd1 * td2.Y) - (vd2 * td1.Y));

                dir.Normalize();

                // Store ready to be returned in vertex order.
                tan1[i1] += dir;
                tan1[i2] += dir;
                tan1[i3] += dir;
            }

            Tangents = new List<Vector3>();

            // Populate tangents in vertex order. 
            for (int v = 0; v < vertexCount; v++)
                Tangents.Add(tan1[v]);
        }

        public virtual void CalculateNormals()
        {
            Normals = new List<Vector3>();

            // clear out the normals
            foreach (Vector3 v in Vertices)
                Normals.Add(Vector3.Zero);

            // Calculate the new normals.
            foreach (Vector3 v in Vertices)
            {
                for (int i = 0; i < Indicies.Count; i += 3)
                {
                    int idxA = Indicies[i];
                    int idxB = Indicies[i + 1];
                    int idxC = Indicies[i + 2];

                    Vector3 A = Vertices[idxA];
                    Vector3 B = Vertices[idxB];
                    Vector3 C = Vertices[idxC];

                    Vector3 p = Vector3.Cross(C - A, B - A);
                    Normals[idxA] += p;
                    Normals[idxB] += p;
                    Normals[idxC] += p;
                }

            }

            // Normalize
            foreach (Vector3 v in Normals)
                v.Normalize();
        }
    }
}
