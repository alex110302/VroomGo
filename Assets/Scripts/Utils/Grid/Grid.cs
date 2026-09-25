using System;
using Unity.VisualScripting;
using UnityEngine;

public class Grid<T>
{
    public GameObject GameObject { get; }

    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public int Width { get; }
    public int Height { get; }
    public float CellSize { get; }
    private T[,] gridArray;
    private Vector3 originPosition;
    private TextMesh[,] debugArray;
    private bool isGridPostionMoving;
    
    public bool DebugInfo { get; set; }

    public Grid(int width, int hight, float cellSize, Vector3 originPosition, Func<Grid<T>, int, int, T> OnCreateGridObj, GameObject gameObject, bool isGridMoving = false, bool debugInfo = false)
    {
        this.Width = width;
        this.Height = hight;
        this.CellSize = cellSize;
        this.originPosition = originPosition;
        this.isGridPostionMoving = isGridMoving;
        GameObject = gameObject;
        DebugInfo = debugInfo;

        gridArray = new T[width, hight];
        debugArray = new TextMesh[width, hight];

        for (int x = 0; x < gridArray.GetLength(0); x++)
            for (int y = 0; y < gridArray.GetLength(1); y++)
                gridArray[x, y] = OnCreateGridObj(this, x, y);

        if (debugInfo)
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    debugArray[x, y] = Utils.CrateWorldText(gridArray[x, y]?.ToString(), null, GetCenterOfCellPosition(x, y), 35, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }

            Debug.DrawLine(GetWorldPosition(0, hight), GetWorldPosition(width, hight), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, hight), Color.white, 100f);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
            {
                debugArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y].ToString();
            };
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * CellSize + (isGridPostionMoving ? GameObject.transform.position : originPosition);
    }

    /// <summary>
    /// Converts world space cords to grid space cords
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - (isGridPostionMoving ? GameObject.transform.position : originPosition)).x / CellSize);
        y = Mathf.FloorToInt((worldPosition - (isGridPostionMoving ? GameObject.transform.position : originPosition)).y / CellSize);
    }

    public Vector3 GetCenterOfCellPosition(int x, int y)
    {
        return GetWorldPosition(x, y) + (new Vector3(CellSize, CellSize) * .5f);
    }

    /// <summary>
    /// Sets object using grid space cords
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="value"></param>
    public void SetObject(int x, int y, T value)
    {
        if (x < 0 || y < 0 || x > Width - 1 || y > Height - 1) return;

        gridArray[x, y] = value;

        OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs { x = x, y = y });

        if (DebugInfo) debugArray[x, y].text = gridArray[x, y].ToString();
    }

    /// <summary>
    /// Sets an objects value using world space cords
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="value"></param>
    public void SetObject(Vector3 worldPosition, T value)
    {
        int x;
        int y;

        GetXY(worldPosition, out x, out y);
        
        SetObject(x, y, value);
    }

    public void TriggerObjectChanged(int x, int y)
    {
        OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs { x = x, y = y });
    }

    /// <summary>
    /// Gets an object using x y cords in grid space
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public T GetObject(int x, int y)
    {
        if (x < 0 || y < 0 || x > Width - 1 || y > Height - 1)
        {
            Debug.LogWarning($"Invalid values for X:{x} and Y:{y}");
            return default(T); //default returns the defult value of that type ex.. int = 0 and any custom tpye = null
        }
            
        return gridArray[x, y];
    }

    /// <summary>
    /// Gets grid object form a world space cords
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public T GetObject(Vector3 worldPosition)
    {
        int x;
        int y;

        GetXY(worldPosition, out x, out y);

        return GetObject(x, y);
    }
}