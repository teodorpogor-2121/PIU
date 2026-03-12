using System;
using System.Collections.Generic;
using Parfumuri;

namespace Parfumuri
{


    public class Program 
    {
        static void Main(string[] args)
        {
            
            Parfum parfumulMeu = new Parfum(
                "001",
                "The Matcha 26",
                "Le Labo",
                Concentratie.EDP,
                100,
                15,
                550.50m
            );

            
            Client clientNou = new Client("C001", "Teodor Pogor", "0722123456", "teodor.pgr@email.com");

            
            Console.WriteLine(" Detalii Parfum ");
            Console.WriteLine($"Nume: {parfumulMeu.Nume}");
            Console.WriteLine($"Brand: {parfumulMeu.Brand}");
            Console.WriteLine($"Concentratie: {parfumulMeu.Concentratie}");
            Console.WriteLine($"Pret: {parfumulMeu.Pret} RON");

            Console.WriteLine("\n Detalii Client ");
            Console.WriteLine($"Client: {clientNou.Nume}");
            Console.WriteLine($"Valid: {(clientNou.EsteValid() ? "Da" : "Nu")}");

            
            Console.WriteLine("\ninchide...");
            Console.ReadKey();
        }
    }
    public enum Concentratie
    {
        EDP,    // Eau de Parfum
        EDT,    // Eau de Toilette
        EDC,    // Eau de Cologne
        Parfum  //  Parfum
    }

    
    

    
    
    
    public enum Rol { Admin, Consultant, Manager }

    




}


