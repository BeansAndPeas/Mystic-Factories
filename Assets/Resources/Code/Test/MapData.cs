namespace Resources.Code.Test {
    public class MapData {
        public readonly float[,] Data;
        public float Min { get; set; }
        public float Max { get; set; }

        public MapData(int width, int height) {
            Data = new float[width, height];
            Min = float.MaxValue;
            Max = float.MinValue;
        }
    }
}
