﻿using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class GenerateMaze : MonoBehaviour
{
	[SerializeField] private GameObject tilePrefab = null;
	[SerializeField] private GameObject borderPrefab = null;
	[SerializeField] private GameObject wallPrefab = null;

	private int set;
	
	public void StartGeneration(int size)
	{
		Spawngrid(size);
		Debug.Log("spawned");
		SpawnBorders(size);
		SpawnWalls(size);
	}

	private void Spawngrid(int size)
	{
		for (int x = 0; x < size; ++x)
		{
			GameObject column = new GameObject();
			column.name = "Column" + x;
			for(int y = 0; y < size; ++y)
			{
				Vector3 position = new Vector3(x, y);
				GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
				tile.transform.parent = column.transform;
			}

			column.transform.parent = gameObject.transform;
		}
	}

	private void SpawnBorders(int size)
	{
		GameObject borders = new GameObject();
		borders.name = "Borders";

		float offset = 0.5f;
		
		
		List<Vector3> positions = new List<Vector3>();

		List<Vector3> verticals = new List<Vector3>();
		for (int x = 0; x < size; ++x)
		{
			for (int y = 0; y < size; ++y)
			{
				if (x == 0)
				{
					verticals.Add(new Vector3(x - offset, y));
				}
				else if (x == size - 1)
				{
					verticals.Add(new Vector3(x + offset, y));
				}

				if (y == 0)
				{
					positions.Add(new Vector3(x, y - offset));
				}
				else if (y == size - 1)
				{
					positions.Add(new Vector3(x, y + offset));
				}
			}
		}
		
		foreach (Vector3 position in positions)
		{
			var border = Instantiate(borderPrefab, position, Quaternion.identity);
			border.transform.parent = borders.transform;
		}

		foreach (Vector3 position in verticals)
		{
			var border = Instantiate(borderPrefab, position, Quaternion.identity);
			border.transform.parent = borders.transform;
			border.transform.Rotate(new Vector3(0,0,1), 90);
		}
	}

	private void SpawnWalls(int size)
	{
		List<List<int>> setMap = new List<List<int>>();
		GameObject walls = new GameObject();
		walls.name = "walls";
		
		List<int> previousColumn = new List<int>();
		set = 0;
		for (; set < size; ++set)
		{
			previousColumn.Add(set);
		}

		previousColumn = MergeColumn(previousColumn);
		setMap.Add(previousColumn);

		for (int i = 1; i < previousColumn.Count; ++i)
		{
			if (previousColumn[i] != previousColumn[i - 1])
			{
				var wall = Instantiate(wallPrefab, new Vector3(0, i - 0.5f), Quaternion.identity);
				wall.name = "wall";
				wall.transform.parent = walls.transform;
			}
		}

		for (int i = 1; i < size-1; ++i)
		{
			List<int> currentColumn = GenerateNextColumn(previousColumn);
			InsertNewSets(currentColumn);

			for (int k = 0; k < size; ++k)
			{
				if (previousColumn[k] != currentColumn[k])
				{
					var wall = Instantiate(wallPrefab, new Vector3(i - 0.5f, k), Quaternion.identity);
					wall.name = "wall";
					wall.transform.Rotate(new Vector3(0,0,1), 90);
					wall.transform.parent = walls.transform;
				}
			}
			
			currentColumn = MergeColumn(currentColumn);

			for (int k = 1; k < currentColumn.Count; ++k)
			{
				if (currentColumn[k] != currentColumn[k - 1])
				{
					var wall = Instantiate(wallPrefab, new Vector3(i, k - 0.5f), Quaternion.identity);
					wall.name = "wall";
					wall.transform.parent = walls.transform;
				}
			}
			
			setMap.Add(currentColumn);
			previousColumn = currentColumn;
		}

		List<int> finalColumn = GenerateNextColumn(previousColumn);
		InsertNewSets(finalColumn);
		finalColumn = MergeColumn(finalColumn);

		List<bool> finalWalls = new List<bool>();
		for (int i = 0; i < finalColumn.Count - 1; ++i)
		{
			finalWalls.Add(finalColumn[i] != finalColumn[i + 1]);
		}

		for (int i = 0; i < finalColumn.Count - 1; ++i)
		{
			if (finalColumn[i] != finalColumn[i + 1])
			{
				finalWalls[i] = false;
				
				int unifiedSet = finalColumn[i];
				int unifyingSet = finalColumn[i + 1];
				for (int k = i; k < finalColumn.Count; ++k)
				{
					if (finalColumn[k] == unifyingSet)
					{
						finalColumn[k] = unifiedSet;
					}
				}
			}
		}

		for (int i = 0; i < finalWalls.Count; ++i)
		{
			if (finalWalls[i])
			{
				var wall = Instantiate(wallPrefab, new Vector3(size - 1, i + 1 - 0.5f), Quaternion.identity);
				wall.name = "wall";
				wall.transform.parent = walls.transform;
			}
		}
	}

	private List<int> MergeColumn(List<int> column)
	{
		List<int> mergedColumn = new List<int>();
		
		for (int i = 1; i < column.Count; ++i)
		{
			if (column[i] != column[i - 1])
			{
				int result = Random.Range(0, 2);
				if (result == 1)
				{
					column[i] = column[i - 1];
				}
			}
		}
		
		return column;
	}

	private List<int> GenerateNextColumn(List<int> column)
	{
		List<int> nextColumn = new List<int>();

		int currentSet = column[0];
		int setStart = 0;
		bool isSetConnected = false;

		for (int i = 0; i < column.Count; ++i)
		{
			int set = column[i];

			if (set != currentSet)
			{
				if (!isSetConnected)
				{
					int position = Random.Range(setStart, i);
					nextColumn[position] = currentSet;
				}

				currentSet = set;
				setStart = i;
				isSetConnected = false;
			}
			
			int result = Random.Range(0, 2);
			
			if (result == 1)
			{
				isSetConnected = true;
				nextColumn.Add(set);
			}
			else
			{
				nextColumn.Add(-1);
			}
		}

		if (!isSetConnected)
		{
			int position = Random.Range(setStart, column.Count);
			nextColumn[position] = currentSet;
		}

		return nextColumn;
	}

	private void InsertNewSets(List<int> column)
	{
		for(int i = 0; i < column.Count; ++i)
		{
			if (column[i] == -1)
			{
				++set;
				column[i] = set;
			}
		}
	}
}
