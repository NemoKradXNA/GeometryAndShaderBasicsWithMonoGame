using Microsoft.Xna.Framework;

namespace GeometryAndShaderBasicsWithMonoGame.Interfaces
{
    /// <summary>
    /// This is the interface for a basic transform
    /// </summary>
    public interface ITransform
    {
        /// <summary>
        /// Scale of the object to be transformed. Vector.One is default.
        /// </summary>
        Vector3 Scale { get; set; }

        /// <summary>
        /// Position of object to be transformed.
        /// </summary>
        Vector3 Position { get; set; }

        /// <summary>
        /// Rotation of object to be transformed.
        /// </summary>
        Quaternion Rotation { get; set; }

        /// <summary>
        /// Objects world matrix
        /// </summary>
        Matrix World { get; }
    }
}
