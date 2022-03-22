namespace Resources.Code {
    public class TilePos {
        private int x, y;

        public TilePos(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public int X
        {
            get => x;
            set => x = value;
        }

        public int Y
        {
            get => y;
            set => y = value;
        }
    }
}
