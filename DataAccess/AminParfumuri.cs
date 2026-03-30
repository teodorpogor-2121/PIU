using System.Collections.Generic;
using System.Linq;
using Modele;

namespace DataAccess
{
    public class AdminParfumuri

    {
        private List<Parfum> _parfumuri = new List<Parfum>();

        public void Adauga(Parfum parfum)
        {
            _parfumuri.Add(parfum);
        }

        public List<Parfum> GetToate()
        {
            return _parfumuri.ToList();
        }

        public List<Parfum> CautaDupaBrand(string brand)
        {
            return _parfumuri
                .Where(p => p.Brand.ToLower().Contains(brand.ToLower()))
                .OrderBy(p => p.Pret)
                .ToList();
        }

        public List<Parfum> CautaDupaPretMaxim(decimal pretMax)
        {
            return _parfumuri
                .Where(p => p.Pret <= pretMax)
                .OrderBy(p => p.Pret)
                .ToList();
        }
    }
}