using System;
using System.Collections.Generic;
using Parfumuri;

namespace Parfumuri
{
    public class Program
    {
        
        static List<Parfum> parfumuri = new List<Parfum>();
        static List<Client> clienti = new List<Client>();

        static void Main(string[] args)
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine(" EssenceFlow – Meniu Principal ");
                Console.WriteLine("1. adauga parfum");
                Console.WriteLine("2. afiseaza parfumurile");
                Console.WriteLine("3. cauta parfum dupa brand");
                Console.WriteLine("4. cauta parfum dupa pret maxim");
                Console.WriteLine("5. adauga client");
                Console.WriteLine("6. afiseaza clientii");
                Console.WriteLine("7. cauta client dupa nume");
                Console.WriteLine("0. exit");
                Console.Write("\nAlegere: ");

                string opt = Console.ReadLine();

                switch (opt)
                {
                    case "1": AdaugaParfum(); break;
                    case "2": AfiseazaParfumuri(); break;
                    case "3": CautaParfumDupaBrand(); break;
                    case "4": CautaParfumDupaPret(); break;
                    case "5": AdaugaClient(); break;
                    case "6": AfiseazaClienti(); break;
                    case "7": CautaClientDupaNume(); break;
                    case "0": running = false; break;
                    default: Console.WriteLine("Eroare"); break;
                }
            }
        }

        
        static void AdaugaParfum()
        {
            Console.WriteLine("\nAdauga Parfum ");

            Console.Write("Id: ");
            string id = Console.ReadLine();

            Console.Write("Nume: ");
            string nume = Console.ReadLine();

            Console.Write("Brand: ");
            string brand = Console.ReadLine();

            Console.WriteLine("Concentratie (0=EDP, 1=EDT, 2=EDC, 3=Parfum): ");
            Concentratie concentratie = Concentratie.EDP;
            if (int.TryParse(Console.ReadLine(), out int concInt) && Enum.IsDefined(typeof(Concentratie), concInt))
                concentratie = (Concentratie)concInt;

            Console.Write("Cantitate (ml): ");
            int.TryParse(Console.ReadLine(), out int cantitate);

            Console.Write("Stoc: ");
            int.TryParse(Console.ReadLine(), out int stoc);

            Console.Write("Pret (RON): ");
            decimal.TryParse(Console.ReadLine(), out decimal pret);

            
            Parfum parfum = new Parfum(id, nume, brand, concentratie, cantitate, stoc, pret);
            parfumuri.Add(parfum);

            Console.WriteLine($"\nParfumul \"{nume}\" a fost adaugat cu succes!");
        }

        static void AdaugaClient()
        {
            Console.WriteLine("\n Adauga Client ");

            Console.Write("Id: ");
            string id = Console.ReadLine();

            Console.Write("Nume: ");
            string nume = Console.ReadLine();

            Console.Write("Telefon: ");
            string telefon = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Puncte loialitate ");
            int.TryParse(Console.ReadLine(), out int puncte);

           
            Client client = new Client(id, nume, telefon, email, puncte);

            if (!client.EsteValid())
            {
                Console.WriteLine("Clientul nu este valid, Numar de telefon?");
                return;
            }

            clienti.Add(client);
            Console.WriteLine($"\nClientul \"{nume}\" a fost adaugat");
        }

        

        static void AfiseazaParfumuri()
        {
            Console.WriteLine("\n Lista Parfumuri");

            if (parfumuri.Count == 0)
            {
                Console.WriteLine("Nu exista parfumuri.");
                return;
            }

            for (int i = 0; i < parfumuri.Count; i++)
            {
                Parfum p = parfumuri[i];
                Console.WriteLine($"\n[{i + 1}] {p.Nume} – {p.Brand}");
                Console.WriteLine($"    Concentratie: {p.Concentratie} | {p.CantitateMl}ml");
                Console.WriteLine($"    Stoc: {p.Stoc} buc , Pret: {p.Pret} RON");
            }
        }

        static void AfiseazaClienti()
        {
            Console.WriteLine("\n Lista Clienti ");

            if (clienti.Count == 0)
            {
                Console.WriteLine("Nu exista clienti.");
                return;
            }

            for (int i = 0; i < clienti.Count; i++)
            {
                Client c = clienti[i];
                Console.WriteLine($"\n[{i + 1}] {c.Nume}");
                Console.WriteLine($"    Telefon: {c.Telefon} | Email: {c.Email}");
                Console.WriteLine($"    Puncte loialitate: {c.PuncteLoialitate}");
            }
        }

        

        static void CautaParfumDupaBrand()
        {
            Console.Write("\nIntroduceti brandul cautat: ");
            string brand = Console.ReadLine();

            List<Parfum> rezultate = new List<Parfum>();
            foreach (Parfum p in parfumuri)
            {
                if (p.Brand.ToLower().Contains(brand.ToLower()))
                    rezultate.Add(p);
            }

            if (rezultate.Count == 0)
            {
                Console.WriteLine($"Nu s-au gasit parfumuri cu brandul \"{brand}\".");
                return;
            }

            Console.WriteLine($"\nRezultate pentru brandul \"{brand}\":");
            foreach (Parfum p in rezultate)
            {
                Console.WriteLine($"  - {p.Nume} | {p.Concentratie} | {p.Pret} RON | Stoc: {p.Stoc}");
            }
        }

        static void CautaParfumDupaPret()
        {
            Console.Write("\nIntroduceti pretul maxim in RONI: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal pretMax))
            {
                Console.WriteLine("Pret invalid");
                return;
            }

            List<Parfum> rezultate = new List<Parfum>();
            foreach (Parfum p in parfumuri)
            {
                if (p.Pret <= pretMax)
                    rezultate.Add(p);
            }

            if (rezultate.Count == 0)
            {
                Console.WriteLine($"Nu s-au gasit parfumuri sub {pretMax} RON.");
                return;
            }

            Console.WriteLine($"\nParfumuri sub {pretMax} RON:");
            foreach (Parfum p in rezultate)
            {
                Console.WriteLine($"  - {p.Nume} | {p.Brand} | {p.Pret} RON | Stoc: {p.Stoc}");
            }
        }

        static void CautaClientDupaNume()
        {
            Console.Write("\nIntroduceti numele clientului: ");
            string nume = Console.ReadLine();

            List<Client> rezultate = new List<Client>();
            foreach (Client c in clienti)
            {
                if (c.Nume.ToLower().Contains(nume.ToLower()))
                    rezultate.Add(c);
            }

            if (rezultate.Count == 0)
            {
                Console.WriteLine($"Nu s-au gasit clienti cu numele \"{nume}\".");
                return;
            }

            Console.WriteLine($"\nRezultate pentru \"{nume}\":");
            foreach (Client c in rezultate)
            {
                Console.WriteLine($"  - {c.Nume} | {c.Telefon} | Puncte: {c.PuncteLoialitate}");
            }
        }
    }

    public enum Concentratie
    {
        EDP,
        EDT,
        EDC,
        Parfum
    }

    public enum Rol { Admin, Consultant, Manager }
}