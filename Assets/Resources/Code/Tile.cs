using UnityEditor;
using UnityEngine;

namespace Resources.Code {
    public abstract class Tile {
        public string Name { get; }
        public Sprite Sprite { get; }

        public Tile(Builder builder) {
            Name = builder.name;
            Sprite = builder.sprite;
        }

        public class Builder {
            internal readonly string name;
            internal readonly Sprite sprite;

            public Builder(Sprite sprite, string name) {
                this.sprite = sprite;
                this.name = name;
            }
            
            // TODO: Other properties ig
        }
    }
}
