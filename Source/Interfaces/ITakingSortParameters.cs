using SmartfonManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartfonManager.Source.Interfaces
{
    public interface ITakingSortParameters
    {
        void TakeSortParameters(TypeSort typeSort, TypeSortOrder typeSortOrder);
    }
}
