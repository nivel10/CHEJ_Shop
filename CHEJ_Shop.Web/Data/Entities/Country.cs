﻿namespace CHEJ_Shop.Web.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Country : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Country")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Name { get; set; }

        public ICollection<City> Cities { get; set; }

        [Display(Name = "# Cities")]
        [NotMapped]
        public int NumberCities => this.Cities == null ? 0 : this.Cities.Count;
    }
}