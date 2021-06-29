using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SDAA{
    public class Utilis
    {
        private static int debugCount = 0;

        public static Vector2 MetricToWorldPoint(Pathfinder pathfinder, GridPosition gridPosition){
            float ss = pathfinder.SquareSize;
            Vector2 offset = new Vector2(-pathfinder.Width, pathfinder.Height)*ss*0.5f;
            float dx = gridPosition.col * ss ;
            float dy = -gridPosition.line * ss;
            return offset + (Vector2)pathfinder.transform.position + new Vector2(dx,dy); 

        }

        public static GridPosition WorldToMetricPoint(Pathfinder pathfinder, Vector2 worldPosition){
            float ss = pathfinder.SquareSize;
            Vector2 offset = new Vector2(-pathfinder.Width*0.5f,pathfinder.Height*0.5f)*ss;
            Vector2 leftCorner = (Vector2)pathfinder.transform.position + offset;
            int col = (int)((worldPosition - leftCorner).x/ss);
            int line = -(int)((worldPosition - leftCorner).y/ss);
            if(col<0){
                col = 0;
            }
            else if(col>= pathfinder.Width){
                col = pathfinder.Width;
            }

            if(line < 0){
                line = 0;
            }
            else if(line >= pathfinder.Height){
                line = pathfinder.Height;
            }

            return new GridPosition(line,col);
            
        }


        public static void Log(string s){
            Debug.Log(":: " + debugCount + " :: " + s);
            debugCount++;
        }

    }
}