using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity
{
    public static class SkeletonAnimation_Setter
    {
        public static float setAnimation(this SkeletonAnimation sa,string name, bool loop, int trackIndex = 0, float time = 0)
        {
            TrackEntry e = sa.state.SetAnimation(trackIndex, name, loop);
            return e == null ? 0 : e.Animation.Duration;
        }
        
        public static float setAnimation(this SkeletonGraphic sa,string name, bool loop, int trackIndex = 0, float time = 0)
        {
            TrackEntry e = sa.AnimationState.SetAnimation(trackIndex, name, loop);
            return e == null ? 0 : e.Animation.Duration;
        }
        
        public static float playAnimation(this SkeletonAnimation sa,string name, bool loop)
        {
            try {
                TrackEntry e = sa.AnimationState.SetAnimation(0, name, loop);
                return e == null ? 0 : e.Animation.Duration;
            }
            catch (Exception exception) {
                Debug.LogError($"{sa.name}没有{name}动画");
                return 0;
            }
        }
        
        public static float playAnimation(this SkeletonGraphic sa,string name, bool loop)
        {
            try {
                TrackEntry e = sa.AnimationState.SetAnimation(0, name, loop);
                return e == null ? 0 : e.Animation.Duration;
            }
            catch (Exception exception) {
                Debug.LogError($"{sa.name}没有{name}动画");
                return 0;
            }
        }
    }
}

