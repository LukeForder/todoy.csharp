﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todoy.Dto
{
    public class ErrorDto
    {
        public IEnumerable<string> Errors
        {
            get;
            set;
        }
    }
}
