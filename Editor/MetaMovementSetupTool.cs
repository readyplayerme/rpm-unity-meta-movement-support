using System.Linq;
using Oculus.Movement.Experimental;
using Oculus.Movement.Tracking;
using ReadyPlayerMe.Core;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.MetaMovement.Editor
{
    public static class MetaMovementSetupTool
    {
        private static readonly string[] BlendshapeMeshNames = { "EyeLeft", "EyeRight", "Head", "Teeth", "Beard", "Avatar" };

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

        [MenuItem("GameObject/Ready Player Me/Meta Movement/ Setup Twist Bones")]
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
                //Todo add twist bone setup after editing HierarchyTwist.cs
            }
            else
            {
                Debug.LogWarning("No GameObject selected. Please select a GameObject to add components to.");
            }
        }
    }
}