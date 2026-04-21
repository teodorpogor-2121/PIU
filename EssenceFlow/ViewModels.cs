namespace EssenceFlow
{
    public class ParfumVM
    {
        public string Id { get; set; }
        public string Nume { get; set; }
        public string Brand { get; set; }
        public string Concentratie { get; set; }
        public int CantitateMl { get; set; }
        public int Stoc { get; set; }
        public decimal Pret { get; set; }
        public string SezonRecomandat { get; set; }

        public string PretText => $"{Pret:N2} RON";
        public string StocText => $"Stoc: {Stoc}";

        public ParfumVM(string id, string nume, string brand,
                        string concentratie, int ml, int stoc,
                        decimal pret, string sezon)
        {
            Id = id; Nume = nume; Brand = brand;
            Concentratie = concentratie; CantitateMl = ml;
            Stoc = stoc; Pret = pret; SezonRecomandat = sezon;
        }
    }

    public class ClientVM
    {
        public string Id { get; set; }
        public string Nume { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public int PuncteLoialitate { get; set; }

        public ClientVM(string id, string nume, string telefon,
                        string email, int puncte)
        {
            Id = id; Nume = nume; Telefon = telefon;
            Email = email; PuncteLoialitate = puncte;
        }
    }
}