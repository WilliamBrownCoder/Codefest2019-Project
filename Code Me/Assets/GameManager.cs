using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum Dir {north, south, east, west};

    #region Public Variables
    public GridLayout grid;
    public GameObject player;
    public GameObject north;
    public GameObject south;
    public GameObject west;
    public GameObject east;
    public Dir direction;
    public Vector3Int spawnLocation;
    public Vector3Int endLocation;
    public Vector2Int bottomLeft;
    public Vector2Int topRight;
    public List<Vector3Int> trashLocations;
    public GameObject functionGenerator;
    public float delay;
    #endregion

    #region Private Variables
    private Vector3Int _playerLocation;
    private Dir startingDir;
    private bool running = false;
    #endregion

    void Start()
    {
        startingDir = direction;
        _playerLocation = spawnLocation;
        Vector3 cellPosition = grid.CellToWorld(_playerLocation);
        cellPosition.y += grid.cellSize.y / 2f;
        player = Instantiate(player, cellPosition, Quaternion.identity);

        north = player.transform.GetChild(0).gameObject;
        south = player.transform.GetChild(1).gameObject;
        west = player.transform.GetChild(2).gameObject;
        east = player.transform.GetChild(3).gameObject;
        changeDir();
    }

    private void changeDir()
    {
        switch (direction)
        {
            case Dir.north:
                north.SetActive(true);
                south.SetActive(false);
                west.SetActive(false);
                east.SetActive(false);
                break;

            case Dir.south:
                south.SetActive(true);
                north.SetActive(false);
                west.SetActive(false);
                east.SetActive(false);
                break;

            case Dir.west:
                west.SetActive(true);
                north.SetActive(false);
                south.SetActive(false);
                east.SetActive(false);
                break;

            case Dir.east:
                east.SetActive(true);
                north.SetActive(false);
                south.SetActive(false);
                west.SetActive(false);
                break;
        }
    }

    public void RunCode()
    {
        if (!running)
        {
            running = true;
            StartCoroutine(WaitWhileRunning());
        }
    }

    IEnumerator WaitWhileRunning()
    {
        for (int i = 0; i < functionGenerator.transform.childCount; i++)
        {
            string function = functionGenerator.transform.GetChild(i).GetChild(0).GetComponent<Text>().text;

            if (function == "Move();")
            {
                Move();
            }
            else if (function == "TurnLeft();")
            {
                TurnLeft();
            }
            else if (function == "TurnRight();")
            {
                TurnRight();
            }

            yield return new WaitForSeconds(delay);
        }

        Reset();

        running = false;
    }

    private void Reset()
    {
        Destroy(player);
        direction = startingDir;
        _playerLocation = spawnLocation;
        Vector3 cellPosition = grid.CellToWorld(_playerLocation);
        cellPosition.y += grid.cellSize.y / 2f;
        player = Instantiate(player, cellPosition, Quaternion.identity);

        north = player.transform.GetChild(0).gameObject;
        south = player.transform.GetChild(1).gameObject;
        west = player.transform.GetChild(2).gameObject;
        east = player.transform.GetChild(3).gameObject;
        changeDir();
    }

    #region Functions
    private void Move()
    {
        Vector3Int temp = _playerLocation;
        switch (direction)
        {
            case Dir.north:
                temp.y++;
                break;

            case Dir.south:
                temp.y--;
                break;

            case Dir.west:
                temp.x--;
                break;

            case Dir.east:
                temp.x++;
                break;
        }
        if (temp.x >= bottomLeft.x && temp.y >= bottomLeft.y && temp.x <= topRight.x && temp.y <= topRight.y)
        {
            _playerLocation = temp;
            Vector3 cellPosition = grid.CellToWorld(_playerLocation);
            cellPosition.y += grid.cellSize.y / 2f;
            player.transform.position = cellPosition;
        }
    }

    private void TurnLeft()
    {
        switch (direction)
        {
            case Dir.north:
                direction = Dir.west;
                break;

            case Dir.south:
                direction = Dir.east;
                break;

            case Dir.west:
                direction = Dir.south;
                break;

            case Dir.east:
                direction = Dir.north;
                break;
        }

        changeDir();
    }

    private void TurnRight()
    {
        switch (direction)
        {
            case Dir.north:
                direction = Dir.east;
                break;

            case Dir.south:
                direction = Dir.west;
                break;

            case Dir.west:
                direction = Dir.north;
                break;

            case Dir.east:
                direction = Dir.south;
                break;
        }

        changeDir();
    }
    #endregion
}
