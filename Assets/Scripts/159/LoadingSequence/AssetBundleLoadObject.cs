using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleLoadObject : BasesLoadingObject
{
    public override string desc => LocalizedUtils.Format("正在加载资源({0}%)……", this.maxCount <= 0 ? "100" : 
        (100 - Mathf.RoundToInt(this.modList.Count / (float) this.maxCount * 100)).toMPString());

    enum LoadState
    {
        None,
        PackPath,
        Module,
        Over,
    }

    private int maxCount;

    private List<string> modList;
    private LoadState _loadState;
    private LoadState loadState
    {
        get => this._loadState;
        set
        {
            if (this._loadState == value) {
                return;
            }

            this._loadState = value;
            if (this._loadState == LoadState.None) {
                this._loadState = LoadState.PackPath;
            }
            else if (this._loadState == LoadState.PackPath) {
                CacheManager.Instance.loadAssetBundleModAsync(() =>
                {
                    this.loadState = LoadState.Module;
                });
            }
            else if (this._loadState == LoadState.Module) {
                //暂时刚进入游戏后，加载全部ab包，后续要按需加载
                foreach (var kvp in CacheManager.Instance.assetBundlePaths) {
                    var mod = kvp.Value;
                    if (this.modList.Contains(mod)) {
                        continue;
                    }

                    this.modList.Add(mod);
                }

                this.maxCount = this.modList.Count;
                this.tryLoadMod();
            }
            else if (this._loadState == LoadState.Over) {
                this.complete();
            }
        }
    }

    public override void init()
    {
        base.init();
        this.modList = new List<string>();
    }

    public override void update(float dt)
    {
        base.update(dt);

        if (this.loadState == LoadState.None) {
            this.loadState = LoadState.PackPath;
        }
        else if (this.loadState == LoadState.Module) {
            
        }
        else if (this.loadState == LoadState.Over) {
            
        }
    }

    void tryLoadMod()
    {
        if (this.modList.Count == 0) {
            this.loadState = LoadState.Over;
            return;
        }

        var mod = this.modList[0];
        this.modList.RemoveAt(0);
        CacheManager.Instance.loadModuleByAssetBundleAysnc(mod, bundle => this.tryLoadMod());
    }
}