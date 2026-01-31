using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Application_Suivi_De_Temps.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Nom { get; set; } = "";
        public int Annee { get; set; }
        public int Date { get; set; }

    }
}
