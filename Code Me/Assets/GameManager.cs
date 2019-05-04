using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Public Variables
    public GridLayout grid;
    public GameObject player;
    public Vector3Int spawnLocation;
    public Vector3Int endLocation;
    public Vector2Int bottomLeft;
    public Vector2Int topRight;
    public List<Vector3Int> trashLocations;
    #endregion

    #region Private Variables
    private Vector3Int _playerLocation;
    #endregion

    void Start()
    {
        _playerLocation = spawnLocation;
        Vector3 cellPosition = grid.CellToWorld(_playerLocation);
        cellPosition.y += grid.cellSize.y / 2f;
        player = Instantiate(player, cellPosition, Quaternion.identity);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _playerLocation.x--;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _playerLocation.x++;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _playerLocation.y--;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            _playerLocation.y++;
        }

        Move();
    }

    private void Move()
    {
        Vector3 cellPosition = grid.CellToWorld(_playerLocation);
        cellPosition.y += grid.cellSize.y / 2f;
        player.transform.position = cellPosition;
    }
}
