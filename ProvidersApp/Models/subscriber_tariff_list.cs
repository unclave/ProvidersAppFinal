
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------


namespace ProvidersApp.Models
{

using System;
    using System.Collections.Generic;
    
public partial class subscriber_tariff_list
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public subscriber_tariff_list()
    {

        this.payments = new HashSet<payments>();

    }


    public int id { get; set; }

    public int id_tariff { get; set; }

    public int id_subscriber { get; set; }

    public System.DateTime date { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<payments> payments { get; set; }

    public virtual subscribers subscribers { get; set; }

    public virtual tariffs tariffs { get; set; }

}

}
