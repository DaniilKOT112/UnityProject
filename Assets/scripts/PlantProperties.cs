using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]


public class PlantProperties 
{
    public int plantIndex;
    public string name;
    public TileBase plantGrowingTile;
    public TileBase fullyGrownPlantTile;
    public float growthTime;
    public int price;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="index">���������� ������.</param>
    /// <param name="growingTile">���������� ������ ����������� ��������.</param>
    /// <param name="fullyGrownTile">���������� ������ ����������� ��������.</param>
    /// <param name="growthTime">���������� ����� �����������.</param>
    /// <param name="price">���������� ����.</param>
    public PlantProperties(int index, string name, TileBase growingTile, TileBase fullyGrownTile, float growthTime, int price)
    {
        this.plantIndex = index;
        this.name = name;
        this.plantGrowingTile = growingTile;
        this.fullyGrownPlantTile = fullyGrownTile;
        this.growthTime = growthTime;
        this.price = price;
    }
}
