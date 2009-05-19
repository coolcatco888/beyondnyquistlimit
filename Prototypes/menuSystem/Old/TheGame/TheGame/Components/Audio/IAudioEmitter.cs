#region Using Statements

using Microsoft.Xna.Framework;

#endregion 

namespace TheGame
{
    /// <summary>
    /// Interface used by the AudioManager to reference
    /// position and velocity of audio emitters.
    /// </summary>
    public interface IAudioEmitter
    {
        Vector3 Position { get; }
        Vector3 Forward { get; }
        Vector3 Up { get; }
        Vector3 Velocity { get; }
    }
}
