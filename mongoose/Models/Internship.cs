//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace mongoose.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Internship
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Internship()
        {
            this.Internship_Major = new HashSet<Internship_Major>();
            this.Student_Internship = new HashSet<Student_Internship>();
            this.Saved_Internship = new HashSet<Saved_Internship>();
            this.Applications = new HashSet<Application>();
        }
        [Display(Name = "Internship ID")]
        public int InternshipId { get; set; }
        [Display(Name = "Employer ID")]
        public int EmployerId { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Title")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string Length { get; set; }
        [DataType(DataType.Currency)]
        public Nullable<decimal> Rate { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string Location { get; set; }
        [DataType(DataType.Date)]
        [Display(Name ="Start Date")]
        [Required(ErrorMessage = "This field is required")]
        public Nullable<System.DateTime> StartDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date Posted")]
        public System.DateTime PostDate { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public payMe Paid { get; set; }
        [Display(Name = "Employer")]
        public virtual Employer Employer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "Major")]
        public virtual ICollection<Internship_Major> Internship_Major { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Student_Internship> Student_Internship { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Saved_Internship> Saved_Internship { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Application> Applications { get; set; }
    }
}
