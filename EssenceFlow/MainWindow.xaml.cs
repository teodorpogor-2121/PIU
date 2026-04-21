using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Modele;
using DataAccess;

namespace EssenceFlow
{
    public partial class MainWindow : Window
    {
        
        private const int MAX_LUNGIME_NUME = 50;
        private const int MAX_LUNGIME_BRAND = 30;
        private const int MIN_CANTITATE_ML = 1;
        private const int MAX_CANTITATE_ML = 1000;
        private const int MIN_STOC = 0;
        private const int MAX_STOC = 9999;
        private const decimal MIN_PRET = 0.01m;
        private const decimal MAX_PRET = 99999m;

        
        private static readonly SolidColorBrush CuloareLabelNormal =
            new SolidColorBrush(Color.FromRgb(0x3E, 0x27, 0x23));
        private static readonly SolidColorBrush CuloareLabelEroare =
            new SolidColorBrush(Color.FromRgb(0xC0, 0x39, 0x2B));
        private static readonly SolidColorBrush CuloareBorduraNormala =
            new SolidColorBrush(Color.FromRgb(0xD7, 0xCC, 0xC8));
        private static readonly SolidColorBrush CuloareBorduraEroare =
            new SolidColorBrush(Color.FromRgb(0xC0, 0x39, 0x2B));

        
        private Parfum _ultimulParfumAdaugat = null;
        private AdminParfumuri _adminParfumuri = new AdminParfumuri();

        public MainWindow()
        {
            InitializeComponent();
            CmbConcentratie.SelectedIndex = 0;
            CmbSezon.SelectedIndex = 4; 
        }

        
        private void BtnAdauga_Click(object sender, RoutedEventArgs e)
        {
            TxtMesajSucces.Visibility = Visibility.Collapsed;

            int nrErori = Valideaza();
            if (nrErori > 0) return;

            Concentratie concentratie = (Concentratie)CmbConcentratie.SelectedIndex;
            Sezon sezon = (Sezon)Enum.Parse(
                typeof(Sezon),
                ((ComboBoxItem)CmbSezon.SelectedItem).Tag.ToString());

            int.TryParse(TxtCantitate.Text, out int cantitate);
            int.TryParse(TxtStoc.Text, out int stoc);
            decimal.TryParse(TxtPret.Text, out decimal pret);

            Parfum parfumNou = new Parfum(
                TxtId.Text.Trim(),
                TxtNume.Text.Trim(),
                TxtBrand.Text.Trim(),
                concentratie,
                cantitate,
                stoc,
                pret,
                sezon
            );

            _adminParfumuri.Adauga(parfumNou);
            _ultimulParfumAdaugat = parfumNou;

            TxtMesajSucces.Text = $"✔ Parfumul \"{parfumNou.Nume}\" a fost adăugat!";
            TxtMesajSucces.Foreground = new SolidColorBrush(Color.FromRgb(0x2E, 0x7D, 0x32));
            TxtMesajSucces.Visibility = Visibility.Visible;

            GolesteFormular();
        }

        
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            if (_ultimulParfumAdaugat == null)
            {
                TxtMesajSucces.Text = "Nu a fost adăugat niciun parfum încă.";
                TxtMesajSucces.Foreground = new SolidColorBrush(Colors.Gray);
                TxtMesajSucces.Visibility = Visibility.Visible;
                return;
            }

            LblId.Content = $"ID Parfum:    {_ultimulParfumAdaugat.Id}";
            LblNume.Content = $"Nume:         {_ultimulParfumAdaugat.Nume}";
            LblBrand.Content = $"Brand:        {_ultimulParfumAdaugat.Brand}";
            LblConcentratie.Content = $"Concentrație: {_ultimulParfumAdaugat.Concentratie}";
            LblCantitate.Content = $"Cantitate:    {_ultimulParfumAdaugat.CantitateMl} ml";
            LblStoc.Content = $"Stoc:         {_ultimulParfumAdaugat.Stoc} buc";
            LblPret.Content = $"Preț:         {_ultimulParfumAdaugat.Pret} RON";
            LblSezon.Content = $"Sezon:        {_ultimulParfumAdaugat.SezonRecomandat}";

