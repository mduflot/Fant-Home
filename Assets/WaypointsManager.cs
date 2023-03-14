using UnityEngine;

public class WaypointsManager : MonoBehaviour
{
    public static WaypointsManager Instance;

    [SerializeField] private Transform[] _waypoints;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);

        Instance = this;
    }

    public Transform[] GetWaypoints()
    {
        return _waypoints;
    }
}
