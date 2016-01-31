using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Rhythm : MonoBehaviour {
    public interface Listener {
        void OnTick();
    }

    private class ListenerBinding {
        public Listener listener;
        public float headsUp;
        public int lastBeat = -1;
    }

    //[SerializeField] private AudioClip _music;
    //[SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip[] _layerClips;
    [SerializeField] private AudioSource[] _layerSources;
    private readonly List<ListenerBinding> _listeners = new List<ListenerBinding>();

    private float _musicTime;
    private float _bps;
    private float _bpm;
    private float _firstBeat;
    private bool _running;

    protected void Awake() {
        Instance = this;
    }

    public static Rhythm Instance { get; private set; }

    public float BPM {
        get { return _bpm; }
    }

    private void PlayMusic(float delay, float firstBeat, float bpm) {
        for (int i = 0; i < _layerClips.Length; i++)
        {
            _layerSources[i].Stop();
            _layerSources[i].volume = (i>0)?0f:1f;
            _layerSources[i].clip = _layerClips[i];
            _layerSources[i].PlayDelayed(delay);
        }
        //_source.clip = music;
        //_source.PlayDelayed(delay);
        _bpm = bpm;
        _bps = bpm / 60.0f;
        _musicTime = -delay;
        _firstBeat = firstBeat;
    }

    public void FadeLayerTowards(int layer, float volume) {
        Tween.Stop(_layerSources[layer].gameObject);
        Tween.VolumeTo(_layerSources[layer].gameObject, volume, 1f, Tween.EaseType.Linear);
    }

    // start fading in a given layer source
    public void FadeInLayer(int layer)
    {
        Tween.Stop(_layerSources[layer].gameObject);
        Tween.VolumeTo(_layerSources[layer].gameObject, 1f, 1f, Tween.EaseType.Linear);
    }

    private void FadeOutLayer(int layer)
    {
        Tween.Stop(_layerSources[layer].gameObject);
        Tween.VolumeTo(_layerSources[layer].gameObject, 0f, 1f, Tween.EaseType.Linear);
    }

    private void ToggleLayer(int layer)
    {
        if (_layerSources[layer].volume > 0f)
        {
            FadeOutLayer(layer);
        }
        else
        {
            FadeInLayer(layer);
        }
    }

    protected void Update() {
        if (_running) {
            _musicTime += Time.deltaTime;

            foreach (var l in _listeners) {
                int curTick = Mathf.FloorToInt((_musicTime - _firstBeat + l.headsUp) * _bps);
                if (curTick > l.lastBeat) {
                    l.lastBeat = curTick;
                    l.listener.OnTick();
                }
            }

#if UNITY_EDITOR
            for (int i = 1; i < _layerSources.Length; i++)
            {
                if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1 + (i-1))))
                {
                    ToggleLayer(i);
                }
            }
#endif
        }
    }

    public void Register(Listener listener, float headsUp = 0) {
        _listeners.Add(new ListenerBinding { listener = listener, headsUp = headsUp });
    }

    public void Remove(Listener listener) {
        _listeners.Remove(_listeners.Find(o => o.listener == listener));
    }

    public void StartRunning() {
        PlayMusic(2.0f, 0.2f, 90);
        _running = true;
    }

    public void StopRunning() {
        foreach (var l in _listeners) {
            l.lastBeat = -1;
        }
        StartCoroutine(DoStop());
        _running = false;
    }

    private IEnumerator DoStop() {
        
        for (float t = 0; t < 1f; t += Time.deltaTime * 0.25f)
        {
            for (int i = 0; i < _layerSources.Length; i++)
            {
                _layerSources[i].volume = 1f - t;
               
            }
            yield return null;
        }
        for (int i = 0; i < _layerSources.Length; i++)
        {
            _layerSources[i].Stop();
        }
    }
}
