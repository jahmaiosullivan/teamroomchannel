using System.Collections.Generic;
using HobbyClue.Data.Models;

namespace HobbyClue.Web.ViewModels
{
    public class ChooseLocationViewModel
    {
        public SortedDictionary<string, IList<City>> Regions { get; set; }
        public bool Override { get; set; }
    }
}