using System;
using System.Collections.Generic;
using Modele;
using DataAccess;

namespace Parfumuri
{
    public class Program
    {

        static AdminParfumuri adminParfumuri = new AdminParfumuri();
        static AdminClienti adminClienti = new AdminClienti();

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
                Console.WriteLine("8. salveaza parfumuri in fisier");
                Console.WriteLine("9. incarca parfumuri din fisier");
                Console.WriteLine("10. modifica pret parfum");
                Console.WriteLine("11. modifica stoc parfum");
                Console.WriteLine("12. salveaza clienti in fisier");
                Console.WriteLine("13. incarca clienti din fisier");
                Console.WriteLine("14. modifica puncte client");
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
                    case "8": SalveazaParfumuri(); break;
                    case "9": IncarcaParfumuri(); break;
                    case "10": ModificaParfum(); break;
                    case "11": ModificaStocParfum(); break;
                    case "12": SalveazaClienti(); break;
                    case "13": IncarcaClienti(); break;
                    case "14": ModificaPuncteClient(); break;
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
            adminParfumuri.Adauga(parfum);

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

            adminClienti.Adauga(client);
            Console.WriteLine($"\nClientul \"{nume}\" a fost adaugat");
        }



        static void AfiseazaParfumuri()
        {
            Console.WriteLine("\n Lista Parfumuri");

            if (adminParfumuri.GetToate().Count == 0)
            {
                Console.WriteLine("Nu exista parfumuri.");
                return;
            }

            List<Parfum> lista = adminParfumuri.GetToate();
            for (int i = 0; i < lista.Count; i++)
            {
                Parfum p = lista[i];
                Console.WriteLine($"\n[{i + 1}] {p.Nume} – {p.Brand}");
                Console.WriteLine($"    Concentratie: {p.Concentratie} | {p.CantitateMl}ml");
                Console.WriteLine($"    Stoc: {p.Stoc} buc , Pret: {p.Pret} RON");
            }
        }

        static void AfiseazaClienti()
        {
            Console.WriteLine("\n Lista Clienti ");

            if (adminClienti.GetToate().Count == 0)
            {
                Console.WriteLine("Nu exista clienti.");
                return;
            }

            List<Client> lista = adminClienti.GetToate();
            for (int i = 0; i < lista.Count; i++)
            {
                Client c = lista[i];
                Console.WriteLine($"\n[{i + 1}] {c.Nume}");
                Console.WriteLine($"    Telefon: {c.Telefon} | Email: {c.Email}");
                Console.WriteLine($"    Puncte loialitate: {c.PuncteLoialitate}");
            }
        }



        static void CautaParfumDupaBrand()
        {
            Console.Write("\nIntroduceti brandul cautat: ");
            string brand = Console.ReadLine();

            List<Parfum> rezultate = adminParfumuri.CautaDupaBrand(brand);
           

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

            List<Parfum> rezultate = adminParfumuri.CautaDupaPretMaxim(pretMax);

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
        static void SalveazaParfumuri()
        {
            adminParfumuri.SalveazaInFisier("parfumuri.txt");
            Console.WriteLine("\nParfumurile au fost salvate in parfumuri.txt");
        }

        static void IncarcaParfumuri()
        {
            adminParfumuri.IncarcaDinFisier("parfumuri.txt");
            Console.WriteLine("\nParfumurile au fost incarcate din parfumuri.txt");
        }

        static void ModificaParfum()
        {
            Console.Write("\nId parfum de modificat: ");
            string id = Console.ReadLine();

            Console.Write("Pret nou (RON): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal pretNou))
            {
                Console.WriteLine("Pret invalid.");
                return;
            }

            if (adminParfumuri.ModificaPret(id, pretNou))
                Console.WriteLine("Pret actualizat cu succes!");
            else
                Console.WriteLine("Parfumul nu a fost gasit.");
        }

        static void ModificaStocParfum()
        {
            Console.Write("\nId parfum de modificat: ");
            string id = Console.ReadLine();

            Console.Write("Stoc nou: ");
            if (!int.TryParse(Console.ReadLine(), out int stocNou))
            {
                Console.WriteLine("Stoc invalid.");
                return;
            }

            if (adminParfumuri.ModificaStoc(id, stocNou))
                Console.WriteLine("Stoc actualizat cu succes!");
            else
                Console.WriteLine("Parfumul nu a fost gasit.");
        }
        static void SalveazaClienti()
        {
            adminClienti.SalveazaInFisier("clienti.txt");
            Console.WriteLine("\nClientii au fost salvati in clienti.txt");
        }

        static void IncarcaClienti()
        {
            adminClienti.IncarcaDinFisier("clienti.txt");
            Console.WriteLine("\nClientii au fost incarcati din clienti.txt");
        }

        static void ModificaPuncteClient()
        {
            Console.Write("\nId client de modificat: ");
            string id = Console.ReadLine();

            Console.Write("Puncte noi: ");
            if (!int.TryParse(Console.ReadLine(), out int puncteNoi))
            {
                Console.WriteLine("Valoare invalida.");
                return;
            }

            if (adminClienti.ModificaPuncte(id, puncteNoi))
                Console.WriteLine("Puncte actualizate cu succes!");
            else
                Console.WriteLine("Clientul nu a fost gasit.");
        }

        static void CautaClientDupaNume()
        {
            Console.Write("\nIntroduceti numele clientului: ");
            string nume = Console.ReadLine();

            List<Client> rezultate = adminClienti.CautaDupaNume(nume);

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
}

    