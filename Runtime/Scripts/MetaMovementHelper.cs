using System.Collections.Generic;
using Oculus.Movement.AnimationRigging;
using Oculus.Movement.Tracking;
using Oculus.Movement.Utils;
using ReadyPlayerMe.Core;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.MetaMovement.Runtime
{
    public static class MetaMovementHelper
    {
        private static readonly string[] TwistBoneNames = { "Armature/Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftArmTwist", "Armature/Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftArmTwist/LeftForeArm/LeftForeArmTwist","Armature/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightArmTwist", "Armature/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightArmTwist/RightForeArm/RightForeArmTwist" };
        
        public static void RuntimeRetargetingSetup(GameObject avatar, RestPoseObjectHumanoid restPoseObjectHumanoid)
        {
            AddComponentsRuntime.SetupCharacterForAnimationRiggingRetargeting(avatar.gameObject,
                true, true, restPoseObjectHumanoid);
            var deformation = avatar.GetComponentInChildren<FullBodyDeformationConstraint>();
            ApplyDeformationSettings(deformation);
            ApplyRetargetingSettings(avatar);
            var headMeshes = AvatarMeshHelper.GetHeadMeshes(avatar);
            foreach (var headMesh in headMeshes)
            {
                var skinMeshRenderer = headMesh.GetComponent<SkinnedMeshRenderer>();
                if (skinMeshRenderer == null || skinMeshRenderer.sharedMesh == null ||
                    skinMeshRenderer.sharedMesh.blendShapeCount == 0)
                {
                    continue;
                }
                AddComponentsHelper.SetUpCharacterForARKitFace(headMesh.gameObject, true, true);
                var arKitFace = headMesh.GetComponent<ARKitFace>();
                arKitFace.BlendShapeStrengthMultiplier = 1.0f;
                arKitFace.Mappings[49] = OVRFaceExpressions.FaceExpression.TongueOut;
            }

            SetLayerRecursively(avatar, 10);
            SetupHierarchyTwist(avatar);
        }

        public static void ApplyRetargetingSettings(GameObject avatar)
        {
            if (avatar == null)
            {
                Debug.LogError("Avatar is null. Cannot apply retargeting settings.");
                return;
            }

            var retargetingLayer = avatar.GetComponent<RetargetingLayer>();
            if (retargetingLayer == null)
            {
                Debug.LogError("RetargetingLayer component is missing from the avatar.");
                return;
            }

            foreach (var processor in retargetingLayer.RetargetingProcessors)
            {
                if (processor is RetargetingProcessorCorrectBones correctBonesProcessor)
                {
                    correctBonesProcessor.ShoulderCorrectionWeightLateUpdate = 0.0f;
                }
            }
        }

        public static void ApplyDeformationSettings(FullBodyDeformationConstraint deformation)
        {
            deformation.data.OriginalSpinePositionsWeight = 0.75f;
            deformation.data.SpineLowerAlignmentWeight = 0.0f;
            deformation.data.SpineUpperAlignmentWeight = 0.0f;
        }

        public static void SetLayerRecursively(GameObject targetObject, int newLayer)
        {
            if (targetObject == null) return;

            targetObject.layer = newLayer;

            foreach (Transform child in targetObject.transform)
            {
                if (child == null) continue;
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
        
        public static void SetupHierarchyTwist(GameObject avatar)
        {
            var twistboneComponent = avatar.GetComponent<HierarchyTwist>();
            if (twistboneComponent == null)
            {
                twistboneComponent = avatar.AddComponent<HierarchyTwist>();
            }

            var twistBoneList = new List<Transform>();
            foreach (var twistBoneName in TwistBoneNames)
            {
                var twistBone = avatar.transform.Find(twistBoneName);
                if (twistBone != null)
                {
                    twistBoneList.Add(twistBone);
                }
            }
            twistboneComponent.SetupTwistBones(twistBoneList.ToArray(), 0.5f, 0f);
            #if UNITY_EDITOR
            EditorUtility.SetDirty(twistboneComponent);
            #endif
        }
    }
}