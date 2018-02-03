using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppTandT.BLL.Help;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions.Abstractions;

namespace AppTandT.BLL
{
    public static class Geolocator
    {
        /// <summary>
        /// Get the position of current device
        /// </summary>
        /// <param name="SecondsTimeout">Timeout</param>
        /// <param name="DesiredAccuracy">Desired accuracy</param>
        /// <returns></returns>
        public static async Task<Position> GetPositionAsync(int SecondsTimeout = 10, double DesiredAccuracy = 500.0)
        {
            if (CrossGeolocator.IsSupported)
            {
                if (!CrossGeolocator.Current.IsGeolocationEnabled)
                    throw new ServiceException("Please, enable the gps.");

                if (!CrossGeolocator.Current.IsGeolocationAvailable)
                    throw new ServiceException("Sorry, gps is not availible.");

                var hasPermission = await Utils.CheckPermissions(Permission.Location);
                if (!hasPermission) return null;

                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = DesiredAccuracy;

                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(SecondsTimeout));

                if (position != null)
                {
                    return position;
                }
            }
            throw new Exception();
        }
    }
}
