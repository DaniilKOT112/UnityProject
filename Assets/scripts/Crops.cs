using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

public class Crops : MonoBehaviour
{
    public Tilemap groundTilemap; // Тайлмап для земли
    public Tilemap plantsTilemap; // Тайлмап для растений
    public TileBase tilledTile; // Спрайт для вспаханной земли

    private bool isNearPlayer; // Флаг, указывающий, что клетка рядом с игроком
    private bool isTilled; // Флаг, указывающий, что клетка вспахана
    private bool isPlanted; // Флаг, указывающий, что в клетке посажено растение
    private bool isFullyGrown; // Флаг, указывающий, что растение полностью выросло

    public float interactionRadius = 3f; // Радиус взаимодействия с объектами
    public LayerMask interactableLayer; // Слой, содержащий объекты, с которыми можно взаимодействовать

    public List<PlantProperties> plantList = new List<PlantProperties>();


    void Start()
    {
        plantList.Add(new PlantProperties(0, "Гриб", Resources.Load<TileBase>("Sprites/Grass2"), Resources.Load<TileBase>("Sprites/Grass1"), 3f, 25));
    }
    void Update()
    {
        // Текущая позиция мыши в мировых координатах
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int mouseCellPos = groundTilemap.WorldToCell(mousePos);

        // Текущая позиция игрока в мировых координатах
        var playerWorldPos = this.transform.position;
        Vector3Int playerCellPos = groundTilemap.WorldToCell(playerWorldPos);

        // Проверяем расстояние от клетки до игрока
        isNearPlayer = Vector3Int.Distance(mouseCellPos, playerCellPos) <= interactionRadius;

        // Проверяем, есть ли объекты с коллайдерами в радиусе взаимодействия с игроком
        Collider2D[] colliders = Physics2D.OverlapCircleAll(playerWorldPos, interactionRadius, interactableLayer);

        bool hasCollidersNearby = colliders.Length > 0;

        // Обработка нажатия клавиши (например, клавиши мыши)
        if (Input.GetMouseButtonDown(0) && !hasCollidersNearby) // Добавляем условие, что нельзя копать грядки, если есть объекты с коллайдерами рядом
        {
            // Получаем состояние клетки, на которую нажал игрок
            GetCellState(mouseCellPos);

            // Если клетка рядом с игроком и она не вспахана
            if (isNearPlayer && !isTilled)
            {
                groundTilemap.SetTile(mouseCellPos, tilledTile); // Вспахиваем землю
                isTilled = true; // Отмечаем клетку как вспаханную
            }
            // Если клетка рядом с игроком, вспахана и в ней нет посаженного растения
            else if (isNearPlayer && isTilled && !isPlanted)
            {
                plantsTilemap.SetTile(mouseCellPos, plantList[0].plantGrowingTile); // Сажаем растение
                isPlanted = true; // Отмечаем, что в клетке посажено растение
                StartCoroutine(GrowPlant(mouseCellPos, 0)); // Запускаем таймер роста растения
                isFullyGrown = true; // Разрешаем сбор растения
            }
            // Если клетка рядом с игроком, вспахана, растение выросло и его можно собрать
            else if (isNearPlayer && isTilled && isFullyGrown)
            {
                // Собираем растение
                plantsTilemap.SetTile(mouseCellPos, null);
                isPlanted = false; // Сбрасываем состояние посаженного растения
                isFullyGrown = false; // Сбрасываем состояние роста растения
            }
        }
    }

    IEnumerator GrowPlant(Vector3Int position, int plantIndex)
    {
        yield return new WaitForSeconds(plantList[plantIndex].growthTime);
        plantsTilemap.SetTile(position, plantList[plantIndex].fullyGrownPlantTile); // Изменяем спрайт на выросшее растение
    }

    // Получение состояния клетки
    void GetCellState(Vector3Int position)
    {
        isTilled = groundTilemap.GetTile(position) == tilledTile; //вспахан
        isPlanted = plantsTilemap.GetTile(position) != null; //посажен
        isFullyGrown = plantsTilemap.GetTile(position) == plantList[0].fullyGrownPlantTile; //выращен
    }
}
