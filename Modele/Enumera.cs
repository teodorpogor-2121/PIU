using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modele
{
    public enum Concentratie
    {
        EDP,
        EDT,
        EDC,
        Parfum
    }

    public enum Rol { Admin, Consultant, Manager }

    public enum Sezon
    {
        Primavara,
        Vara,
        Toamna,
        Iarna,
        Universal
    }

    [Flags]
    public enum Ocazie
    {
        Niciuna = 0,
        Casual = 1,
        Business = 2,
        Seara = 4,
        Eveniment = 8,
        Sport = 16
    }
}

