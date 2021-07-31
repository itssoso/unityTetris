using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHandler : MonoBehaviour
{

    public Vector3 rotationPoint;
    private static int height = 20;
    private static int width = 10;
    private float fallTime = 0.8f;
    private float moveTime = 0.25f;
    private float prevFallTime;
    private float prevMoveTime;
    private float prevRotateTime;
    private static Transform[,] grid = new Transform[height, width];

    void Start()
    {
        if (!ValidMove())
        {
            Destroy(transform.gameObject);
            FindObjectOfType<GameHandler>().ShowMenu(true);
        }
    }

    public static void ResetGrid() => grid = new Transform[height, width];

    void Update()
    {
        if (SimpleInput.GetAxis("Horizontal") != 0 && (Time.time - prevMoveTime) > moveTime)
        {
            if (SimpleInput.GetAxis("Horizontal") < 0) MoveBlock(Vector3.left);
            else if (SimpleInput.GetAxis("Horizontal") > 0) MoveBlock(Vector3.right);
            prevMoveTime = Time.time;
        }

        if (SimpleInput.GetAxis("Vertical") > 0 && (Time.time - prevRotateTime) > moveTime)
        {
            RotateBlock(Vector3.forward);
            prevRotateTime = Time.time;
        }

        fallTime = 1f / FindObjectOfType<GameHandler>().Level + .2f;
        Debug.Log(fallTime);
        if ((Time.time - prevFallTime) > (SimpleInput.GetAxis("Vertical") < 0 ? fallTime / 30 : fallTime))
        {
            transform.position += Vector3.down;
            if (!ValidMove())
            {
                transform.position -= Vector3.down;
                int[] linesToDelete = AddToGrid();
                if (linesToDelete.Length > 0)
                {
                    DeleteLines(linesToDelete);
                    FindObjectOfType<GameHandler>().IncreaseRows(linesToDelete.Length);
                }
                this.enabled = false;
                FindObjectOfType<Spawn>().NewTetromino();
            }
            prevFallTime = Time.time;
        }
    }

    void OnTransformChildrenChanged()
    {
        if (transform.childCount == 0) Destroy(transform.gameObject);
    }

    void RotateBlock(Vector3 direction)
    {
        transform.RotateAround(transform.TransformPoint(rotationPoint), direction, 90);
        if (!ValidMove()) transform.RotateAround(transform.TransformPoint(rotationPoint), direction, -90);
    }

    void MoveBlock(Vector3 direction)
    {
        transform.position += direction;
        if (!ValidMove()) transform.position -= direction;
    }

    int[] AddToGrid()
    {
        List<int> linesToDelete = new List<int>();
        foreach (Transform children in transform)
        {
            int x = Mathf.RoundToInt(children.transform.position.x);
            int y = Mathf.RoundToInt(children.transform.position.y);
            grid[y, x] = children;
            if (HasFullLine(y)) linesToDelete.Add(y);
        }
        linesToDelete.Sort((a, b) => b.CompareTo(a));
        return linesToDelete.ToArray();
    }

    bool HasFullLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[y, x] == null) return false;
        }
        return true;
    }

    void DeleteLines(int[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            int y = lines[i];
            for (int x = 0; x < width; x++)
            {
                Destroy(grid[y, x].gameObject);
                grid[y, x] = null;
            }
            DropLines(y);
        }
    }

    void DropLines(int i)
    {
        for (int y = i + 1; y < height; y++)
        {
            bool breaktime = true;
            for (int x = 0; x < width; x++)
            {
                if (grid[y, x] != null)
                {
                    breaktime = false;
                    grid[y - 1, x] = grid[y, x];
                    grid[y, x] = null;
                    grid[y - 1, x].transform.position += Vector3.down;
                }
            }
            if (breaktime) return;
        }
    }

    bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int x = Mathf.RoundToInt(children.transform.position.x);
            int y = Mathf.RoundToInt(children.transform.position.y);
            if (width <= x || x < 0 || height <= y || y < 0 || grid[y, x] != null)
            {
                return false;
            }
        }
        return true;
    }
}
