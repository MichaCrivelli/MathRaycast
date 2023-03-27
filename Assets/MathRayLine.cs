using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MathRayLine : MonoBehaviour
{
    private LineRenderer renderer;

    private void Awake()
    {
        renderer = GetComponent<LineRenderer>();
    }

    public void UpdateRay(Vector3 start, Vector3 end)
    {
        renderer.SetPosition(0, start);
        renderer.SetPosition(1, end);
    }
}
