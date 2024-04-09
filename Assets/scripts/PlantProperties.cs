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
    /// <param name="index">Переданный индекс.</param>
    /// <param name="growingTile">Переданный спрайт посаженного растения.</param>
    /// <param name="fullyGrownTile">Переданный спрайт выращенного растения.</param>
    /// <param name="growthTime">Переданное время выращивания.</param>
    /// <param name="price">Переданная цена.</param>
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
