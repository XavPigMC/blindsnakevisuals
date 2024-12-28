using System;
using UnityEngine;

public class SnakeCurveFill
{
    private Vector2Int snakePosition = new Vector2Int(1, 1);
    private readonly Texture2D texture;

    /// <summary>
    /// returns coordinates multiplied
    /// </summary>
    /// <returns></returns>
    public int snakeSurface()
    {
        return snakePosition.x * snakePosition.y;
    }

    public SnakeCurveFill(Texture2D texture)
    {
        this.texture = texture;
    }


    public void DO_IT()
    {
        //notes for future optimization -
        
        //fields that are used to traverse back from surface end points are not used, but ignored
        //using them can improve efficiency of future S
        
        
        
        
        uint currentPowerOfTwo = 0;

        //needs code here

        //assuming the snake is at the far right of current power 
        //START OF THIS CURVE
        currentPowerOfTwo++;
        while (snakePosition.x <= Math.Pow(2, currentPowerOfTwo))
        {
            RIGHT();
        }
        
        UP();

        while (snakePosition.x > Math.Pow(2, currentPowerOfTwo) / 3)
        {
            LEFT();
        }
        
        //keep in mind it still has to enter the central mass (its on the root of a branch right now)
        //right here, snake needs to fill central mass from left to up
        
        //assuming snake is at the highest point of central mass
        UP();
        LEFT();
        while (snakePosition.y <= Math.Pow(2, currentPowerOfTwo))
        {
            UP();
        }
        //now, the snake is at the highest point of the area under the curve (AND THIS CURVE IS FINISHED)
        //END OF THIS CURVE
        
        //assuming snake is at the far top of the previous curve
        //START OF THIS CURVE
        currentPowerOfTwo++;
        while (snakePosition.y <= Math.Pow(2, currentPowerOfTwo))
        {
            UP();
        }
        
        RIGHT();

        while (snakePosition.y > Math.Pow(2, currentPowerOfTwo) / 3)
        {
            DOWN();
        } 
        //keep in mind it still has to enter the central mass (its on the root of a branch right now)
        //it's time to fill the central mass again!
        
        //now, the snake is at the far right of the central mass
        DOWN();
        while (snakePosition.x <= Math.Pow(2, currentPowerOfTwo))
        {
            RIGHT();
        }
        
        //here, we repeat! right now the snake is on a far right of this surface, so it aligns with the assumption
        //of it being far right
        


    }

    #region moving

    public void UP()
    {
        DrawTail();
        snakePosition.y++;
        DrawHead();
    }

    public void DOWN()
    {
        DrawTail();
        snakePosition.y--;
        DrawHead();
    }

    public void LEFT()
    {
        DrawTail();
        snakePosition.x--;
        DrawHead();
    }

    public void RIGHT()
    {
        DrawTail();
        snakePosition.x++;
        DrawHead();
    }

    #endregion
    
    #region drawing

    /// <summary>
    /// counting from 1
    /// </summary>
    /// <param name="position"></param>
    public void DrawTail(Vector2Int position)
    {
        if ((position.x + position.y) % 2 == 0)
        {
            ColorUtility.TryParseHtmlString("#edde39", out Color c);
            texture.SetPixel(position.x - 1, position.y - 1, c);
        }
        else
        {
            ColorUtility.TryParseHtmlString("#a19510", out Color c);
            texture.SetPixel(position.x - 1, position.y - 1, c);
        }
    }

    private void DrawTail()
    {
        if ((snakePosition.x + snakePosition.y) % 2 == 0)
        {
            ColorUtility.TryParseHtmlString("#edde39", out Color c);
            texture.SetPixel(snakePosition.x - 1, snakePosition.y - 1, c);
        }
        else
        {
            ColorUtility.TryParseHtmlString("#a19510", out Color c);
            texture.SetPixel(snakePosition.x - 1, snakePosition.y - 1, c);
        }
    }


    /// <summary>
    /// counting from 1
    /// </summary>
    /// <param name="position"></param>
    public void DrawHead(Vector2Int position)
    {
        texture.SetPixel(position.x - 1, position.y - 1, Color.blue);
        texture.Apply();
    }

    private void DrawHead()
    {
        texture.SetPixel(snakePosition.x - 1, snakePosition.y - 1, Color.blue);
        texture.Apply();
    }

    #endregion
}
