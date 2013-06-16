namespace SchoolReports.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public enum Smiley
    {
        None,
        VeryGood,
        Good,
        Sufficient,
        Insufficient,
        Weak
    }

    public class SmileyOption
    {
        public Smiley Smiley { get; set; }

        public string DisplayValue
        {
            get
            {
                switch (this.Smiley)
                {
                    case Smiley.None:
                        return "Geen gezichtje";
                    case Smiley.VeryGood:
                        return "Zeer goed";
                    case Smiley.Good:
                        return "Goed";
                    case Smiley.Sufficient:
                        return "Voldoende";
                    case Smiley.Insufficient:
                        return "Onvoldoende";
                    case Smiley.Weak:
                        return "Zwak";
                    default:
                        return "Ongeldige waarde";
                }
            }
        }

        public static string GetImageForSmiley(Smiley smiley)
        {
            switch (smiley)
            {
                case Smiley.None:
                    return "smiley_none.png";
                case Smiley.VeryGood:
                    return "smiley_verygood.png";
                case Smiley.Good:
                    return "smiley_good.png";
                case Smiley.Sufficient:
                    return "smiley_sufficient.png";
                case Smiley.Insufficient:
                    return "smiley_insufficient.png";
                case Smiley.Weak:
                    return "smiley_weak.png";
                default:
                    return "smiley_none.png";
            }
        }

        public static IEnumerable<SmileyOption> SmileyOptions()
        {
            return from Smiley e in Enum.GetValues(typeof(Smiley))
                   select new SmileyOption { Smiley = e };
        }
    }
}