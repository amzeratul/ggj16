using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
    private Vector2 _lastInput;
    [SerializeField] private int _playerNumber = 0;
    [SerializeField] private int _joystickNumber = 0;
    [SerializeField] private int _stickNumber = 0;
    private DanceExecuter _dance;
    private PlayerMoves _myMove;

    public enum PlayerMoves {
        Idle,
        Left,
        Right,
        Up,
        Down
    }
    
    protected void Update () {
	    SetInput(GetInputStick());
	}

    public void Init(DanceExecuter dance, int playerNumber) {
        _dance = dance;
        _playerNumber = playerNumber;
        if (playerNumber == 1) {
            _stickNumber = 1;
        }
    }

    private void SetInput(Vector2 input) {
        _lastInput = input;
    }

    private Vector2 GetInputStick() {
        var joy = "Joy" + _joystickNumber + " " + (_stickNumber == 0 ? "Left" : "Right");
        var x = joy + "X";
        var y = joy + "Y";
        return new Vector2(Input.GetAxisRaw(x), Input.GetAxisRaw(y));
    }

    public void OnDanceTick() {
        var move = PlayerMoves.Idle;
        if (_lastInput.magnitude > 0.5f) {
            if (Mathf.Abs(_lastInput.x) > Mathf.Abs(_lastInput.y)) {
                move = _lastInput.x > 0 ? PlayerMoves.Right : PlayerMoves.Left;
            } else {
                move = _lastInput.y > 0 ? PlayerMoves.Down : PlayerMoves.Up;
            }
        }
        DoMove(move);
    }

    private void DoMove(PlayerMoves move) {
        switch (move) {
        case PlayerMoves.Down:
            StartCoroutine(Jump(0, 0));
            break;
        case PlayerMoves.Up:
            StartCoroutine(Jump(0, 1f));
            break;
        case PlayerMoves.Left:
            StartCoroutine(Jump(-1, 0.5f));
            break;
        case PlayerMoves.Right:
            StartCoroutine(Jump(1, 0.5f));
            break;
        default:
            break;
        }
        _myMove = move;
    }

    private IEnumerator Jump(float deltaX, float height) {
        Vector2 startPos = transform.position;
        float length = 0.4f;
        float factor = 1 / length;
        for (float t = 0; t < 1; t += Time.deltaTime * factor) {
            transform.position = startPos + new Vector2(deltaX * t, height * 4 * t * (1 - t));
            yield return null;
        }
        transform.position = startPos + new Vector2(deltaX, 0);
    }

    public PlayerMoves GetMove() {
        return _myMove;
    }
}
