EssenceFlow – Magazin de parfumuri
________________________________________ 
(UI Design)
Comertul cu parfumuri reflecta lux si prestanta.
Cromatică: Paletă de culori  ( Negru antracit și Alb perlat).
Layout: Un meniu lateral (Sidebar) pentru navigare rapidă și o zonă centrală de tip "Dashboard".
________________________________________
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

________________________________________
3. Fluxuri de Lucru .
Funcție	Descriere Logică
Cumpără Parfum	Selectezi parfumul -> Selectezi clientul (opțional) -> Scazi stocul cu 1 .
Returnează Parfum	Cauti tranzacția/parfumul -> Crești stocul cu 1 .
Actualizează Listă	Un buton de "Refresh" care reîncarcă datele din baza de date .
________________________________________
