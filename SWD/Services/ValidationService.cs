using SWD.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Services
{
    public static class ValidationService
    {
        public static bool TableIsValid(Model.Table table)
        {
            foreach (Row row in table.Rows)
                foreach (Cell cell in row.Cells)
                    if (String.IsNullOrEmpty(cell.Value.Replace(" ", String.Empty)))
                        return false;

            return true;
        }
    }
}
