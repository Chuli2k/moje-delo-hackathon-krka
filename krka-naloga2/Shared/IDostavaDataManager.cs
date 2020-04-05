using krka_naloga2.Data;
using krka_naloga2.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace krka_naloga2.Shared
{
    public interface IDostavaDataManager
    {
        Task<IEnumerable<Dostava>> GetAllSeznamDostavAsync(DateTime from, DateTime to, Uporabnik uporabnik, int pageSize, int pageStart);
        Skladisce GetSkladisceFromSifraDostave(string sifraDostave);
        Dostava GetDostava(string sifraDostave);
        Task<string> CheckEditDostavaAsync(Dostava dostava, Uporabnik uporabnik);
        IEnumerable<TerminTockeSkladisca> GetAllZasedeniTermini(DateTime start, DateTime end, int skladisceId);
        Task<IEnumerable<Uporabnik>> GetAllUporabnikiForVnosDostaveAsync();
        Task UrediDostavoAsync(IzberiterminDostaveModel podatki, Uporabnik uporabnikPrijave);
        Task DodajDostavoAsync(IzberiterminDostaveModel podatki, Uporabnik uporabnikPrijave);
        void DeleteDostava(string sifraDostave);
        void SetStatusDostave(string sifraDostave, StatusDostave status);
    }
}
