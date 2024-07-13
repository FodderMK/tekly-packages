using System.Text;
using Tekly.Common.TimeProviders;

namespace Tekly.DataModels.Models
{
    public class DateModel : ValueModel<TkDateTime>
    {
        public DateModel(TkDateTime value) : base(value) {}
		
        public override int CompareTo(IValueModel valueModel)
        {
            var other = valueModel as DateModel;
            return m_value.CompareTo(other.Value);
        }
        
        public override void ToJson(StringBuilder sb)
        {
            sb.Append($"\"{Value}\"");
        }

        protected override string OnToDisplayString()
        {
            return Value.ToString();
        }
    }
}