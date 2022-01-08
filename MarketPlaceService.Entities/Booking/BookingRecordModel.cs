using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MarketPlaceService.Entities.Booking
{
    public class BookingRecordModel
    {
    
    public Guid Id {get;set;}    
    public  DateTime? Date{get;set;}

    public String UserName{get;set;}
    
     public String EntityName {get;set;}

     public  String Event {get;set;}

     public String Status{get;set;}   

     [IgnoreDataMember]
     public int StatusId{get;set;}

     public String Details {get;set;}

     public String BookingReference {get;set;}

     public String SubscriberData{get;set;}     

     public String PublisherData{get;set;}
    }
}
