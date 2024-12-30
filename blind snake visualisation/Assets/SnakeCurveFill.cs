using UnityEngine;

public class SnakeCurveFill
{
    public Vector2Int snakePosition = new Vector2Int(1, 1);
    private readonly Vector2Int applePosition;
    private readonly Texture2D texture;


    protected SnakeCurveFill(Texture2D texture, Vector2Int applePosition)
    {
        this.texture = texture;
        this.applePosition = applePosition;
    }


    #region moving

    protected bool UP()
    {
        DrawTail();
        snakePosition.y++;
        DrawHead();
        Debug.Log(snakePosition);
        
        
        return snakePosition == applePosition;
    }

    protected bool DOWN()
    {
        DrawTail();
        snakePosition.y--;
        DrawHead();
        
        Debug.Log(snakePosition);
        
        return snakePosition == applePosition;
    }

    protected bool LEFT()
    {
        DrawTail();
        snakePosition.x--;
        DrawHead();
        
        Debug.Log(snakePosition);
        
        
        return snakePosition == applePosition;
    }

    protected bool RIGHT()
    {
        DrawTail();
        snakePosition.x++;
        DrawHead();
        
        Debug.Log(snakePosition);
        
        
        return snakePosition == applePosition;
    }

    #endregion
    
    #region drawing

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


    private void DrawHead()
    {
        texture.SetPixel(snakePosition.x - 1, snakePosition.y - 1, Color.black);
        texture.Apply();
    }

    #endregion
}
