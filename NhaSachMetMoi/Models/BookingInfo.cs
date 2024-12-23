//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NhaSachMetMoi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BookingInfo
    {
        public int BookingID { get; set; }
        public Nullable<int> MaKH { get; set; }
        public string RoomPackage { get; set; }
        public int Duration { get; set; }
        public int NumberOfParticipants { get; set; }
        public System.DateTime BookingDate { get; set; }
        public System.TimeSpan BookingTime { get; set; }
        public string Note { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public string ConfirmationStatus { get; set; }
    
        public virtual KH KH { get; set; }
    }
}
