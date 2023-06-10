using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CellArray2D
{
    public Cell[] cells;
    public int width;
    public int height;

    public CellArray2D(int width, int height)
    {
        this.width = width;
        this.height = height;
        cells = new Cell[width * height];
    }

    public Cell this[int x, int y]
    {
        get { return cells[x + y * width]; }
        set { cells[x + y * width] = value; }
    }
}

