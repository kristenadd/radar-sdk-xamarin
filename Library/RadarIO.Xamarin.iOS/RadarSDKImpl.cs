﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadarIO.Xamarin
{
    public class RadarSDKImpl : RadarSDK
    {
        public override void Initialize(string publishableKey)
        {
            iOSBinding.Radar.InitializeWithPublishableKey(publishableKey);
        }

        public override Task<(RadarStatus, RadarLocation, RadarEvent[], RadarUser)> TrackOnce()
        {
            var task = new TaskCompletionSource<(RadarStatus, RadarLocation, RadarEvent[], RadarUser)>();
            iOSBinding.Radar.TrackOnceWithCompletionHandler((status, location, ev, user) =>
            {
                task.SetResult((status.ToSDK(), location?.ToSDK(), ev?.Select(Conversion.ToSDK).ToArray(), user?.ToSDK()));
            });
            return task.Task;
        }

        public override void StartTracking(RadarTrackingOptions options)
        {
            iOSBinding.Radar.StartTrackingWithOptions(options.ToBinding());
        }

        public override void StopTracking()
        {
            iOSBinding.Radar.StopTracking();
        }
    }

    public partial class RadarTrackingOptions
    {
        public bool ShowBlueBar;
        public bool UseVisits;
        public bool UseSignificantLocationChanges;
        public static RadarTrackingOptions Continuous
            => iOSBinding.RadarTrackingOptions.Continuous.ToSDK();
        public static RadarTrackingOptions Responsive
            => iOSBinding.RadarTrackingOptions.Responsive.ToSDK();
    }
}
