using krka_naloga2.Data;
using System.Collections.Generic;

namespace krka_naloga2.Shared
{
    public interface IPrijavaDataManager
    {
        IEnumerable<Podjetje> GetAllPodjetja();
    }
}
