using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.LightTransport;

public class RoadSpawner : MonoBehaviour 
{
    [field: SerializeField] public GameObject RoadPrefab { get; private set; }
    public Road Road { get; private set; }

    private void Start()
    {
        Road = Instantiate(RoadPrefab, transform.position, Quaternion.identity).GetComponent<Road>();
        
    }
    private void Update()
    {
        SpawnNewRoad();
    }

    private void SpawnNewRoad()
    {
        if (Road.transform.position.y < -10) 
            Road = Instantiate(RoadPrefab, 
            Road.NewRoadSegSpawnPoint.transform.position, 
            Quaternion.identity).GetComponent<Road>();
    }
}
