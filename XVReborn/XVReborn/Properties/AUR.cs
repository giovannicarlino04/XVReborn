using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XVReborn.Properties
{
    struct Aura
    {
        public int[] Color;
        
        public Aura(int[] color)
        {
            Color = color;
        }
    }

    struct Charlisting
    {
        public int Name;
        public int Costume;
        public int ID;
        public bool inf;
        
        public Charlisting(int name, int costume, int id, bool inf)
        {
            Name = name;
            Costume = costume;
            ID = id;
            this.inf = inf;
        }
    }

    struct CharName
    {
        public int ID;
        public string Name;

        public CharName(int i, string n)
        {
            ID = i;
            Name = n;
        }
    }

}
