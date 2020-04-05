using krka_naloga2.Data;
using krka_naloga2.Models;
using System.Threading.Tasks;

namespace krka_naloga2.Shared
{
    public interface IModelValidator
    {
        string ValidateVnosSifraDostave(string sifraDostave);
        string ValidateSifraDostave(string sifraDostave);
        Task<string> ValidateVnosTermina(IzberiterminDostaveModel podatki, Uporabnik uporabnikPrijave);
    }
}
