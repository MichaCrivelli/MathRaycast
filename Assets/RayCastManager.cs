using System.Collections.Generic;
using UnityEngine;

public class RayCastManager : MonoBehaviour
{

    [SerializeField] Vector2 boundary;
    [SerializeField] Wall wallPrefab;
    List<Wall> walls = new List<Wall>();
    public List<Wall> Walls => walls;

    public static RayCastManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else Destroy(gameObject);
        walls.AddRange(GameObject.FindObjectsOfType<Wall>());
    }

}
