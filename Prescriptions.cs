//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Информационная_система_медицинской_клиники
{
    using System;
    using System.Collections.Generic;
    
    public partial class Prescriptions
    {
        public int PrescriptionID { get; set; }
        public Nullable<int> RecordID { get; set; }
        public string MedicationName { get; set; }
        public string DosageInstructions { get; set; }
    
        public virtual MedicalRecords MedicalRecords { get; set; }
        public virtual Medications Medications { get; set; }
    }
}
