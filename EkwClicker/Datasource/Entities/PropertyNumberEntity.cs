using System;

namespace EkwClicker.Datasource.Entities
{
    internal class PropertyNumberEntity
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public string Number { get; set; }
    }
}