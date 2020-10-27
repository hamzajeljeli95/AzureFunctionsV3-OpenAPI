using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenApiSample
{
    class PostCalculateBody
    {
        [Required]
        public int Num1 { get; set; }
        [Required]
        public int Num2 { get; set; }

        public int calculate()
        {
            return this.Num1 + this.Num2;
        }
    }
}
