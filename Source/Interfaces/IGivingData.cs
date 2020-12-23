using SmartfonManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartfonManager.Source.Interfaces
{
    public interface IGivingData
    {
        void GivePathToLoad();
        void GiveDeleteCacheSettings(bool shouldDelete);
        void GiveSortedTypeSettings(TypeSort typeOfSort);
        void GiveSortedTypeOrderSettings(TypeSortOrder typeOfSortOrder);
        void GiveViewSettings(bool visibilityList, bool visibilityGrid);
    }
}
