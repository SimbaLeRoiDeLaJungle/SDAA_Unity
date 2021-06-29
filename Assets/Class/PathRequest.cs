namespace SDAA{
    /// <summary>
    /// Use to request a path to the PathManager.
    /// </summary>
    public struct PathRequest{
        public GridPosition start;
        public GridPosition dest;
        public int id;
        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        public PathRequest(GridPosition start, GridPosition dest, int id){
            this.start = start;
            this.dest = dest;
            this.id = id;
        }
    }
}