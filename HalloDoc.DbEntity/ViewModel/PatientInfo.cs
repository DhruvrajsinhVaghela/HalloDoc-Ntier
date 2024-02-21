//using HalloDoc.Models.HalloDocMvc.ViewModel;
//using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace HalloDoc.DbEntity.ViewModels
{
    public class PatientInfo
    {
        [Required(ErrorMessage = "Notes is required")]
        public required string Notes { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        public required string PhoneNumber { get; set; }

        public required string ZipCode { get; set; }

        public required string State { get; set; }

        public required string City { get; set; }

        public required string Street { get; set; }

        public string? Password { get; set; }


        [Compare(nameof(Password), ErrorMessage = "Password Doesn't Match.")]
        public string? CPassword { get; set; }

        public required string Room { get; set; }

        [Required(ErrorMessage = "Birth Date is required")]
        public DateOnly BirthDate { get; set; }

        public List<Microsoft.AspNetCore.Http.IFormFile>? Files { get; set; }

        public int RegionID { get; set; }

        public string? RelationName { get; set; }
    }


}

