using RestSharp;
using System.Text.Json.Nodes;
using System.Text.Json;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using NetTopologySuite.Features;
using System.Collections.ObjectModel;
using System.Runtime.Versioning;

namespace KolumbusRouteApi
{



    public class NextPlatform
    {
        public string id { get; set; }
        public string nsr_id { get; set; }
        public string external_id { get; set; }
        public string stop_place_id { get; set; }
        public string name { get; set; }
        public string modification { get; set; }
        public object description { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string type { get; set; }
        public string transport_mode { get; set; }
        public string sub_mode_type { get; set; }
        public string public_code { get; set; }
        public string private_code { get; set; }
        public DateTime changed { get; set; }
        public DateTime created { get; set; }
    }

    public class Vehicle
    {
        public Point Location => new Point(longitude, latitude);

        //public object depot { get; set; }
        //public NextPlatform next_platform { get; set; }
        public string journey_id { get; set; }
        public string vehicle_id { get; set; }
        public string company_id { get; set; }
        public bool is_active { get; set; }
        public int delay { get; set; }
        public string line_id { get; set; }
        public string line_name { get; set; }
        public string destination_display { get; set; }
        public string origin_name { get; set; }
        public string destination_name { get; set; }
        public string previous_stop_id { get; set; }
        public string previous_stop_order { get; set; }
        public object next_stop_id { get; set; }
        public string next_stop_name { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public double speed { get; set; }
        public double heading { get; set; }
        public int link_distance { get; set; }
        public int percentage { get; set; }
        public DateTime recorded_at_time { get; set; }
        public DateTime origin_aimed_departure_time { get; set; }
        public DateTime destination_aimed_arrival_time { get; set; }
        //public object trip_state { get; set; }
    }




    public class RouteApi
    {


        //public string GetBusses()



        public string GetVehicles()
        {
            // send GET request with RestSharp
            var client = new RestClient("https://api.kolumbus.no/api/");
            var request = new RestRequest("vehicles/realtimehub");
            var response = client.ExecuteGet(request);

            // deserialize json string response to JsonNode object
            var activeVehicles = JsonSerializer.Deserialize<List<Vehicle>>(response.Content!)!.Where(v=>v.is_active).ToList();

            //var points = data.Select(v => new Point(v.latitude, v.longitude)).ToList();

            var me = new Point( 5.7344217, 58.9676191);


            activeVehicles = activeVehicles.OrderBy(p => p.Location.Distance(me)).Take(50).ToList();


            //var closest = activeVehicles.Select( v=>  new { vehicle = v.line_name + " " + v.line_id, distance = v.Location.Distance()   }  ).ToList();

            FeatureCollection features = new FeatureCollection();

            //Collection<IFeature> features = new Collection<IFeature> {  };

            int index = 0;
            foreach (var vehicle in activeVehicles)
            {
                var attributes = new AttributesTable() { };
                attributes.Add("bus", vehicle.line_name);
                attributes.Add("index", index++);
                attributes.Add("distance", vehicle.Location.Distance(me));

                features.Add(new Feature()
                {
                    Geometry = vehicle.Location,
                    Attributes = attributes

                });
            
            }

            var feature = new Feature();
            var myattributes = new AttributesTable() { };
            
            feature.Attributes = myattributes;
            feature.Geometry = me;
            myattributes.Add("bus", "me");
            myattributes.Add("distance", 0);
            myattributes.Add("index", index++);

            features.Add(feature);

            GeoJsonWriter _geoJsonWriter = new GeoJsonWriter();
            var str = _geoJsonWriter.Write( features );

            //File.WriteAllText("busses.json", str);




            //return activeVehicles;
            return str;

        }

        public string GetVehicles(float lat, float lon)
        {
            // send GET request with RestSharp
            var client = new RestClient("https://api.kolumbus.no/api/");
            var request = new RestRequest("vehicles/realtimehub");
            var response = client.ExecuteGet(request);

            // deserialize json string response to JsonNode object
            var activeVehicles = JsonSerializer.Deserialize<List<Vehicle>>(response.Content!)!.Where(v => v.is_active).ToList();

            //var points = data.Select(v => new Point(v.latitude, v.longitude)).ToList();

            var me = new Point(5.7344217, 58.9676191);


            activeVehicles = activeVehicles.OrderBy(p => p.Location.Distance(me)).Take(50).ToList();


            //var closest = activeVehicles.Select( v=>  new { vehicle = v.line_name + " " + v.line_id, distance = v.Location.Distance()   }  ).ToList();

            FeatureCollection features = new FeatureCollection();

            //Collection<IFeature> features = new Collection<IFeature> {  };

            int index = 0;
            foreach (var vehicle in activeVehicles)
            {
                var attributes = new AttributesTable() { };
                attributes.Add("bus", vehicle.line_name);
                attributes.Add("index", index++);
                attributes.Add("distance", vehicle.Location.Distance(me));

                features.Add(new Feature()
                {
                    Geometry = vehicle.Location,
                    Attributes = attributes

                });

            }

            var feature = new Feature();
            var myattributes = new AttributesTable() { };

            feature.Attributes = myattributes;
            feature.Geometry = me;
            myattributes.Add("bus", "me");
            myattributes.Add("distance", 0);
            myattributes.Add("index", index++);

            features.Add(feature);

            GeoJsonWriter _geoJsonWriter = new GeoJsonWriter();
            var str = _geoJsonWriter.Write(features);

            //File.WriteAllText("busses.json", str);




            //return activeVehicles;
            return str;

        }




    }
}