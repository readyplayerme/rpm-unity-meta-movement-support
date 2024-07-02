using Oculus.Movement.AnimationRigging;
using Oculus.Movement.Utils;
using ReadyPlayerMe.Core;
using UnityEngine;

namespace ReadyPlayerMe.MetaMovement.Samples
{
    public class MeshTransferLoader : MonoBehaviour
    {
        /// <summary>
        /// The configuration which to load the RPM avatar with.
        /// </summary>
        [SerializeField] protected AvatarConfig avatarConfig;
        
        [SerializeField] private string avatarUrl = "https://api.readyplayer.me/v1/avatars/66265a785181f3ac31691d35.glb";
        
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

        private void AvatarLoaderOnFailed(object sender, FailureEventArgs args)
        {
            
        }

        private void AvatarLoaderOnCompleted(object sender, CompletionEventArgs args)
        {
            AvatarMeshHelper.TransferMesh(args.Avatar, gameObject);
            var retargetingLayer = gameObject.GetComponent<RetargetingLayer>();
            var animator = gameObject.GetComponent<Animator>();
            animator.StopPlayback();
            animator.playbackTime = 0;
            var ovrBody = GetComponent<OVRBody>();
            ovrBody.enabled = false;
            var faceExpressions = gameObject.GetComponent<OVRFaceExpressions>();
            faceExpressions.enabled = false;
            AddComponentsHelper.AddJointAdjustments(animator, retargetingLayer);
            ovrBody.enabled = true;
            faceExpressions.enabled = true;
            animator.StartPlayback();
            // Cleanup
            Destroy(args.Avatar);
        }
    }
}