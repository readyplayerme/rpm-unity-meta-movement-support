using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Events;

namespace ReadyPlayerMe.MetaMovement.Runtime
{
    public class MovementPrefabLoader : MonoBehaviour, IAvatarLoadFromUrl
    {
        public GameObject maleMovementPrefab;
        public GameObject femaleMovementPrefab;
        
        /// <summary>
        /// Parent to place game object under.
        /// </summary>
        [SerializeField]
        protected Transform parent;

        /// <summary>
        /// Spawn position of character.
        /// </summary>
        [SerializeField]
        protected Vector3 spawnOffset;
        
        /// <summary>
        /// The configuration which to load the RPM avatar with. If not set, it will use the settings from the global AvatarLoaderSettings.
        /// </summary>
        [SerializeField]
        protected AvatarConfig avatarConfig;

        public UnityEvent<GameObject> OnAvatarObjectLoaded;
        
        private GameObject avatar;
        
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
        
        private void AvatarLoaderOnFailed(object sender, FailureEventArgs args)
        {
            Debug.LogWarning($"Failed to load RPM character {args}.");
        }

        private void AvatarLoaderOnCompleted(object sender, CompletionEventArgs args)
        {
            Debug.Log($"Loaded avatar {args}");

            var prefabToLoad = args.Metadata.OutfitGender == OutfitGender.Masculine ? maleMovementPrefab : femaleMovementPrefab;
            var movementAvatar = Instantiate(prefabToLoad, parent);
            movementAvatar.transform.localPosition = spawnOffset;
            avatar = movementAvatar.gameObject;
            avatar.transform.localPosition = spawnOffset;
            AvatarMeshHelper.TransferMesh(args.Avatar, avatar);
            MetaMovementHelper.UpdateFaceTrackingMeshes(avatar);
            // Cleanup unused avatar copy
            Destroy(args.Avatar);
            OnAvatarObjectLoaded?.Invoke(avatar);
        }
    }
}