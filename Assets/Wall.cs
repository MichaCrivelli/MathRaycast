using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Wall : MonoBehaviour
{
    [SerializeField] private bool transparent = false;
    [SerializeField] private Color color = Color.white;
    private Line line;
    public Line Line => line;

    private LineRenderer renderer;

    private void Awake()
    {
        renderer = GetComponent<LineRenderer>();
        UpdateLine();
    }

    public void UpdateTransform(Vector3 position, float width, float angle)
    {
        transform.position = position;
        transform.localScale = Vector3.one * width;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        UpdateLine();
    }

    private Line GetLine()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        float deg = rot.z * Mathf.Deg2Rad;
        Vector3 start = new Vector3(Mathf.Cos(deg) * transform.localScale.x, Mathf.Sin(deg) * transform.localScale.x, 0);
        return new Line(transform.position + start, transform.position - start, color, transparent);
    }

    private void UpdateLine()
    {
        line = GetLine();
        renderer.SetPosition(0, line.start);
        renderer.SetPosition(1, line.end);
        Color col = line.GetColor();
        renderer.startColor = col;
        renderer.endColor = col;
    }

    private void OnDrawGizmos()
    {
        Line line = GetLine();
        Gizmos.color = line.GetColor();
        Gizmos.DrawLine(line.start, line.end);
        Gizmos.color = Color.white;
    }
}
