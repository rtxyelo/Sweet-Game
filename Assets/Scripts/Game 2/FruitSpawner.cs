using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _ballsPrefabs = new(4);

    [SerializeField]
    private List<Transform> _spawnPositions = new(9);

    private bool SpawnAllow = true;

    private float _spawnTime = 0.5f;

    public float SpawnTime { get => _spawnTime; set => _spawnTime = value; }

    private GameTwoController _gameController;

    private AudioController _audioController;

    private void Awake()
    {
        _audioController = FindObjectOfType<AudioController>();
        _gameController = FindObjectOfType<GameTwoController>();
    }

    private void Start()
    {
        _gameController.IsGameOver.AddListener(GameOver);
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        while (SpawnAllow)
        {
            int spawnIndex = Random.Range(0, _spawnPositions.Count);
            int baloonIndex = Random.Range(0, _ballsPrefabs.Count);

            GameObject ball = Instantiate(_ballsPrefabs[baloonIndex], _spawnPositions[spawnIndex].transform.position, _ballsPrefabs[baloonIndex].transform.rotation);
            //ball.GetComponent<BallController>().IsBallHit.AddListener(BallBurst);
            yield return new WaitForSeconds(_spawnTime);
        }
    }

    //private void BallBurst()
    //{
    //    //Debug.Log("Ball Burst!");
    //}

    private void GameOver()
    {
        SpawnAllow = false;

        gameObject.SetActive(false);
    }
}
