using System;

namespace android.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }

        public Boolean Switch { get
            {
                return switch_;
            }
            set
            {
                Console.WriteLine("HURA!!!!");
                switch_ = value;
            }
        }

        private Boolean switch_;
    }
}