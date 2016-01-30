using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DanceExecuter : MonoBehaviour {

    [SerializeField] private float _beatLength = 1;
    [SerializeField] private PlayerControl[] _players;
    [SerializeField] private DanceLibrary _lib;
    [SerializeField] private BeatVisualizer _visualizer;

    private float _danceTime;
    private readonly List<DanceStepPair> _commandHistory = new List<DanceStepPair>();
    private readonly List<DanceStepPair> _commandHistoryFlip = new List<DanceStepPair>(); // :)
    private DanceMove[] _danceLibrary;
    
    protected void Start() {
        int i = 0;
        foreach (var p in _players) {
            p.Init(this, i++);
        }
        _danceLibrary = _lib.Moves;
    }

    protected void Update() {
        _danceTime += Time.deltaTime;
        if (_danceTime >= _beatLength) {
            _danceTime -= _beatLength;
            Tick();
            _visualizer.OnDanceTick();
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
        bool flip = ArePlayersFlipped();

        var move = new DanceStepPair {
            p0 = GetDanceStep(moves[0], moves[1], flip),
            p1 = GetDanceStep(moves[1], moves[0], !flip)
        };

        _commandHistory.Add(move);
        _commandHistoryFlip.Add(move.Flip());

        if (_commandHistory.Count > 10) {
            _commandHistory.RemoveAt(0);
            _commandHistoryFlip.RemoveAt(0);
        }

        CheckDanceSequence();
    }

    private void CheckDanceSequence() {
        var bestMatch = _danceLibrary.Where(SequenceMatches).OrderBy(m => m.Steps.Length).LastOrDefault();
        if (bestMatch != null) {
            ExecuteDanceMove(bestMatch);
        }
    }

    private void ExecuteDanceMove(DanceMove bestMatch) {
        Debug.Log("Executing move: " + bestMatch);
        _commandHistory.Clear();
        _commandHistoryFlip.Clear();
    }

    private bool SequenceMatches(DanceMove danceMove) {
        if (danceMove.Steps.Length > _commandHistory.Count) {
            return false;
        }

        return ExactSequenceMatches(danceMove, _commandHistory) || ExactSequenceMatches(danceMove, _commandHistory);
    }

    private bool ExactSequenceMatches(DanceMove danceMove, List<DanceStepPair> history) {
        return !danceMove.Steps.Where((t, i) => !Match(history[history.Count - i - 1], danceMove.Steps[danceMove.Steps.Length - i - 1])).Any();
    }

    private bool Match(DanceStepPair pair0, DanceStepPair pair1) {
        return (pair0.p0 == pair1.p0) && (pair0.p1 == pair1.p1);
    }

    private DanceStep GetDanceStep(PlayerControl.PlayerMoves move, PlayerControl.PlayerMoves otherMove, bool flip) {
        if (move == otherMove) {
            switch (move) {
                case PlayerControl.PlayerMoves.Left:
                    return DanceStep.TandemLeft;
                case PlayerControl.PlayerMoves.Right:
                    return DanceStep.TandemRight;
            }
        }

        switch (move) {
            case PlayerControl.PlayerMoves.Down:
                return DanceStep.Down;
            case PlayerControl.PlayerMoves.Up:
                return DanceStep.Up;
            case PlayerControl.PlayerMoves.Left:
                return flip ? DanceStep.Close : DanceStep.Away;
            case PlayerControl.PlayerMoves.Right:
                return flip ? DanceStep.Away : DanceStep.Close;
        }

        return DanceStep.Idle;
    }

    private bool ArePlayersFlipped() {
        return _players[1].transform.position.x < _players[0].transform.position.y;
    }
}
