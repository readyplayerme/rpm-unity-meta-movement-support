using System.Collections.Generic;
using UnityEngine;

namespace ReadyPlayerMe.MetaMovement
{
    /// <summary>
    /// Applies twisting transformations to bones in a hierarchy based on position and rotation weights.
    /// </summary>
    [DefaultExecutionOrder(250)]
    public class HierarchyTwist : MonoBehaviour
    {
        [System.Serializable]
        public class TwistSection
        {
            [SerializeField]
            private Transform startBone;

            [SerializeField]
            private Transform twistBone;

            [SerializeField]
            private Transform endBone;

            [SerializeField]
            private float positionWeight;

            [SerializeField]
            private float rotationWeight;
            
            /// <summary>
            /// Initializes a new instance of the <see cref="TwistSection"/> class.
            /// </summary>
            /// <param name="twistBone">The bone to be twisted.</param>
            /// <param name="positionWeight">The weight for position interpolation.</param>
            /// <param name="rotationWeight">The weight for rotation interpolation.</param>
            public TwistSection(Transform twistBone, float positionWeight = 0.5f, float rotationWeight = 0.5f)
            {
                startBone = twistBone.parent;
                this.twistBone = twistBone;
                endBone = twistBone.GetChild(0);
                this.positionWeight = positionWeight;
                this.rotationWeight = rotationWeight;
            }

            /// <summary>
            /// Applies the twist transformation to the twistBone based on the position and rotation weights.
            /// </summary>
            public void ApplyTwist()
            {
                var startPos = startBone.position;
                var startRot = startBone.rotation;
                var endPos = endBone.position;
                var endRot = endBone.rotation;
                
                // Interpolates between start and end positions and rotations based on weights.
                var targetPos = Vector3.Lerp(startPos, endPos, positionWeight);
                var targetRot = Quaternion.Lerp(startRot, endRot, rotationWeight);
                
                twistBone.SetPositionAndRotation(targetPos, targetRot);
                endBone.SetPositionAndRotation(endPos, endRot);
            }
        }
        
        [Tooltip("Array of twist sections to be processed.")]
        [SerializeField]
        private TwistSection[] twists;

        private void LateUpdate()
        {
            if(twists == null) return;
            
            // Applies the twist to each section in the twists array.
            foreach (var twist in twists)
            {
                twist.ApplyTwist();
            }
        }

        /// <summary>
        /// Sets up the twist sections based on provided twist bones and weights for position and rotation.
        /// </summary>
        /// <param name="twistBones">Array of bones to be twisted.</param>
        /// <param name="positionWeight">The weight for position interpolation.</param>
        /// <param name="rotationWeight">The weight for rotation interpolation.</param>
        public void SetupTwistBones(Transform[] twistBones, float positionWeight = 0.5f, float rotationWeight = 0.5f)
        { 
            var twistSections = new List<TwistSection>();
            foreach (var twistBone in twistBones)
            {
                var twistSection = new TwistSection(twistBone, positionWeight, rotationWeight);
                twistSections.Add(twistSection);
            }
            twists = twistSections.ToArray();
        }
    }
}
