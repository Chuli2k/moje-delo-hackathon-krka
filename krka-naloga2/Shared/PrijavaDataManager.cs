using krka_naloga2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace krka_naloga2.Shared
{

    public class PrijavaDataManager : IPrijavaDataManager
    {
        private readonly IKrkaRepo _krkaRepo;
        public PrijavaDataManager(IKrkaRepo krkaRepo)
        {
            _krkaRepo = krkaRepo;
        }

        public IEnumerable<Podjetje> GetAllPodjetja()
        {
            return _krkaRepo.GetAllPodjetja();
        }
    }
}