            TxtMesajSucces.Text = " Date afișate pentru ultimul parfum adăugat.";
            TxtMesajSucces.Foreground = new SolidColorBrush(Color.FromRgb(0x15, 0x65, 0xC0));
            TxtMesajSucces.Visibility = Visibility.Visible;
        }

        
        private int Valideaza()
        {
            int nrErori = 0;

            bool idValid = !string.IsNullOrWhiteSpace(TxtId.Text);
            MarcheazaCamp(TxtId, ErrId, LblId, idValid);
            if (!idValid) nrErori++;

            bool numeValid = !string.IsNullOrWhiteSpace(TxtNume.Text)
                             && TxtNume.Text.Trim().Length <= MAX_LUNGIME_NUME;
            MarcheazaCamp(TxtNume, ErrNume, LblNume, numeValid);
            if (!numeValid) nrErori++;

            bool brandValid = !string.IsNullOrWhiteSpace(TxtBrand.Text)
                              && TxtBrand.Text.Trim().Length <= MAX_LUNGIME_BRAND;
            MarcheazaCamp(TxtBrand, ErrBrand, LblBrand, brandValid);
            if (!brandValid) nrErori++;

            bool concentratieValida = CmbConcentratie.SelectedIndex >= 0;
            MarcheazaComboBox(CmbConcentratie, ErrConcentratie, LblConcentratie, concentratieValida);
            if (!concentratieValida) nrErori++;

            bool cantitateValida = int.TryParse(TxtCantitate.Text, out int cant)
                                   && cant >= MIN_CANTITATE_ML
                                   && cant <= MAX_CANTITATE_ML;
            MarcheazaCamp(TxtCantitate, ErrCantitate, LblCantitate, cantitateValida);
            if (!cantitateValida) nrErori++;

            bool stocValid = int.TryParse(TxtStoc.Text, out int stoc)
                             && stoc >= MIN_STOC
                             && stoc <= MAX_STOC;
            MarcheazaCamp(TxtStoc, ErrStoc, LblStoc, stocValid);
            if (!stocValid) nrErori++;

            bool pretValid = decimal.TryParse(TxtPret.Text, out decimal pret)
                             && pret >= MIN_PRET
                             && pret <= MAX_PRET;
            MarcheazaCamp(TxtPret, ErrPret, LblPret, pretValid);
            if (!pretValid) nrErori++;

            bool sezonValid = CmbSezon.SelectedIndex >= 0;
            MarcheazaComboBox(CmbSezon, ErrSezon, LblSezon, sezonValid);
            if (!sezonValid) nrErori++;

            return nrErori;
        }

        //  textbox ca invalid sau valid 
        private void MarcheazaCamp(TextBox textBox, TextBlock errBlock,
                                   Label label, bool esteValid)
        {
            if (esteValid)
            {
                textBox.BorderBrush = CuloareBorduraNormala;
                errBlock.Visibility = Visibility.Collapsed;
                label.Foreground = CuloareLabelNormal;
            }
            else
            {
                textBox.BorderBrush = CuloareBorduraEroare;
                errBlock.Visibility = Visibility.Visible;
                label.Foreground = CuloareLabelEroare;
            }
        }

        //  marcheaza ca valid & invalid
        private void MarcheazaComboBox(ComboBox combo, TextBlock errBlock,
                                       Label label, bool esteValid)
        {
            if (esteValid)
            {
                combo.BorderBrush = CuloareBorduraNormala;
                errBlock.Visibility = Visibility.Collapsed;
                label.Foreground = CuloareLabelNormal;
            }
            else
            {
                combo.BorderBrush = CuloareBorduraEroare;
                errBlock.Visibility = Visibility.Visible;
                label.Foreground = CuloareLabelEroare;
            }
        }

        //  goleste formularul dupa adaug
        private void GolesteFormular()
        {
            TxtId.Text = string.Empty;
            TxtNume.Text = string.Empty;
            TxtBrand.Text = string.Empty;
            TxtCantitate.Text = string.Empty;
            TxtStoc.Text = string.Empty;
            TxtPret.Text = string.Empty;
            CmbConcentratie.SelectedIndex = 0;
            CmbSezon.SelectedIndex = 4;

            LblId.Content = "_ID Parfum";
            LblNume.Content = "_Nume Parfum";
            LblBrand.Content = "_Brand";
            LblConcentratie.Content = "_Concentrație";
            LblCantitate.Content = "Cantitate (_ml)";
            LblStoc.Content = "_Stoc (buc)";
            LblPret.Content = "_Preț (RON)";
            LblSezon.Content = "_Sezon Recomandat";
        }
    }
}
