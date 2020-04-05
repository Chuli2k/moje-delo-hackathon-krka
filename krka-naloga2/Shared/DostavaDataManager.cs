using krka_naloga2.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace krka_naloga2.Shared
{
    public class DostavaDataManager : IDostavaDataManager
    {
        private readonly IKrkaRepo _krkaRepo;
        private readonly UserManager<Uporabnik> _userManager;

        public DostavaDataManager(
            IKrkaRepo krkaRepo,
            UserManager<Uporabnik> userManager)
        {
            _krkaRepo = krkaRepo;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Dostava>> GetAllSeznamDostavAsync(DateTime from, DateTime to, Uporabnik uporabnik)
        {
            if (uporabnik == null)
                throw new ArgumentNullException(nameof(uporabnik));
            if (from == null)
                throw new ArgumentNullException(nameof(from));
            if (to == null)
                throw new ArgumentNullException(nameof(to));

            var roles = await _userManager.GetRolesAsync(uporabnik);

            Expression<Func<Dostava, bool>> filter = d => false; //Ne vrni nič

            if (roles.Contains("Admin"))
            {
                filter = null; //Vrni vse
            } 
            else if(roles.Contains("Uporabnik"))
            {
                filter = d => d.UporabnikId == uporabnik.Id; //Vrni vse za uporabnika
            } 
            else if (roles.Contains("Skladiscnik"))
            {
                filter = d => d.TockaSkladisca.SkladisceId == 1; //TODO: filter za skladiščnika
            }

            var dostave = _krkaRepo.GetAllDostave(from, to, filter);

            return dostave;
        }
    }

    public interface IDostavaDataManager
    {
        Task<IEnumerable<Dostava>> GetAllSeznamDostavAsync(DateTime from, DateTime to, Uporabnik uporabnik);
    }
}
