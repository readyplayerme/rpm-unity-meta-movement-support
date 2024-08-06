
namespace ReadyPlayerMe.MetaMovement
{
    /// <summary>
    /// Interface defining the method required to load an avatar from a URL.
    /// </summary>
    public interface IAvatarLoadFromUrl
    {
        /// <summary>
        /// Loads an avatar from the specified URL.
        /// </summary>
        /// <param name="url">The URL from which to load the avatar.</param>
        public void LoadAvatar(string url);
    }
}