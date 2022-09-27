using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Cen.Common.Domain.Interfaces;

namespace Cen.Common.Domain.Models
{
    public class DataModel: IDataWithKey
    {
        [IgnoreDataMember]
        [NotMapped]
        public virtual string SequenceName => throw new NotImplementedException();

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid? Id { get; set; }
    }
}