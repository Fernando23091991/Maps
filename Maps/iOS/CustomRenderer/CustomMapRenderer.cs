using System;
using System.Collections.Generic;
using CoreGraphics;
using MapKit;
using Maps.CustomRenderer;
using Maps.iOS.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;

[assembly: ExportRenderer(typeof(ExtendedMap), typeof(CustomMapRenderer))]

namespace Maps.iOS.CustomRenderer
{
    public class CustomMapRenderer : MapRenderer
    {
        UIView customPinView;
        List<ExtendedPin> customPins;

        public CustomMapRenderer()
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                var nativeMap = Control as MKMapView;
                if (nativeMap != null)
                {
                    nativeMap.RemoveAnnotations(nativeMap.Annotations);
                    nativeMap.GetViewForAnnotation = null;
                    nativeMap.CalloutAccessoryControlTapped -= OnCalloutAccessoryControlTapped;
                    nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
                    nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
                }
            }

            if (e.NewElement != null)
            {
                var formsMap = (ExtendedMap)e.NewElement;
                var nativeMap = Control as MKMapView;
                customPins = formsMap.CustomPins;

                nativeMap.GetViewForAnnotation = GetViewForAnnotation;
                nativeMap.CalloutAccessoryControlTapped += OnCalloutAccessoryControlTapped;
                nativeMap.DidSelectAnnotationView += OnDidSelectAnnotationView;
                nativeMap.DidDeselectAnnotationView += OnDidDeselectAnnotationView;
            }
        }

        MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView annotationView = null;

            if (annotation is MKUserLocation)
                return null;

            var customPin = GetCustomPin(annotation as MKPointAnnotation);
            if (customPin == null)
            {
                throw new Exception("Custom pin not found");
            }

            annotationView = mapView.DequeueReusableAnnotation(customPin.Id);
            if (annotationView == null)
            {
                annotationView = new CustomMKAnnotationView(annotation, customPin.Id);
                annotationView.Image = UIImage.FromFile("location.png");
                annotationView.CalloutOffset = new CGPoint(0, 0);
                annotationView.LeftCalloutAccessoryView = new UIImageView(UIImage.FromFile("location.png"));
                annotationView.RightCalloutAccessoryView = UIButton.FromType(UIButtonType.DetailDisclosure);
                annotationView.Draw(new CGRect(new CGPoint(0, 0),new CGSize(30,30)));
                ((CustomMKAnnotationView)annotationView).Id = customPin.Id;
                ((CustomMKAnnotationView)annotationView).Url = customPin.Url;

            }
            annotationView.CanShowCallout = true;

            return annotationView;
        }

        void OnCalloutAccessoryControlTapped(object sender, MKMapViewAccessoryTappedEventArgs e)
        {
            var customView = e.View as CustomMKAnnotationView;
            if (!string.IsNullOrWhiteSpace(customView.Url))
            {
                UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl(customView.Url));
            }
        }

        void OnDidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            var customView = e.View as CustomMKAnnotationView;
            customPinView = new UIView();

            if (customView.Id == "Xamarin")
            {
                customPinView.Frame = new CGRect(0, 0, 200, 84);
                var image = new UIImageView(new CGRect(0, 0, 200, 84));
                image.Image = UIImage.FromFile("xamarin.png");
                customPinView.AddSubview(image);
                customPinView.Center = new CGPoint(0, -(e.View.Frame.Height + 75));
                e.View.AddSubview(customPinView);
            }
        }

        void OnDidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            if (!e.View.Selected)
            {
                customPinView.RemoveFromSuperview();
                customPinView.Dispose();
                customPinView = null;
            }
        }

        ExtendedPin GetCustomPin(MKPointAnnotation annotation)
        {
            var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);
            foreach (var pin in customPins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }

    }
}
