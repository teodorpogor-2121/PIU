using System.Collections.Generic;
using System.Linq;
using System.IO;
using Modele;
using System;

namespace DataAccess
{
    public class AdminParfumuri
    {
        private List<Parfum> _parfumuri = new List<Parfum>();

        public void Adauga(Parfum parfum)
        {
            _parfumuri.Add(parfum);
        }

        public List<Parfum> GetToate()
        {
            return _parfumuri.ToList();
        }

        public Parfum GetDupaId(string id)
        {
            return _parfumuri.FirstOrDefault(x => x.Id == id);
        }

        public List<Parfum> CautaDupaBrand(string brand)
        {
            return _parfumuri
                .Where(p => p.Brand.ToLower().Contains(brand.ToLower()))
                .OrderBy(p => p.Pret)
                .ToList();
        }

        public List<Parfum> CautaDupaPretMaxim(decimal pretMax)
        {
            return _parfumuri
                .Where(p => p.Pret <= pretMax)
                .OrderBy(p => p.Pret)
                .ToList();
        }

        // Metoda de modificare parfum Lab 9
        public bool ModificaParfum(string id, string numeNou, string brandNou,
                                   Concentratie concentratieNoua, int cantitateMlNou,
                                   int stocNou, decimal pretNou,
                                   Sezon sezonNou, Ocazie ocaziiNoi)
        {
            Parfum p = _parfumuri.FirstOrDefault(x => x.Id == id);
            if (p == null) return false;

            p.Nume = numeNou;
            p.Brand = brandNou;
            p.Concentratie = concentratieNoua;
            p.CantitateMl = cantitateMlNou;
            p.Stoc = stocNou;
            p.Pret = pretNou;
            p.SezonRecomandat = sezonNou;
            p.OcaziiPotrivite = ocaziiNoi;
            p.DataActualizare = DateTime.Now; // actualizat la data curenta
            return true;
        }

        public void SalveazaInFisier(string cale)
        {
            using (StreamWriter sw = new StreamWriter(cale))
            {
                foreach (Parfum p in _parfumuri)
                {
                    sw.WriteLine($"{p.Id}|{p.Nume}|{p.Brand}|{p.Concentratie}|{p.CantitateMl}|{p.Stoc}|{p.Pret}|{p.SezonRecomandat}|{p.OcaziiPotrivite}");
                }
            }
        }

        public void IncarcaDinFisier(string cale)
        {
            if (!File.Exists(cale)) return;
            _parfumuri.Clear();
            foreach (string linie in File.ReadAllLines(cale))
            {
                string[] p = linie.Split('|');
                if (p.Length < 9) continue;
                _parfumuri.Add(new Parfum(
                    p[0], p[1], p[2],
                    (Concentratie)Enum.Parse(typeof(Concentratie), p[3]),
                    int.Parse(p[4]),
                    int.Parse(p[5]),
                    decimal.Parse(p[6]),
                    (Sezon)Enum.Parse(typeof(Sezon), p[7]),
                    (Ocazie)Enum.Parse(typeof(Ocazie), p[8])
                ));
            }
        }

        public bool ModificaPret(string id, decimal pretNou)
        {
            Parfum p = _parfumuri.FirstOrDefault(x => x.Id == id);
            if (p == null) return false;
            p.Pret = pretNou;
            p.DataActualizare = DateTime.Now;
            return true;
        }

        public bool ModificaStoc(string id, int stocNou)
        {
            Parfum p = _parfumuri.FirstOrDefault(x => x.Id == id);
            if (p == null) return false;
            p.Stoc = stocNou;
            p.DataActualizare = DateTime.Now;
            return true;
        }
    }
}