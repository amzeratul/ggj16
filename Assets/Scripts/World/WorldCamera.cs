using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class WorldCamera : MonoBehaviour {

    [Range(0, 1)] public float Progress;
    [SerializeField] public Vector2 StartPos;
    [SerializeField] public Vector2 EndPos;
    [SerializeField] public float StartSize;
    [SerializeField] public float EndSize;
    private Camera _cam;

    protected void Awake() {
        _cam = GetComponent<Camera>();
    }

    protected void Update () {
        _cam.orthographicSize = Mathf.Lerp(StartSize, EndSize, Progress);
        var p = Vector2.Lerp(StartPos, EndPos, Progress);
        transform.position = new Vector3(p.x, p.y, -10);
    }

    public void Reset() {
        Progress = 0;
    }

}
