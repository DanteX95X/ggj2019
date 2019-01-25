using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Networking;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class GenerateMaze : MonoBehaviour
{
	[SerializeField] private int size = 6;
	[SerializeField] private GameObject tilePrefab = null;
	[SerializeField] private GameObject borderPrefab = null;
	
	private List<List<GameObject>> map = new List<List<GameObject>>();
	
	private void Start()
	{
		Spawngrid(size);
		SpawnBorders();
		SpawnWalls();
	}

	private void Spawngrid(int size)
	{
		for (int x = 0; x < size; ++x)
		{
			GameObject column = new GameObject();
			column.name = "Column" + x;
			List<GameObject> abstractColumn = new List<GameObject>();
			
			for(int y = 0; y < size; ++y)
			{
				Vector3 position = new Vector3(x, y);
				GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
				tile.transform.parent = column.transform;
				
				abstractColumn.Add(tile);
			}

			column.transform.parent = gameObject.transform;
			map.Add(abstractColumn);
		}
	}

	private void SpawnBorders()
	{
		List<Vector3> positions = new List<Vector3>();
		for (int x = 0; x < size; ++x)
		{
			for (int y = 0; y < size; ++y)
			{
				if (x == 0)
				{
					positions.Add(new Vector3(x - 1, y));
				}
				else if (x == size - 1)
				{
					positions.Add(new Vector3(x + 1, y));
				}

				if (y == 0)
				{
					positions.Add(new Vector3(x, y - 1));
				}
				else if (y == size - 1)
				{
					positions.Add(new Vector3(x, y + 1));
				}
			}
		}

		positions.Add(new Vector3(-1, -1));
		positions.Add(new Vector3(-1, size));
		positions.Add(new Vector3(size, -1));
		positions.Add(new Vector3(size, size));
		
		foreach (Vector3 position in positions)
		{
			Instantiate(borderPrefab, position, Quaternion.identity);
		}
	}

	private void SpawnWalls()
	{
		
	}
}
