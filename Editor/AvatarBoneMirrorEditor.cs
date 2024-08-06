using ReadyPlayerMe.MetaMovement;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.MetaMovement.Editor
{
    /// <summary>
    /// Custom editor for Avatar Bone Mirror component.
    /// </summary>
    [CustomEditor(typeof(AvatarBoneMirror)), CanEditMultipleObjects]
    public class AvatarBoneMirrorEditor : UnityEditor.Editor
    {
        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
            var mirroredObject = (AvatarBoneMirror) target;
            if(mirroredObject.AvatarToMirror != null)
            {
                if (GUILayout.Button("Get Mirrored Bone Pairs "))
                {
                    mirroredObject.SetMirroredBonePair();
                }
            }
            else
            {
                var mirroredTransformPairs = serializedObject.FindProperty("mirroredBonePairs");
                mirroredTransformPairs.ClearArray();
            }
            serializedObject.ApplyModifiedProperties();
            GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
            DrawDefaultInspector();
        }
    }
}
