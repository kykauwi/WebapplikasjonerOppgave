using Microsoft.EntityFrameworkCore;
using Webapplikasjoner1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Webapplikasjoner1.DAL
{


    public class Billetter
    {
        public int Id { get; set; }  
        public string Fornavn { get; set; }
        public string Etternavn { get; set; }
        public string Dato { get; set; }
        public string Klokkeslett { get; set; }
        public bool Retur { get; set; }
        public string ReturDato { get; set; }
        public string Type { get; set; }
        public int Pris { get; set; }
        public int Antall { get; set; } 
        public virtual Strekninger FraSted { get; set; }
        public virtual Strekninger TilSted { get; set; }
    }

    public class Strekninger {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public virtual Lokasjoner FraSted { get; set; }
        public virtual Lokasjoner TilSted { get; set; }
        public int Studentpris { get; set; }
        public int HonnørPris { get; set; }
        public int OrdinærPris { get; set; }
        public int BarnPris { get; set; }
    }
    public class Lokasjoner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int StedsNavn { get; set; }
    }
    public class Adminer
    {
        public int Id {get; set;}
        public string Brukernavn {get; set;}
        public byte[] Passord {get; set;}
        public byte[] Salt {get; set;}
    }

    public class BillettKontekst : DbContext
    {
        public BillettKontekst(DbContextOptions<BillettKontekst> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Billetter> Billetter { get; set; }
        public DbSet<Adminer> Admins {get; set;}
        public DbSet<Strekninger> Strekninger { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}

    