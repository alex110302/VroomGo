using UnityEngine;

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
        
           
    }
}
