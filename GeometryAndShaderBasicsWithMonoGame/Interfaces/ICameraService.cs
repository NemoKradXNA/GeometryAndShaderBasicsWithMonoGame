using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryAndShaderBasicsWithMonoGame.Interfaces
{
    /// <summary>
    /// Interface for a basic camera, implements IHasTransform
    /// </summary>
    public interface ICameraService : IHasTransform
    {
        /// <summary>
        /// The cameras aspect ratio
        /// </summary>
        float AspectRatio { get; }

        /// <summary>
        /// The current viewport we are rendering to
        /// </summary>
        Viewport Viewport { get; }

        /// <summary>
        /// The cameras View Matrix (View Space)
        /// </summary>
        Matrix View { get; }

        /// <summary>
        /// The cameras Projection Matrix (Projection Space)
        /// </summary>
        Matrix Projection { get; }

        /// <summary>
        /// The cameras field of view
        /// </summary>
        float FieldOfView { get; set; }

        /// <summary>
        /// The cameras near clip plane distance, anything closer than this value to the camera will not be drawn.
        /// </summary>
        float NearClipPlane { get; set; }

        /// <summary>
        /// The cameras far clip plane distance, anything further than this value will not be drawn
        /// </summary>
        float FarClipPlane { get; set; }

        /// <summary>
        /// The color used to clear the graphics device before each draw call.
        /// </summary>
        Color ClearColor { get; set; }
    }
}
