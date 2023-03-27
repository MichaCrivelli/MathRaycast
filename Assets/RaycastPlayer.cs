using System;
using System.Collections.Generic;
using UnityEngine;


public class RaycastPlayer : MonoBehaviour
{
    private float boundaryWidth = 15f;

    public int RayCount = 72;
    public Vector2 Boundary = new Vector2(15, 10);

    public Color WallColor = Color.green;
    public List<Line> Lines = new List<Line>();
    private List<MathRay> rays = new List<MathRay>();
    private List<Function> walls = new List<Function>();
    private List<Line> boundary = new List<Line>();

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        float delta = Time.deltaTime;

        Vector3 position = cam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButton(0))
        {
            transform.position = position;
        }
    }

    private void OnDrawGizmos()
    {
        rays.Clear();
        walls.Clear();
        boundary = new List<Line>()
        {
            new Line(new Vector2(-Boundary.x, -Boundary.y),new Vector2(Boundary.x, -Boundary.y),Color.white),
            new Line(new Vector2(Boundary.x, -Boundary.y),new Vector2(Boundary.x, Boundary.y),Color.white),
            new Line(new Vector2(Boundary.x, Boundary.y),new Vector2(-Boundary.x, Boundary.y),Color.white),
            new Line(new Vector2(-Boundary.x, Boundary.y),new Vector2(-Boundary.x, -Boundary.y),Color.white)
        };

        // create rays
        for (float a = 0.1f; a < 360f; a += 360f / (RayCount > 0 ? RayCount : 1))
        {
            MathRay ray = new MathRay(new Vector2(transform.position.x, transform.position.y), a);
            rays.Add(ray);
        }

        boundary.AddRange(Lines);
        // draw walls
        for (int i = 0; i < boundary.Count; i++)
        {
            if (boundary[i].color == new Color(0, 0, 0, 0)) Gizmos.color = WallColor;
            else Gizmos.color = boundary[i].color;
            if (boundary[i].isTransparent) Gizmos.color = new Color(Gizmos.color.r / 2, Gizmos.color.g / 2, Gizmos.color.b / 2);

            Function func = new Function(boundary[i].start, boundary[i].end);
            walls.Add(func);

            Gizmos.DrawLine(boundary[i].start, boundary[i].end);
        }
        Gizmos.color = Color.white;

        // Draw Rays
        for (int i = 0; i < rays.Count; i++)
        {
            List<HitPoint> collisions = new List<HitPoint>();

            // get intersections
            for (int j = 0; j < walls.Count; j++)
            {
                Vector2 intersection = rays[i].function.GetIntersection(walls[j]);
                if (boundary[j].IsOnLine(intersection) && rays[i].IsInFront(intersection))
                {
                    Vector3 dir = new Vector3(intersection.x, intersection.y, 0) - transform.position;
                    float dist = dir.sqrMagnitude;
                    HitPoint hitPoint = new HitPoint(intersection, dist, boundary[j].isTransparent);
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
                for (int j = 0; j < collisions.Count; j++)
                {
                    if (j == 0) Gizmos.DrawLine(transform.position, new Vector3(collisions[j].position.x, collisions[j].position.y, 0));
                    else
                    {
                        Gizmos.color = new Color(Gizmos.color.r / 2, Gizmos.color.g / 2, Gizmos.color.b / 2);
                        Gizmos.DrawLine(collisions[j - 1].position, collisions[j].position);
                    }
                    if (!collisions[j].isTransparent) break;
                }
            }
            Gizmos.color = Color.white;


        }

    }

}
