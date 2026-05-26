using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Modele;
using DataAccess;

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

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object parameter) => _execute(parameter);
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly AdminParfumuri _adminParfumuri = new AdminParfumuri();
        private readonly AdminClienti _adminClienti = new AdminClienti();

        public event PropertyChangedEventHandler PropertyChanged;
        private void Notify(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private ObservableCollection<Client> _clienti = new ObservableCollection<Client>();
        public ObservableCollection<Client> Clienti
        {
            get => _clienti;
            set { _clienti = value; Notify(nameof(Clienti)); }
        }

        private ObservableCollection<Parfum> _parfumuri = new ObservableCollection<Parfum>();
        public ObservableCollection<Parfum> Parfumuri
        {
            get => _parfumuri;
            set { _parfumuri = value; Notify(nameof(Parfumuri)); }
        }

        private Client _clientSelectat;
        public Client ClientSelectat
        {
            get => _clientSelectat;
            set
            {
                _clientSelectat = value;
                Notify(nameof(ClientSelectat));
                Notify(nameof(AreClientSelectat));
                PopuleazaFormularClient(value);
            }
        }

        private Parfum _parfumSelectat;
        public Parfum ParfumSelectat
        {
            get => _parfumSelectat;
            set { _parfumSelectat = value; Notify(nameof(ParfumSelectat)); }
        }

        public bool AreClientSelectat => _clientSelectat != null;

        private int _numarClienti;
        public int NumarClienti
        {
            get => _numarClienti;
            set { _numarClienti = value; Notify(nameof(NumarClienti)); }
        }

        private int _numarParfumuri;
        public int NumarParfumuri
        {
            get => _numarParfumuri;
            set { _numarParfumuri = value; Notify(nameof(NumarParfumuri)); }
        }

        private string _mesajStatus = "Aplicație pornită. Gata.";
        public string MesajStatus
        {
            get => _mesajStatus;
            set { _mesajStatus = $"[{DateTime.Now:HH:mm:ss}] {value}"; Notify(nameof(MesajStatus)); }
        }

        private string _mesajFormular;
        public string MesajFormular
        {
            get => _mesajFormular;
            set { _mesajFormular = value; Notify(nameof(MesajFormular)); }
        }

        private string _filtruClienti = string.Empty;
        public string FiltruClienti
        {
            get => _filtruClienti;
            set { _filtruClienti = value; Notify(nameof(FiltruClienti)); AplicaFiltruClienti(); }
        }

        private string _filtruParfumuri = string.Empty;
        public string FiltruParfumuri
        {
            get => _filtruParfumuri;
            set { _filtruParfumuri = value; Notify(nameof(FiltruParfumuri)); AplicaFiltruParfumuri(); }
        }

        private string _formClientId = string.Empty;
        public string FormClientId
        {
            get => _formClientId;
            set { _formClientId = value; Notify(nameof(FormClientId)); }
        }

        private string _formClientNume = string.Empty;
        public string FormClientNume
        {
            get => _formClientNume;
            set { _formClientNume = value; Notify(nameof(FormClientNume)); }
        }

        private string _formClientTelefon = string.Empty;
        public string FormClientTelefon
        {
            get => _formClientTelefon;
            set { _formClientTelefon = value; Notify(nameof(FormClientTelefon)); }
        }

        private string _formClientEmail = string.Empty;
        public string FormClientEmail
        {
            get => _formClientEmail;
            set { _formClientEmail = value; Notify(nameof(FormClientEmail)); }
        }

        private int _formClientPuncte;
        public int FormClientPuncte
        {
            get => _formClientPuncte;
            set { _formClientPuncte = value; Notify(nameof(FormClientPuncte)); }
        }

        private DateTime? _formClientDataNasterii;
        public DateTime? FormClientDataNasterii
        {
            get => _formClientDataNasterii;
            set { _formClientDataNasterii = value; Notify(nameof(FormClientDataNasterii)); }
        }

        private bool _modAdaugareClient = false;
        public bool ModAdaugareClient
        {
            get => _modAdaugareClient;
            set { _modAdaugareClient = value; Notify(nameof(ModAdaugareClient)); Notify(nameof(TitluFormular)); }
        }

        public string TitluFormular => ModAdaugareClient
            ? "Client Nou"
            : (ClientSelectat != null ? $"Editare: {ClientSelectat.Nume}" : "Selectează un client din listă");

        public ICommand ComandaSalveazaParfumuri { get; }
        public ICommand ComandaIncarcaParfumuri { get; }
        public ICommand ComandaSalveazaClienti { get; }
        public ICommand ComandaIncarcaClienti { get; }
        public ICommand ComandaClientNou { get; }
        public ICommand ComandaSalveazaClient { get; }
        public ICommand ComandaStergeClient { get; }
        public ICommand ComandaAnuleazaClient { get; }

        public MainViewModel()
        {
            ComandaSalveazaParfumuri = new RelayCommand(_ =>
            {
                _adminParfumuri.SalveazaInFisier("parfumuri.txt");
                MesajStatus = "Parfumurile au fost salvate.";
            });

            ComandaIncarcaParfumuri = new RelayCommand(_ =>
            {
                _adminParfumuri.IncarcaDinFisier("parfumuri.txt");
                RefreshParfumuri();
                MesajStatus = $"Parfumuri încărcate. Total: {NumarParfumuri}";
            });

            ComandaSalveazaClienti = new RelayCommand(_ =>
            {
                _adminClienti.SalveazaInFisier("clienti.txt");
                MesajStatus = "Clienții au fost salvați.";
            });

            ComandaIncarcaClienti = new RelayCommand(_ =>
            {
                _adminClienti.IncarcaDinFisier("clienti.txt");
                RefreshClienti();
                MesajStatus = $"Clienți încărcați. Total: {NumarClienti}";
            });

            ComandaClientNou = new RelayCommand(_ =>
            {
                ClientSelectat = null;
                ModAdaugareClient = true;
                FormClientId = string.Empty;
                FormClientNume = string.Empty;
                FormClientTelefon = string.Empty;
                FormClientEmail = string.Empty;
                FormClientPuncte = 0;
                FormClientDataNasterii = null;
                MesajFormular = string.Empty;
                Notify(nameof(TitluFormular));
            });

            ComandaSalveazaClient = new RelayCommand(_ =>
            {
                if (!ValidFormClient()) return;

                if (ModAdaugareClient)
                {
                    if (_adminClienti.GetToate().Any(c => c.Id == FormClientId))
                    {
                        MesajFormular = "Există deja un client cu acest ID.";
                        return;
                    }
                    var nou = new Client(FormClientId, FormClientNume, FormClientTelefon,
                        FormClientEmail, FormClientPuncte, FormClientDataNasterii ?? DateTime.Today);
                    _adminClienti.Adauga(nou);
                    MesajStatus = $"Client adăugat: {nou.Nume}";
                    ModAdaugareClient = false;
                }
                else if (ClientSelectat != null)
                {
                    _adminClienti.ModificaClient(ClientSelectat.Id, FormClientNume,
                        FormClientTelefon, FormClientEmail, FormClientPuncte,
                        FormClientDataNasterii ?? DateTime.Today);
                    MesajStatus = $"Client actualizat: {FormClientNume}";
                }

                RefreshClienti();
                MesajFormular = string.Empty;
            });

            ComandaStergeClient = new RelayCommand(_ =>
            {
                if (ClientSelectat == null) return;
                string nume = ClientSelectat.Nume;
                _adminClienti.Sterge(ClientSelectat.Id);
                ClientSelectat = null;
                RefreshClienti();
                MesajStatus = $"Client șters: {nume}";
            }, _ => AreClientSelectat);

            ComandaAnuleazaClient = new RelayCommand(_ =>
            {
                ClientSelectat = null;
                ModAdaugareClient = false;
                FormClientId = FormClientNume = FormClientTelefon = FormClientEmail = string.Empty;
                FormClientPuncte = 0;
                FormClientDataNasterii = null;
                MesajFormular = string.Empty;
                Notify(nameof(TitluFormular));
            });
        }

        private void PopuleazaFormularClient(Client c)
        {
            if (c == null) return;
            FormClientId = c.Id;
            FormClientNume = c.Nume;
            FormClientTelefon = c.Telefon;
            FormClientEmail = c.Email;
            FormClientPuncte = c.PuncteLoialitate;
            FormClientDataNasterii = c.DataNasterii;
            ModAdaugareClient = false;
            Notify(nameof(TitluFormular));
        }

        private bool ValidFormClient()
        {
            if (string.IsNullOrWhiteSpace(FormClientNume)) { MesajFormular = "Numele nu poate fi gol."; return false; }
            if (string.IsNullOrWhiteSpace(FormClientTelefon)) { MesajFormular = "Telefonul nu poate fi gol."; return false; }
            if (!FormClientEmail.Contains("@")) { MesajFormular = "Email invalid."; return false; }
            if (FormClientPuncte < 0) { MesajFormular = "Punctele nu pot fi negative."; return false; }
            return true;
        }

        public void RefreshClienti(string filtru = "")
        {
            Clienti.Clear();
            var sursa = string.IsNullOrWhiteSpace(filtru)
                ? _adminClienti.GetToate()
                : _adminClienti.CautaDupaNume(filtru);
            foreach (var c in sursa) Clienti.Add(c);
            NumarClienti = _adminClienti.GetToate().Count;
        }

        public void RefreshParfumuri(string filtru = "")
        {
            Parfumuri.Clear();
            var sursa = string.IsNullOrWhiteSpace(filtru)
                ? _adminParfumuri.GetToate()
                : _adminParfumuri.GetToate().Where(p =>
                    p.Nume.ToLower().Contains(filtru.ToLower()) ||
                    p.Brand.ToLower().Contains(filtru.ToLower())).ToList();
            foreach (var p in sursa) Parfumuri.Add(p);
            NumarParfumuri = _adminParfumuri.GetToate().Count;
        }

        private void AplicaFiltruClienti() => RefreshClienti(_filtruClienti);
        private void AplicaFiltruParfumuri() => RefreshParfumuri(_filtruParfumuri);
    }
}