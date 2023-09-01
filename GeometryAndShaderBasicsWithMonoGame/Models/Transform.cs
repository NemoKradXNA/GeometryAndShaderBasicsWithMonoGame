using GeometryAndShaderBasicsWithMonoGame.Interfaces;
using Microsoft.Xna.Framework;

namespace GeometryAndShaderBasicsWithMonoGame.Models
{
    /// <summary>
    /// This is a very basic transform class.
    /// </summary>
    public class Transform : ITransform
    {
        /// <summary>
        /// Scale of the object to be transformed. Vector.One is default.
        /// </summary>
        public virtual Vector3 Scale { get; set; } = Vector3.One;

        /// <summary>
        /// Position of object to be transformed.
        /// </summary>
        public virtual Vector3 Position { get; set; } = Vector3.Zero;

        /// <summary>
        /// Rotation of object to be transformed.
        /// </summary>
        public virtual Quaternion Rotation { get; set; } = Quaternion.Identity;

       
        /// <summary>
        /// Objects world matrix
        /// </summary>
        public virtual Matrix World
        {
            get
            {
                return Matrix.CreateScale(Scale) *
                   Matrix.CreateFromQuaternion(Rotation) *
                   Matrix.CreateTranslation(Position);
            }
        }
    }
}
