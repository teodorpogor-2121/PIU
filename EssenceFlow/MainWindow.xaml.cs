using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Modele;
using DataAccess;

namespace EssenceFlow
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        
        private const int MAX_LUNGIME_NUME = 50;
        private const int MAX_LUNGIME_BRAND = 30;
        private const int MIN_CANTITATE_ML = 1;
        private const int MAX_CANTITATE_ML = 1000;
        private const int MIN_STOC = 0;
        private const int MAX_STOC = 9999;
        private const decimal MIN_PRET = 0.01m;
        private const decimal MAX_PRET = 99999m;

        
        private static readonly SolidColorBrush CuloareLabelNormal = new SolidColorBrush(Color.FromRgb(0x3E, 0x27, 0x23));
        private static readonly SolidColorBrush CuloareLabelEroare = new SolidColorBrush(Color.FromRgb(0xC0, 0x39, 0x2B));
        private static readonly SolidColorBrush CuloareBorduraNorm = new SolidColorBrush(Color.FromRgb(0xD7, 0xCC, 0xC8));
        private static readonly SolidColorBrush CuloareBorduraErr = new SolidColorBrush(Color.FromRgb(0xC0, 0x39, 0x2B));

        
        private Parfum _ultimulParfumAdaugat = null;
        private readonly AdminParfumuri _adminParfumuri = new AdminParfumuri();
        private readonly AdminClienti _adminClienti = new AdminClienti();

        //CRUD
        private ObservableCollection<Client> _clientiView = new ObservableCollection<Client>();
        private Client _clientSelectatGestiune = null;
        private bool _modAdaugare = false;

        //BINDING
        public event PropertyChangedEventHandler PropertyChanged;
        private void Notify(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private int _numarClienti = 0;
        public int NumarClienti
        {
            get => _numarClienti;
            set { _numarClienti = value; Notify(nameof(NumarClienti)); }
        }

        
        public MainWindow()
        {
            InitializeComponent();
            NavigheazaCatre(TabParfumuri, BtnNavParfumuri);
            SetStatus("Aplicație pornită. Gata.");
        }

        
        //  NAVIGARE LATERALA
        

        private void BtnNavParfumuri_Click(object sender, RoutedEventArgs e)
            => NavigheazaCatre(TabParfumuri, BtnNavParfumuri);

        private void BtnNavClienti_Click(object sender, RoutedEventArgs e)
            => NavigheazaCatre(TabClienti, BtnNavClienti);

        private void BtnNavCautare_Click(object sender, RoutedEventArgs e)
            => NavigheazaCatre(TabCautare, BtnNavCautare);

        private void BtnNavGestiune_Click(object sender, RoutedEventArgs e)
        {
            RefreshListaGestiune();
            NavigheazaCatre(TabGestiune, BtnNavGestiune);
        }

        private void NavigheazaCatre(TabItem tab, Button butonActiv)
        {
            TabPrincipal.SelectedItem = tab;
            foreach (Button b in new[] { BtnNavParfumuri, BtnNavClienti, BtnNavCautare, BtnNavGestiune })
            {
                b.Background = new SolidColorBrush(Colors.Transparent);
                b.Foreground = new SolidColorBrush(Color.FromRgb(0xD7, 0xCC, 0xC8));
            }
            butonActiv.Background = new SolidColorBrush(Color.FromRgb(0x6D, 0x4C, 0x41));
            butonActiv.Foreground = new SolidColorBrush(Colors.White);
        }

        
        //  MENIU
        

        private void MnuSalveazaParfumuri_Click(object sender, RoutedEventArgs e)
        {
            _adminParfumuri.SalveazaInFisier("parfumuri.txt");
            SetStatus("Parfumurile au fost salvate în parfumuri.txt");
        }

        private void MnuIncarcaParfumuri_Click(object sender, RoutedEventArgs e)
        {
            _adminParfumuri.IncarcaDinFisier("parfumuri.txt");
            SetStatus($"Parfumuri încărcate. Total: {_adminParfumuri.GetToate().Count}");
        }

        private void MnuSalveazaClienti_Click(object sender, RoutedEventArgs e)
        {
            _adminClienti.SalveazaInFisier("clienti.txt");
            SetStatus("Clienții au fost salvați în clienti.txt");
        }

        private void MnuIncarcaClienti_Click(object sender, RoutedEventArgs e)
        {
            _adminClienti.IncarcaDinFisier("clienti.txt");
            SetStatus($"Clienți încărcați. Total: {_adminClienti.GetToate().Count}");
        }

        private void MnuIesire_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Sigur vrei să ieși din EssenceFlow?", "Confirmare",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }

        private void MnuVizParfumuri_Click(object sender, RoutedEventArgs e)
            => NavigheazaCatre(TabParfumuri, BtnNavParfumuri);

        private void MnuVizClienti_Click(object sender, RoutedEventArgs e)
            => NavigheazaCatre(TabClienti, BtnNavClienti);

        private void MnuDespre_Click(object sender, RoutedEventArgs e)
            => MessageBox.Show(
                "EssenceFlow v2.0\nSistem de management pentru magazin de parfumuri.\n\n© 2025",
                "Despre EssenceFlow", MessageBoxButton.OK, MessageBoxImage.Information);

        
        //  TAB PARFUMURI – Adaugare
        

        private void BtnAdauga_Click(object sender, RoutedEventArgs e)
        {
            TxtMesajSucces.Visibility = Visibility.Collapsed;
            if (ValideazaParfum() > 0) return;

            Concentratie concentratie = Concentratie.EDP;
            if (RbEDT.IsChecked == true) concentratie = Concentratie.EDT;
            else if (RbEDC.IsChecked == true) concentratie = Concentratie.EDC;
            else if (RbParfum.IsChecked == true) concentratie = Concentratie.Parfum;

            Sezon sezon = Sezon.Universal;
            if (RbPrimavara.IsChecked == true) sezon = Sezon.Primavara;
            else if (RbVara.IsChecked == true) sezon = Sezon.Vara;
            else if (RbToamna.IsChecked == true) sezon = Sezon.Toamna;
            else if (RbIarna.IsChecked == true) sezon = Sezon.Iarna;

            Ocazie ocazii = Ocazie.Niciuna;
            if (ChkCasual.IsChecked == true) ocazii |= Ocazie.Casual;
            if (ChkBusiness.IsChecked == true) ocazii |= Ocazie.Business;
            if (ChkSeara.IsChecked == true) ocazii |= Ocazie.Seara;
            if (ChkEveniment.IsChecked == true) ocazii |= Ocazie.Eveniment;
            if (ChkSport.IsChecked == true) ocazii |= Ocazie.Sport;
            if (ocazii == Ocazie.Niciuna) ocazii = Ocazie.Casual;

            int.TryParse(TxtCantitate.Text, out int cantitate);
            int.TryParse(TxtStoc.Text, out int stoc);
            decimal.TryParse(TxtPret.Text, out decimal pret);

            var parfum = new Parfum(
                TxtId.Text.Trim(), TxtNume.Text.Trim(), TxtBrand.Text.Trim(),
                concentratie, cantitate, stoc, pret, sezon, ocazii);

            _adminParfumuri.Adauga(parfum);
            _ultimulParfumAdaugat = parfum;

            AfiseazaMesaj(TxtMesajSucces,
                $" Parfumul \"{parfum.Nume}\" a fost adăugat cu succes!",
                Color.FromRgb(0x2E, 0x7D, 0x32));
            SetStatus($"Parfum adăugat: {parfum.Nume} | Brand: {parfum.Brand} | Stoc: {parfum.Stoc}");
            GolesteFormularParfum();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            if (_ultimulParfumAdaugat == null)
            {
                AfiseazaMesaj(TxtMesajSucces, "Nu a fost adăugat niciun parfum încă.", Colors.Gray);
                return;
            }
            var p = _ultimulParfumAdaugat;
            AfiseazaMesaj(TxtMesajSucces,
                $" Ultimul adăugat:\n  ID: {p.Id} | Nume: {p.Nume} | Brand: {p.Brand}\n" +
                $"  {p.Concentratie} | {p.CantitateMl}ml | Stoc: {p.Stoc} | {p.Pret} RON\n" +
                $"  Sezon: {p.SezonRecomandat} | Ocazii: {p.OcaziiPotrivite}",
                Color.FromRgb(0x15, 0x65, 0xC0));
        }

        private void BtnGoleste_Click(object sender, RoutedEventArgs e)
        {
            GolesteFormularParfum();
            TxtMesajSucces.Visibility = Visibility.Collapsed;
        }

        
        //  TAB CLIENTI – Adaugare
        

        private void BtnAdaugaClient_Click(object sender, RoutedEventArgs e)
        {
            TxtMesajClient.Visibility = Visibility.Collapsed;
            if (ValideazaClient() > 0) return;

            int.TryParse(TxtClientPuncte.Text, out int puncte);

            var client = new Client(
                TxtClientId.Text.Trim(),
                TxtClientNume.Text.Trim(),
                TxtClientTelefon.Text.Trim(),
                TxtClientEmail.Text.Trim(),
                puncte);

            if (!client.EsteValid())
            {
                AfiseazaMesaj(TxtMesajClient, " Datele clientului nu sunt valide!", Colors.OrangeRed);
                return;
            }

            _adminClienti.Adauga(client);
            AfiseazaMesaj(TxtMesajClient,
                $" Clientul \"{client.Nume}\" a fost adăugat!",
                Color.FromRgb(0x2E, 0x7D, 0x32));
            SetStatus($"Client adăugat: {client.Nume} | Tel: {client.Telefon}");
            GolesteFormularClient();
        }

        private void BtnGolesteClient_Click(object sender, RoutedEventArgs e)
        {
            GolesteFormularClient();
            TxtMesajClient.Visibility = Visibility.Collapsed;
        }

        
        //  TAB CAUTARE
        

        private void BtnCautareRapida_Click(object sender, RoutedEventArgs e)
        {
            string termen = TxtCautareRapida.Text.Trim();
            if (string.IsNullOrWhiteSpace(termen)) return;
            NavigheazaCatre(TabCautare, BtnNavCautare);
            TxtCautareParfum.Text = termen;
            RbCautBrand.IsChecked = true;
            EfectueazaCautareParfum(termen, "brand");
            TxtCautareClient.Text = termen;
            RbCautClientNume.IsChecked = true;
            EfectueazaCautareClient(termen, "nume");
        }

        private void BtnCautaParfum_Click(object sender, RoutedEventArgs e)
        {
            string termen = TxtCautareParfum.Text.Trim();
            if (string.IsNullOrWhiteSpace(termen)) { SetStatus("Introduceți un termen de căutare."); return; }
            string criteriu = "brand";
            if (RbCautNume.IsChecked == true) criteriu = "nume";
            if (RbCautPretMax.IsChecked == true) criteriu = "pret";
            EfectueazaCautareParfum(termen, criteriu);
        }

        private void EfectueazaCautareParfum(string termen, string criteriu)
        {
            LstRezultateParfumuri.Items.Clear();
            List<Parfum> rezultate;
            switch (criteriu)
            {
                case "nume":
                    rezultate = _adminParfumuri.GetToate()
                        .Where(p => p.Nume.ToLower().Contains(termen.ToLower()))
                        .OrderBy(p => p.Pret).ToList();
                    break;
                case "pret":
                    if (!decimal.TryParse(termen, out decimal pretMax))
                    { SetStatus("Introduceți o valoare numerică pentru preț."); return; }
                    rezultate = _adminParfumuri.CautaDupaPretMaxim(pretMax);
                    break;
                default:
                    rezultate = _adminParfumuri.CautaDupaBrand(termen);
                    break;
            }
            if (rezultate.Count == 0)
            {
                LstRezultateParfumuri.Items.Add("— Niciun rezultat găsit —");
                SetStatus($"Căutare parfumuri după {criteriu}: niciun rezultat.");
                return;
            }
            foreach (var p in rezultate)
                LstRezultateParfumuri.Items.Add(
                    $"{p.Nume}  |  {p.Brand}  |  {p.Concentratie}  |  {p.Pret} RON  |  Stoc: {p.Stoc}  |  {p.SezonRecomandat}");
            SetStatus($"Găsite {rezultate.Count} parfumuri pentru \"{termen}\".");
        }

        private void BtnCautaClient_Click(object sender, RoutedEventArgs e)
        {
            string termen = TxtCautareClient.Text.Trim();
            if (string.IsNullOrWhiteSpace(termen)) { SetStatus("Introduceți un termen de căutare client."); return; }
            string criteriu = RbCautClientTelefon.IsChecked == true ? "telefon" : "nume";
            EfectueazaCautareClient(termen, criteriu);
        }

        private void EfectueazaCautareClient(string termen, string criteriu)
        {
            LstRezultateClienti.Items.Clear();
            List<Client> rezultate;
            if (criteriu == "telefon")
                rezultate = _adminClienti.GetToate()
                    .Where(c => c.Telefon.Contains(termen))
                    .OrderBy(c => c.Nume).ToList();
            else
                rezultate = _adminClienti.CautaDupaNume(termen);

            if (rezultate.Count == 0)
            {
                LstRezultateClienti.Items.Add("— Niciun client găsit —");
                SetStatus($"Căutare clienți după {criteriu}: niciun rezultat.");
                return;
            }
            foreach (var c in rezultate)
                LstRezultateClienti.Items.Add(
                    $"{c.Nume}  |  {c.Telefon}  |  {c.Email}  |  Puncte: {c.PuncteLoialitate}");
            SetStatus($"Găsiți {rezultate.Count} clienți pentru \"{termen}\".");
        }

        
        //  TAB GESTIUNE CRUD BINDING
        

        //  Refresh lista BINDING  
        private void RefreshListaGestiune(string filtru = "")
        {
            _clientiView.Clear();
            var sursa = string.IsNullOrWhiteSpace(filtru)
                ? _adminClienti.GetToate()
                : _adminClienti.CautaDupaNume(filtru);
            foreach (var c in sursa)
                _clientiView.Add(c);

            
            LstClientiGestiune.ItemsSource = null;
            LstClientiGestiune.ItemsSource = _clientiView;

            
            NumarClienti = _adminClienti.GetToate().Count;
        }

        private void TxtFiltruClienti_TextChanged(object sender, TextChangedEventArgs e)
            => RefreshListaGestiune(TxtFiltruClienti.Text.Trim());

        //  READ selectie in lista 
        private void LstClientiGestiune_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LstClientiGestiune.SelectedItem is not Client c) return;
            _clientSelectatGestiune = c;
            _modAdaugare = false;

            TxtGestClientId.Text = c.Id;
            TxtGestClientNume.Text = c.Nume;
            TxtGestClientTelefon.Text = c.Telefon;
            TxtGestClientEmail.Text = c.Email;
            TxtGestClientPuncte.Text = c.PuncteLoialitate.ToString();
            DtpGestDataNasterii.SelectedDate = c.DataNasterii;

            TxtGestDataActualizare.Text = $"Ultima actualizare: {c.DataActualizare:dd.MM.yyyy HH:mm}";
            TxtGestDataActualizare.Visibility = Visibility.Visible;
            TxtTitluFormular.Text = $"Editare: {c.Nume}";
            TxtModCurent.Text = $" Editare client  (ID: {c.Id})";

            AscundeEroariGestiune();
            TxtMesajGestiune.Visibility = Visibility.Collapsed;
        }

        //  CREATE client nou 
        private void BtnGestClientNou_Click(object sender, RoutedEventArgs e)
        {
            _clientSelectatGestiune = null;
            _modAdaugare = true;
            LstClientiGestiune.SelectedItem = null;

            TxtGestClientId.IsReadOnly = false;
            TxtGestClientId.Background = Brushes.White;
            TxtGestClientId.Foreground = Brushes.Black;
            TxtGestClientId.Text = string.Empty;
            TxtGestClientNume.Text = string.Empty;
            TxtGestClientTelefon.Text = string.Empty;
            TxtGestClientEmail.Text = string.Empty;
            TxtGestClientPuncte.Text = "0";
            DtpGestDataNasterii.SelectedDate = null;

            TxtGestDataActualizare.Visibility = Visibility.Collapsed;
            TxtTitluFormular.Text = " Client Nou";
            TxtModCurent.Text = " Adăugare client nou";
            TxtMesajGestiune.Visibility = Visibility.Collapsed;
            AscundeEroariGestiune();
            TxtGestClientId.Focus();
        }

        //  CREATE  UPDATE salvare 
        private void BtnGestSalveaza_Click(object sender, RoutedEventArgs e)
        {
            TxtMesajGestiune.Visibility = Visibility.Collapsed;
            if (!ValideazaFormularGestiune()) return;

            int.TryParse(TxtGestClientPuncte.Text, out int puncte);
            DateTime dataNasterii = DtpGestDataNasterii.SelectedDate ?? DateTime.Today;

            if (_modAdaugare)
            {
                // CREATE
                string idNou = TxtGestClientId.Text.Trim();
                if (_adminClienti.GetToate().Any(c => c.Id == idNou))
                {
                    AfiseazaMesaj(TxtMesajGestiune,
                        " Există deja un client cu acest ID. Alegeți alt ID.", Colors.OrangeRed);
                    return;
                }

                var clientNou = new Client(
                    idNou,
                    TxtGestClientNume.Text.Trim(),
                    TxtGestClientTelefon.Text.Trim(),
                    TxtGestClientEmail.Text.Trim(),
                    puncte, dataNasterii);

                _adminClienti.Adauga(clientNou);
                AfiseazaMesaj(TxtMesajGestiune,
                    $" Clientul \"{clientNou.Nume}\" a fost adăugat cu succes!",
                    Color.FromRgb(0x2E, 0x7D, 0x32));
                SetStatus($"Client nou adăugat: {clientNou.Nume}");

                TxtGestClientId.IsReadOnly = true;
                TxtGestClientId.Background = new SolidColorBrush(Color.FromRgb(0xF5, 0xF5, 0xF5));
                TxtGestClientId.Foreground = Brushes.Gray;
                _modAdaugare = false;
            }
            else
            {
                // UPDATE
                if (_clientSelectatGestiune == null)
                {
                    AfiseazaMesaj(TxtMesajGestiune,
                        " Selectați un client din listă sau apăsați 'Client Nou'.", Colors.OrangeRed);
                    return;
                }

                bool ok = _adminClienti.ModificaClient(
                    _clientSelectatGestiune.Id,
                    TxtGestClientNume.Text.Trim(),
                    TxtGestClientTelefon.Text.Trim(),
                    TxtGestClientEmail.Text.Trim(),
                    puncte, dataNasterii);

                if (!ok) { AfiseazaMesaj(TxtMesajGestiune, " Clientul nu a fost găsit.", Colors.OrangeRed); return; }

                AfiseazaMesaj(TxtMesajGestiune,
                    $" Clientul \"{TxtGestClientNume.Text.Trim()}\" a fost actualizat! DataActualizare: {DateTime.Now:dd.MM.yyyy HH:mm}",
                    Color.FromRgb(0x2E, 0x7D, 0x32));
                SetStatus($"Client actualizat: {TxtGestClientNume.Text.Trim()}");
                TxtGestDataActualizare.Text = $"Ultima actualizare: {DateTime.Now:dd.MM.yyyy HH:mm}";
            }

            RefreshListaGestiune(TxtFiltruClienti.Text.Trim());
        }

        // DELETE 
        private void BtnGestSterge_Click(object sender, RoutedEventArgs e)
        {
            TxtMesajGestiune.Visibility = Visibility.Collapsed;
            if (_clientSelectatGestiune == null)
            {
                AfiseazaMesaj(TxtMesajGestiune,
                    " Selectați un client din listă pentru a-l șterge.", Colors.OrangeRed);
                return;
            }

            var rez = MessageBox.Show(
                $"Ești sigur că vrei să ștergi clientul \"{_clientSelectatGestiune.Nume}\"?\nAceastă acțiune nu poate fi anulată.",
                "Confirmare ștergere", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (rez != MessageBoxResult.Yes) return;

            string nume = _clientSelectatGestiune.Nume;
            _adminClienti.Sterge(_clientSelectatGestiune.Id);
            _clientSelectatGestiune = null;

            BtnGestAnuleaza_Click(null, null);
            AfiseazaMesaj(TxtMesajGestiune, $" Clientul \"{nume}\" a fost șters.",
                Color.FromRgb(0x2E, 0x7D, 0x32));
            SetStatus($"Client șters: {nume}");
            RefreshListaGestiune(TxtFiltruClienti.Text.Trim());
        }

        // Anulare 
        private void BtnGestAnuleaza_Click(object sender, RoutedEventArgs e)
        {
            _clientSelectatGestiune = null;
            _modAdaugare = false;
            LstClientiGestiune.SelectedItem = null;

            TxtGestClientId.IsReadOnly = true;
            TxtGestClientId.Background = new SolidColorBrush(Color.FromRgb(0xF5, 0xF5, 0xF5));
            TxtGestClientId.Foreground = Brushes.Gray;
            TxtGestClientId.Text = string.Empty;
            TxtGestClientNume.Text = string.Empty;
            TxtGestClientTelefon.Text = string.Empty;
            TxtGestClientEmail.Text = string.Empty;
            TxtGestClientPuncte.Text = string.Empty;
            DtpGestDataNasterii.SelectedDate = null;

            TxtGestDataActualizare.Visibility = Visibility.Collapsed;
            TxtMesajGestiune.Visibility = Visibility.Collapsed;
            TxtTitluFormular.Text = "Selectează un client din listă pentru a edita";
            TxtModCurent.Text = "— Niciun client selectat —";
            AscundeEroariGestiune();
        }

        
        //  VALIDARE
        

        private int ValideazaParfum()
        {
            int erori = 0;
            bool idOk = !string.IsNullOrWhiteSpace(TxtId.Text);
            MarcheazaCamp(TxtId, ErrId, LblId, idOk); if (!idOk) erori++;

            bool numeOk = !string.IsNullOrWhiteSpace(TxtNume.Text) && TxtNume.Text.Trim().Length <= MAX_LUNGIME_NUME;
            MarcheazaCamp(TxtNume, ErrNume, LblNume, numeOk); if (!numeOk) erori++;

            bool brandOk = !string.IsNullOrWhiteSpace(TxtBrand.Text) && TxtBrand.Text.Trim().Length <= MAX_LUNGIME_BRAND;
            MarcheazaCamp(TxtBrand, ErrBrand, LblBrand, brandOk); if (!brandOk) erori++;

            bool cantOk = int.TryParse(TxtCantitate.Text, out int c) && c >= MIN_CANTITATE_ML && c <= MAX_CANTITATE_ML;
            MarcheazaCamp(TxtCantitate, ErrCantitate, LblCantitate, cantOk); if (!cantOk) erori++;

            bool stocOk = int.TryParse(TxtStoc.Text, out int s) && s >= MIN_STOC && s <= MAX_STOC;
            MarcheazaCamp(TxtStoc, ErrStoc, LblStoc, stocOk); if (!stocOk) erori++;

            bool pretOk = decimal.TryParse(TxtPret.Text, out decimal p) && p >= MIN_PRET && p <= MAX_PRET;
            MarcheazaCamp(TxtPret, ErrPret, LblPret, pretOk); if (!pretOk) erori++;

            bool concOk = RbEDP.IsChecked == true || RbEDT.IsChecked == true ||
                          RbEDC.IsChecked == true || RbParfum.IsChecked == true;
            ErrConcentratie.Visibility = concOk ? Visibility.Collapsed : Visibility.Visible;
            if (!concOk) erori++;

            bool sezonOk = RbPrimavara.IsChecked == true || RbVara.IsChecked == true ||
                           RbToamna.IsChecked == true || RbIarna.IsChecked == true ||
                           RbUniversal.IsChecked == true;
            ErrSezon.Visibility = sezonOk ? Visibility.Collapsed : Visibility.Visible;
            if (!sezonOk) erori++;

            return erori;
        }

        private int ValideazaClient()
        {
            int erori = 0;
            bool idOk = !string.IsNullOrWhiteSpace(TxtClientId.Text);
            MarcheazaCamp(TxtClientId, ErrClientId, LblClientId, idOk); if (!idOk) erori++;

            bool numeOk = !string.IsNullOrWhiteSpace(TxtClientNume.Text);
            MarcheazaCamp(TxtClientNume, ErrClientNume, LblClientNume, numeOk); if (!numeOk) erori++;

            bool telOk = !string.IsNullOrWhiteSpace(TxtClientTelefon.Text);
            MarcheazaCamp(TxtClientTelefon, ErrClientTelefon, LblClientTelefon, telOk); if (!telOk) erori++;

            bool emailOk = !string.IsNullOrWhiteSpace(TxtClientEmail.Text) &&
                           TxtClientEmail.Text.Contains("@");
            MarcheazaCamp(TxtClientEmail, ErrClientEmail, LblClientEmail, emailOk); if (!emailOk) erori++;

            bool puncteOk = int.TryParse(TxtClientPuncte.Text, out int pt) && pt >= 0;
            MarcheazaCamp(TxtClientPuncte, ErrClientPuncte, LblClientPuncte, puncteOk); if (!puncteOk) erori++;

            return erori;
        }

        private bool ValideazaFormularGestiune()
        {
            bool valid = true;
            if (_modAdaugare)
            {
                bool idOk = !string.IsNullOrWhiteSpace(TxtGestClientId.Text);
                ErrGestId.Visibility = idOk ? Visibility.Collapsed : Visibility.Visible;
                if (!idOk) valid = false;
            }
            bool numeOk = !string.IsNullOrWhiteSpace(TxtGestClientNume.Text);
            ErrGestNume.Visibility = numeOk ? Visibility.Collapsed : Visibility.Visible;
            if (!numeOk) valid = false;

            bool telOk = !string.IsNullOrWhiteSpace(TxtGestClientTelefon.Text);
            ErrGestTelefon.Visibility = telOk ? Visibility.Collapsed : Visibility.Visible;
            if (!telOk) valid = false;

            bool emailOk = !string.IsNullOrWhiteSpace(TxtGestClientEmail.Text)
                           && TxtGestClientEmail.Text.Contains("@");
            ErrGestEmail.Visibility = emailOk ? Visibility.Collapsed : Visibility.Visible;
            if (!emailOk) valid = false;

            bool puncteOk = int.TryParse(TxtGestClientPuncte.Text, out int pt) && pt >= 0;
            ErrGestPuncte.Visibility = puncteOk ? Visibility.Collapsed : Visibility.Visible;
            if (!puncteOk) valid = false;

            return valid;
        }

        private void AscundeEroariGestiune()
        {
            ErrGestId.Visibility = Visibility.Collapsed;
            ErrGestNume.Visibility = Visibility.Collapsed;
            ErrGestTelefon.Visibility = Visibility.Collapsed;
            ErrGestEmail.Visibility = Visibility.Collapsed;
            ErrGestPuncte.Visibility = Visibility.Collapsed;
        }

        
        //  HELPERS UI
        

        private void MarcheazaCamp(TextBox txt, TextBlock err, Label lbl, bool valid)
        {
            txt.BorderBrush = valid ? CuloareBorduraNorm : CuloareBorduraErr;
            err.Visibility = valid ? Visibility.Collapsed : Visibility.Visible;
            lbl.Foreground = valid ? CuloareLabelNormal : CuloareLabelEroare;
        }

        private void AfiseazaMesaj(TextBlock bloc, string text, Color culoare)
        {
            bloc.Text = text;
            bloc.Foreground = new SolidColorBrush(culoare);
            bloc.Visibility = Visibility.Visible;
        }

        private void SetStatus(string mesaj)
            => TxtStatus.Text = $"[{DateTime.Now:HH:mm:ss}] {mesaj}";

        private void GolesteFormularParfum()
        {
            TxtId.Text = TxtNume.Text = TxtBrand.Text =
            TxtCantitate.Text = TxtStoc.Text = TxtPret.Text = string.Empty;
            RbEDP.IsChecked = true;
            RbUniversal.IsChecked = true;
            ChkCasual.IsChecked = ChkBusiness.IsChecked = ChkSeara.IsChecked =
            ChkEveniment.IsChecked = ChkSport.IsChecked = false;
            foreach (var tb in new[] { ErrId, ErrNume, ErrBrand, ErrCantitate, ErrStoc, ErrPret, ErrConcentratie, ErrSezon })
                tb.Visibility = Visibility.Collapsed;
            foreach (var lbl in new[] { LblId, LblNume, LblBrand, LblCantitate, LblStoc, LblPret })
                lbl.Foreground = CuloareLabelNormal;
        }

        private void GolesteFormularClient()
        {
            TxtClientId.Text = TxtClientNume.Text = TxtClientTelefon.Text =
            TxtClientEmail.Text = string.Empty;
            TxtClientPuncte.Text = "0";
            RbContactEmail.IsChecked = true;
            ChkPrefEDP.IsChecked = ChkPrefEDT.IsChecked = ChkPrefFloral.IsChecked =
            ChkPrefLemn.IsChecked = ChkPrefFruct.IsChecked = ChkPrefOri.IsChecked = false;
            foreach (var tb in new[] { ErrClientId, ErrClientNume, ErrClientTelefon, ErrClientEmail, ErrClientPuncte })
                tb.Visibility = Visibility.Collapsed;
        }
    }
}