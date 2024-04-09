using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

public class Crops : MonoBehaviour
{
    public Tilemap groundTilemap; // ������� ��� �����
    public Tilemap plantsTilemap; // ������� ��� ��������
    public TileBase tilledTile; // ������ ��� ���������� �����

    private bool isNearPlayer; // ����, �����������, ��� ������ ����� � �������
    private bool isTilled; // ����, �����������, ��� ������ ��������
    private bool isPlanted; // ����, �����������, ��� � ������ �������� ��������
    private bool isFullyGrown; // ����, �����������, ��� �������� ��������� �������

    public float interactionRadius = 3f; // ������ �������������� � ���������
    public LayerMask interactableLayer; // ����, ���������� �������, � �������� ����� �����������������

    public List<PlantProperties> plantList = new List<PlantProperties>();


    void Start()
    {
        plantList.Add(new PlantProperties(0, "����", Resources.Load<TileBase>("Sprites/Grass2"), Resources.Load<TileBase>("Sprites/Grass1"), 3f, 25));
    }
    void Update()
    {
        // ������� ������� ���� � ������� �����������
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int mouseCellPos = groundTilemap.WorldToCell(mousePos);

        // ������� ������� ������ � ������� �����������
        var playerWorldPos = this.transform.position;
        Vector3Int playerCellPos = groundTilemap.WorldToCell(playerWorldPos);

        // ��������� ���������� �� ������ �� ������
        isNearPlayer = Vector3Int.Distance(mouseCellPos, playerCellPos) <= interactionRadius;

        // ���������, ���� �� ������� � ������������ � ������� �������������� � �������
        Collider2D[] colliders = Physics2D.OverlapCircleAll(playerWorldPos, interactionRadius, interactableLayer);

        bool hasCollidersNearby = colliders.Length > 0;

        // ��������� ������� ������� (��������, ������� ����)
        if (Input.GetMouseButtonDown(0) && !hasCollidersNearby) // ��������� �������, ��� ������ ������ ������, ���� ���� ������� � ������������ �����
        {
            // �������� ��������� ������, �� ������� ����� �����
            GetCellState(mouseCellPos);

            // ���� ������ ����� � ������� � ��� �� ��������
            if (isNearPlayer && !isTilled)
            {
                groundTilemap.SetTile(mouseCellPos, tilledTile); // ���������� �����
                isTilled = true; // �������� ������ ��� ����������
            }
            // ���� ������ ����� � �������, �������� � � ��� ��� ����������� ��������
            else if (isNearPlayer && isTilled && !isPlanted)
            {
                plantsTilemap.SetTile(mouseCellPos, plantList[0].plantGrowingTile); // ������ ��������
                isPlanted = true; // ��������, ��� � ������ �������� ��������
                StartCoroutine(GrowPlant(mouseCellPos, 0)); // ��������� ������ ����� ��������
                isFullyGrown = true; // ��������� ���� ��������
            }
            // ���� ������ ����� � �������, ��������, �������� ������� � ��� ����� �������
            else if (isNearPlayer && isTilled && isFullyGrown)
            {
                // �������� ��������
                plantsTilemap.SetTile(mouseCellPos, null);
                isPlanted = false; // ���������� ��������� ����������� ��������
                isFullyGrown = false; // ���������� ��������� ����� ��������
            }
        }
    }

    IEnumerator GrowPlant(Vector3Int position, int plantIndex)
    {
        yield return new WaitForSeconds(plantList[plantIndex].growthTime);
        plantsTilemap.SetTile(position, plantList[plantIndex].fullyGrownPlantTile); // �������� ������ �� �������� ��������
    }

    // ��������� ��������� ������
    void GetCellState(Vector3Int position)
    {
        isTilled = groundTilemap.GetTile(position) == tilledTile; //�������
        isPlanted = plantsTilemap.GetTile(position) != null; //�������
        isFullyGrown = plantsTilemap.GetTile(position) == plantList[0].fullyGrownPlantTile; //�������
    }
}
