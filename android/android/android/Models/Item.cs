using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace android.Models
{
    public class Item : IComparable<Item>
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Text { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("amount")]
        public double Amount { get; set; }
        [JsonProperty("unit")]
        public string Unit { get; set; } = "";

        bool bought;
        int lastUpdate;

        public string Quantity { get
            {
                if(Amount == 0.0)
                {
                    return "";
                }

                return Amount.ToString() + " " + Unit;

            } }

        public bool Switch { get
            {
                return switch_;
            }
            set
            {
                if (switch_ != value)
                {
                    switch_ = value;
                    SwitchChanged.Invoke(this);
                }
            }
        }

        public int CompareTo(Item rhs)
        {
            if (switch_ == rhs.switch_)
            {
                return 0;
            }

            if(rhs.switch_)
            {
                return -1;
            }

            return 1;
        }

        private bool switch_;

        public event Action<Item> SwitchChanged;

    }
}