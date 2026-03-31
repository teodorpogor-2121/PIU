using System.Collections.Generic;
using System.Linq;
using System.IO;
using Modele;

namespace DataAccess
{
    public class AdminClienti
    {
        private List<Client> _clienti = new List<Client>();

        public void Adauga(Client client)
        {
            _clienti.Add(client);
        }

        public List<Client> GetToate()
        {
            return _clienti.ToList();
        }

        public List<Client> CautaDupaNume(string nume)
        {
            return _clienti
                .Where(c => c.Nume.ToLower().Contains(nume.ToLower()))
                .OrderBy(c => c.Nume)
                .ToList();
        }

        public void SalveazaInFisier(string cale)
        {
            using (StreamWriter sw = new StreamWriter(cale))
            {
                foreach (Client c in _clienti)
                {
                    sw.WriteLine($"{c.Id}|{c.Nume}|{c.Telefon}|{c.Email}|{c.PuncteLoialitate}");
                }
            }
        }

        public void IncarcaDinFisier(string cale)
        {
            if (!File.Exists(cale)) return;
            _clienti.Clear();
            foreach (string linie in File.ReadAllLines(cale))
            {
                string[] p = linie.Split('|');
                if (p.Length < 5) continue;
                _clienti.Add(new Client(
                    p[0], p[1], p[2], p[3],
                    int.Parse(p[4])
                ));
            }
        }

        public bool ModificaPuncte(string id, int puncteNoi)
        {
            Client c = _clienti.FirstOrDefault(x => x.Id == id);
            if (c == null) return false;
            c.PuncteLoialitate = puncteNoi;
            return true;
        }
    }
}