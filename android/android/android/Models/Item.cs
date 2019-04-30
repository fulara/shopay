using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace android.Models
{
    public class Item : IComparable<Item>
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }
        [JsonProperty("name", Required = Required.Always)]
        public string Text { get; set; }
        [JsonProperty("description", Required = Required.Always)]
        public string Description { get; set; }

        [JsonProperty("category", Required = Required.Always)]
        public string Category { get; set; } = "";
        [JsonIgnore]
        public double Amount { get; set; } = 0.0;
        [JsonProperty("unit", Required = Required.Always)]
        public string Unit { get; set; } = "";

        [JsonProperty("bought", Required = Required.Always)]
        public bool Bought = false;

        [JsonProperty("lastUpdateTimestamp", Required = Required.Always)]
        public int LastUpdate;

        public Item()
        {
            Id = Guid.NewGuid().ToString();
        }

        [JsonIgnore]
        public string Quantity { get
            {
                if(Amount == 0.0)
                {
                    return "";
                }

                return Amount.ToString() + " " + Unit;

            } }

        [JsonIgnore]
        public bool Switch { get
            {
                return Bought;
            }
            set
            {
                if (Bought != value)
                {
                    Bought = value;
                    SwitchChanged.Invoke(this);
                }
            }
        }

        [JsonProperty("amount", Required = Required.Always)]
        public string JsonAmount
        {
            get
            {
                return Amount.ToString();
            }

            set
            {
                Amount = Double.Parse(value);
            }
        }

        public int CompareTo(Item rhs)
        {
            if (Bought == rhs.Bought)
            {
                return 0;
            }

            if(rhs.Bought)
            {
                return -1;
            }

            return 1;
        }

        public event Action<Item> SwitchChanged;
    }
}