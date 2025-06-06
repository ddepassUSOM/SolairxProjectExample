using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolairxExample.Model.ViewModels
{
    public class ProjectVM
    {
        public Project? Project { get; set; }

        public List<SelectListItem>? ProjectManagerList { get; set; }

        public string? ProjectManager { get; set; }
    }
    

}
