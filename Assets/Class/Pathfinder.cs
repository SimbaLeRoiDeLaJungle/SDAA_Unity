using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace SDAA{
    /// <summary>
    /// The main Class, use for calculate paths.
    /// </summary>
    public static class Pathfinder
    {
        public static Metric metric{get;set;}
        private static int pathCount; // just for debug
        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        public static Path Pathfinding(GridPosition start,GridPosition dest){
            DateTime time = DateTime.Now;
            TimeSpan interval;
            /*----------------- initializer ----------------------*/
            int height = metric.Height;
            int width = metric.Width;
            float squareSize = metric.SquareSize;

            Node[,] nodes = new Node[height+1,width+1];
            bool[,] inOpen = new bool[height+1,width+1];
            bool[,] inClose = new bool[height+1,width+1];
            for(int line = 0; line <= height ; line++){
                for(int col = 0; col <= width ; col++){
                    nodes[line,col] = new Node(line,col);
                    inOpen[line,col] = false;
                    inClose[line,col] = false;
                }
            }
            AStarsPriorityQueue openList = new AStarsPriorityQueue(width*height);
            nodes[start.line,start.col].g = 0;
            nodes[start.line,start.col].h = CalculH(nodes[start.line,start.col],dest);
            nodes[start.line,start.col].f = nodes[start.line,start.col].h;
            if(dest.col == start.col && start.line == dest.line){
                interval = DateTime.Now - time;
                Utilitary.Log("----Pathfinding Duration----\nTime : " + interval.TotalSeconds + "\n------------------------------\n");
                return new Path();
            }

            /* ------------------ loop ------------------------*/    
            int h = nodes[start.line,start.col].h;
            GridPosition currentPos = start;
            while(h!= 0){

                List<GridPosition> nearNodesPos = metric.GetNearNodes(currentPos);
                // calculation of g,h & f cost  
                for(int i = 0 ; i< nearNodesPos.Count ; i++){
                    int line = nearNodesPos[i].line;
                    int col = nearNodesPos[i].col;
                    if(!inOpen[line,col] && !inClose[line,col] && metric.Walkables(line,col)){
                        // some calcul and Enqueue in the open set
                        nodes[line,col].parentLine = nodes[currentPos.line,currentPos.col].line;
                        nodes[line,col].parentCol = nodes[currentPos.line,currentPos.col].col;
                        int parentG = nodes[nodes[line,col].parentLine,nodes[line,col].parentCol].g;
                        nodes[line,col].g = CalculG(nodes[line,col],parentG);
                        nodes[line,col].h = CalculH(nodes[line,col], dest);
                        nodes[line,col].f = nodes[line,col].g + nodes[line,col].h;
                        inOpen[line,col] = true;
                        openList.Enqueue(ref nodes[line,col]);
                    }
                    else if(inOpen[line,col])
                    {   
                        // some calcul and Update the open set (is that realy usefull ?)
                        int parentLine = nodes[currentPos.line,currentPos.col].parentLine;
                        int parentCol = nodes[currentPos.line,currentPos.col].parentCol;
                        if((parentCol != -1) && (parentLine != -1)){
                            Node parent = nodes[parentLine,parentCol];
                            int oldParentLine = nodes[parentLine,parentCol].parentLine;
                            int oldParentCol = nodes[parentLine,parentCol].parentCol;
                            if((oldParentCol != -1) && (parentLine != -1) ){
                                Node oldParent = nodes[oldParentLine,parentCol];
                                int CGWOP = CalculG(nodes[line,col],oldParent.g);
                                if(CGWOP < nodes[line,col].g){
                                    nodes[line,col].g = CGWOP;
                                    nodes[line,col].f = nodes[line,col].g + nodes[line,col].h;
                                    openList.Update(ref nodes[line,col]);
                                }
                            }
                        }
                    }
                }

                if(openList.Empty()){
                    interval = DateTime.Now - time;
                    Utilitary.Log("----Pathfinding Duration----\nTime : " + interval.TotalSeconds + "\n------------------------------\n");
                    return new Path();
                }

                inOpen[currentPos.line,currentPos.col] = false;
                inClose[currentPos.line,currentPos.col] = true;
                Node n = openList.Dequeue(false);
                currentPos.line = n.line;
                currentPos.col = n.col;
                h = n.h;
            }

            /* ------------------- end ----------------------*/
            Stack<GridPosition> way = new Stack<GridPosition>();
            way.Push(currentPos);
            while(nodes[currentPos.line,currentPos.col].parentLine != -1){
                int line = nodes[currentPos.line,currentPos.col].parentLine;
                int col = nodes[currentPos.line,currentPos.col].parentCol;

                currentPos.line = line;
                currentPos.col = col;

                way.Push(currentPos);
            }
            interval = DateTime.Now - time;
            Utilitary.Log("----Pathfinding Duration----\nTime : " + interval.TotalSeconds + "\n------------------------------\n");
            return new Path(new List<GridPosition>(way.ToArray()));

        }

        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        public static int CalculH(Node node , GridPosition dest){
            int X = Mathf.Abs(node.col-dest.col);
            int Y = Mathf.Abs(node.line-dest.line);
            return X+Y;
        }

        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        public static int CalculG(Node node,int parentG){
            int tempG;
            if(node.parentLine == -1){
                return 0;
            }

            tempG = parentG + 1;
            return tempG;
        }

        public static void AddMetric(Metric m){
            metric = m;
        }

    }
}