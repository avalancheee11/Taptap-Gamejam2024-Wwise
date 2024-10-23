using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;
using Event = Spine.Event;

public class PlayerSpineAniName
{
    public const string AtkL = "atkL";
    public const string AtkR = "atkR";
    public const string CollectFishL = "collectFishL";
    public const string CollectFishR = "collectFishR";
    public const string CollectPlantL = "collectPlantL";
    public const string CollectPlantR = "collectPlantR";
    public const string DieL = "dieL";
    public const string DieR = "dieR";
    public const string IdleD = "idleD";
    public const string IdleD2 = "idleD2";
    public const string IdleL = "idleL";
    public const string IdleR = "idleR";
    public const string IdleU = "idleU";
    public const string MiningL = "miningL";
    public const string MiningR = "miningR";
    
    public const string RunD = "runD";
    public const string RunL = "runL";
    public const string RunR = "runR";
    public const string RunU = "runU";
    
    // public const string UseL = "useL";
    // public const string UseR = "useR";
    
    public const string WalkD = "walkD";
    public const string WalkL = "walkL";
    public const string WalkR = "walkR";
    public const string WalkU = "walkU";
}

public class PlayerSpineAniEvent
{
    public const string Atk = "event.ani.atk";
}


public class PlayerSpineNode : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation sa;

    public event Action<string> onSaEvent; 

    private string currentAnimation = ""; // 当前动画状态


    public void StartAni()
    {
        this.sa.AnimationState.Event += AnimationStateOnEvent;
        this.sa.AnimationState.Complete += AnimationStateOnComplete; // 动画完成事件

    }

    public void StopAni()
    {
        this.sa.AnimationState.Event -= AnimationStateOnEvent;
        this.sa.AnimationState.Complete -= AnimationStateOnComplete; // 动画完成事件

    }
    
    public virtual float PlayAnime(string anime, bool loop)
    {
        if (anime == currentAnimation) return 0; // 避免重复播放同一动画

        try {
            var e = sa.AnimationState.SetAnimation(0, anime, loop);
            currentAnimation = anime; // 更新当前动画状态
            return e == null ? 0 : e.Animation.Duration;
        }
        catch (Exception exception) {
            Debug.LogError($"{exception}-{sa.name}没有{anime}动画");
            return 0;
        }
    }
    
    private void AnimationStateOnEvent(TrackEntry trackentry, Event e)
    {
        this.onSaEvent?.Invoke(e.Data.Name);
    }

    // private void AnimationStateOnComplete(TrackEntry trackentry)
    // {
    //     // 当攻击动画播放完毕后，切换回idle动画
    //     if (trackentry.Animation.Name.Contains("atk"))
    //     {
    //         string idleAnimation = "";
    //         if (currentAnimation.Contains("L")) idleAnimation = PlayerSpineAniName.IdleL;
    //         else if (currentAnimation.Contains("R")) idleAnimation = PlayerSpineAniName.IdleR;

    //         PlayAnime(idleAnimation, true);
    //     }
    // }

    private void AnimationStateOnComplete(TrackEntry trackentry)
{
    // 获取当前动画名称
    string animationName = trackentry.Animation.Name;

    // 如果是攻击、采集、采矿、捕鱼的动作动画，完成后切换回 idle 动画
    if (animationName.Contains("atk") || animationName.Contains("collect") || animationName.Contains("mining"))
    {
        // 根据当前方向选择合适的 idle 动画
        string idleAnimation = "";
        if (currentAnimation.Contains("L")) 
            idleAnimation = PlayerSpineAniName.IdleL;
        else if (currentAnimation.Contains("R")) 
            idleAnimation = PlayerSpineAniName.IdleR;
        else if (currentAnimation.Contains("D")) 
            idleAnimation = PlayerSpineAniName.IdleD;
        else if (currentAnimation.Contains("U")) 
            idleAnimation = PlayerSpineAniName.IdleU;

        // 切换到 idle 动画
        PlayAnime(idleAnimation, true);
    }
}

}
