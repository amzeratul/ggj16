using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DanceExecuter : MonoBehaviour, Rhythm.Listener {

    [SerializeField] private Rhythm _rhythm;
    [SerializeField] private PlayerControl[] _players;
    [SerializeField] private DanceLibrary _lib;
    [SerializeField] private BeatVisualizer _visualizer;
    [SerializeField] private World _world;
    [SerializeField] private DanceIcon _icon;

    private readonly List<DanceStepPair> _commandHistory = new List<DanceStepPair>();
    private readonly List<DanceStepPair> _commandHistoryFlip = new List<DanceStepPair>(); // :)
    private DanceMove[] _danceLibrary;
    
    protected void Start() {
        Reset();
        _danceLibrary = _lib.Moves;
        _rhythm.Register(this);
    }

    public void Reset() {
        int i = 0;
        foreach (var p in _players) {
            p.Init(this, i++);
        }
        ClearHistory();
    }

    public void OnTick() {
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
        
        var dist = Mathf.Abs(_players[0].GetPosition() - _players[1].GetPosition());
        var p0 = GetDanceStep(moves[0], moves[1], flip, dist);
        var p1 = GetDanceStep(moves[1], moves[0], !flip, dist);
        // Jesus christ special cases :(
        if ((p0 == DanceStep.Away && p1 == DanceStep.Away && dist > 1) || (p0 == DanceStep.Close && p1 == DanceStep.Close && dist < 3)) {
            p0 = p1 = DanceStep.Fumble;
        }

        _commandHistory.Add(new DanceStepPair { p0 = p0, p1 = p1 });
        _commandHistoryFlip.Add(new DanceStepPair { p0 = p1, p1 = p0 });

        if (_commandHistory.Count > 10) {
            _commandHistory.RemoveAt(0);
            _commandHistoryFlip.RemoveAt(0);
        }

        CheckDanceSequence();

        _players[0].DoMove(p0 == DanceStep.Fumble ? PlayerControl.PlayerMoves.Fumble : moves[0]);
        _players[1].DoMove(p1 == DanceStep.Fumble ? PlayerControl.PlayerMoves.Fumble : moves[1]);
    }
    
    private void CheckDanceSequence() {
        if (_commandHistory.Count == 0) {
            // Nothing to do here!
            return;
        }

        var last = _commandHistory[_commandHistory.Count - 1];
        if (last.p0 == DanceStep.Fumble || last.p1 == DanceStep.Fumble) {
            _world.Fumble();
            ClearHistory();
            return;
        }

        var bestMatch = _danceLibrary.Where(SequenceMatches).OrderBy(m => m.Steps.Length).LastOrDefault();
        if (bestMatch != null) {
            ExecuteDanceMove(bestMatch);
            ClearHistory();
        }
    }

    private void ClearHistory() {
        _commandHistory.Clear();
        _commandHistoryFlip.Clear();
    }

    private void ExecuteDanceMove(DanceMove bestMatch) {
        _icon.ShowDanceIcon(bestMatch.Id);
        bestMatch.Effect(_world);
    }

    private bool SequenceMatches(DanceMove danceMove) {
        if (danceMove.Steps.Length == 0) {
            return false;
        }

        if (danceMove.Steps.Length > _commandHistory.Count) {
            return false;
        }

        if (!danceMove.IsAvailable(_world)) {
            return false;
        }

        return ExactSequenceMatches(danceMove, _commandHistory) || ExactSequenceMatches(danceMove, _commandHistoryFlip);
    }

    private bool ExactSequenceMatches(DanceMove danceMove, List<DanceStepPair> history) {
        return !danceMove.Steps.Where((t, i) => !Match(history[history.Count - i - 1], danceMove.Steps[danceMove.Steps.Length - i - 1])).Any();
    }

    private bool Match(DanceStepPair pair0, DanceStepPair pair1) {
        return (pair0.p0 == pair1.p0) && (pair0.p1 == pair1.p1);
    }

    private DanceStep GetDanceStep(PlayerControl.PlayerMoves move, PlayerControl.PlayerMoves otherMove, bool flip, int distance) {
        // Tandem
        if (move == otherMove) {
            switch (move) {
                case PlayerControl.PlayerMoves.Left:
                    return DanceStep.TandemLeft;
                case PlayerControl.PlayerMoves.Right:
                    return DanceStep.TandemRight;
            }
        }

        // Swap
        if (distance == 1 && ((flip && move == PlayerControl.PlayerMoves.Left && otherMove == PlayerControl.PlayerMoves.Right) || (!flip && move == PlayerControl.PlayerMoves.Right && otherMove == PlayerControl.PlayerMoves.Left))) {
            return DanceStep.Swap;
        }

        // Normal
            switch (move) {
            case PlayerControl.PlayerMoves.Down:
                return DanceStep.Down;
            case PlayerControl.PlayerMoves.Up:
                return DanceStep.Up;
            case PlayerControl.PlayerMoves.Left:
                return flip ? TryClose(distance) : TryAway(distance);
            case PlayerControl.PlayerMoves.Right:
                return flip ? TryAway(distance) : TryClose(distance);
            case PlayerControl.PlayerMoves.Fumble:
                return DanceStep.Fumble;
        }

        return DanceStep.Idle;
    }

    private static DanceStep TryClose(int distance) {
        return distance > 1 ? DanceStep.Close : DanceStep.Fumble;
    }

    private static DanceStep TryAway(int distance) {
        return distance < 3 ? DanceStep.Away : DanceStep.Fumble;
    }

    private bool ArePlayersFlipped() {
        return _players[1].GetPosition() < _players[0].GetPosition();
    }
}
