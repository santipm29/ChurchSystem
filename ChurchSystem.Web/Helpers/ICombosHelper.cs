using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ChurchSystem.Web.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboProfessions();
    }
}
