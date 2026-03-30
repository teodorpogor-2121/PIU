EssenceFlow – Magazin de parfumuri
(UI Design)
Comertul cu parfumuri reflecta lux si prestanta.
Cromatică: Paletă de culori  ( Negru antracit și Alb perlat).
Layout: Un meniu lateral (Sidebar) pentru navigare rapidă și o zonă centrală de tip "Dashboard".

2. Structura Aplicației 
A. Dashboard-ul Principal
Statistici rapide: Număr total de parfumuri în stoc, număr de clienți fideli și vânzările zilei.
Acces rapid: Butoane mari pentru "Vânzare Nouă" și "Adăugare Client".
B. Gestiune Parfumuri (Inventory)
Tabel/Grid: O listă cu coloane pentru: Nume, Brand, Concentrație (EDP/EDT), Cantitate (ml), Stoc și Preț.
Funcția Caută: Un search bar care filtrează lista în timp real pe măsură ce tastezi.
Acțiuni: Butoane de Modifică (deschide un pop-up) și Șterge (cu confirmare).
C. Gestiune Clienți Fideli
Lista Clienților: Include detalii precum Nume, Telefon, Email și Puncte de loialitate.
Funcții: * Adaugă/Modifică Client: Formular validat (ex: nu poți salva fără număr de telefon).


  3.Cum lucreaza

Cumpără Parfum	Selectezi parfumul < Selectezi clientul  < Scazi stocul cu 1 .
Returnează Parfum	Cauti tranzacția/parfumul < Crești stocul cu 1 .
Actualizează Listă	Un buton de "Refresh" care reîncarcă datele din baza de date .

Parfumuri/
 Class1.cs       # Model Parfum
 Class2.cs       # Model Client
 Class3.cs       # Model Tranzactie
 Class4.cs       # Model Promotie
 Class5.cs       # Model Furnizor
Class6.cs       # Model Angajat
 .cs             # Program + Enum-uri (Concentratie, Rol)
 Parfumuri.csproj
