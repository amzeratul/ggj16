using UnityEngine;
using System.Collections;

public class MoveableWorldBit : MonoBehaviour {

    [SerializeField] private GameObject _target;
    [SerializeField] private Vector2[] _positions;

    protected void Start() {
        _target.transform.position = _positions[0];
    }

    public void OnVariableSet(int level) {
        if (level == 0) {
            _target.transform.position = _positions[0];
        } else {
            int actualLevel = Mathf.Clamp(level, 0, _positions.Length - 1);
            StartCoroutine(DoMoveTowards(_target, _positions[actualLevel], 2.0f));
        }
    }

    private IEnumerator DoMoveTowards(GameObject target, Vector2 endPos, float time) {
        Vector2 startPos = target.transform.position;
        for (float t = 0; t < 1; t += Time.deltaTime / time) {
            target.transform.position = MathUtil.Lerp(startPos, endPos, MathUtil.Overshoot(MathUtil.Smooth(t)));
            yield return null;
        }
        target.transform.position = endPos;
    }
}
