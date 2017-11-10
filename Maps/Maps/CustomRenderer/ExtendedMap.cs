using System;
using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace Maps.CustomRenderer
{
    public class ExtendedMap : Map
    {
        public List<ExtendedPin> CustomPins { get; set; } = new List<ExtendedPin>();

        public Action DissmissViewRol;

        public Position CenterPosition { get; set; }

        public Action<double, double> MoveMapToRegion;

        public Action<List<ExtendedPin>> AddMarkers;

    }
}
