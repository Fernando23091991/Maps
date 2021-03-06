﻿using System;
using MapKit;

namespace Maps.iOS.CustomRenderer
{
    public class CustomMKAnnotationView : MKAnnotationView
    {        
        public string Id { get; set; }

        public string Url { get; set; }

        public CustomMKAnnotationView(IMKAnnotation annotation, string id) : base(annotation, id)
        {
            
        }

    }
}
