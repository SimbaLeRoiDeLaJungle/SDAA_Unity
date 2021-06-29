namespace SDAA{
    /// <summary>
    /// Just a Vector2 of Int with x = col and y = line /!\ in the constructor line is before col.
    /// </summary>
    public struct GridPosition
    {
        public int line;
        public int col;
        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        public GridPosition(int line,int col){
            this.line = line;
            this.col = col;
        }

        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        /*---------------------- OVERRIDE == ---------------------*/
        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        public bool Equals(GridPosition other)
        {
            return (line == other.line) && (col == other.col);   
        }

        public override bool Equals(object obj){
            return Equals((GridPosition)obj);
        }
        
        public static bool operator ==(GridPosition gp1, GridPosition gp2){
            return gp1.Equals(gp2);
        }

        public static bool operator !=(GridPosition gp1, GridPosition gp2){
            return !gp1.Equals(gp2);
        }

        // maybe that's wierd
        public override int GetHashCode(){
            unchecked{
                int result = 1;
                result = (result * 77) ^ line;
                result = (result * 77) ^ col;
                return result.GetHashCode();
            }
        }

        public static implicit operator GridPositionFloat(GridPosition gp){
            return new GridPositionFloat(gp.line,gp.col);
        }

        public static GridPosition operator +(GridPosition gp1, GridPosition gp2){
            return new GridPosition(gp1.line + gp2.line, gp1.col+gp2.col);
        }

        public static GridPosition operator -(GridPosition gp1, GridPosition gp2){
            return new GridPosition(gp1.line - gp2.line, gp1.col - gp2.col);
        }

        public override string ToString(){
            return "(line : " + line + ", col : " + col + ")";
        }
    }
}