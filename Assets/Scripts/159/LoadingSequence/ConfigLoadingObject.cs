using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigLoadingObject : BasesLoadingObject
{
    public override string desc => LocalizedUtils.StringForKey("正在加载配置……");

    enum LoadingState
    {
        None,
        Config,
        Over,
    }

    private LoadingState _loadingState;

    private LoadingState loadingState
    {
        get => this._loadingState;
        set
        {
            if (this._loadingState == value) {
                return;
            }

            this._loadingState = value;
            if (this._loadingState == LoadingState.Config) {
                DataBaseManager.Instance.load();
            }
            else if (this._loadingState == LoadingState.Over) {
                this.complete();
            }
        }
    }

    public override void update(float dt)
    {
        base.update(dt);
        if (this.loadingState == LoadingState.None) {
            this.loadingState = LoadingState.Config;
        }
        else if (this.loadingState == LoadingState.Config) {
            if (DataBaseManager.Instance.loadOver) {
                this.loadingState = LoadingState.Over;
            }
        }
    }
}