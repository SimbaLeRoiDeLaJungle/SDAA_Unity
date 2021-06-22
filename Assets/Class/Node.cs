using System.Collections;
using System.Collections.Generic;
using System;
namespace SDAA{
    public struct Node
    {
        // position
        public int col;
        public int line;
        public int parentLine;
        public int parentCol;

        public int h;
        public int g;
        public int f;

        public Node(int line, int col){
            this.col = col;
            this.line = line;
            h = int.MaxValue;
            g = int.MaxValue;
            f = int.MaxValue;
            parentLine = -1;
            parentCol = -1;
        }

        /*-----------------------------------------------------------------*/
        /*------------------------- OVERRIDE == ---------------------------*/
        /*-----------------------------------------------------------------*/
        public bool Equals(Node other)
        {
            return (line == other.line) && (col == other.col);   
        }

        public override bool Equals(object obj){
            return Equals((Node)obj);
        }
        
        public static bool operator ==(Node node1, Node node2){
            return node1.Equals(node2);
        }

        public static bool operator !=(Node node1, Node node2){
            return !node1.Equals(node2);
        }

        // maybe that's wierd
        public override int GetHashCode(){
            unchecked{
                int result = 0;
                result = (result * 77) ^ line;
                result = (result * 77) ^ col;
                return result.GetHashCode();
            }
        }

    }
}