//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RetailTrade.Dashboard
{
    using System;
    using System.Collections.Generic;
    
    public partial class Shifts
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Shifts()
        {
            this.Receipts = new HashSet<Receipts>();
        }
    
        public int Id { get; set; }
        public System.DateTime OpeningDate { get; set; }
        public Nullable<System.DateTime> ClosingDate { get; set; }
        public int UserId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Receipts> Receipts { get; set; }
        public virtual Users Users { get; set; }
    }
}
