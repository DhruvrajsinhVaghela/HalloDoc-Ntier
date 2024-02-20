//using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace HalloDoc.ViewModels
{
    public class PatientConciergeInfo
    {

        /*for Concierge Info*/
        [Required(ErrorMessage = "FirstName is required")]
        public required string CFirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public required string CLastName { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        public required string CPhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public required string CEmail { get; set; }

        [Required(ErrorMessage = "Propert/Hotel Name is required")]
        public required string CHotelName { get; set; }

        [Required(ErrorMessage = "street is required")]
        public required string CStreet { get; set; }

        public required string CCity { get; set; }

        public required string CState { get; set; }

        [Required(ErrorMessage = "Zip Code is required")]
        public required string CZipCode { get; set; }


        /*for patient*/
        [Required(ErrorMessage = "Notes is required")]
        public required string Notes { get; set; }



        [Required(ErrorMessage = "FirstName is required")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public required string LastName { get; set; }

        public required DateTime BirthDate { get; set; }


        [Required(ErrorMessage = "UserName is required")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        public required string PhoneNumber { get; set; }

        public required string Room { get; set; }


    }
}
