using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridGenerator : MonoBehaviour
{
	public HexTile tilePrefab;//reference to the tile prefab
	//size of the grid
	public int height = 100;
	public int width = 100;

	HexTile[] cells;
	private SimulationManager simManager; //reference to the SimulationManager

	private void Start()
	{
		simManager = FindObjectOfType<SimulationManager>(); //could also have made it a singleton and reference its instance
		cells = new HexTile[height * width];
		int i = 0;
		for (int x = 0; x < height; x++)
		{
			for (int z = 0; z < width; z++)
			{
				CreateCell(x, z, i++);
			}
		}
		StartSimulation();
	}


	void CreateCell(int x, int z, int i)
	{
		Vector3 position;
		position.x = x * (-1.73f);
		position.y = 0f;
		position.z = z * (-2) - (x % 2);

		HexTile cell = cells[i] = Instantiate(tilePrefab, position, Quaternion.Euler(270, 0, 90));
		if (x % 2 == 0)
		{
			if (z % 2 == 0)
            {
				cell.SetColor(Color.green);
			}
            else
            {
				cell.SetColor(Color.yellow);
			}

		}
		else
		{
			if (z % 2 == 0)
            {
				cell.SetColor(Color.cyan);
			}
            else
            {
				cell.SetColor(Color.magenta);
			}	
		}
		cell.coordinates = new HexTile.HexCoordinates(x, z);
		cell.transform.SetParent(transform);
		//cell.transform.position = position;

	}

	/// <summary>
	/// starts the simulation
	/// </summary>
	public void StartSimulation()
	{
		simManager.StartSimulation(cells, height, width);
	}
}
