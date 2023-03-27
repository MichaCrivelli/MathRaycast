using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    [SerializeField] int rayCount = 36;
    [SerializeField] MathRayLine RayPrefab;

    List<MathRay> rays = new List<MathRay>();
    List<MathRayLine> lines = new List<MathRayLine>();
    List<Wall> wallObjs = new List<Wall>();
    List<Function> walls = new List<Function>();

    private Camera cam;
    private Animator animator;
    [SerializeField] private float waitTime = 5f;
    private float cooldown = 5f;

    [SerializeField] private float goBackTime = 3f;
    private bool inAnimation = true;
    private bool goingBack = false;
    private float elapsed = 0f;
    private Vector3 startPos;
    private Vector3 lastPos;

    private void Awake()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        for (float a = 0.1f; a < 360f; a += 360f / (rayCount > 0 ? rayCount : 1))
        {
            lines.Add(Instantiate<MathRayLine>(RayPrefab, transform));
        }
        wallObjs = RayCastManager.Instance.Walls;
        foreach (Wall wall in wallObjs)
        {
            walls.Add(new Function(wall.Line.start, wall.Line.end));
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 position = cam.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;
            if (animator.isActiveAndEnabled)
            {
                animator.enabled = false;
                lastPos = transform.position;
                inAnimation = false;
            }
            cooldown = waitTime;
            goingBack = false;
            transform.position = position;
        }
        else
        {
            if (cooldown <= 0 && !inAnimation)
            {
                if (!goingBack)
                {
                    startPos = transform.position;
                    goingBack = true;
                    elapsed = 0f;
                }
                elapsed += Time.deltaTime;
                float perc = elapsed / goBackTime;
                transform.position = Vector3.Lerp(startPos, lastPos, Mathf.SmoothStep(0, 1, perc));
                if (perc >= 1)
                {
                    animator.enabled = true;
                    goingBack = false;
                    inAnimation = true;
                }
            }
            else cooldown -= Time.deltaTime;
        }

        rays.Clear();

        for (float a = 0.1f; a < 360f; a += 360f / (rayCount > 0 ? rayCount : 1))
        {
            rays.Add(new MathRay(new Vector2(transform.position.x, transform.position.y), a));
        }

        for (int i = 0; i < rays.Count; i++)
        {
            List<HitPoint> collisions = new List<HitPoint>();

            // get intersections
            for (int j = 0; j < walls.Count; j++)
            {
                Vector2 intersection = rays[i].function.GetIntersection(walls[j]);
                if (wallObjs[j].Line.IsOnLine(intersection) && !wallObjs[j].Line.isTransparent && rays[i].IsInFront(intersection))
                {
                    Vector3 dir = new Vector3(intersection.x, intersection.y, 0) - transform.position;
                    float dist = dir.sqrMagnitude;
                    HitPoint hitPoint = new HitPoint(intersection, dist, wallObjs[j].Line.isTransparent);
                    if (collisions.Count > 0)
                    {
                        for (int c = 0; c < collisions.Count; c++)
                        {
                            if (dist < collisions[c].distance)
                            {
                                collisions.Insert(c, hitPoint);
                                break;
                            }
                        }
                    }
                    else collisions.Add(hitPoint);
                }
            }

            if (collisions.Count > 0)
            {
                lines[i].UpdateRay(transform.position, collisions[0].position);
            }

        }

    }

}
