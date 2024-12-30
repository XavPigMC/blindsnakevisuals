using System;
using UnityEngine;

public class SnakeCurveFillSegmented : SnakeCurveFill
{
    //This is temporary class to refactor the old scheme of the pathing algorithm by copying, checking, and stuff. 
    //The old scheme algorithm is given in function DO_IT_OLD();
    //this inherits SnakeCurveFill because I'm lazy and I don't want to rewrite all the old functions and stuff 
    //time for some 2 monitor copy and paste action

    public bool DO_IT_NEW()
    {
        // format is (x,y)
        //assuming we start at 1,1

        uint currentPowerOfTwo = 1;
        while (currentPowerOfTwo <= 20)
        {
            if (DO_IT_SURFACE(currentPowerOfTwo))
            {
                return true;
            }
            currentPowerOfTwo++;
        }

        return false; //this shouldn't happen unless ive made a mistake
    }

    public bool DO_IT_SURFACE(uint currentPowerOfTwo)
    {
        //NOTE : subregion division completely breaks for power < 3 
        
        if (currentPowerOfTwo == 1) //hard coding power 1 because the subregion division completely breaks for power < 3 
        {
            if (UP()) return true;
            if (DOWN()) return true;
            if (RIGHT()) return true;
            return false;
        }
        
        if (currentPowerOfTwo == 2) //hard coding power 2 because the subregion division completely breaks for power < 3 
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
        Debug.Log("Index: " + currentPowerOfTwo + " current surface " + currentSurfaceArea + " and previous surface area " + previousSurfaceArea + " max diagonal " + maxDiagonalCoordinates);
            
        
        //assuming snakePosition = currentPowerOfTwo % 2 == 1 ? new Vector2Int(1, (int)Math.Pow(2, currentPowerOfTwo-1)) : new Vector2Int((int)Math.Pow(2, currentPowerOfTwo-1), 1);
        if (currentPowerOfTwo % 2 == 0)
        {
            Debug.Log("assuming starting on the right edge");
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


            while (snakePosition.x - 1 > currentSurfaceArea*1f / 3)
            {
                if (LEFT())
                    return true;
            }
                
                
                
            #endregion

            #region S1
            Debug.Log("STARTING SR 1");
            //we're at the bottom                
            bool goingUp = true;
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

                    if ((snakePosition.y + 1) * (snakePosition.x - 1) <= currentSurfaceArea) //if the highest point of the next column is higher than we are right now
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
            //now, we're at the leftmost column of S1 

            #endregion
                
            #region S2
            Debug.Log("STARTING SR 2");
            //still not inside S2, now on the edge of S1
            while (snakePosition.x - 1 > previousSurfaceArea/maxDiagonalCoordinates)
            {
                if (LEFT())
                    return true;
                if (goingUp)
                {
                    while ((snakePosition.y + 1)<= maxDiagonalCoordinates)
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
                while ((snakePosition.y + 1)<= maxDiagonalCoordinates) //this does repeat steps, but it probably does not loop more than 2 or 3 times
                {
                    if (UP())
                        return true;

                }
            }
            if((snakePosition.y + 1) * (snakePosition.x-1) > previousSurfaceArea) //gotta catch em all
                if (LEFT())
                    return true;

            #endregion

            #region S3
            Debug.Log("STARTING SR 3");
            //right now, the snake is below the left-bottom most point of S3
            bool goingRight = true;

            while (snakePosition.y + 1 <= currentSurfaceArea / 3)
            {
                if (UP())
                    return true;
                if (goingRight)
                {
                    while((snakePosition.x + 1)*snakePosition.y <= currentSurfaceArea)
                        if (RIGHT())
                            return true;
                        
                    goingRight = false;
                }
                else
                {
                    while((snakePosition.x - 1) * snakePosition.y > previousSurfaceArea)
                        if (LEFT())
                            return true;
                        
                    if((snakePosition.x-1)*(snakePosition.y+1) > previousSurfaceArea)
                        if (LEFT())
                            return true;
                    goingRight = true;
                }
            }
                
            //the current x coordinate is either 2 or 3, depending on if we were moving right or left last cycle
            //we want it to be 2, because the filling of upper edge leg starts from x = 2 going up, than later moving one step to the left and going straight up again up to the end.
            if(!goingRight) //this passes if we were going to the right last cycle (meaning were on x = 3 column)
                if (LEFT())
                    return true;

            #endregion
                
            #region upper edge leg
                
            while(snakePosition.y + 1 <= currentSurfaceArea/2)
                if (UP())
                    return true;
            if (LEFT())
                return true;
            while(snakePosition.y+1 <= currentSurfaceArea)
                if (UP())
                    return true;
            //now we're at the highest point of current surface ! Rough journey for our little guy, wasn't it?
            #endregion

        }
        else
        {
            Debug.Log("assuming starting on the up edge");
            #region upper edge leg
                
            //current power of 2 is odd, we go top to bottom, left to right
            while(snakePosition.x * (snakePosition.y+1) <= currentSurfaceArea)
                if (UP())
                    return true;
            if (RIGHT())
                return true;
            while(snakePosition.y - 1 > currentSurfaceArea / 3)
                if (DOWN())
                    return true;
                
            #endregion
                
            #region SR3
            //now our snake is one spot higher than the left up most square of SR3
            Debug.Log("STARTING SR 3");

            bool goingRight = true;
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
                    while ((snakePosition.x - 1) * snakePosition.y > previousSurfaceArea)
                        if (LEFT())
                            return true;
                    goingRight = true;
                }
            }
                
            #endregion
                
            #region SR2
            //still not inside SR2
            Debug.Log("Starting SR 2");
            while (snakePosition.y - 1 > previousSurfaceArea / maxDiagonalCoordinates)
            {
                if (DOWN())
                    return true;
                if (goingRight)
                {
                    while (snakePosition.x + 1<=maxDiagonalCoordinates)
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
                while(snakePosition.x + 1 <= maxDiagonalCoordinates)
                    if(RIGHT())
                        return true;
            }
            if((snakePosition.y - 1) * (snakePosition.x + 1) > previousSurfaceArea)
                if(DOWN())
                    return true;
                
                
                
            #endregion
                
            #region SR1
            //currently sitting one square left of the bottom-leftmost most square of S1
            Debug.Log("Starting SR 1");
            bool goingUp = true;

            while (snakePosition.x + 1 <= currentSurfaceArea / 3)
            {
                if(RIGHT())
                    return true;

                if (goingUp)
                {
                    while((snakePosition.y + 1)*snakePosition.x <= currentSurfaceArea)
                        if (UP())
                            return true;
                        
                    goingUp = false;
                }
                else
                {
                    while((snakePosition.y - 1)*snakePosition.x > previousSurfaceArea)
                        if (DOWN())
                            return true;
                        
                    if((snakePosition.x+1)*(snakePosition.y-1) > previousSurfaceArea)
                        if(DOWN())
                            return true;
                        
                    goingUp = true;
                }
                    
            }
                
            //the current y coordinate is either 2 or 3
            //we want it to be 2 because filling of right edge leg starts from 2, then at one point moves down, and continues going right at y=1
            if (!goingUp)
            {
                //this passes if we were going up last cycle (meaning we're at y=3 right now)
                if (DOWN())
                    return true;
            }
                
            #endregion
                
            #region right edge leg
                
            while(snakePosition.x + 1 <= currentSurfaceArea/2)
                if (RIGHT())
                    return true;
            if (DOWN())
                return true;
            while(snakePosition.x + 1 <= currentSurfaceArea)
                if (RIGHT())
                    return true;
                
            #endregion

        }

        return false;
    }



