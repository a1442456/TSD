using System.ComponentModel.DataAnnotations;

namespace Cen.Common.Domain.Models
{
    public class DataModelWithName: DataModel
    {
        [MaxLength(36)]
        public string Code { get; set; }
        
        [MaxLength(255)]
        public string Name { get; set; }
    }
}