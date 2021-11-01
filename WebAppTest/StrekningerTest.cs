﻿using System;
using Xunit;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Webapplikasjoner1.Controllers;
using Webapplikasjoner1.DAL;
using Webapplikasjoner1.Models;

namespace WebAppTest
{
    public class StrekningerTest
    {
        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";
        

        private readonly Mock<IStrekningRepository> mockRep = new Mock<IStrekningRepository>();
        private readonly Mock<ILogger<StrekningController>> mockLog = new Mock<ILogger<StrekningController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();
        
        // Tester for Lagre Strekning i controller
        [Fact]
        public async Task LagreStrekningLoggInnOk()
        {
            Strekning strekning = new Strekning()
            {
                FraSted = 1,
                TilSted = 2,
            };
            
            mockRep.Setup(s => s.LagreStrekning(strekning)).ReturnsAsync(true);

            var strekningController = new StrekningController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            strekningController.ControllerContext.HttpContext = mockHttpContext.Object;
            
            // Act
            var resultat = await strekningController.LagreStrekning(strekning) as OkObjectResult;
            
            // Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal("Strekning lagret", resultat.Value);
        }

        [Fact]
        public async Task LagreStrekningIkkeLoggetInn()
        {
            mockRep.Setup(s => s.LagreStrekning(It.IsAny<Strekning>())).ReturnsAsync(true);

            var strekningController = new StrekningController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            strekningController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await strekningController.LagreStrekning(It.IsAny<Strekning>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }
        
        [Fact]
        public async Task LagreStrekningValideringFeilFraSted()
        {
            Strekning strekning = new Strekning()
            {
                FraSted = -3000,
                TilSted = 2,
            };
            
            mockRep.Setup(s => s.LagreStrekning(strekning)).ReturnsAsync(true);

            var strekningController = new StrekningController(mockRep.Object, mockLog.Object);

            strekningController.ModelState.AddModelError("FraSted", "Feil i inputvalidering av strekning på server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            strekningController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await strekningController.LagreStrekning(strekning) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering av strekning på server", resultat.Value);
        }
        
        [Fact]
        public async Task LagreStrekningValideringFeilTilSted()
        {
            Strekning strekning = new Strekning()
            {
                FraSted = 2,
                TilSted = -3000,
            };
            
            mockRep.Setup(s => s.LagreStrekning(strekning)).ReturnsAsync(true);

            var strekningController = new StrekningController(mockRep.Object, mockLog.Object);

            strekningController.ModelState.AddModelError("TilSted", "Feil i inputvalidering av strekning på server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            strekningController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await strekningController.LagreStrekning(strekning) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering av strekning på server", resultat.Value);
        }
        
        [Fact]
        public async Task LagreStrekningFeilIDb()
        {
            Strekning strekning = new Strekning()
            {
                FraSted = 2,
                TilSted = 3,
            };
            
            mockRep.Setup(k => k.LagreStrekning(strekning)).ReturnsAsync(false);

            var strekningController = new StrekningController(mockRep.Object, mockLog.Object);
            

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            strekningController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await strekningController.LagreStrekning(strekning) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Strekningen ble ikke lagret", resultat.Value);
        }

        // Tester for Endre Strekning i controller
        [Fact]
        public async Task EndreStrekningLoggInnOk()
        {


        }
        
        [Fact]
        public async Task EndreStrekningIkkeLoggetInn()
        {


        }
        
        [Fact]
        public async Task EndreStrekningValideringFeil()
        {


        }
        [Fact]
        public async Task EndreStrekningFeilIDb()
        {


        }
        
        // Tester for Slett Strekning
        [Fact]
        public async Task SlettStrekningLoggInnOk()
        {


        } 
        
        [Fact]
        public async Task SlettStrekningIkkeLoggetInn()
        {


        } 
        
        [Fact]
        public async Task SlettStrekningFeilIDB()
        {


        }
        
        // Tester for HentAlle Strekninger
        
        [Fact]
        public async Task HentAlleLoggetInnOk()
        {


        }
        
        [Fact]
        public async Task HentAlleIkkeLoggetInn()
        {


        }
        // Tester for HentEn Strekning
        
        [Fact]
        public async Task HentEnIkkeLoggetInn()
        {


        }
        [Fact]
        public async Task HentEnLoggetInnOk()
        {


        }
        
        [Fact]
        public async Task HentEnFeilIDB()
        {


        }
       
        
    }
}