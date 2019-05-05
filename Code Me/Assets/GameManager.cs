using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System;

public class GameManager : MonoBehaviour
{
    public enum Dir {north, south, east, west};

    #region Public Variables
    public GridLayout grid;
    public GameObject boat;
    public GameObject north;
    public GameObject south;
    public GameObject west;
    public GameObject east;
    public Dir direction;
    public Vector3Int spawnLocation;
    public Vector3Int endLocation;
    public GameObject end;
    public Vector2Int bottomLeft;
    public Vector2Int topRight;
    public List<Vector3Int> trashLocations;
    public GameObject trash;
    public GameObject functionGenerator;
    public float delay;
    public GameObject display;
    public string scene;
    #endregion

    #region Private Variables
    private Vector3Int _playerLocation;
    private GameObject particle;
    private Dir startingDir;
    private bool running = false;
    private GameObject player;
    public GameObject finish;
    private Animator playerAnim;
    private List<GameObject> trashList = new List<GameObject>();
    private bool isDisplayed = false;
    private int trashTotal;
    #endregion

    void Start()
    {
        startingDir = direction;
        _playerLocation = spawnLocation;
        Vector3 cellPosition = grid.CellToWorld(_playerLocation);
        cellPosition.y += grid.cellSize.y / 2f;
        player = Instantiate(boat, cellPosition, Quaternion.identity);
        particle = player.transform.GetChild(0).GetChild(4).gameObject;
        playerAnim = player.transform.GetChild(0).GetComponent<Animator>();

        north = player.transform.GetChild(0).GetChild(0).gameObject;
        south = player.transform.GetChild(0).GetChild(1).gameObject;
        west = player.transform.GetChild(0).GetChild(2).gameObject;
        east = player.transform.GetChild(0).GetChild(3).gameObject;
        changeDir();

        cellPosition = grid.CellToWorld(endLocation);
        cellPosition.y += grid.cellSize.y / 2f;
        finish = Instantiate(end, cellPosition, Quaternion.identity);

        for (int i = 0; i < trashLocations.Count; i++)
        {
            cellPosition = grid.CellToWorld(trashLocations[i]);
            cellPosition.y += grid.cellSize.y / 2f;
            trashList.Add(Instantiate(trash, cellPosition, Quaternion.identity));
        }
        trashTotal = trashLocations.Count;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Level1");
        }
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
        bool forlooped = false;
        for (int i = 0; i < functionGenerator.transform.childCount; i++)
        {
            string function = functionGenerator.transform.GetChild(i).GetChild(0).GetComponent<Text>().text;

            if (forlooped)
            {
                forlooped = false;
                continue;
            }

            if (function == "Move();")
            {
                playerAnim.Play("Move");
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
            else if (function == "Clean();")
            {
                Clean();
            }
            else
            {
                int val = Convert.ToInt32(Regex.Match(function, @"(?<=\().+?(?=\))").Value);
                Debug.Log(val);
                if (i + 1 < functionGenerator.transform.childCount)
                {
                    function = functionGenerator.transform.GetChild(i + 1).GetChild(0).GetComponent<Text>().text;
                    for (int j = 0; j < val; j++)
                    {
                        if (function == "Move();")
                        {
                            playerAnim.Play("Move");
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
                        else if (function == "Clean();")
                        {
                            Clean();
                        }
                        yield return new WaitForSeconds(delay);
                    }
                    forlooped = true;
                }
            }

            if (_playerLocation == endLocation)
            {
                if (trashTotal == 0)
                {
                    yield return new WaitForSeconds(delay);
                    particle.SetActive(true);
                    StartCoroutine(Wait(2f));
                    yield break;
                }

                yield return new WaitForSeconds(delay);

                Reset();
                running = false;
                yield break;
            }

            yield return new WaitForSeconds(delay);
        }

        Reset();

        running = false;
    }

    IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(scene);
    }

