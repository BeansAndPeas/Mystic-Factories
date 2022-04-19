using UnityEngine;

namespace Resources.Code {
    public class Tile {
        public string Name { get; }
        public string Sprite { get; }
        public Color Color { get; }

        public Tile(Builder builder) {
            Name = builder.name;
            Sprite = builder.sprite;
            Color = builder.color;
        }

        public class Builder {
            internal readonly string name;
            internal readonly string sprite;
            internal readonly Color color;

            public Builder(string sprite, Color color, string name) {
                this.sprite = sprite;
                this.name = name;
                this.color = color;
            }
            
            // TODO: Other properties ig
        }
    }
}
