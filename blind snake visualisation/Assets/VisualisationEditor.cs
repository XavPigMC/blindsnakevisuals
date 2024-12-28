using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VisualisationEditor : EditorWindow
{
    public int quadRangeX = 5;
    public int quadRangeY = 5;
    Vector2 startPos = new Vector2(0, 0);
    
    public int gridWidth = 20;  // Width of the grid
    public int gridHeight = 20; // Height of the grid
    public Vector2Int snakePosition = new Vector2Int(0, 0); // Snake's starting position
    public Vector2Int applePosition = new Vector2Int(5, 5); // Apple's starting position

    private Texture2D texture;
    private Material material;
    
    
}
