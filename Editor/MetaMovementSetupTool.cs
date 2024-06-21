using System.Collections.Generic;
using System.Linq;
using Oculus.Movement.Tracking;
using ReadyPlayerMe.Core;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.MetaMovement.Editor
{
    public static class MetaMovementSetupTool
    {
        private static readonly string[] BlendshapeMeshNames = { "EyeLeft", "EyeRight", "Head", "Teeth", "Beard", "Avatar" };
        private static readonly string[] TwistBoneNames = { "Armature/Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftArmTwist", "Armature/Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftArmTwist/LeftForeArm/LeftForeArmTwist","Armature/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightArmTwist", "Armature/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightArmTwist/RightForeArm/RightForeArmTwist" };
        
        [MenuItem("GameObject/Ready Player Me/Meta Movement/Setup FaceTracking")]
        private static void MetaFaceTrackingSetup()
        {
            var activeGameObject = Selection.activeGameObject;

            if (activeGameObject != null)
            {
                var ovrFaceExpression = activeGameObject.GetComponent<OVRFaceExpressions>();
                if (ovrFaceExpression == null)
                {
                    ovrFaceExpression = activeGameObject.AddComponent<OVRFaceExpressions>();
                }

                var skeletalMeshes = AvatarMeshHelper.GetHeadMeshes(activeGameObject);
                foreach (var mesh in skeletalMeshes)
                {
                    var skinMeshRenderer = mesh.GetComponent<SkinnedMeshRenderer>();
                    if (!BlendshapeMeshNames.Any(mesh.name.Contains)) continue;
                    if (skinMeshRenderer == null || skinMeshRenderer.sharedMesh == null ||
                        skinMeshRenderer.sharedMesh.blendShapeCount == 0)
                    {
                        Debug.LogWarning(
                            $"No SkinnedMeshRenderer or blendshapes found on {mesh.name}  skip adding ARKitFace component.");
                        continue;
                    }

                    var arkitFaceComponent = mesh.gameObject.GetComponent<ARKitFace>();
                    if (arkitFaceComponent == null)
                    {
                        arkitFaceComponent = mesh.gameObject.AddComponent<ARKitFace>();
                    }

                    arkitFaceComponent.AutoMapBlendshapes();
                    arkitFaceComponent.BlendShapeStrengthMultiplier = 1;
                }
            }
            else
            {
                Debug.LogWarning("No GameObject selected. Please select a GameObject to add components to.");
            }
        }

         [MenuItem("GameObject/Ready Player Me/Meta Movement/Setup Twist Bones")]
         private static void MetaTwistBoneSetup()
         {
             var activeGameObject = Selection.activeGameObject;
        
             if (activeGameObject != null)
             {
                 var twistboneComponent = activeGameObject.GetComponent<HierarchyTwist>();
                 if (twistboneComponent == null)
                 {
                     twistboneComponent = activeGameObject.AddComponent<HierarchyTwist>();
                 }

                 var twistBoneList = new List<Transform>();
                 foreach (var twistBoneName in TwistBoneNames)
                {
                    var twistBone = activeGameObject.transform.Find(twistBoneName);
                    if (twistBone != null)
                    {
                        twistBoneList.Add(twistBone);
                    }
                }
                 twistboneComponent.SetupTwistBones(twistBoneList.ToArray(), 1f);
             }
             else
             {
                 Debug.LogWarning("No GameObject selected. Please select a GameObject to add components to.");
             }
         }
    }
}