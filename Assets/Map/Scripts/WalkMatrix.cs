using System.Collections.Generic;
using UnityEngine;


public struct PairInt
{
    public int I { get; private set; }
    public int J { get; private set; }

    public PairInt(int i, int j)
    {
        I = i;
        J = j;
    }
}

public class WalkMatrix : MonoBehaviour
{
    public MapInfo MapInfoObj;
    //public TowerManager TowerManagerObj;
    public int[,] dp;
    private int[,] dpNext;
    private bool[,] towers;
    private int newX, newY;
    private bool canConfirm;

    void Start()
    {
        towers = new bool[MapInfoObj.SizeX, MapInfoObj.SizeY];
        buildCheckDP();
        dp = dpNext;
    }

    private void buildCheckDP()
    {
        dpNext = new int[MapInfoObj.SizeX, MapInfoObj.SizeY];

        var st = new Stack<PairInt>();
        var st2 = new Stack<PairInt>();
        Stack<PairInt> stTemp;

        st2.Push(new PairInt(MapInfoObj.EndX, MapInfoObj.EndY));

        var count = 1;
        while (st2.Count > 0)
        {
            stTemp = st2;
            st2 = st;
            st = stTemp;

            while (st.Count > 0)
            {
                int i = st.Peek().I;
                int j = st.Peek().J;

                if (dpNext[i, j] == 0)
                {
                    dpNext[i, j] = count;
                    if (i > 0 && dpNext[i - 1, j] == 0 && !towers[i - 1, j]) st2.Push(new PairInt(i - 1, j));
                    if (i < MapInfoObj.SizeX - 1 && dpNext[i + 1, j] == 0 && !towers[i + 1, j]) st2.Push(new PairInt(i + 1, j));
                    if (j > 0 && dpNext[i, j - 1] == 0 && !towers[i, j - 1]) st2.Push(new PairInt(i, j - 1));
                    if (j < MapInfoObj.SizeY - 1 && dpNext[i, j + 1] == 0 && !towers[i, j + 1]) st2.Push(new PairInt(i, j + 1));
                }
                st.Pop();
            }

            count++;
        }
    }

    public void ConfirmBuilding()
    {
        if (!canConfirm) return;
        dp = dpNext;
        towers[newX, newY] = true;
        canConfirm = false;
    }

    public bool CanBuild(int x, int y)
    {
        canConfirm = false;
        if ((x == MapInfoObj.StartX && y == MapInfoObj.StartY) || (x == MapInfoObj.EndX && y == MapInfoObj.EndY))
            return false;

        newX = x; newY = y;
        if (towers[newX, newY]) return false;
        towers[newX, newY] = true;
        buildCheckDP();
        towers[newX, newY] = false;
        if (dpNext[MapInfoObj.StartX, MapInfoObj.StartY] == 0) return false;
        canConfirm = true;
        return true;
    }

    public void Delete(int x, int y)
    {
        towers[x, y] = false;
        buildCheckDP();
        dp = dpNext;
    }
}

