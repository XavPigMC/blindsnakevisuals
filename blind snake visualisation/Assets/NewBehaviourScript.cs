using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public int quadRangeX = 5;
    public int quadRangeY = 5;
    Vector2 startPos = new Vector2(0, 0);

    public int gridWidth = 20; // Width of the grid
    public int gridHeight = 20; // Height of the grid
    public Vector2Int snakePosition = new Vector2Int(0, 0); // Snake's starting position
    public Vector2Int applePosition = new Vector2Int(5, 5); // Apple's starting position

    private Texture2D texture;
    private Material material;

    public double lenght_up_to_max = 0f;

    void Start()
    {
        max_moves = 35 * gridWidth * gridHeight;
        // Create a texture
        texture = new Texture2D(gridWidth, gridHeight);
        texture.filterMode = FilterMode.Point; // Keep the texture crisp
        UpdateTexture();

        // Create a material to display the texture
        material = new Material(Shader.Find("Unlit/Texture"));
        material.mainTexture = texture;

        // Scale the Quad to match the grid size
        Vector2 size = new Vector2(gridWidth, gridHeight);
        size.Normalize();
        for (int i = -quadRangeX; i < quadRangeX; i++)
        {
            for (int j = -quadRangeY; j < quadRangeY; j++)
            {
                // Create a Quad and assign the material

                GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                quad.transform.parent = transform;
                quad.transform.position = new Vector2(startPos.x + i * size.x, startPos.y + j * size.y);
                quad.GetComponent<Renderer>().material = material;
                quad.transform.localScale = new Vector3(size.x, size.y, 1);
            }
        }
    }

    public double counter = 0;
    public double diagonal_coef = Math.PI;
    public double total_move = 0;
    public double max_moves = Math.PI;

    public bool AutoMoveSnake = false;
    public float Timer = 0;
    public float speed = 5f;

    void Update()
    {
        if (AutoMoveSnake)
        {
            Timer += Time.deltaTime;
            while (Timer >= (1f / speed))
            {
                MoveSnake();
                Timer -= 1f / speed;
            }
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveSnake();
        }
    }

    private void MoveSnake()
    {
        if (total_move >= max_moves)
        {
            return;
        }
        total_move++;
        //actual place to put the move function here!
        
        
        
        CheckWrap();
    }

    
    /// <summary>
    /// DON'T USE THIS FOR STEP BY STEP, THIS JUST FILLS THE ENTIRE GRAPH FROM ONE CALL!!!!
    /// Fore step by step, use MoveSnakeGeometricSequence
    /// </summary>
    private void FillSnakeInGeometricSequence()
    {
        
    }
    
    /// <summary>
    /// moves the snake to fill y = S/x curves, where S goes in a geometric sequence 
    /// </summary>
    /// <param name="constant">the coefficient of the geometric sequence</param>
    private void MoveSnakeGeometricSequence()
    {
        //assuming starting position is 0,0
        //we assume 
        
        
        int currentPowerOf2 = 1;

    }

    private void UP()
    {
        SnakeTrail();
        snakePosition.y += 1;
        CheckWrap();
        SnakeHead();
    }
    
    
    /// <summary>
    /// moves the snake in a diagonal pattern 
    /// </summary>
    private void MoveSnakeDiagonally()
    {
        lenght_up_to_max = (int)((total_move / max_moves) * 100);

        SnakeTrail();

        if (counter > diagonal_coef)
        {
            snakePosition += new Vector2Int(0, 1);
            counter -= diagonal_coef;
        }
        else
        {
            snakePosition += new Vector2Int(1, 0);
            counter++;
        }
        SnakeHead();
    }

    private void CheckWrap()
    {
        snakePosition.x %= gridWidth;
        snakePosition.y %= gridHeight;
    }

    /// <summary>
    /// called after moving the snake (to recolor the head)
    /// also, applies the texture to the material.
    /// </summary>
    private void SnakeHead()
    {
        texture.SetPixel(snakePosition.x, snakePosition.y, Color.blue);
        texture.Apply();
    }


    /// <summary>
    /// called before moving the snake
    /// </summary>
    private void SnakeTrail()
    {
        if ((snakePosition.x + snakePosition.y) % 2 == 0)
        {
            ColorUtility.TryParseHtmlString("#edde39", out Color c);
            texture.SetPixel(snakePosition.x, snakePosition.y, c);
        }
        else
        {
            ColorUtility.TryParseHtmlString("#a19510", out Color c);
            texture.SetPixel(snakePosition.x, snakePosition.y, c);
        }
    }


    void UpdateTexture()
    {
        // Clear the texture with black pixels
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if ((x + y) % 2 == 0)
                {
                    ColorUtility.TryParseHtmlString("#525151", out Color color);
                    texture.SetPixel(x, y, color);
                }
                else
                {
                    ColorUtility.TryParseHtmlString("#3b3a3a", out Color color);
                    texture.SetPixel(x, y, color);
                }
            }
        }

        // Set snake's position to yellow
        texture.SetPixel(snakePosition.x, snakePosition.y, Color.yellow);

        // Set apple's position to red
        texture.SetPixel(applePosition.x, applePosition.y, Color.red);

        // Apply changes to the texture
        texture.Apply();
    }
}

public enum VisualisationMode
{
    Snake,
    LogarithmicCurveColoring
}