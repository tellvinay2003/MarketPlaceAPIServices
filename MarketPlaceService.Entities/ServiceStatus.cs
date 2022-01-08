using System;

namespace MarketPlaceService.Entities
{
    public class ServiceStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid PublisherId {get;set;}
    }
}
