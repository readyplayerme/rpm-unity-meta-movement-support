using System.Collections.Generic;
using Oculus.Movement.AnimationRigging;
using Oculus.Movement.Tracking.Deprecated;
using Oculus.Movement.Utils;
using ReadyPlayerMe.Core;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.MetaMovement
{
    /// <summary>
    /// Provides various helper methods for setting up and managing avatar movement and animation retargeting.
    /// </summary>
    public static class MetaMovementHelper
    {
        // Names of twist bones used for setup in avatars.
        private static readonly string[] TwistBoneNames = 
        {
            "Armature/Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftArmTwist",
            "Armature/Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftArmTwist/LeftForeArm/LeftForeArmTwist",
            "Armature/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightArmTwist",
            "Armature/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightArmTwist/RightForeArm/RightForeArmTwist"
        };
        
        /// <summary>
        /// Sets up runtime retargeting for the avatar, including adding necessary components and configuring settings.
        /// </summary>
        /// <param name="avatar">The avatar GameObject to setup.</param>
        /// <param name="restPoseObjectHumanoid">The rest pose object to use for retargeting.</param>
        public static void RuntimeRetargetingSetup(GameObject avatar, RestPoseObjectHumanoid restPoseObjectHumanoid)
        {
            var animator = avatar.GetComponent<Animator>();
            if(animator != null)
            {
                AnimationUtilities.UpdateToAnimatorPose(animator);
            }
            
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
                ApplyARKitFaceSettings(arKitFace);
            }

            SetLayerRecursively(avatar, 10);
            SetupHierarchyTwist(avatar);
        }

        /// <summary>
        /// Applies specific retargeting settings to the avatar.
        /// </summary>
        /// <param name="avatar">The avatar GameObject to configure.</param>
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
            var leftShoulder = retargetingLayer.GetFindAdjustment(HumanBodyBones.LeftShoulder);
            var rightShoulder = retargetingLayer.GetFindAdjustment(HumanBodyBones.RightShoulder);
            if (leftShoulder != null)
            {
                leftShoulder.RotationChange = Quaternion.Euler(0, 0, 345f);;
            }
            if (rightShoulder != null)
            {
                rightShoulder.RotationChange = Quaternion.Euler(0, 0, 15f);
            }

            foreach (var processor in retargetingLayer.RetargetingProcessors)
            {
                if (processor is RetargetingProcessorCorrectBones correctBonesProcessor)
                {
                    correctBonesProcessor.ShoulderCorrectionWeightLateUpdate = 0.0f;
                }
            }
        }

        /// <summary>
        /// Applies deformation settings to the FullBodyDeformationConstraint component.
        /// </summary>
        /// <param name="deformation">The FullBodyDeformationConstraint component to configure.</param>
        public static void ApplyDeformationSettings(FullBodyDeformationConstraint deformation)
        {
            deformation.data.OriginalSpinePositionsWeight = 0.75f;
            deformation.data.SpineLowerAlignmentWeight = 0.0f;
            deformation.data.SpineUpperAlignmentWeight = 0.0f;
            deformation.data.ShoulderRollWeight = 0.5f;
        }

        /// <summary>
        /// Recursively sets the layer of the target object and its children.
        /// </summary>
        /// <param name="targetObject">The GameObject to set the layer for.</param>
        /// <param name="newLayer">The new layer to set.</param>
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

        /// <summary>
        /// Configures the ARKitFace component for the specified head mesh.
        /// </summary>
        /// <param name="arKitFace">The ARKitFace component to configure.</param>
        public static void ApplyARKitFaceSettings(ARKitFace arKitFace)
        {
            if (arKitFace == null)
            {
                Debug.LogError("ARKitFace component is null.");
                return;
            }
            arKitFace.RetargetingTypeField = OVRCustomFace.RetargetingType.Custom;
            arKitFace.AutoMapBlendshapes();
            arKitFace.BlendShapeStrengthMultiplier = 1f;

            // Assign tongue out blendshape if mappings are sufficient.
            if (arKitFace.Mappings != null && arKitFace.Mappings.Length >= 50)
            {
                arKitFace.Mappings[49] = OVRFaceExpressions.FaceExpression.TongueOut;
            }
        }
        
        /// <summary>
        /// Sets up the HierarchyTwist component on the avatar, configuring the twist bones.
        /// </summary>
        /// <param name="avatar">The avatar GameObject to setup.</param>
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
            EditorUtility.SetDirty(twistboneComponent); // Mark the component as dirty to save changes in the editor.
            #endif
        }

        /// <summary>
        /// Updates the meshes used for face tracking on the avatar.
        /// </summary>
        /// <param name="avatar">The avatar GameObject to update.</param>
        public static void UpdateFaceTrackingMeshes(GameObject avatar)
        {
            var headMeshes = AvatarMeshHelper.GetHeadMeshes(avatar);
            foreach (var headMesh in headMeshes)
            {
                var skinMeshRenderer = headMesh.GetComponent<SkinnedMeshRenderer>();
                var arkitFaceComponent = skinMeshRenderer.gameObject.GetComponent<ARKitFace>();
                if (skinMeshRenderer == null || skinMeshRenderer.sharedMesh == null ||
                    skinMeshRenderer.sharedMesh.blendShapeCount < 1)
                {
                    if (arkitFaceComponent != null)
                    {
                        arkitFaceComponent.enabled = false;
                    }
                    continue;
                }
 
                if (arkitFaceComponent != null) continue;
                arkitFaceComponent = skinMeshRenderer.gameObject.AddComponent<ARKitFace>();
                ApplyARKitFaceSettings(arkitFaceComponent);
            }
        }
    }
}