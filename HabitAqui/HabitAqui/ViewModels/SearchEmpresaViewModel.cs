using Microsoft.AspNetCore.Mvc;
using HabitAqui.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HabitAqui.ViewModels
{
    public class SearchEmpresasViewModel
    {
        public List<Empresa> ListaEmpresas { get; set; }
        public int NResultados { get; set; }
        [Display(Name = "Pesquisa", Prompt = "Introduzir texto")]
        public string TextoAPesquisar { get; set; }
        [Display(Name = "Empresa disponível")]
        public bool available { get; set; }
    }
}
