using System.Collections;
using System.Collections.Generic;
using ReadyPlayerMe.Core;
using UnityEngine;

namespace ReadyPlayerMe.MetaMovement.Samples
{
    public class BasicAvatarLoader : MonoBehaviour
    {
        /// <summary>
        /// The configuration which to load the RPM avatar with.
        /// </summary>
        [SerializeField] protected AvatarConfig _avatarConfig;
        
        [SerializeField] private string avatarUrl = "https://api.readyplayer.me/v1/avatars/66265a785181f3ac31691d35.glb";
        
        private void Start()
        {
            var avatarLoader = new AvatarObjectLoader();
            avatarLoader.AvatarConfig = _avatarConfig;
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
            
            Destroy(args.Avatar);
        }
    }
}