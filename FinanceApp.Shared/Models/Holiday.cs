﻿using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialAPI.Data
{
    public class Holiday
    { 
        [Key]
        public int Id { get; set; }
        [Key]
        public DateTime Date { get; set; }
        public DateTime DateLastUpdate { get; set; }
    }
}
