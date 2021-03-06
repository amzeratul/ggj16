﻿using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
    [SerializeField] private int _playerNumber = 0;
    [SerializeField] private GameObject[] _sprites;

    private DanceExecuter _dance;
    private PlayerMoves _myMove;
    private int _position;
    private int _moveRange = 5;
    private SpriteRenderer _renderer;
    private Animator _animator;
    private bool _altStick;

    public enum PlayerMoves {
        Idle,
        Left,
        Right,
        Up,
        Down,
        Fumble
    }

    public bool Flipped { get; set; }

    protected void Awake() {
        foreach (var s in _sprites) {
            s.SetActive(false);
        }
    }

    protected void Update () {
	    SetInput(GetInputStick());
        _renderer.flipX = Flipped;
    }

    private Vector2 GetInputStick() {
        if (_playerNumber == 0) {
            return GetStick(0, 0);
        } else {
            var altStickValue = GetStick(1, 0);
            if (_altStick || altStickValue.magnitude > 0.5) {
                _altStick = true;
                return altStickValue;
            }
            return GetStick(0, 1);
        }
    }

    private static Vector2 GetStick(int joystick, int stick) {
        var joy = "Joy" + joystick + " " + (stick == 0 ? "Left" : "Right");
        var x = joy + "X";
        var y = joy + "Y";
        return new Vector2(Input.GetAxis(x), Input.GetAxis(y));
    }

    public void Init(DanceExecuter dance, int playerNumber) {
        _dance = dance;
        _playerNumber = playerNumber;
        _altStick = false;

        _position = _playerNumber * 2 - 1;
        transform.position = GetScreenPosition(_position);

        var sprite = _sprites[_playerNumber];
        sprite.SetActive(true);
        _renderer = sprite.GetComponent<SpriteRenderer>();
        _animator = sprite.GetComponent<Animator>();
    }

    private void SetInput(Vector2 input) {
        var move = PlayerMoves.Idle;
        if (input.magnitude > 0.5f) {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y)) {
                move = input.x > 0 ? PlayerMoves.Right : PlayerMoves.Left;
            } else {
                move = input.y > 0 ? PlayerMoves.Up : PlayerMoves.Down;
            }
        }
        _myMove = move;
    }

    public void OnDanceTick() {
    }

    private PlayerMoves FilterMove(PlayerMoves move) {
        if ((move == PlayerMoves.Left && _position <= -_moveRange) || (move == PlayerMoves.Right && _position >= _moveRange)) {
            return PlayerMoves.Idle;
        }

        return move;
    }

    public PlayerMoves GetMove() {
        return FilterMove(_myMove);
    }

    public void DoMove(PlayerMoves move) {
        switch (move) {
        case PlayerMoves.Down:
            _animator.SetTrigger("down");
            StartCoroutine(Jump(_position, _position, 0));
            break;
        case PlayerMoves.Up:
            _animator.SetTrigger("jump");
            StartCoroutine(Jump(_position, _position, 1f));
            break;
        case PlayerMoves.Left:
            _animator.SetTrigger(Flipped ? "forward" : "back");
            StartCoroutine(Jump(_position, _position - 1, 0.3f));
            break;
        case PlayerMoves.Right:
            _animator.SetTrigger(Flipped ? "back" : "forward");
            StartCoroutine(Jump(_position, _position + 1, 0.3f));
            break;
        case PlayerMoves.Fumble:
            _animator.SetTrigger("fumble");
            StartCoroutine(Fumble());
            break;
        }
        _myMove = move;
    }

    private const float _stepLength = 0.4f;

    private IEnumerator Fumble() {
        var startCol = _renderer.color;
        float factor = 1 / _stepLength;
        for (float t = 0; t < 1; t += Time.deltaTime * factor) {
            float t2 = MathUtil.Smooth(t);
            _renderer.color = Color.Lerp(Color.red, startCol, t2);
            yield return null;
        }
        _renderer.color = startCol;
    }

    private IEnumerator Jump(int p0, int p1, float height) {
        Vector2 startPos = GetScreenPosition(p0);
        Vector2 endPos = GetScreenPosition(p1);
        float factor = 1 / _stepLength;
        for (float t = 0; t < 1; t += Time.deltaTime * factor) {
            float t2 = MathUtil.Smooth(t);
            transform.position = Vector2.Lerp(startPos, endPos, t2) + new Vector2(0, height * 4 * t2 * (1 - t2));
            yield return null;
        }
        _position = p1;
        transform.position = endPos;
    }

    private static Vector2 GetScreenPosition(int p) {
        return new Vector2(p * 1, -2.5f);
    }

    public int GetPosition() {
        return _position;
    }
}
