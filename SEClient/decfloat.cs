using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.pHLab.SE.V1
{
    public partial class decfloat
    {
        public decfloat()
        {
            DecimalValue = null;
        }

        public decfloat(decimal value)
        {
            DecimalValue = value;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public decimal? DecimalValue
        {
            get
            {
                decimal value;

                if (decimal.TryParse(this.valueField, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                {
                    return value;
                }

                return null;
            }

            set
            {
                if (value.HasValue)
                {
                    this.valueField = string.Format(CultureInfo.InvariantCulture, "{0}", value);
                }
                else
                {
                    this.valueField = "NaN";
                }
            }
        }

        public static implicit operator decfloat(decimal value)
        {
            return new decfloat(value);
        }

        public override string ToString()
        {
            decimal? value = DecimalValue;
            if (value.HasValue)
            {
                return value.Value.ToString();
            }
            else
            {
                return "not defined";
            }
        }

    }
}
