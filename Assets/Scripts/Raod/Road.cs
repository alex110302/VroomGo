using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour 
{
    //Perfabs
    [field: SerializeField] public GameObject carPrefab {  get; private set; }
    public CarController Car { get; private set; }

    //Scripts
    public Pathfinding Pathfinding { get; private set; }

    private void Awake()
    {
        Pathfinding = new Pathfinding(8, 1, 2.5f, transform.position, 0, true);
    }

    private void Start()
    {
        GenerateCars();
    }

    public void FixedUpdate()
    {
        transform.position += (Vector3.up * Time.fixedDeltaTime) * -10; 
    }

    public void GenerateCars(uint minCars = 0, uint maxCars = 6)
    {
        if (minCars == maxCars || maxCars > Pathfinding.Grid.Width - 1)
        {
            minCars = 0;
            maxCars = 0;
        }

        int spawnLocations = Random.Range(0, 6);

        //Allocate Arrays for ditermaning safe postions to place a car
        List<int> safePositions = new List<int>(Pathfinding.Grid.Width - 1);
        int[] unsafePositions = new int[Pathfinding.Grid.Width - 1];
        for (int i = 0; i < spawnLocations; i++) safePositions.Add(i);

        for (int i = 0; i < spawnLocations; i++)
        {
            int gridPostion = Random.Range(0, Pathfinding.Grid.Width);
            unsafePositions[i] = gridPostion;

            foreach (int postion in unsafePositions)
            {
                if (i == 0) break;
                else if (gridPostion == postion)
                {
                    gridPostion = safePositions[Random.Range(0, safePositions.Count)];
                    unsafePositions[i] = gridPostion;
                    safePositions.Remove(postion);
                    break;
                }
            }

            Car = Instantiate(carPrefab, transform).GetComponent<CarController>();

            Car.transform.position = Pathfinding.Grid.GetCenterOfCellPosition(gridPostion, 0);
        }
    }
}