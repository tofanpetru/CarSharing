using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Attributes
{
    public class DateAttribute : RangeAttribute
    {
        public DateAttribute()
          : base(typeof(DateTime), DateTime.MinValue.ToShortDateString(), DateTime.Now.ToShortDateString()) { }
    }
}
