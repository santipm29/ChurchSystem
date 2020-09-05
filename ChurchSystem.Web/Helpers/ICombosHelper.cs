using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ChurchSystem.Web.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboProfessions();
        IEnumerable<SelectListItem> GetComboFields();

        IEnumerable<SelectListItem> GetComboDistricts(int fieldId);

        IEnumerable<SelectListItem> GetComboChurches(int districtId);

    }
}
