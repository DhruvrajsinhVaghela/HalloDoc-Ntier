//using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace HalloDoc.DbEntity.ViewModel
{
    public class PatientBusinessInfoVM
    {
        [Required(ErrorMessage = "Notes is required")]
        public required string Notes { get; set; }
        /*for Business*/
        [Required(ErrorMessage = "FirstName is required")]
        public required string BFirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public required string BLastName { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        public required string BPhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public required string BEmail { get; set; }

        [Required(ErrorMessage = "Relation with patient is required")]
        public required string BBusinessName { get; set; }

        public required string BCaseNo { get; set; }

        /*for patient*/


        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "ZipCode is required")]
        public required string ZipCode { get; set; }

        public required string State { get; set; }

        public required string City { get; set; }

        [Required(ErrorMessage = "Street is required")]
        public required string Street { get; set; }

        public required string Room { get; set; }

        [Required(ErrorMessage = "BirthDate is required")]
        public required DateTime BirthDate { get; set; }
    }
}
