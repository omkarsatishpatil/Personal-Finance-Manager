//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PersonalFinanceManager.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Expens
    {
        public int ExpenseID { get; set; }
        public string ExpenseName { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public System.DateTime ExpenseDate { get; set; }
        public string Notes { get; set; }
        public string userName { get; set; }
    }
}
