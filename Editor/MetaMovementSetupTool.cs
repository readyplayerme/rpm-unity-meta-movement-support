using Oculus.Movement.AnimationRigging;
using Oculus.Movement.Tracking;
using Oculus.Movement.Utils;
using ReadyPlayerMe.Core;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ReadyPlayerMe.MetaMovement.Editor
{
    public static class MetaMovementSetupTool
    {
        private const string META_SETUP_MENU_FUNCTION = "GameObject/Ready Player Me/Meta Movement/Run Avatar Setup";
        private const string META_CHARACTER_LAYER = "Character";

        [MenuItem(META_SETUP_MENU_FUNCTION)]
        public static void MetaMovementSetup()
        {
            var activeGameObject = Selection.activeGameObject;

            if (activeGameObject != null)
            {
                var animator = activeGameObject.GetComponent<Animator>();
                if(animator == null)
                {
                    Debug.LogWarning("No Animator component found on the selected GameObject. Please add an Animator component to the GameObject.");
                    return;
                }
                var restPoseObjectHumanoid = AddComponentsHelper.GetRestPoseObject(AddComponentsHelper.CheckIfTPose(animator));
                AnimationUtilities.UpdateToAnimatorPose(animator);
                HelperMenusBody.SetupCharacterForAnimationRiggingRetargetingConstraints(activeGameObject, restPoseObjectHumanoid, true, true);
                if(LayerMask.NameToLayer(META_CHARACTER_LAYER) >= 0)
                {
                    MetaMovementHelper.SetLayerRecursively(activeGameObject, LayerMask.NameToLayer(META_CHARACTER_LAYER));
                    return;
                }
                SetupFaceTracking(activeGameObject);
                MetaMovementHelper.SetupHierarchyTwist(activeGameObject);
                var deformation = activeGameObject.GetComponentInChildren<FullBodyDeformationConstraint>();
                MetaMovementHelper.ApplyDeformationSettings(deformation);
                MetaMovementHelper.ApplyRetargetingSettings(activeGameObject);
                EditorSceneManager.MarkSceneDirty(activeGameObject.scene);
            }
            else
            {
                Debug.LogWarning("No GameObject selected. Please select a GameObject to add components to.");
            }
        }
        
        [MenuItem(META_SETUP_MENU_FUNCTION, true)]
        private static bool ValidateMetaMovementSetup()
        {
            return Selection.activeGameObject != null;
        }

        private static void SetupFaceTracking(GameObject activeGameObject)
        {
            var ovrFaceExpression = activeGameObject.GetComponent<OVRFaceExpressions>();
            if (ovrFaceExpression == null)
            {
                activeGameObject.AddComponent<OVRFaceExpressions>();
            }

            var skeletalMeshes = AvatarMeshHelper.GetHeadMeshes(activeGameObject);
            foreach (var mesh in skeletalMeshes)
            {
                var skinMeshRenderer = mesh.GetComponent<SkinnedMeshRenderer>();
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
                arkitFaceComponent.OnBeforeSerialize();
                MetaMovementHelper.ApplyARKitFaceSettings(arkitFaceComponent);
                EditorUtility.SetDirty(arkitFaceComponent);
            }
        }
    }
}