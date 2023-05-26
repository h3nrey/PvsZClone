using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class GridBuilder : MonoBehaviour
{
    [SerializeField] int cellsX, cellsY;
    [SerializeField] 
    [Range(1f, 5f)] float offsetX, offsetY;
    [SerializeField] GameObject cellPrefab;
    [SerializeField] Transform startPoint;
    [SerializeField] Transform cellsHolder;
    // Start is called before the first frame update
    void Start()
    {
        GenerateBuild();
    }

    [Button("Generate Grid")]
    void GenerateBuild() {
        Vector2 startPos = startPoint.position;
        for (int i = 0; i < cellsX; i++) {
            for (int j = 0; j < cellsY; j++) {
                Vector2 cellPos = new Vector2((startPos.x + (i * offsetX)), (startPos.y - (j * offsetY)));
                GameObject cell = Instantiate(cellPrefab, cellPos, Quaternion.identity, cellsHolder);
                cell.name = $"cell {i}x{j}";
            }
        }
    }
}