    #region old
        
    public SnakeCurveFillSegmented(Texture2D texture, Vector2Int applePosition) : base(texture, applePosition)
    {
    }
        
        
    //old. don't use this. it has some mistakes
    public void DO_IT_OLD()
    {
        //notes for future optimization -

        //fields that are used to traverse back from surface end points are not used, but ignored
        //using them can improve efficiency of future S
        //during the writing of this, moving methods did not return anything.
        //they just moved (I planned this refactoring so at the time, I did not bother with returns 



        uint currentPowerOfTwo = 0;

        //needs code here

        #region start right edge

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

        //keep in mind it still has to enter the
        //central mass (its on the root of a branch right now)
        //right here, snake needs to fill central mass from left to up

        //finding the borders to the regions 
        //uint maxDiagonalInRegion = (uint)Math.Sqrt(Math.Pow(2, currentPowerOfTwo));
        uint maxDiagonalInRegion = (uint)Math.Pow(2, currentPowerOfTwo * 1f / 2);

        #endregion

        #region SR1

        LEFT(); //to step into SR 1
        bool goingUp = true; //true if snake is going up, false if snake is going down
        while (snakePosition.x > maxDiagonalInRegion)
        {
            if (goingUp)
            {
                while (snakePosition.x * (snakePosition.y + 1) <= Math.Pow(2, currentPowerOfTwo))
                {
                    UP();
                }

                //if the highest point of the next column is higher than the snake, we need to get it before coming down
                if ((snakePosition.x - 1) * (snakePosition.y + 1) <= Math.Pow(2, currentPowerOfTwo))
                {
                    UP();
                }

                LEFT();
                goingUp = false;
                //now snake starts going down in the next iteration of while
            }
            else
            {
                while (snakePosition.x * (snakePosition.y - 1) > Math.Pow(2, currentPowerOfTwo - 1))
                {
                    DOWN();
                }

                LEFT();
                goingUp = true;
                //now snake starts going up!
            }

        }

        #endregion

        #region SR2

        //(already inside) 
        while (snakePosition.x > Math.Pow(2, currentPowerOfTwo - 1) / maxDiagonalInRegion)
        {
            if (goingUp)
            {
                while (snakePosition.x * (snakePosition.y + 1) <= maxDiagonalInRegion)
                {
                    UP();
                }

                LEFT();
                goingUp = false;
                //now snake starts going down in the next iteration of while
            }
            else
            {
                while (snakePosition.x * (snakePosition.y - 1) > Math.Pow(2, currentPowerOfTwo - 1))
                {
                    DOWN();
                }

                LEFT();
                goingUp = true;
                //now snake starts going up!
            }

        }

        if (goingUp) //this means our snake was down up until now!
        {
            //we want our snake to be at the up-left most point of SR2
            while (snakePosition.y + 1 <= maxDiagonalInRegion)
            {
                UP();
            }
        }

        if ((snakePosition.x - 1) * (snakePosition.y + 1) > Math.Pow(2, currentPowerOfTwo - 1))
        {
            LEFT();
        }

        UP();

        #endregion

        #region SR3

        //GREAT! (im severally burnt out)
        bool goingRight = true; //similar to goingUp, but for left-right movement!
        while (snakePosition.y <= Math.Pow(2, currentPowerOfTwo - 1))
        {
            if (goingRight)
            {
                while (snakePosition.y * (snakePosition.x + 1) <= Math.Pow(2, currentPowerOfTwo))
                {
                    RIGHT();
                }

                UP();
                goingRight = false;
            }
            else
            {
                while (snakePosition.y * (snakePosition.x - 1) > Math.Pow(2, currentPowerOfTwo - 1))
                {
                    LEFT();
                }

                if ((snakePosition.y + 1) * (snakePosition.x - 1) > Math.Pow(2, currentPowerOfTwo - 1))
                {
                    LEFT();
                }

                UP();
                goingRight = true;
            }
        }

        #endregion


        #region end up edge

        //assuming snake is one higher than the highest point of central mass
        LEFT();
        while (snakePosition.y <= Math.Pow(2, currentPowerOfTwo))
        {
            UP();
        }
        //now, the snake is at the highest point of the area under the curve (AND THIS CURVE IS FINISHED)
        //END OF THIS CURVE

        #endregion

        #region start up edge

        //assuming snake is at the far top of the previous curve
        //START OF THIS CURVE
        currentPowerOfTwo++;
        maxDiagonalInRegion = (uint)Math.Pow(2, currentPowerOfTwo * 1f / 2);
        while (snakePosition.y <= Math.Pow(2, currentPowerOfTwo))
        {
            UP();
        }

        RIGHT();

        while (snakePosition.y > Math.Pow(2, currentPowerOfTwo) / 3)
        {
            DOWN();
        }
        //keep in mind it still has to enter the central mass (its on the root of a branch up now)
        //it's time to fill the central mass again!

        #endregion

        #region SR3

        DOWN(); //to enter the region
        goingRight = true;

        while (snakePosition.y > maxDiagonalInRegion)
        {
            if (goingRight)
            {
                while (snakePosition.y * (snakePosition.x + 1) <= Math.Pow(2, currentPowerOfTwo))
                {
                    RIGHT();
                }

                if ((snakePosition.y - 1) * (snakePosition.x + 1) <= Math.Pow(2, currentPowerOfTwo))
                {
                    RIGHT();
                }

                DOWN();
                goingRight = false;
            }
            else
            {
                while (snakePosition.y * (snakePosition.x - 1) > Math.Pow(2, currentPowerOfTwo - 1))
                {
                    LEFT();
                }

                DOWN();
                goingRight = true;
            }
        }

        #endregion

        #region SR2

        //It's already inside SR2

        while (snakePosition.y > Math.Pow(2, currentPowerOfTwo - 1) / maxDiagonalInRegion)
        {
            if (goingRight)
            {
                while (snakePosition.x + 1 <= maxDiagonalInRegion)
                {
                    RIGHT();
                }

                DOWN();
                goingRight = false;
            }
            else
            {
                while (snakePosition.y * (snakePosition.x - 1) > Math.Pow(2, currentPowerOfTwo - 1))
                {
                    LEFT();
                }

                DOWN();
                goingRight = true;
            }
        }

        if (goingRight) //this means our snake was going left until now
        {
            //we want the snake to be at the down-right most square of S2
            while (snakePosition.x + 1 <= maxDiagonalInRegion)
            {
                RIGHT();
            }

        }

        if ((snakePosition.x + 1) * (snakePosition.y - 1) > Math.Pow(2, currentPowerOfTwo - 1))
        {
            DOWN();
        }

        RIGHT();

        #endregion

        #region SR1

        //YAY!
        goingUp = true;
        while (snakePosition.y <= Math.Pow(2, currentPowerOfTwo + 1) / 2)
        {
            if (goingUp)
            {
                while (snakePosition.x * (snakePosition.y + 1) <= Math.Pow(2, currentPowerOfTwo))
                {
                    UP();
                }

                RIGHT();
                goingUp = false;
            }
            else
            {
                while (snakePosition.x * (snakePosition.y - 1) > Math.Pow(2, currentPowerOfTwo - 1))
                {
                    DOWN();
                }

                if ((snakePosition.x + 1) - (snakePosition.y - 1) > Math.Pow(2, currentPowerOfTwo - 1))
                {
                    DOWN();
                }

                RIGHT();

                goingUp = true;
            }
        }


        #endregion

        #region end up edge

        //now, the snake is one right of the far right of the central mass
        DOWN();
        while (snakePosition.x <= Math.Pow(2, currentPowerOfTwo))
        {
            RIGHT();
        }

        #endregion MyRegion

        //here, we repeat! right now the snake is on a far right of this surface, so it aligns with the assumption
        //of it being far right



    }
        
    #endregion

}