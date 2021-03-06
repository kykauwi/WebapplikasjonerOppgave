using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Webapplikasjoner1.DAL;
using Webapplikasjoner1.Models;
using Webapplikasjoner1.Validation;
using Microsoft.AspNetCore.Http;

namespace Webapplikasjoner1.Controllers
{
    [Route("[controller]/[action]")]
    public class AdminsController : ControllerBase
    {
        private readonly IAdminRepository _db;
        private ILogger<AdminsController> _log;
        private const string _loggetInn = "loggetInn";

        public AdminsController(IAdminRepository db, ILogger<AdminsController> log)
        {
            _db = db;
            _log = log;
        }

        //Legg inn ting her
        public async Task<ActionResult> LoggInn(Admin admin)
        {
            bool validerBrukernavn = Validering.GyldigBrukernavn(admin.Brukernavn);
            bool validerPassord = Validering.GyldigPassord(admin.Passord);

            if (validerBrukernavn && validerPassord)
            {
                bool returOK = await _db.LoggInn(admin);
                if (!returOK)
                {
                    _log.LogInformation("Kunne ikke logge inn admin " + admin.Brukernavn);
                    HttpContext.Session.SetString(_loggetInn,"");
                    return Unauthorized(false);
                }
                _log.LogInformation("Bruker ble logget inn");
                HttpContext.Session.SetString(_loggetInn, "loggetInn");
                return Ok(true);
            }
            _log.LogInformation("Feil i inputvalidering");
            HttpContext.Session.SetString(_loggetInn,"");
            return BadRequest("Feil i inputvalidering på server");
        }

        public void LoggUt()
        {
            HttpContext.Session.SetString(_loggetInn,"");
            _log.LogInformation("Admin ble logget ut");
        }

        public async Task<ActionResult> SjekkLoggetInn()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Ikke logget inn");
            }

            return Ok("Logget inn");
        }
    }
}
