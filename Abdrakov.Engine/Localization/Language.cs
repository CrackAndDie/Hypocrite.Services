using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Engine.Localization
{
    public struct Language
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Language(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
