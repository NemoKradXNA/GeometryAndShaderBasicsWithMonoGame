namespace GeometryAndShaderBasicsWithMonoGame.Interfaces
{
    /// <summary>
    /// Interface used to indicate that an object has an ITransform Property.
    /// </summary>
    public interface IHasTransform
    {
        /// <summary>
        /// The objects transform
        /// </summary>
        ITransform Transform { get; set; }
    }
}
