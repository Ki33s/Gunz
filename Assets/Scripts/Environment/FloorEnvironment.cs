using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorEnvironment : MonoBehaviour
{
    public int carGenProb = 50;
    public int benchGenProb = 50;
    public int treesGenProb = 100;
    public int trashGenProb = 50;

    public GameObject[] carSpawnPoints;
    public GameObject[] benchSpawnPoints;
    public GameObject[] treeSpawnPoints;
    public GameObject[] trashSpawnPoints;

    public GameObject[] cars;
    public GameObject[] trees;
    public GameObject[] trash;
    public GameObject bench;
    public Material[] materials;
    // Start is called before the first frame update
    void Start()
    {
        GenerateVehicles();
        GenerateBenches();
        GenerateTrees();
        GenerateTrash();
    }

    void GenerateVehicles()
    {
        if (Random.Range(0, 100) < carGenProb)
        {
            int carRotationY = 90;
            if (Random.Range(0, 2) == 1)
                carRotationY = -90;

            GameObject newCar = Instantiate(cars[Random.Range(0, cars.Length)], carSpawnPoints[0].transform.position, Quaternion.Euler(0, carRotationY, 0), carSpawnPoints[0].transform);
            newCar.transform.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
        }

        if (Random.Range(0, 100) < carGenProb)
        {
            int carRotationY = 90;
            if (Random.Range(0, 2) == 1)
                carRotationY = -90;

            GameObject newCar = Instantiate(cars[Random.Range(0, cars.Length)], carSpawnPoints[1].transform.position, Quaternion.Euler(0, carRotationY, 0), carSpawnPoints[1].transform);
            newCar.transform.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
        }
    }

    void GenerateBenches()
    {
        if (Random.Range(0, 100) < benchGenProb)
        {
            GameObject newBench = Instantiate(bench, benchSpawnPoints[0].transform.position, benchSpawnPoints[0].transform.rotation, benchSpawnPoints[0].transform);
            newBench.transform.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
        }

        if (Random.Range(0, 100) < benchGenProb)
        {
            GameObject newBench = Instantiate(bench, benchSpawnPoints[1].transform.position, benchSpawnPoints[1].transform.rotation, benchSpawnPoints[1].transform);
            newBench.transform.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
        }
    }

    void GenerateTrees()
    {
        if (Random.Range(0, 100) < treesGenProb)
        {
            GameObject newTree = Instantiate(trees[Random.Range(0, trees.Length)], treeSpawnPoints[0].transform.position, treeSpawnPoints[0].transform.rotation, treeSpawnPoints[0].transform);
            newTree.transform.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
        }

        if (Random.Range(0, 100) < treesGenProb)
        {
            GameObject newTree = Instantiate(trees[Random.Range(0, trees.Length)], treeSpawnPoints[1].transform.position, treeSpawnPoints[1].transform.rotation, treeSpawnPoints[1].transform);
            newTree.transform.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
        }
    }

    void GenerateTrash()
    {
        if (Random.Range(0, 100) < trashGenProb)
        {
            GameObject newTrash = Instantiate(trash[Random.Range(0, trashSpawnPoints.Length)], trashSpawnPoints[0].transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0), trashSpawnPoints[0].transform);
            newTrash.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
        }

        if (Random.Range(0, 100) < trashGenProb)
        {
            GameObject newTrash = Instantiate(trash[Random.Range(0, trashSpawnPoints.Length)], trashSpawnPoints[1].transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0), trashSpawnPoints[1].transform);
            newTrash.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
        }

        if (Random.Range(0, 100) < trashGenProb)
        {
            GameObject newTrash = Instantiate(trash[Random.Range(0, trashSpawnPoints.Length)], trashSpawnPoints[2].transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0), trashSpawnPoints[2].transform);
            newTrash.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
        }

        if (Random.Range(0, 100) < trashGenProb)
        {
            GameObject newTrash = Instantiate(trash[Random.Range(0, trashSpawnPoints.Length)], trashSpawnPoints[3].transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0), trashSpawnPoints[3].transform);
            newTrash.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
        }

        if (Random.Range(0, 100) < trashGenProb)
        {
            GameObject newTrash = Instantiate(trash[Random.Range(0, trashSpawnPoints.Length)], trashSpawnPoints[4].transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0), trashSpawnPoints[4].transform);
            newTrash.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
        }

        if (Random.Range(0, 100) < trashGenProb)
        {
            GameObject newTrash = Instantiate(trash[Random.Range(0, trashSpawnPoints.Length)], trashSpawnPoints[5].transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0), trashSpawnPoints[5].transform);
            newTrash.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
        }
    }
}
