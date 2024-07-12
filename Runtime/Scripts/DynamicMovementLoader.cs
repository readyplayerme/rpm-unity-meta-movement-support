using Oculus.Movement;
using Oculus.Movement.Utils;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Events;

namespace ReadyPlayerMe.MetaMovement
{
    public class DynamicMovementLoader : MonoBehaviour, IAvatarLoadFromUrl
    {
        /// <summary>
        /// Parent to place game object under.
        /// </summary>
        [SerializeField, Tooltip("Parent to place game object under. If not set, it will be placed in the root of the scene.")]
        protected Transform parent;

        /// <summary>
        /// Spawn position of character.
        /// </summary>
        [SerializeField, Tooltip("Spawn position offset of the character relative to the parent.")]
        protected Vector3 spawnOffset;

        /// <summary>
        /// The rest T-pose humanoid object.
        /// </summary>
        [SerializeField]
        [Tooltip(RetargetingMenuTooltips.RestTPoseObject)]
        protected RestPoseObjectHumanoid restTPoseObject_M;        
        
        /// <summary>
        /// The rest T-pose humanoid object.
        /// </summary>
        [SerializeField]
        [Tooltip(RetargetingMenuTooltips.RestTPoseObject)]
        protected RestPoseObjectHumanoid restTPoseObject_F;

        /// <summary>
        /// The configuration which to load the RPM avatar with. If not set, it will use the settings from the global AvatarLoaderSettings.
        /// </summary>
        [SerializeField, Tooltip("The configuration which to load the RPM avatar with. If not set, it will use the settings from the global AvatarLoaderSettings.")]
        protected AvatarConfig avatarConfig;

        /// <summary>
        /// Custom avatar.
        /// </summary>
        [SerializeField, Tooltip("If set, the loaded avatar will use this animation avatar instead of the one from the default.")]
        protected Avatar animationAvatarOverride;
        
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

        private void AvatarLoaderOnFailed(object sender, FailureEventArgs e)
        {
            Debug.LogWarning($"Failed to load RPM character {e}.");
        }

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

        private void OnDestroy()
        {
            if (avatar != null)
            {
                Destroy(avatar);
            }
        }
    }
}