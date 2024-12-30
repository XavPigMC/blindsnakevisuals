using System;
using UnityEngine;

public class CurveVisualisation : MonoBehaviour
{
    public int gridSize = 400;  // Width of the grid
    public float someConstant = 2;

    public uint maxS = 1;
    
    private Texture2D _texture;
    private Material _material;
    private readonly Vector3 _quadSize = new Vector3(10, 10, 0);
    public bool showDiagonal;

    public uint currentPowerOfTwo = 2;
    private SnakeCurveFillSegmentedOptimized curveFillSegmented;

    // Start is called before the first frame update
    void Start()
    {
        
        _texture = new Texture2D(gridSize, gridSize)
        {
            filterMode = FilterMode.Point
        };
        _material = new Material(Shader.Find("Unlit/Texture"))
        {
            mainTexture = _texture
        };


        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.transform.parent = transform;
        quad.GetComponent<Renderer>().material = _material;
        quad.transform.localScale = _quadSize;
        curveFillSegmented = new SnakeCurveFillSegmentedOptimized(_texture, new Vector2Int(-1, -1)); //-1 because I don't want the program to stop.
        BaseTexture();
    }

    public void FillSegment()
    {
        curveFillSegmented.snakePosition = currentPowerOfTwo % 2 == 1 ? new Vector2Int(1, (int)Math.Pow(2, currentPowerOfTwo-1)) : new Vector2Int((int)Math.Pow(2, currentPowerOfTwo-1), 1);
        Debug.Log("starting at : " + curveFillSegmented.snakePosition);
        curveFillSegmented.DO_IT_SURFACE_OPTIMIZED(currentPowerOfTwo);
        Debug.Log("ending at : " + curveFillSegmented.snakePosition);
        
    }
    
    public void BaseTexture()
    {
        for (int i = 0; i < _texture.width; i++)
        {
            for (int j = 0; j < _texture.height; j++)
            {
                _texture.SetPixel(i, j, (i + j) % 2 == 0 ? Color.gray : Color.white);
            }
        }

        ColorForSurface_geometricSurface(2);
        if (showDiagonal)
        {
            for (int i = 0; i < _texture.width; i++)
            {
                _texture.SetPixel(i, i, Color.red);
            }
        }

        _texture.Apply();
    }

    #region various coloring patterns
    // ReSharper disable once UnusedMember.Local
    private void ColorForSurface_geometricTwoThenFive()
    {
        uint s = 2;
        int step = 2; //2 just to keep it in sync with others
        int coefficient = 5;
        while (s <= maxS)
        {
            step++;
            Color color = new Color(step%2==0? .8f : 0, 0, step%2==1 ? .8f : 0);
            
            for (int i = 1; i <= s; i++)
            {
                for (int j = 1; j <= s/i; j++)
                {
                    if (i * j <= s && i * j > (s *1f)/(coefficient == 2 ? 5 : 2))
                    {
                        _texture.SetPixel(i - 1, j - 1, color);
                    }
                }
            }
            
            s = (uint) (s * coefficient);
            coefficient = coefficient == 2 ? 5 : 2;
        }
    }
    
    private void ColorForSurface_geometricSurface(float coefficient)
    {
        uint s = 1;
        int step = 2; //2 just to keep it in sync with others
        while (s <= maxS)
        {
            step++;
            Color color = new Color(step%3==0? .8f : 0, step%3==1? 0 : .8f, step%3==2? 0 : .8f);
            
            for (int i = 1; i <= s; i++)
            {
                for (int j = 1; j <= s/i; j++)
                {
                    if (i * j <= s && i * j > (s *1f)/coefficient)
                    {
                        _texture.SetPixel(i - 1, j - 1, color);
                    }
                }
            }

            s = (uint) (s * coefficient);
        }
    }


    // ReSharper disable once UnusedMember.Local
    private void ColorForSurface_cubeIntegers()
    {
        for (uint step = 2; step <= Math.Pow(maxS, 1f/3); step++)
        {
            Color color = new Color(step%3==0? .8f : 0, step%3==1? 0 : .8f, step%3==2? 0 : .8f);
            
            uint s = step * step * step;
            uint previousS = (uint)Math.Pow(step - 1, 3);
            for (int i = 1; i <= s; i++)
            {
                for (int j = 1; j <= s/i; j++)
                {
                    if (i * j <= s && i * j > previousS)
                    {
                        _texture.SetPixel(i - 1, j - 1, color);
                    }
                }
            }
        }
    }
    
    // ReSharper disable once UnusedMember.Local
    private void ColorForSurface_constantIncrement(uint increment)
    {
        for (uint step = 1; step <= maxS; step++)
        {
            Color color = new Color(step%3==0? .8f : 0, step%3==1? 0 : .8f, step%3==2? 0 : .8f);
            
            
            uint s = step * increment;
            for (int i = 1; i <= s; i++)
            {
                for (int j = 1; j <= s / i; j++)
                {
                    if (i * j <= s && i * j > (step - 1) * increment)
                    {
                        _texture.SetPixel(i - 1, j - 1, color);
                    }
                }
            }
        }
    }

    // ReSharper disable once UnusedMember.Local
    private void ColorForSurface_squareIntegers()
    {
        for (uint sRoot = 2; sRoot <= Math.Sqrt(maxS); sRoot++)
        {
            Color color = new Color(sRoot%3==0? .8f : 0, sRoot%3==1? 0 : .8f, sRoot%3==2? 0 : .8f);
            
            
            uint s = sRoot  * sRoot;
            for (int i = 1; i <= s; i++)
            {
                for (int j = 1; j <= s; j++)
                {
                    
                    if (i * j <= s && i * j > (sRoot - 1) * (sRoot - 1))
                    {
                        _texture.SetPixel(i - 1, j - 1, color);
                    }
                }
            }
        }
    }
    #endregion
    
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Vector3 start = transform.position - _quadSize / 2;

        Vector3 incrementX = new Vector3(_quadSize.x / gridSize, 0, 0);
        Vector3 incrementY = new Vector3(0, _quadSize.y / gridSize, 0);
        
        for (int i = 0; i < gridSize; i++)
        {
            Gizmos.DrawLine(start + incrementX * i, start + new Vector3(0, _quadSize.y,0) + incrementX * i);
            Gizmos.DrawLine(start+incrementY * i, start + new Vector3(_quadSize.x, 0,0) + incrementY * i);
        }
    }

}
