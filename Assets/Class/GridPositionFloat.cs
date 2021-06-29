using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SDAA{
    public struct GridPositionFloat{
        public float col;
        public float line;
        public GridPositionFloat(float line, float col){
            this.line = line;
            this.col = col;
        }

        public static GridPositionFloat operator *(GridPositionFloat gpf, float f){
            return new GridPositionFloat(gpf.line*f,gpf.col*f);
        }

        public static GridPositionFloat operator +(GridPositionFloat gp, GridPositionFloat gpf){
            return new GridPositionFloat(gp.line + gpf.line, gp.col+gpf.col);
        }
        public static implicit operator GridPosition(GridPositionFloat gpf){
            int line = (int)gpf.line;
            int col = (int) gpf.col;
            return new GridPosition(line,col); 
        }

        public override string ToString(){
            return "(line : " + line + ", col : " + col + ")";
        }

        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        /*---------------------- OVERRIDE == ---------------------*/
        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        public bool Equals(GridPositionFloat other)
        {
            return (line == other.line) && (col == other.col);   
        }

        public override bool Equals(object obj){
            return Equals((GridPositionFloat)obj);
        }
        
        public static bool operator ==(GridPositionFloat gp1, GridPositionFloat gp2){
            return gp1.Equals(gp2);
        }

        public static bool operator !=(GridPositionFloat gp1, GridPositionFloat gp2){
            return !gp1.Equals(gp2);
        }

        // maybe that's wierd
        public override int GetHashCode(){
            unchecked{
                int result = 1;
                result = (result * 77) ^ (int)line;
                result = (result * 77) ^ (int)col;
                return result.GetHashCode();
            }
        }

    }
}
