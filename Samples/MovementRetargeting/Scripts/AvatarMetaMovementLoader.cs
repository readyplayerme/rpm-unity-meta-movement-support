using Oculus.Interaction;
using Oculus.Movement;
using Oculus.Movement.Utils;
using ReadyPlayerMe.Core;
using UnityEngine;

namespace ReadyPlayerMe.MetaMovement.Runtime
{
    public class AvatarMetaMovementLoader : MonoBehaviour
    {
        [SerializeField]
        private string avatarUrl = "https://api.readyplayer.me/v1/avatars/66265a785181f3ac31691d35.glb";

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
        /// The rest T-pose humanoid object.
        /// </summary>
        [SerializeField]
        [Tooltip(RetargetingMenuTooltips.RestTPoseObject)]
        protected RestPoseObjectHumanoid restTPoseObject;

        /// <summary>
        /// The configuration which to load the RPM avatar with. If not set, it will use the settings from the global AvatarLoaderSettings.
        /// </summary>
        [SerializeField, Optional]
        protected AvatarConfig avatarConfig;

        /// <summary>
        /// Custom avatar.
        /// </summary>
        [SerializeField, Optional]
        protected Avatar avatarOverride;

        private GameObject avatar;

        private void Start()
        {
            var avatarLoader = new AvatarObjectLoader();
            if (avatarConfig != null)
            {
                avatarLoader.AvatarConfig = avatarConfig;
            }
            avatarLoader.LoadAvatar(avatarUrl);
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
            if (avatarOverride != null)
            {
                animatorComp.avatar = avatarOverride;
            }
            MetaMovementHelper.RuntimeRetargetingSetup(avatar, restTPoseObject);
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