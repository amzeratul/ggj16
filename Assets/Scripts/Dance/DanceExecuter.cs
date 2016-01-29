using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DanceExecuter : MonoBehaviour {

    [SerializeField] private float _beatLength = 1;
    [SerializeField] private PlayerControl[] _players;
    private float _danceTime;

    protected void Start() {
        int i = 0;
        foreach (var p in _players) {
            p.Init(this, i++);
        }
    }

    protected void Update() {
        _danceTime += Time.deltaTime;
        if (_danceTime >= _beatLength) {
            _danceTime -= _beatLength;
            Tick();
        }
    }

    private void Tick() {
        int i = 0;
        var moves = new PlayerControl.PlayerMoves[2];
        foreach (var p in _players) {
            p.OnDanceTick();
            moves[i++] = p.GetMove();
        }
        ProcessMoves(moves);
    }

    private void ProcessMoves(PlayerControl.PlayerMoves[] moves) {
        Debug.Log(moves[0] + ", " + moves[1]);
    }
}
