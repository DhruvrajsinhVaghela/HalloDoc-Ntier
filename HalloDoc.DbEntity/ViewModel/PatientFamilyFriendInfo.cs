//using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HalloDoc.DbEntity.ViewModel
{
    public class PatientFamilyFriendInfo
    {
        [Required(ErrorMessage = "Notes is required")]
        public required string Notes { get; set; }
        /*for family friend*/
        [Required(ErrorMessage = "FirstName is required")]
        public required string FFFirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public required string FFLastName { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        public required string FFPhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public required string FFEmail { get; set; }

        [Required(ErrorMessage = "Relation with patient is required")]
        public required string FFRelation { get; set; }

        /*for patient*/
        [Required(ErrorMessage = "UserName is required")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Zip Code is required")]
        public required string ZipCode { get; set; }

        public required string State { get; set; }

        public required string City { get; set; }

        [Required(ErrorMessage = "Street is required")]
        public required string Street { get; set; }

        public required string Room { get; set; }


        [Required(ErrorMessage = "BirthDate is required")]
        public required DateTime BirthDate { get; set; }

        public List<IFormFile>? Files { get; set; }
    }


}
