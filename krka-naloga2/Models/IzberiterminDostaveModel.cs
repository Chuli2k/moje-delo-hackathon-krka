using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace krka_naloga2.Models
{
    public class IzberiterminDostaveModel
    {
        public string SifraDostave { get; set; }
        public IList<IzbiraTerminaModel> SeznaTerminov { get; set; }
    }
}