    private void Reset()
    {
        Destroy(player);
        direction = startingDir;
        _playerLocation = spawnLocation;
        Vector3 cellPosition = grid.CellToWorld(_playerLocation);
        cellPosition.y += grid.cellSize.y / 2f;
        player = Instantiate(boat, cellPosition, Quaternion.identity);
        particle = player.transform.GetChild(0).GetChild(4).gameObject;
        playerAnim = player.transform.GetChild(0).GetComponent<Animator>();

        north = player.transform.GetChild(0).GetChild(0).gameObject;
        south = player.transform.GetChild(0).GetChild(1).gameObject;
        west = player.transform.GetChild(0).GetChild(2).gameObject;
        east = player.transform.GetChild(0).GetChild(3).gameObject;
        changeDir();

        Destroy(finish);
        cellPosition = grid.CellToWorld(endLocation);
        cellPosition.y += grid.cellSize.y / 2f;
        finish = Instantiate(end, cellPosition, Quaternion.identity);

        foreach (GameObject obj in trashList)
        {
            Destroy(obj);
        }

        trashList = new List<GameObject>();

        for (int i = 0; i < trashLocations.Count; i++)
        {
            cellPosition = grid.CellToWorld(trashLocations[i]);
            cellPosition.y += grid.cellSize.y / 2f;
            trashList.Add(Instantiate(trash, cellPosition, Quaternion.identity));
        }
        trashTotal = trashLocations.Count;
    }

    public void Display()
    {
        if (isDisplayed)
        {
            display.SetActive(false);
            isDisplayed = false;
        }
        else
        {
            display.SetActive(true);
            isDisplayed = true;
        }
    }

    #region Functions
    IEnumerator MoveFromTo(Transform objectToMove, Vector3 a, Vector3 b, float speed)
    {
        float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            objectToMove.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        objectToMove.position = b;
        playerAnim.Play("Idle");
    }

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
            //player.transform.position = cellPosition;
            StartCoroutine(MoveFromTo(player.transform, player.transform.position, cellPosition, 2f));
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

    private void Clean()
    {
        switch (direction)
        {
            case Dir.north:
                for (int i = 0; i < trashLocations.Count; i++)
                {
                    Vector3Int temp = _playerLocation;
                    temp.y++;
                    if (temp == trashLocations[i] && trashList[i] != null)
                    {
                        StartCoroutine(Laser(trashList[i], player.transform.GetChild(0).GetChild(0).GetChild(0).gameObject));
                        trashTotal--;
                    }
                }
                break;

            case Dir.south:
                for (int i = 0; i < trashLocations.Count; i++)
                {
                    Vector3Int temp = _playerLocation;
                    temp.y--;
                    if (temp == trashLocations[i] && trashList[i] != null)
                    {

                        StartCoroutine(Laser(trashList[i], player.transform.GetChild(0).GetChild(1).GetChild(0).gameObject));
                        trashTotal--;
                    }
                }
                break;

            case Dir.west:
                for (int i = 0; i < trashLocations.Count; i++)
                {
                    Vector3Int temp = _playerLocation;
                    temp.x--;
                    if (temp == trashLocations[i] && trashList[i] != null)
                    {
                        StartCoroutine(Laser(trashList[i], player.transform.GetChild(0).GetChild(2).GetChild(0).gameObject));
                        trashTotal--;
                    }
                }
                break;

            case Dir.east:
                for (int i = 0; i < trashLocations.Count; i++)
                {
                    Vector3Int temp = _playerLocation;
                    temp.x++;
                    if (temp == trashLocations[i] && trashList[i] != null)
                    {
                        StartCoroutine(Laser(trashList[i], player.transform.GetChild(0).GetChild(3).GetChild(0).gameObject));
                        trashTotal--;
                    }
                }
                break;
        }

        changeDir();
    }

    IEnumerator Laser(GameObject trashObj, GameObject beam)
    {
        beam.SetActive(true);

        yield return new WaitForSeconds(.3f);

        beam.SetActive(false);
        Destroy(trashObj);
    }
    #endregion
}
