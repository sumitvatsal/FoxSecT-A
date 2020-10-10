using System.ComponentModel.DataAnnotations;

namespace FoxSec.Common.Enums
{
    public enum CardTypes
    {
        [Display(Name = "Proxy card")]
        ProxyCard = 7,
        [Display(Name = "Magnetic card")]
        MagneticCard  = 8,
        [Display(Name = "Fingerprint")]
        Fingerprint = 11,
        [Display(Name = "Licence plate")]
        LicencePlate  = 13,
        [Display(Name = "PIN")]
        PIN = 14,
        [Display(Name = "Barcode")]
        Barcode = 15,
        [Display(Name = "Mobile ID")]
        MobileID = 16,
        [Display(Name = "Iris recognition")]
        IrisRecognition = 17
    }
}