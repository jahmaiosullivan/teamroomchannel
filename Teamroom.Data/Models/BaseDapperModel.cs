using HobbyClue.Common.Attributes;

namespace HobbyClue.Data.Models
{
    public class BaseDapperModel
    {
        [PrimaryKey]
        [AutoSuppliedFromDatabase]
        public long Id { get; set; }
      
    }
}
