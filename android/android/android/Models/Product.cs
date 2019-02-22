using System;
using System.Collections.Generic;
using System.Text;

namespace android.Models
{
    class Product
    {
        Category category;
        String name;
        String location;
        int count;

        bool kupione;
    }

    struct Category
    {
        //A,B,C,D
        String representation;
    }
}
