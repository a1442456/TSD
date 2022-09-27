using System;

namespace Cen.Wms.Client.Models.Dtos
{
    public class PurchaseTaskDto
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ChangedAt { get; set; }

        public string DisplayText
        {
            get { return CreatedAt.ToLocalTime().ToString("yyyy.MM.dd HH:mm") + ", " + Code; }
        }
    }
}
