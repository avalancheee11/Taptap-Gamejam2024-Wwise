using System;
using System.Collections.Generic;
using UnityEngine;

namespace XHFrameWork
{
    public class AudioSpawnObject
    {
        private GameObject gameObject;
        public AudioSource audioSource { get; private set; }
        public bool isOver => !this.audioSource.isPlaying;
        private float volume;
        private float volumeRate;
        private float finalVolume => this.volume * this.volumeRate;
        
        public AudioSpawnObject()
        {
            this.gameObject = new GameObject("AudioSpawnObj");
            this.gameObject.transform.SetParent(AudioManager.gameObject.transform);
            this.audioSource = this.gameObject.AddComponent<AudioSource>();
            this.audioSource.playOnAwake = false;
            this.audioSource.loop = false;
            this.volumeRate = 1;
        }

        public void playAudio(AudioClip clip, float volume, Vector3 position)
        {
            this.volume = volume * (1 - 0.1f.toCCRandom01());
            this.gameObject.transform.position = position;
            this.audioSource.clip = clip;
            this.audioSource.volume = this.finalVolume;
            this.audioSource.Play();
        }

        public void stopAudio()
        {
            this.audioSource.Stop();
        }

        public void refreshVolume(float volume)
        {
            this.volume = volume * (1 - 0.1f.toCCRandom01());
            this.audioSource.volume = this.finalVolume;
        }

        public void refreshVolueRate(float rate)
        {
            this.volumeRate = rate;
            this.audioSource.volume = this.finalVolume;
        }
    }

    public class AudioManager :Singleton<AudioManager>
    {
        private static Dictionary<AudioClip, List<AudioSpawnObject>> spawnObjs = new Dictionary<AudioClip, List<AudioSpawnObject>>();
        private static Stack<AudioSpawnObject> cacheObjs = new Stack<AudioSpawnObject>();
        public static GameObject gameObject;
        private AudioSource bgSound;
        AudioSource FXsound;
        
        private float musicValue => this.isMult ? 0 : this.MusicValue;
        private float _MusicValue;
        public float MusicValue
        {
            get
            {
                if (Math.Abs(this._MusicValue - IC.NotFound) < float.Epsilon) {
                    this._MusicValue = PlayerPrefs.GetFloat("musicValue", 0.35f);
                }
                return this._MusicValue;
            }
            set
            {
                this._MusicValue = value;
                PlayerPrefs.SetFloat("musicValue", value);
                PlayerPrefs.Save();
                this.refresnMusicVolume();
            }
        }

        private int maxSoundCount => 20;
        private float _SoundValue;
        private float soundValue => this.isMult ? 0 : this.SoundValue;
        public float SoundValue
        {
            get
            {
                if (Math.Abs(this._SoundValue - IC.NotFound) < float.Epsilon) {
                    this._SoundValue = PlayerPrefs.GetFloat("soundValue", 1);
                }
                return this._SoundValue;
            }
            set
            {
                this._SoundValue = value;
                PlayerPrefs.SetFloat("soundValue", value);
                PlayerPrefs.Save();
                this.refreshAllSound();
            }
        }
        
        public bool isMult
        {
            get => PlayerPrefs.GetInt("isMult", 0) == 1;
            set
            {
                PlayerPrefs.SetInt("isMult", value ? 1 : 0);
                PlayerPrefs.Save();
                this.refreshAllSound();
                this.refresnMusicVolume();
            }
        }

        public override void Init()
        {
            this._SoundValue = IC.NotFound;
            this._MusicValue = IC.NotFound;
            if (gameObject == null) {
                gameObject = new GameObject("AudioManager");
                GameObject.DontDestroyOnLoad(gameObject);
            }

            GameObject.DontDestroyOnLoad(gameObject);
            bgSound = gameObject.AddComponent<AudioSource>();
            FXsound = gameObject.AddComponent<AudioSource>();
        }

        public void update(float dt)
        {
            foreach (var kvp in spawnObjs) {
                var needRemove = false;
                foreach (var o in kvp.Value) {
                    if (o.isOver) {
                        needRemove = true;
                        this.pushSpawnObj(o);
                    }
                }

                if (needRemove) {
                    kvp.Value.RemoveAll(x => x.isOver);
                    // this.refreshAudioSpawnVolume(kvp.Value);
                }
            }
        }

        //播放背景音乐
        public void Play(string clipName)
        {
            if (bgSound.clip != null && bgSound.clip.name == clipName) {
                return;
            }
            var clip = CacheManager.Instance.loadAudioClipByAssetBundle(clipName);
            bgSound.clip = clip;
            bgSound.playOnAwake = true;
            bgSound.loop = true;
            bgSound.volume = this.musicValue;
            bgSound.Play();
        }

        //停止背景音乐
        public void Stop()
        {
            bgSound.Stop();
        }
        
        //刷新音乐音量
        void refresnMusicVolume() 
        {
            bgSound.volume = this.musicValue;
        }

        public AudioSource PlaySound(GameObject _soundRes, List<string> clips)
        {
            if (clips.Count == 0) {
                return null;
            }
            return this.PlaySound(_soundRes, clips.getRandomOne());
        }

        public AudioSource PlaySound(GameObject _soundRes, string clipName, float volumeRate = 1)
        {
            var clip = CacheManager.Instance.loadAudioClipByAssetBundle(clipName);
            if (spawnObjs.containsKey(clip) && spawnObjs[clip].Count > this.maxSoundCount) {
                var l = spawnObjs[clip];
                var oo = l[0];
                l.RemoveAt(0);
                oo.stopAudio();
                this.pushSpawnObj(oo);
            }
            var o = this.popSpawnObj();
            o.playAudio(clip, this.soundValue, _soundRes.gameObject.transform.position);
            o.refreshVolueRate(volumeRate);
            var list = spawnObjs.objectValue(clip);
            if (list == null) {
                list = new List<AudioSpawnObject>();
                spawnObjs[clip] = list;
            }
            list.Add(o);
            // this.refreshAudioSpawnVolume(list);
            return o.audioSource;
        }
        
        public void PlaySound(string clipName)
        {
            var clip = CacheManager.Instance.loadAudioClipByAssetBundle(clipName);
            if (clip == null) {
                return;
            }
            FXsound.clip = clip;
            FXsound.loop = false;
            FXsound.volume = this.soundValue;
            FXsound.Play();
        }

        public void PlaySound(AudioClip clip)
        {
            FXsound.clip = clip;
            FXsound.loop = false;
            FXsound.volume = this.soundValue;
            FXsound.Play();
        }

        public void refreshAllSound()
        {
            FXsound.volume = this.soundValue;
            foreach (var kvp in spawnObjs) {
                foreach (var o in kvp.Value) {
                    o.refreshVolume(this.soundValue);
                }
            }
        }

        AudioSpawnObject popSpawnObj()
        {
            if (cacheObjs.Count > 0) {
                return cacheObjs.Pop();
            }

            return new AudioSpawnObject();
        }

        void pushSpawnObj(AudioSpawnObject o)
        {
            cacheObjs.Push(o);
        }

        void refreshAudioSpawnVolume(List<AudioSpawnObject> list)
        {
            if (list.Count == 0) {
                return;
            }
            var rate = Mathf.Min(1f, this.maxSoundCount / (float) list.Count);
            foreach (var o in list) {
                o.refreshVolueRate(rate);
            }
        }
    }
}