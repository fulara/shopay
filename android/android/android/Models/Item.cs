using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace android.Models
{
    public class Item : IComparable<Item>
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }

        public bool Switch { get
            {
                return switch_;
            }
            set
            {
                if (switch_ != value)
                {
                    switch_ = value;

                    Console.WriteLine("wuut? " + Text);
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