using System.Collections.Generic;
using UnityEngine;

namespace KSPCommSys.Extensions
{
    public static class PartExtensions
    {
        public static bool IsPrimary(this Part thisPart, List<Part> partsList, int moduleClassId)
        {
            foreach (Part part in partsList)
            {
                if (part.Modules.Contains(moduleClassId))
                {
                    if (part == thisPart)
                        return true;
                    else break;
                }
            }
            return false;
        }
    }
}
