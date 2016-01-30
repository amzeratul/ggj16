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

    [SerializeField] private AudioClip _music;
    [SerializeField] private AudioSource _source;
    private readonly List<ListenerBinding> _listeners = new List<ListenerBinding>();

    private float _musicTime;
    private float _bps;
    private float _firstBeat;
    private bool _running;

    private void PlayMusic(AudioClip music, float delay, float firstBeat, float bpm) {
        _source.Stop();
        _source.volume = 1;
        _source.clip = music;
        _source.PlayDelayed(delay);
        _bps = bpm / 60.0f;
        _musicTime = -delay;
        _firstBeat = firstBeat;
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
        }
    }

    public void Register(Listener listener, float headsUp = 0) {
        _listeners.Add(new ListenerBinding { listener = listener, headsUp = headsUp });
    }

    public void Remove(Listener listener) {
        _listeners.Remove(_listeners.Find(o => o.listener == listener));
    }

    public void StartRunning() {
        PlayMusic(_music, 2.0f, 0.2f, 120);
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
        for (float t = 0; t < 1; t += Time.deltaTime * 0.25f) {
            _source.volume = 1 - t;
            yield return null;
        }
        _source.Stop();
    }
}
