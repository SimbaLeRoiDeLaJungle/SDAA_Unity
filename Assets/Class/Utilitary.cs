using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SDAA{
    public static class Utilitary
    {
        private static int debugCount = 0;

        private static Metric metric;
        
        public static Vector2 MetricToWorldPoint(GridPosition gridPosition,Metric m){
            float ss = m.SquareSize;
            Vector2 offset = new Vector2(-m.Width, m.Height)*ss*0.5f;
            float dx = gridPosition.col * ss ;
            float dy = -gridPosition.line * ss;
            return offset + (Vector2)m.transform.position + new Vector2(dx,dy); 

        }
        public static Vector2 MetricToWorldPoint(GridPositionFloat gridPosition){
            float ss = metric.SquareSize;
            Vector2 offset = new Vector2(-metric.Width, metric.Height)*ss*0.5f;
            float dx = gridPosition.col * ss ;
            float dy = -gridPosition.line * ss;
            return offset + (Vector2)metric.transform.position + new Vector2(dx,dy); 

        }

        public static GridPositionFloat WorldToMetricPoint(Vector2 worldPosition){
            float ss = metric.SquareSize;
            Vector2 offset = new Vector2(-metric.Width*0.5f,metric.Height*0.5f)*ss;
            Vector2 leftCorner = (Vector2)metric.transform.position + offset;
            float col = ((worldPosition - leftCorner).x/ss);
            float line = -((worldPosition - leftCorner).y/ss);
            if(col<0){
                col = 0;
            }
            else if(col>= metric.Width){
                col = metric.Width;
            }

            if(line < 0){
                line = 0;
            }
            else if(line >= metric.Height){
                line = metric.Height;
            }

            return new GridPositionFloat(line,col);
            
        }
        public static GridPosition WorldToMetricPoint(Vector2 worldPosition,ref GridPositionFloat internPos){
            float ss = metric.SquareSize;
            Vector2 offset = new Vector2(-metric.Width*0.5f,metric.Height*0.5f)*ss;
            Vector2 leftCorner = (Vector2)metric.transform.position + offset;
            int col = (int)((worldPosition - leftCorner).x/ss);
            int line = -(int)((worldPosition - leftCorner).y/ss);
            if(col<0){
                col = 0;
            }
            else if(col>= metric.Width){
                col = metric.Width;
            }

            if(line < 0){
                line = 0;
            }
            else if(line >= metric.Height){
                line = metric.Height;
            }
            internPos.col = (worldPosition - leftCorner).x/ss - col;
            internPos.line = (leftCorner - worldPosition).y/ss-line;
            return new GridPosition(line,col);
            
        }

        public static void Log(string s){
            Debug.Log(":: " + debugCount + " :: " + s);
            debugCount++;
        }

        public static GridPositionFloat DirectionToGridPositionFloat(Direction dir){
            if(dir == Direction.RIGHT){
                return new GridPositionFloat(0,1f);
            }
            else if(dir == Direction.LEFT){
                return new GridPositionFloat(0,-1f);
            }
            else if(dir == Direction.DOWN){
                return new GridPositionFloat(1f,0f);
            }
            else if(dir == Direction.UP){
                return new GridPositionFloat(-1f,0f);
            }
            return new GridPositionFloat(0f,0f);
        }

        public static void SetMetric(Metric m){
            metric = m;
        }

        public static void SetDirectionByDest(GridPositionFloat gridPosition, GridPosition dest, ref Direction direction){
            GridPositionFloat dr = new GridPositionFloat(dest.line-gridPosition.line,dest.col - gridPosition.col);
            Debug.Log("dr : " + dr);
            Debug.Log("dest : " + dest);
            if(dr.col == 0){
                if(dr.line>0){
                    direction = Direction.DOWN;
                }
                else if(dr.line<0){
                    direction = Direction.UP;
                }
            }
            else if(dr.line == 0){
                if(dr.col > 0){
                    direction = Direction.RIGHT;
                }
                else if(dr.col<0){
                    direction = Direction.LEFT;
                }
            }
            
        }

        public static float DistanceInMetric(GridPositionFloat gp1, GridPositionFloat gp2){
            float x = Mathf.Abs(gp1.col - gp2.col);
            float y = Mathf.Abs(gp1.line - gp2.line);
            return Mathf.Sqrt(x*x + y*y);
        }
    }
}