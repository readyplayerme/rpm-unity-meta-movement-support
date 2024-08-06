using Oculus.Movement;
using Oculus.Movement.Utils;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Events;

namespace ReadyPlayerMe.MetaMovement
{
    /// <summary>
    /// Manages the dynamic loading of avatars from a URL, handling their configuration, positioning, and retargeting setup.
    /// </summary>
    public class DynamicMovementLoader : MonoBehaviour, IAvatarLoadFromUrl
    {
        /// <summary>
        /// Parent to place the loaded game object under. If not set, it will be placed in the root of the scene.
        /// </summary>
        [SerializeField, Tooltip("Parent to place game object under. If not set, it will be placed in the root of the scene.")]
        protected Transform parent;

        /// <summary>
        /// Spawn position offset of the character relative to the parent.
        /// </summary>
        [SerializeField, Tooltip("Spawn position offset of the character relative to the parent.")]
        protected Vector3 spawnOffset;

        /// <summary>
        /// The rest T-pose humanoid object for masculine avatars.
        /// </summary>
        [SerializeField]
        [Tooltip(RetargetingMenuTooltips.RestTPoseObject)]
        protected RestPoseObjectHumanoid restTPoseObject_M;        
        
        /// <summary>
        /// The rest T-pose humanoid object for feminine avatars.
        /// </summary>
        [SerializeField]
        [Tooltip(RetargetingMenuTooltips.RestTPoseObject)]
        protected RestPoseObjectHumanoid restTPoseObject_F;

        /// <summary>
        /// The configuration used to load the Ready Player Me (RPM) avatar. If not set, global AvatarLoaderSettings will be used.
        /// </summary>
        [SerializeField, Tooltip("The configuration which to load the RPM avatar with. If not set, it will use the settings from the global AvatarLoaderSettings.")]
        protected AvatarConfig avatarConfig;

        
        /// <summary>
        /// If set, the loaded avatar will use this animation avatar instead of the default.
        /// </summary>
        [SerializeField, Tooltip("If set, the loaded avatar will use this animation avatar instead of the one from the default.")]
        protected Avatar animationAvatarOverride;
        
        /// <summary>
        /// Event triggered when the avatar object is successfully loaded.
        /// </summary>
        public UnityEvent<GameObject> OnAvatarObjectLoaded;
        
        private GameObject avatar;
        
        /// <summary>
        /// Initiates the process to load an avatar from a specified URL.
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
        /// Callback invoked when the avatar loading fails.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments containing failure details.</param>
        private void AvatarLoaderOnFailed(object sender, FailureEventArgs e)
        {
            Debug.LogWarning($"Failed to load RPM character {e}.");
        }

        /// <summary>
        /// Callback invoked when the avatar loading is completed successfully.
        /// Sets up the avatar's transform, animator, and retargeting.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments containing completion details.</param>
        private void AvatarLoaderOnCompleted(object sender, CompletionEventArgs e)
        {
            Debug.Log($"Loaded avatar {e}.");
            avatar = e.Avatar;
            var avatarTransform = avatar.transform;
            if (parent != null)
            {
                avatarTransform.SetParent(parent, false);
            }
            avatarTransform.localPosition = spawnOffset;

            AvatarAnimationHelper.SetupAnimator(e.Metadata, avatar);
            var animatorComp = avatar.GetComponent<Animator>();
            if (animationAvatarOverride != null)
            {
                animatorComp.avatar = animationAvatarOverride;
            }
            var avatarGender = e.Metadata.OutfitGender == OutfitGender.Masculine ? restTPoseObject_M : restTPoseObject_F;
            MetaMovementHelper.RuntimeRetargetingSetup(avatar, avatarGender);
            OnAvatarObjectLoaded?.Invoke(avatar);
        }
        
        /// <summary>
        /// Cleanup method called when the script or GameObject is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            if (avatar != null)
            {
                Destroy(avatar);
            }
        }
    }
}