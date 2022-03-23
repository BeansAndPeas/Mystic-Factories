using UnityEngine;

namespace Resources.Code {
    public class Biome {
        public string Name { get; }
        public Color Color { get; }
        public float Temperature { get; }
        public float Wetness { get; }
        
        public Biome(Builder builder) {
            Name = builder.name;
            Color = builder.color;
            Temperature = builder.temperature;
            Wetness = builder.wetness;
        }

        public class Builder {
            internal string name;
            internal Color color;
            internal float temperature, wetness;
            
            public Builder(string name, Color color) {
                this.name = name;
                this.color = color;
            }

            public Builder SetTemperature(float temperature) {
                this.temperature = Mathf.Clamp(0, 1, temperature);
                return this;
            }

            public Builder SetWetness(float wetness) {
                this.wetness = Mathf.Clamp(0, 1, wetness);
                return this;
            }
        }
    }
}