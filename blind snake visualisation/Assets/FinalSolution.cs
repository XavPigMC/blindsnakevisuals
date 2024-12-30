using System;
using UnityEngine;

public class FinalSolution
{
    private Vector2Int snakePosition = new Vector2Int(1, 1);

    
    //arbitrary method send signal given as an example 
    public bool SendSignal(string signal)
    {
        return false;
    }
    
    public bool UP()
    {
        snakePosition.y++;
        return SendSignal("UP");
    }

    public bool DOWN()
    {
        snakePosition.y--;
        return SendSignal("DOWN");
    }

    public bool RIGHT()
    {
        snakePosition.x++;
        return SendSignal("RIGHT");
    }

    public bool LEFT()
    {
        snakePosition.x--;
        return SendSignal("LEFT");
    }

    public bool START_SOLVING()
    {
        uint currentPowerOfTwo = 1;

        while (currentPowerOfTwo < 20)
        {
            if (DO_IT_SURFACE_OPTIMIZED(currentPowerOfTwo)) return true;
            currentPowerOfTwo++;
        }

        return false; //this shouldn't happen 
    }
    
    
    public bool DO_IT_SURFACE_OPTIMIZED(uint currentPowerOfTwo)
    {
        //NOTE : subregion division completely breaks for power < 3 

        if (currentPowerOfTwo ==
            1) //hard coding power 1 because the subregion division completely breaks for power < 3 
        {
            if (UP()) return true;
            if (DOWN()) return true;
            if (RIGHT()) return true;
            return false;
        }

        if (currentPowerOfTwo ==
            2) //hard coding power 2 because the subregion division completely breaks for power < 3 
        {
            if (RIGHT()) return true;
            if (RIGHT()) return true;
            if (UP()) return true;
            if (LEFT()) return true;
            if (LEFT()) return true;
            if (LEFT()) return true;
            if (UP()) return true;
            if (UP()) return true;
            return false;
        }



        //WORKS FOR INDEX 3 OR GREATER
        uint currentSurfaceArea = (uint)Math.Pow(2, currentPowerOfTwo);
        uint previousSurfaceArea = (uint)Math.Pow(2, currentPowerOfTwo - 1);
        uint maxDiagonalCoordinates = (uint)Math.Pow(2, (currentPowerOfTwo * 1f) / 2);



        //assuming snakePosition = currentPowerOfTwo % 2 == 1 ? new Vector2Int(1, (int)Math.Pow(2, currentPowerOfTwo-1)) : new Vector2Int((int)Math.Pow(2, currentPowerOfTwo-1), 1);
        if (currentPowerOfTwo % 2 == 0)
        {
            //current power of 2 is even, we go bottom to top, right to left

            //we assume were at the rightmost point of the previous surface

            #region right edge leg


            while (snakePosition.x + 1 <= currentSurfaceArea)
            {
                if (RIGHT())
                    return true;
            }

            if (UP())
                return true;


            while (snakePosition.x - 1 > currentSurfaceArea * 1f / 3)
            {
                if (LEFT())
                    return true;
            }



            #endregion

            #region S1


            //we're at the bottom                
            bool goingUp = true;
            while (snakePosition.x - 1 > maxDiagonalCoordinates && snakePosition.x - 1 > previousSurfaceArea / 2 + 1)
            {
                if (LEFT())
                    return true;
                if (goingUp)
                {
                    while ((snakePosition.y + 1) * snakePosition.x <= currentSurfaceArea)
                    {
                        if (UP())
                            return true;

                    }

                    if ((snakePosition.y + 1) * (snakePosition.x - 1) <=
                        currentSurfaceArea) //if the highest point of the next column is higher than we are right now
                        if (UP())
                            return true;
                    goingUp = false;
                }
                else
                {
                    while ((snakePosition.y - 1) * snakePosition.x > previousSurfaceArea)
                    {
                        if (DOWN())
                            return true;
                    }

                    goingUp = true;
                }
            }
            //now, we're next to the "missing" column

            if (goingUp) //means we've been going down last cycle, which means we're at y = 2
            {
                if (UP()) return true;
            }

            //y=3
            if (UP()) return true;
            //find a spot where we can resume up and down movement
            while (!((snakePosition.x - 1) * (snakePosition.y + 1) <= currentSurfaceArea))
            {
                if (LEFT()) return true;
            }


            //now, we've crossed the missing column and can comfortably resume going up and down 
            //we know that were at the bottom now
            goingUp = true;
            while (snakePosition.x - 1 > maxDiagonalCoordinates)
            {
                if (LEFT())
                    return true;
                if (goingUp)
                {
                    while ((snakePosition.y + 1) * snakePosition.x <= currentSurfaceArea)
                    {
                        if (UP())
                            return true;

                    }

                    if ((snakePosition.y + 1) * (snakePosition.x - 1) <=
                        currentSurfaceArea) //if the highest point of the next column is higher than we are right now
                        if (UP())
                            return true;
                    goingUp = false;
                }
                else
                {
                    while ((snakePosition.y - 1) * snakePosition.x > previousSurfaceArea && snakePosition.y - 1 > 3)
                    {
                        if (DOWN())
                            return true;
                    }

                    goingUp = true;
                }
            }

            #endregion

            #region S2

            //still not inside S2, now on the edge of S1
            while (snakePosition.x - 1 > previousSurfaceArea / maxDiagonalCoordinates)
            {
                if (LEFT())
                    return true;
                if (goingUp)
                {
                    while ((snakePosition.y + 1) <= maxDiagonalCoordinates)
                    {
                        if (UP())
                            return true;

                    }

                    goingUp = false;
                }
                else
                {
                    while ((snakePosition.y - 1) * snakePosition.x > previousSurfaceArea)
                    {
                        if (DOWN())
                            return true;
                    }

                    goingUp = true;
                }
            }

            //we want to end the covering of S2 on the up most row
            if (goingUp)
            {
                //if this is true, that means we were going down last cycle
                while ((snakePosition.y + 1) <=
                       maxDiagonalCoordinates) //this does repeat steps, but it probably does not loop more than 2 or 3 times
                {
                    if (UP())
                        return true;

                }
            }

            if ((snakePosition.y + 1) * (snakePosition.x - 1) > previousSurfaceArea &&
                snakePosition.x - 1 > 2) //gotta catch em all
                if (LEFT())
                    return true;

            #endregion

            #region S3


            //right now, the snake is below the left-bottom most point of S3
            bool goingRight = true;

            while (snakePosition.y + 1 <= currentSurfaceArea / 3)
            {
                if (UP())
                    return true;
                if (goingRight)
                {
                    while ((snakePosition.x + 1) * snakePosition.y <= currentSurfaceArea)
                        if (RIGHT())
                            return true;

                    goingRight = false;
                }
                else
                {
                    while ((snakePosition.x - 1) * snakePosition.y > previousSurfaceArea && snakePosition.x - 1 > 2)
                        if (LEFT())
                            return true;

                    if ((snakePosition.x - 1) * (snakePosition.y + 1) > previousSurfaceArea && snakePosition.x - 1 > 2)
                        if (LEFT())
                            return true;
                    goingRight = true;
                }
            }

            //Now we don't care weather we're on x=2 or x=3, we know we're on 3.

            #endregion

            #region upper edge leg


            while (snakePosition.y + 1 <= currentSurfaceArea / 2)
                if (UP())
                    return true;
            if (UP())
                return true;
            if (LEFT())
                return true;
            if (LEFT())
                return true;
            while (snakePosition.y + 1 <= currentSurfaceArea)
                if (UP())
                    return true;
            //now we're at the highest point of current surface ! Rough journey for our little guy, wasn't it?

            #endregion

        }
        else
        {


            #region upper edge leg

            //current power of 2 is odd, we go top to bottom, left to right
            while (snakePosition.x * (snakePosition.y + 1) <= currentSurfaceArea)
                if (UP())
                    return true;
            if (RIGHT())
                return true;
            while (snakePosition.y - 1 > currentSurfaceArea / 3)
                if (DOWN())
                    return true;

            #endregion

            #region SR3

            //now our snake is one spot higher than the left up most square of SR3


            bool goingRight = true;
            while (snakePosition.y - 1 > previousSurfaceArea / 2 + 1)
            {
                if (DOWN())
                    return true;
                if (goingRight)
                {
                    while ((snakePosition.x + 1) * snakePosition.y <= currentSurfaceArea)
                        if (RIGHT())
                            return true;
                    if ((snakePosition.x + 1) * (snakePosition.y - 1) <= currentSurfaceArea)
                        if (RIGHT())
                            return true;
                    goingRight = false;
                }
                else
                {
                    while ((snakePosition.x - 1) * snakePosition.y > previousSurfaceArea)
                        if (LEFT())
                            return true;
                    goingRight = true;
                }
            }

            //now our snake is next to the missing row
            if (goingRight &&
                (snakePosition.x + 1) * snakePosition.y <=
                currentSurfaceArea) //means it's been going left in the last cycle
            {
                //additional checks because without it, currentSurfaceArea=3 breaks 
                if (RIGHT()) return true; //for 3, surface are is too small so this steps out.
            }

            if (RIGHT()) return true;

            while (!((snakePosition.x + 1) * (snakePosition.y - 1) <= currentSurfaceArea))
            {
                DOWN();
            }

            goingRight = true;


            //now our snake has passed the missing row, and is ready to keep going left-right
            while (snakePosition.y - 1 > maxDiagonalCoordinates)
            {
                if (DOWN())
                    return true;
                if (goingRight)
                {
                    while ((snakePosition.x + 1) * snakePosition.y <= currentSurfaceArea)
                        if (RIGHT())
                            return true;
                    if ((snakePosition.x + 1) * (snakePosition.y - 1) <= currentSurfaceArea)
                        if (RIGHT())
                            return true;
                    goingRight = false;
                }
                else
                {
                    while ((snakePosition.x - 1) * snakePosition.y > previousSurfaceArea && snakePosition.x - 1 > 2)
                        if (LEFT())
                            return true;
                    goingRight = true;
                }
            }

            #endregion

            #region SR2

            //still not inside SR2

            while (snakePosition.y - 1 > previousSurfaceArea / maxDiagonalCoordinates)
            {
                if (DOWN())
                    return true;
                if (goingRight)
                {
                    while (snakePosition.x + 1 <= maxDiagonalCoordinates)
                        if (RIGHT())
                            return true;
                    goingRight = false;
                }
                else
                {
                    while ((snakePosition.x - 1) * snakePosition.y > previousSurfaceArea)
                        if (LEFT())
                            return true;
                    goingRight = true;
                }
            }

            //we want to end on the right-bottom most square of S2

            if (goingRight)
            {
                //this passes if we were going left last cycle, we have to go back right now
                while (snakePosition.x + 1 <= maxDiagonalCoordinates)
                    if (RIGHT())
                        return true;
            }

            if ((snakePosition.y - 1) * (snakePosition.x + 1) > previousSurfaceArea && snakePosition.y - 1 > 2)
                if (DOWN())
                    return true;



            #endregion

            #region SR1

            //currently sitting one square left of the bottom-leftmost most square of S1

            bool goingUp = true;

            while (snakePosition.x + 1 <= currentSurfaceArea / 3)
            {
                if (RIGHT())
                    return true;

                if (goingUp)
                {
                    while ((snakePosition.y + 1) * snakePosition.x <= currentSurfaceArea)
                        if (UP())
                            return true;

                    goingUp = false;
                }
                else
                {
                    while ((snakePosition.y - 1) * snakePosition.x > previousSurfaceArea && snakePosition.y - 1 > 2)
                        if (DOWN())
                            return true;

                    if ((snakePosition.x + 1) * (snakePosition.y - 1) > previousSurfaceArea && snakePosition.y - 1 > 2)
                        if (DOWN())
                            return true;

                    goingUp = true;
                }

            }

            //no more fixing the coordinate since we want to stay on y = 3

            #endregion

            #region right edge leg

            while (snakePosition.x + 1 <= currentSurfaceArea / 2)
                if (RIGHT())
                    return true;
            if (RIGHT())
                return true;
            if (DOWN())
                return true;
            if (DOWN())
                return true;
            while (snakePosition.x + 1 <= currentSurfaceArea)
                if (RIGHT())
                    return true;

            #endregion

        }

        return false;
    }
}