using System;
using System.Collections.Generic;
using ReadyPlayerMe.Core;
using UnityEngine;

namespace ReadyPlayerMe.MetaMovement.Runtime
{
    public class AvatarBoneMirror : MonoBehaviour
    {
        [System.Serializable]
        public class MirroredBonePair
        {
            [HideInInspector] public string Name;
            public Transform SourceBone;
            public Transform MirroredBone;
        }

        [SerializeField]
        protected Transform avatarToMirror;
        public Transform AvatarToMirror => avatarToMirror;
        
        [SerializeField]
        protected MirroredBonePair[] mirroredBonePairs;

        private void Start()
        {
            if (avatarToMirror == null)
            {
                gameObject.SetActive(false);
            }
        }

        public void SetAvatarToMirror(GameObject avatar)
        {
            avatarToMirror = avatar.transform;
            SetMirroredBonePair();
            gameObject.SetActive(true);
        }

        public void TransferMeshAndSetAvatarToMirror(GameObject avatar)
        {
            var avatarCopy = Instantiate(avatar, transform);
            AvatarMeshHelper.TransferMesh(avatarCopy, gameObject);
            MetaMovementHelper.UpdateFaceTrackingMeshes(gameObject);
            SetAvatarToMirror(avatar);
            Destroy(avatarCopy);
        }
        
        private void LateUpdate()
        {
            if(avatarToMirror == null)
            {
                return;
            }
            transform.localPosition = avatarToMirror.localPosition;
            transform.localRotation = avatarToMirror.localRotation;
            foreach (var transformPair in mirroredBonePairs)
            {
                transformPair.MirroredBone.localPosition = transformPair.SourceBone.localPosition;
                transformPair.MirroredBone.localRotation = transformPair.SourceBone.localRotation;
            }
        }
        
        public void SetMirroredBonePair()
        {
            if(avatarToMirror == null)
            {
                Debug.LogError("Avatar to mirror is not set.");
                return;
            }
            var mirrorArmature = AvatarToMirror.transform.Find("Armature");
            if(mirrorArmature == null)
            {
                Debug.LogError("Armature not found skipping bone pair setup.");
                return;
            }
            var childTransforms = new List<Transform>(mirrorArmature.GetComponentsInChildren<Transform>(true));
            var bonePairList = new List<MirroredBonePair>();

            foreach (var childTransform in childTransforms)
            {
                var mirroredTransform =
                    gameObject.transform.FindChildRecursive(childTransform.name);
                if (mirroredTransform != null)
                {
                    bonePairList.Add(new MirroredBonePair
                    {
                        MirroredBone = mirroredTransform,
                        SourceBone = childTransform,
                        Name = childTransform.name
                    });
                }
                else
                {
                    Debug.LogError($"Missing a mirrored transform for: {transform.name}");
                }
            }
            mirroredBonePairs = bonePairList.ToArray();
        }
    }
}