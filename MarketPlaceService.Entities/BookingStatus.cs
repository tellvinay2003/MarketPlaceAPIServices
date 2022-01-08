using System;
using System.ComponentModel;

namespace MarketPlaceService.Entities
{
    public enum BookingStatus
    {
        
        [Description("Error")]
        Error = 1,
        [Description("Syncing")]     
        Syncing,
        [Description("Synced")]     
        Synced,
        [Description("Waiting For Callback From Publisher")]     
        WaitingForCallbackFromPublisher,
        [Description("Waiting For Callback From Subscriber")]  
        WaitingForCallbackFromSubscriber,
        
    };

}
