using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Webapplikasjoner1.Models;

namespace Webapplikasjoner1.DAL
{
    public class LokasjonRepository : ILokasjonRepository
    {
        private readonly BillettKontekst _db;
        private ILogger<LokasjonRepository> _log;

        public LokasjonRepository(BillettKontekst db,ILogger<LokasjonRepository> log )
        {
            _db = db;
            _log = log;
        }
        
        // Legg til, Slett og hentAlle

        public async Task<bool> RegistrerLokasjon(Lokasjon lokasjon)
        {
            try
            {
                var nyLokasjonRad = new Lokasjoner();
                
                nyLokasjonRad.StedsNummer = lokasjon.StedsNummer;
                nyLokasjonRad.Stedsnavn = lokasjon.Stedsnavn;
                
                _db.Lokasjonene.Add(nyLokasjonRad);
                await _db.SaveChangesAsync();
                return true;
            }
            catch(Exception e)
            {
                _log.LogInformation("Kunne ikke registrere ny lokasjon");
                _log.LogInformation(e.Message);
                return false;
            }
        }
        
        public async Task<bool> SlettLokasjon(string id)
        {
            try
            {
                Lokasjoner lokasjon = await _db.Lokasjonene.FindAsync(id);
                _db.Lokasjonene.Remove(lokasjon);
                await _db.SaveChangesAsync();
                return true;  
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Lokasjon>> HentAlle()
        {
            try
            {
                List<Lokasjon> alleLokasjoner = await _db.Lokasjonene.Select(l => new Lokasjon()
                {
                    StedsNummer = l.StedsNummer,
                    Stedsnavn = l.Stedsnavn,
                }).ToListAsync();
                return alleLokasjoner;
            }
            catch
            {
                _log.LogInformation("Kunne ikke hente lokasjoner fra database");
                return null;
            }
        }

        public async Task<Lokasjon> HentEn(string id)
        {
            Lokasjoner lokasjon = await _db.Lokasjonene.FindAsync(id);
            
            if (lokasjon == null)//Returnerer null som gir en NotFound error i controlleren
            {
                return null;
            }

            var hentetLokasjon = new Lokasjon()
            {
               StedsNummer = lokasjon.StedsNummer,
                Stedsnavn = lokasjon.Stedsnavn,
            };
            return hentetLokasjon;
        }
    }
}