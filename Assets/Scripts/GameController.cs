using UnityEngine;
using UnityEngine.Playables;

public class GameController : MonoBehaviour
{
	[SerializeField] private int mazeSize = 10;
	[SerializeField] private int sizeStep = 2;

	[SerializeField] private GenerateMaze mazePrefab = null;
	[SerializeField] private CharacterController characterPrefab = null;
	[SerializeField] private Bed bedPrefab = null;
	[SerializeField] private GameObject hudPrefab = null;
	[SerializeField] private BedPointer pointerPrefab = null;

	private GenerateMaze maze = null;
	private CharacterController character = null;
	private Bed bed = null;
	private GameObject hud = null;
	private Timer timer = null;
	
	private void Awake()
	{
		CreateNewBoard(mazeSize);
		Bed.OnLevelFinished += LevelFinished;
		Timer.OnGameOver += ClearBoard;
	}

	private void CreateNewBoard(int size)
	{
		maze = Instantiate<GenerateMaze>(mazePrefab, Vector3.zero, Quaternion.identity);
		maze.StartGeneration(size);
		
		int mazeMiddle = size / 2;
		bed = Instantiate<Bed>(bedPrefab, new Vector3(mazeMiddle, mazeMiddle, -1), Quaternion.identity);
		
		character = Instantiate<CharacterController>(characterPrefab, RandomizePosition(size), Quaternion.identity);

		hud = Instantiate<GameObject>(hudPrefab, transform.position, Quaternion.identity);
		timer = hud.GetComponentInChildren<Timer>();
		timer.Counter = size + Mathf.Ceil(size / 10.0f);
	}
	
	private void LevelFinished()
	{
		ClearBoard();

		mazeSize += sizeStep;
		CreateNewBoard(mazeSize);
	}

	private void ClearBoard()
	{
		Destroy(bed.gameObject);
		Destroy(character.gameObject);
		Destroy(maze.gameObject);
		Destroy(hud.gameObject);
	}
	
	private Vector3 RandomizePosition(int size)
	{
		int index = Random.Range(0, size);
		int border = Random.Range(0, 4);

		Vector3 position = Vector3.zero;
		switch (border)
		{
			case 0:
			{
				position = new Vector2(0, index);
				break;
			}
			case 1:
			{
				position = new Vector2(index, 0);
				break;
			}
			case 2:
			{
				position = new Vector2(size - 1, index);
				break;
			}
			case 3:
			{
				position = new Vector2(index, size - 1);
				break;
			}
		}

		position.z = -1;
		return position;
	}

	private void Update()
	{
		if (Input.GetMouseButtonUp(1) && timer.Counter > 3)
		{
			var marker = Instantiate<BedPointer>(pointerPrefab, character.transform.position, Quaternion.identity);
			marker.transform.parent = maze.transform;
			timer.Counter -= 3;
		}
	}
}
