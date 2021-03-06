﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heroes.DataAccessLayer.Models
{
    public class Country : Base
    {
        public Country()
        {
            Heroes = new HashSet<Hero>();
        }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public virtual ICollection<Hero> Heroes { get; set; }
    }
}
