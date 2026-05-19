using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using Modele;

namespace DataAccess
{
    public class AdminClienti
    {
        private List<Client> _clienti = new List<Client>();

        // CREATE
        public void Adauga(Client client)
        {
            _clienti.Add(client);
        }

        // READ
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

        // UPDATE
        public bool ModificaClient(string id, string nume, string telefon,
                                   string email, int puncte, DateTime dataNasterii)
        {
            Client c = _clienti.FirstOrDefault(x => x.Id == id);
            if (c == null) return false;
            c.Nume = nume;
            c.Telefon = telefon;
            c.Email = email;
            c.PuncteLoialitate = puncte;
            c.DataNasterii = dataNasterii;
            c.DataActualizare = DateTime.Now;
            return true;
        }

        public bool ModificaPuncte(string id, int puncteNoi)
        {
            Client c = _clienti.FirstOrDefault(x => x.Id == id);
            if (c == null) return false;
            c.PuncteLoialitate = puncteNoi;
            c.DataActualizare = DateTime.Now;
            return true;
        }

        // DELETE
        public bool Sterge(string id)
        {
            Client c = _clienti.FirstOrDefault(x => x.Id == id);
            if (c == null) return false;
            _clienti.Remove(c);
            return true;
        }

        // PERSISTENTA
        public void SalveazaInFisier(string cale)
        {
            using (StreamWriter sw = new StreamWriter(cale))
            {
                foreach (Client c in _clienti)
                    sw.WriteLine($"{c.Id}|{c.Nume}|{c.Telefon}|{c.Email}|{c.PuncteLoialitate}|{c.DataNasterii:yyyy-MM-dd}|{c.DataActualizare:yyyy-MM-dd HH:mm:ss}");
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

                DateTime dataNasterii = DateTime.Today;
                if (p.Length >= 6) DateTime.TryParse(p[5], out dataNasterii);

                var client = new Client(p[0], p[1], p[2], p[3], int.Parse(p[4]), dataNasterii);

                if (p.Length >= 7 && DateTime.TryParse(p[6], out DateTime dataAct))
                    client.DataActualizare = dataAct;

                _clienti.Add(client);
            }
        }
    }
}