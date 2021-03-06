using System;
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
    public class AdminTest
    {
        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";
        

        private readonly Mock<IAdminRepository> mockRep = new Mock<IAdminRepository>();
        private readonly Mock<ILogger<AdminsController>> mockLog = new Mock<ILogger<AdminsController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();
        
        // Tester for Logg inn
        [Fact]
        public async Task LoggInnOk()
        {
            // Arrange
            Admin admin = new Admin()
            {
                Brukernavn = "adminbruker",
                Passord = "Test12345",
            };
            mockRep.Setup(a => a.LoggInn(admin)).ReturnsAsync(true);

            var adminController = new AdminsController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            adminController.ControllerContext.HttpContext = mockHttpContext.Object;

            var resultat = await adminController.LoggInn(admin) as OkObjectResult;
            
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.True((bool)resultat.Value);
        }

        [Fact]
        public async Task LoggInnFeilPassordEllerBruker()
        {
            // Arrange
            Admin admin = new Admin()
            {
                Brukernavn = "Adminbruker",
                Passord = "Test1234",
            };
            mockRep.Setup(a => a.LoggInn(admin)).ReturnsAsync(false);
            
            
            var adminController = new AdminsController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            adminController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await adminController.LoggInn(admin) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.False((bool)resultat.Value);
        }
        
        [Fact]
        public async Task LoggInnInputFeilPassord()
        {
            // Arrange
            Admin admin = new Admin()
            {
                Brukernavn = "Adminbruker",
                Passord = "feil",
            };
            
            mockRep.Setup(k => k.LoggInn(admin)).ReturnsAsync(true);

            var adminController = new AdminsController(mockRep.Object, mockLog.Object);

            adminController.ModelState.AddModelError("Brukernavn", "Feil i inputvalidering p?? server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            adminController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await adminController.LoggInn(admin) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering p?? server", resultat.Value);
        }
        
        [Fact]
        public async Task LoggInnInputFeilBrukernavn()
        {
            // Arrange
            Admin admin = new Admin()
            {
                Brukernavn = "f",
                Passord = "Test12345",
            };
            
            mockRep.Setup(k => k.LoggInn(admin)).ReturnsAsync(true);

            var adminController = new AdminsController(mockRep.Object, mockLog.Object);

            adminController.ModelState.AddModelError("Brukernavn", "Feil i inputvalidering p?? server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            adminController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await adminController.LoggInn(admin) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering p?? server", resultat.Value);
        }

        
        [Fact]
        public async Task LoggInnInputFeilPassordOgBrukernavn()
        {
            // Arrange
            Admin admin = new Admin()
            {
                Brukernavn = "feil",
                Passord = "feil",
            };
            
            mockRep.Setup(k => k.LoggInn(admin)).ReturnsAsync(true);

            var adminController = new AdminsController(mockRep.Object, mockLog.Object);

            adminController.ModelState.AddModelError("Brukernavn", "Feil i inputvalidering p?? server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            adminController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await adminController.LoggInn(admin) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering p?? server", resultat.Value);
        }

        // Tester for logg ut 
        [Fact]
        public void LoggUt()
        {
            // Arrange
            var adminController = new AdminsController(mockRep.Object, mockLog.Object);
            
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            mockSession[_loggetInn] = _loggetInn;
            adminController.ControllerContext.HttpContext = mockHttpContext.Object;
         
            // Act
            adminController.LoggUt();

            // Assert
            Assert.Equal(_ikkeLoggetInn,mockSession[_loggetInn]);
        }
        
        // Tester for sjekk logget inn
        [Fact]
        public void SjekkLoggetInnLoggetInn()
        {
            // Arrange
            var adminController = new AdminsController(mockRep.Object, mockLog.Object);
            
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            mockSession[_loggetInn] = _loggetInn;
            adminController.ControllerContext.HttpContext = mockHttpContext.Object;
         
            // Act
            adminController.SjekkLoggetInn();

            // Assert
            Assert.Equal(_loggetInn,mockSession[_loggetInn]);
        }
        
        [Fact]
        public void SjekkLoggetInnIkkeLoggetInn()
        {
            // Arrange
            var adminController = new AdminsController(mockRep.Object, mockLog.Object);
            
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            adminController.ControllerContext.HttpContext = mockHttpContext.Object;
         
            // Act
            adminController.SjekkLoggetInn();

            // Assert
            Assert.Equal(_ikkeLoggetInn,mockSession[_loggetInn]);
        }
    }
}