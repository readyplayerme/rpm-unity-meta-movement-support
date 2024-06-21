// Copyright (c) Meta Platforms, Inc. and affiliates. Confidential and proprietary.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ReadyPlayerMe.MetaMovement
{
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
            
            public TwistSection(Transform twistBone, float positionWeight = 0.5f, float rotationWeight = 0.5f)
            {
                startBone = twistBone.parent;
                this.twistBone = twistBone;
                endBone = twistBone.GetChild(0);
                this.positionWeight = positionWeight;
                this.rotationWeight = rotationWeight;
            }

            public void ApplyTwist()
            {
                var startPos = startBone.position;
                var startRot = startBone.rotation;
                var endPos = endBone.position;
                var endRot = endBone.rotation;
                var targetPos = Vector3.Lerp(startPos, endPos, positionWeight);
                var targetRot = Quaternion.Lerp(startRot, endRot, rotationWeight);
                twistBone.SetPositionAndRotation(targetPos, targetRot);
                endBone.SetPositionAndRotation(endPos, endRot);
            }
        }

        [SerializeField]
        private TwistSection[] twists;

        private void LateUpdate()
        {
            if(twists == null) return;
            foreach (var twist in twists)
            {
                twist.ApplyTwist();
            }
        }

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
