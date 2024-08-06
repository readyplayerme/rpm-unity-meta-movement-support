using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Events;

namespace ReadyPlayerMe.MetaMovement.Runtime
{
    /// <summary>
    /// Loads and sets up movement prefabs for avatars based on gender, using configuration from Ready Player Me.
    /// </summary>
    public class MovementPrefabLoader : MonoBehaviour, IAvatarLoadFromUrl
    {
        /// <summary>
        /// Prefab for male movement animations.
        /// </summary>
        [Tooltip("Movement Prefab for male avatars")] 
        public GameObject maleMovementPrefab;
        
        /// <summary>
        /// Prefab for female movement animations.
        /// </summary>
        [Tooltip("Movement Prefab for female avatars")] 
        public GameObject femaleMovementPrefab;
        
        /// <summary>
        /// Parent transform under which the loaded GameObject will be placed.
        /// </summary>
        [SerializeField, Tooltip("Parent to place game object under. If not set, it will be placed in the root of the scene.")] 
        protected Transform parent;

        /// <summary>
        /// Offset position for spawning the character relative to the parent.
        /// </summary>
        [SerializeField]
        protected Vector3 spawnOffset;
        
        /// <summary>
        /// Configuration used for loading the Ready Player Me (RPM) avatar.
        /// If not set, global AvatarLoaderSettings will be used.
        /// </summary>
        [SerializeField, Tooltip("The configuration which to load the RPM avatar with. If not set, it will use the settings from the global AvatarLoaderSettings.")]
        protected AvatarConfig avatarConfig;

        /// <summary>
        /// Event triggered when the avatar GameObject is successfully loaded.
        /// </summary>
        public UnityEvent<GameObject> OnAvatarObjectLoaded;
        
        private GameObject avatar;
        
        /// <summary>
        /// Initiates the loading of an avatar from a specified URL.
        /// </summary>
        /// <param name="url">The URL from which to load the avatar.</param>
        public void LoadAvatar(string url)
        {
            var avatarLoader = new AvatarObjectLoader();
            if (avatarConfig != null)
            {
                avatarLoader.AvatarConfig = avatarConfig;
            }
            avatarLoader.LoadAvatar(url);
            avatarLoader.OnCompleted += AvatarLoaderOnCompleted;
            avatarLoader.OnFailed += AvatarLoaderOnFailed;
        }
        
        /// <summary>
        /// Callback invoked when avatar loading fails.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">Event arguments containing failure details.</param>
        private void AvatarLoaderOnFailed(object sender, FailureEventArgs args)
        {
            Debug.LogWarning($"Failed to load RPM character {args}.");
        }

        /// <summary>
        /// Callback invoked when the avatar is successfully loaded.
        /// Instantiates the appropriate movement prefab and sets up the avatar.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">Event arguments containing completion details.</param>
        private void AvatarLoaderOnCompleted(object sender, CompletionEventArgs args)
        {
            // Choose the correct prefab based on the avatar's gender.
            var prefabToLoad = args.Metadata.OutfitGender == OutfitGender.Masculine ? maleMovementPrefab : femaleMovementPrefab;
            
            // Instantiate the selected movement prefab and set it up.
            var movementAvatar = Instantiate(prefabToLoad, parent);
            movementAvatar.transform.localPosition = spawnOffset;
            avatar = movementAvatar.gameObject;
            avatar.transform.localPosition = spawnOffset;
            AvatarMeshHelper.TransferMesh(args.Avatar, avatar);
            MetaMovementHelper.UpdateFaceTrackingMeshes(avatar);
            
            // Clean up the original avatar GameObject.
            Destroy(args.Avatar);
            
            // Trigger the event indicating that the avatar has been successfully loaded.
            OnAvatarObjectLoaded?.Invoke(avatar);
        }
    }
}