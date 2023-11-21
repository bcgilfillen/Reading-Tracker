using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reading_Tracker
{
    internal class Book
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public Book() { }

        public Book(string name, string location, string description)
        {
            this.Name = name;
            this.Location = location;
            this.Description = description;
        }

    }
}
