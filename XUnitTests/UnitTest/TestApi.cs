using System;
using System.Collections.Generic;

using AftershipAPI;
using AftershipAPI.Enums;

using Xunit;

namespace Test
{
    public class TestConnectionAPI
    {
        private ConnectionAPI connection;

        //static int TOTAL_COURIERS_API = 225;

        private readonly string[] _couriersDetected = { "dpd", "fedex" };

        //post tracking number
        private readonly string _trackingNumberPost = "05167019264110";
        private readonly string _slugPost = "dpd";
        private readonly string _orderIDPathPost = "www.whatever.com";
        private readonly string _orderIDPost = "ID 1234";
        private readonly string _customerNamePost = "Mr Smith";
        private readonly string _titlePost = "this title";
        private readonly ISO3Country _countryDestinationPost = ISO3Country.USA;
        private readonly string _email1Post = "email@yourdomain.com";
        private readonly string _email2Post = "another_email@yourdomain.com";
        private readonly string _sms1Post = "+85292345678";
        private readonly string _sms2Post = "+85292345679";
        private readonly string _customProductNamePost = "iPhone Case";
        private readonly string _customProductPricePost = "USD19.99";

        //tracking numbers to detect
        private readonly string _trackingNumberToDetect = "09445246482536";
        private readonly string _trackingNumberToDetectError = "asdq";

        //Tracking to Delete
        private readonly string _trackingNumberDelete = "596454081704";
        private readonly string _slugDelete = "fedex";


        //tracking to DeleteBad
        private readonly string _trackingNumberDelete2 = "798865638020";
        private static bool firstTime = true;
        private Dictionary<string, string> firstCourier = new Dictionary<string, string>();
        private Dictionary<string, string> firstCourierAccount = new Dictionary<string, string>();

        public TestConnectionAPI()
        {
            var key = System.IO.File.ReadAllText(@"C:\dev\misc\aftership-key.txt");
            connection = new ConnectionAPI(key);

            if (firstTime)
            {

                Console.WriteLine("****************SET-UP BEGIN**************");
                firstTime = false;
                //delete the tracking we are going to post (in case it exist)
                var tracking = new Tracking("05167019264110")
                {
                    Slug = "dpd"
                };

                //first courier
                firstCourier.Add("slug", "india-post-int");
                firstCourier.Add("name", "India Post International");
                firstCourier.Add("phone", "+91 1800 11 2011");
                firstCourier.Add("other_name", "भारतीय डाक, Speed Post & eMO, EMS, IPS Web");
                firstCourier.Add("web_url", "http://www.indiapost.gov.in/");

                //first courier in your account
                firstCourierAccount.Add("slug", "usps");
                firstCourierAccount.Add("name", "USPS");
                firstCourierAccount.Add("phone", "+1 800-275-8777");
                firstCourierAccount.Add("other_name", "United States Postal Service");
                firstCourierAccount.Add("web_url", "https://www.usps.com");

                try { connection.DeleteTracking(tracking); }
                catch (Exception e)
                {
                    Console.WriteLine("**1" + e.Message);
                }
                var tracking1 = new Tracking(_trackingNumberToDetect)
                {
                    Slug = "dpd"
                };
                try { connection.DeleteTracking(tracking1); }
                catch (Exception e)
                {
                    Console.WriteLine("**2" + e.Message);
                }
                try
                {
                    var newTracking = new Tracking(_trackingNumberDelete)
                    {
                        Slug = _slugDelete
                    };
                    connection.CreateTracking(newTracking);
                }
                catch (Exception e)
                {
                    Console.WriteLine("**3" + e.Message);
                }
                try
                {
                    var newTracking1 = new Tracking("9400110897700003231250")
                    {
                        Slug = "usps"
                    };
                    connection.CreateTracking(newTracking1);
                }
                catch (Exception e)
                {
                    Console.WriteLine("**4" + e.Message);

                }
                Console.WriteLine("****************SET-UP FINISH**************");

            }

        }

        [Fact]
        public void TestDetectCouriers()
        {

            //get trackings of this number.
            List<Courier> couriers = connection.DetectCouriers(_trackingNumberToDetect);
            Assert.Equal(3, couriers.Count);
            //the couriers should be dpd or fedex
            Console.WriteLine("**0" + couriers[0].Slug);
            Console.WriteLine("**1" + couriers[1].Slug);

            Assert.True(Equals(couriers[0].Slug, _couriersDetected[0])
                || Equals(couriers[1].Slug, _couriersDetected[0]));
            Assert.True(Equals(couriers[0].Slug, _couriersDetected[1])
                || Equals(_couriersDetected[1], couriers[1].Slug));

            //if the trackingNumber doesn't match any courier defined, should give an error.

            try
            {
                List<Courier> couriers1 = connection.DetectCouriers(_trackingNumberToDetectError);
                Assert.Empty(couriers1);
            }
            catch (Exception e)
            {
                Assert.Equal("{\"meta\":{\"code\":4005,\"message\":\"The value of `tracking_number` is invalid.\",\"type\":\"BadRequest\"},\"data\":{\"tracking\":{\"tracking_number\":\"asdq\"}}}", e.Message);
            }

            var slugs = new List<string>
            {
                "dtdc",
                "ukrposhta",
                "usps",
                // "asdfasdfasdfasd",
                "dpd"
            };
            List<Courier> couriers2 = connection.DetectCouriers(_trackingNumberToDetect, "28046", "", null, slugs);
            Assert.Single(couriers2);
        }
        [Fact]
        public void TestCreateTracking()
        {
            var tracking1 = new Tracking(_trackingNumberPost)
            {
                Slug = _slugPost,
                OrderIDPath = _orderIDPathPost,
                CustomerName = _customerNamePost,
                OrderID = _orderIDPost,
                Title = _titlePost,
                DestinationCountryISO3 = _countryDestinationPost
            };
            tracking1.AddEmails(_email1Post);
            tracking1.AddEmails(_email2Post);
            tracking1.AddCustomFields("product_name", _customProductNamePost);
            tracking1.AddCustomFields("product_price", _customProductPricePost);
            tracking1.AddSmses(_sms1Post);
            tracking1.AddSmses(_sms2Post);
            Tracking trackingPosted = connection.CreateTracking(tracking1);

            Assert.Equal(_trackingNumberPost, trackingPosted.TrackingNumber);
            Assert.Equal(_slugPost, trackingPosted.Slug);
            Assert.Equal(_orderIDPathPost, trackingPosted.OrderIDPath);
            Assert.Equal(_orderIDPost, trackingPosted.OrderID);
            Assert.Equal(_countryDestinationPost,
                trackingPosted.DestinationCountryISO3);

            Assert.Contains(_email1Post, trackingPosted.Emails);
            Assert.Contains(_email2Post, trackingPosted.Emails);
            Assert.Equal(2, trackingPosted.Emails.Count);

            Assert.Contains(_sms1Post, trackingPosted.Smses);
            Assert.Contains(_sms2Post, trackingPosted.Smses);
            Assert.Equal(2, trackingPosted.Smses.Count);

            Assert.Equal(_customProductNamePost,
                trackingPosted.CustomFields["product_name"]);
            Assert.Equal(_customProductPricePost,
                trackingPosted.CustomFields["product_price"]);
        }

        [Fact]
        public void TestCreateTrackingEmptySlug()
        {
            //test post only informing trackingNumber (the slug can be dpd and fedex)
            var tracking2 = new Tracking(_trackingNumberToDetect);
            Tracking trackingPosted2 = connection.CreateTracking(tracking2);
            Assert.Equal(_trackingNumberToDetect, trackingPosted2.TrackingNumber);
            Assert.Equal("dpd", trackingPosted2.Slug);//the system assign dpd (it exist)
        }

        //test post tracking number doesn't exist
        [Fact]
        public void TestCreateTrackingError()
        {
            var tracking3 = new Tracking(_trackingNumberToDetectError);

            try
            {
                connection.CreateTracking(tracking3);
                //always should give an exception before this
                Assert.True(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Assert.Equal("{\"meta\":{\"code\":4005,\"message\":\"The value of `tracking_number` is invalid.\",\"type\":\"BadRequest\"},\"data\":{\"tracking\":{\"tracking_number\":\"asdq\",\"title\":\"asdq\"}}}",
                    e.Message);
            }
        }


        [Fact]
        public void TestDeleteTracking()
        {

            //delete a tracking number (posted in the setup)
            var deleteTracking = new Tracking(_trackingNumberDelete)
            {
                Slug = _slugDelete
            };
            Assert.True(connection.DeleteTracking(deleteTracking));

        }

        [Fact]
        public void TestDeleteTracking1()
        {
            //if the slug is bad informed
            try
            {
                var deleteTracking2 = new Tracking(_trackingNumberDelete2);
                Assert.True(connection.DeleteTracking(deleteTracking2));
                //always should give an exception before this
                Assert.True(false);
            }
            catch (Exception e)
            {
                Assert.Equal("{\"meta\":{\"code\":404,\"message\":\"The URI requested is invalid or the resource requested does not exist.\",\"type\":\"NotFound\"},\"data\":{\"resource\":\"/v4/trackings//798865638020\"}}",
                    e.Message);
            }

        }

        [Fact]
        public void TestDeleteTracking2()
        {

            //if the trackingNumber is bad informed
            try
            {
                var deleteTracking3 = new Tracking("adfa")
                {
                    Slug = "fedex"
                };
                Assert.True(connection.DeleteTracking(deleteTracking3));
                //always should give an exception before this
                Assert.True(false);
            }
            catch (Exception e)
            {
                Assert.Equal("{\"meta\":{\"code\":4005,\"message\":\"The value of `tracking_number` is invalid.\",\"type\":\"BadRequest\"},\"data\":{}}",
                    e.Message);
            }
        }

        [Fact]
        public void TestGetTrackingByNumber()
        {
            var trackingNumber = "3799517046";
            var slug = "dhl";

            var trackingGet1 = new Tracking(trackingNumber)
            {
                Slug = slug
            };

            Tracking tracking = connection.GetTrackingByNumber(trackingGet1);
            Assert.Equal(trackingNumber, tracking.TrackingNumber);
            Assert.Equal(slug, tracking.Slug);
            Assert.Null(tracking.ShipmentType);

            List<Checkpoint> checkpoints = tracking.Checkpoints;
            Checkpoint lastCheckpoint = checkpoints[checkpoints.Count - 1];
            Assert.NotNull(checkpoints);
            Assert.True(checkpoints.Count > 1);

            Assert.False(string.IsNullOrEmpty(lastCheckpoint.Message));
            Assert.False(string.IsNullOrEmpty(lastCheckpoint.CountryName));

        }

        [Fact]
        public void TestGetTrackingByNumber2()
        {

            //slug is bad informed
            try
            {
                var trackingGet2 = new Tracking("RC328021065CN");

                connection.GetTrackingByNumber(trackingGet2);
                //always should give an exception before this
                Assert.True(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Assert.Equal("{\"meta\":{\"code\":404,\"message\":\"The URI requested is invalid or the resource requested does not exist.\",\"type\":\"NotFound\"},\"data\":{\"resource\":\"/v4/trackings//RC328021065CN\"}}",
                    e.Message);
            }
        }

        [Fact]
        public void TestGetTrackingByNumber3()
        {

            //if the trackingNumber is bad informed
            try
            {
                var trackingGet3 = new Tracking("adf")
                {
                    Slug = "fedex"
                };
                connection.GetTrackingByNumber(trackingGet3);
                //always should give an exception before this
                Assert.True(false);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                Assert.Equal("{\"meta\":{\"code\":4005,\"message\":\"The value of `tracking_number` is invalid.\",\"type\":\"BadRequest\"},\"data\":{}}",
                    e.Message);
            }
        }

        [Fact]
        public void TestGetLastCheckpointID()
        {
            var trackingGet1 = new Tracking("whatever")
            {
                Id = "5550529d74346ecd5099ab47"
            };
            Checkpoint newCheckpoint = connection.GetLastCheckpoint(trackingGet1);
            Assert.False(string.IsNullOrEmpty(newCheckpoint.Message));
            Assert.Null(newCheckpoint.CountryName);
            Assert.Equal("Delivered", newCheckpoint.Tag);
        }

        [Fact]
        public void TestGetLastCheckpoint2ID()
        {
            var fields = new List<FieldCheckpoint>
            {
                FieldCheckpoint.message
            };
            var trackingGet1 = new Tracking("whatever")
            {
                Id = "555035fe74346ecd50998680"
            };

            Checkpoint newCheckpoint1 = connection.GetLastCheckpoint(trackingGet1, fields, "");
            Assert.False(string.IsNullOrEmpty(newCheckpoint1.Message));
            Assert.Equal("0001-01-01T00:00:00+08:00", DateMethods.ToString(newCheckpoint1.CreatedAt));

            fields.Add(FieldCheckpoint.created_at);
            Checkpoint newCheckpoint2 = connection.GetLastCheckpoint(trackingGet1, fields, "");
            Assert.False(string.IsNullOrEmpty(newCheckpoint2.Message));
            Assert.NotEqual("0001-01-01T00:00:00+00:00", DateMethods.ToString(newCheckpoint2.CreatedAt));
            Assert.False(string.IsNullOrEmpty(DateMethods.ToString(newCheckpoint2.CreatedAt)));

        }

        [Fact]
        public void TestGetLastCheckpoint3ID()
        {
            var fields = new List<FieldCheckpoint>
            {
                FieldCheckpoint.message
            };
            var trackingGet1 = new Tracking("whatever")
            {
                Id = "5550361716f0d77344cb80ad"
            };


            Checkpoint newCheckpoint1 = connection.GetLastCheckpoint(trackingGet1, fields, "");
            Assert.False(string.IsNullOrEmpty(newCheckpoint1.Message));

        }

        [Fact]
        public void TestGetTrackings()
        {


            //get the first 100 Trackings
            List<Tracking> listTrackings100 = connection.GetTrackings(1);
            // Assert.AreEqual(10, listTrackings100.Count);
            //at least we have 10 elements
            Assert.NotNull(listTrackings100[0].ToString());
            Assert.NotNull(listTrackings100[10].ToString());
        }

        [Fact]
        public void TestPutTracking()
        {
            var tracking = new Tracking("00340433836621378669")
            {
                Slug = "dhl-germany",
                Title = "another title"
            };

            Tracking tracking2 = connection.PutTracking(tracking);
            Assert.Equal("another title", tracking2.Title);

            //test post tracking number doesn't exist
            var tracking3 = new Tracking(_trackingNumberToDetectError)
            {
                Title = "another title"
            };

            try
            {
                connection.PutTracking(tracking3);
                //always should give an exception before this
                Assert.True(false);
            }
            catch (Exception e)
            {
                Assert.Equal("{\"meta\":{\"code\":404,\"message\":\"The URI requested is invalid or the resource requested does not exist.\",\"type\":\"NotFound\"},\"data\":{\"resource\":\"/v4/trackings//asdq\"}}", e.Message);
            }
        }

        [Fact]
        public void TestGetAllCouriers()
        {

            List<Courier> couriers = connection.GetAllCouriers();

            //check first courier
            Assert.False(string.IsNullOrEmpty(couriers[0].Slug));
            Assert.False(string.IsNullOrEmpty(couriers[0].Name));
            Assert.False(string.IsNullOrEmpty(couriers[0].Phone));
            Assert.False(string.IsNullOrEmpty(couriers[0].OtherName));
            Assert.False(string.IsNullOrEmpty(couriers[0].WebUrl));

            //total Couriers returned
            Assert.True(couriers.Count > 200);
            //try to acces with a bad API Key
            var connectionBadKey = new ConnectionAPI("badKey");

            try
            {
                connectionBadKey.GetCouriers();
            }
            catch (Exception e)
            {
                Assert.Equal("{\"meta\":{\"code\":401,\"message\":\"Invalid API key.\",\"type\":\"Unauthorized\"},\"data\":{}}", e.Message);
            }
        }

        [Fact]
        public void TestGetCouriers()
        {

            List<Courier> couriers = connection.GetCouriers();
            //total Couriers returned
            Assert.True(couriers.Count > 30);
            //check first courier

            Assert.False(string.IsNullOrEmpty(couriers[0].Slug));
            Assert.False(string.IsNullOrEmpty(couriers[0].Name));
            Assert.False(string.IsNullOrEmpty(couriers[0].Phone));
            Assert.False(string.IsNullOrEmpty(couriers[0].OtherName));
            Assert.False(string.IsNullOrEmpty(couriers[0].WebUrl));

            //try to acces with a bad API Key
            var connectionBadKey = new ConnectionAPI("badKey");

            try
            {
                connectionBadKey.GetCouriers();
            }
            catch (Exception e)
            {
                Assert.Equal("{\"meta\":{\"code\":401,\"message\":\"Invalid API key.\",\"type\":\"Unauthorized\"},\"data\":{}}", e.Message);
            }

        }

        [Fact]
        public void TestGetTrackings_A()
        {

            var parameters = new ParametersTracking();
            parameters.AddSlug("dhl");
            DateTime date = DateTime.Today.AddMonths(-1);


            parameters.CreatedAtMin = date;
            parameters.Limit = 50;

            List<Tracking> totalDHL = connection.GetTrackings(parameters);
            Assert.True(totalDHL.Count >= 1);
        }

        [Fact]
        public void TestGetTrackings_B()
        {

            var param1 = new ParametersTracking();
            param1.AddDestination(ISO3Country.DEU);
            param1.Limit = 20;
            List<Tracking> totalSpain = connection.GetTrackings(param1);
            Assert.True(totalSpain.Count >= 1);
        }

        [Fact]
        public void TestGetTrackings_C()
        {
            var param2 = new ParametersTracking();
            param2.AddTag(StatusTag.Delivered);
            param2.Limit = 50;

            List<Tracking> totalOutDelivery = connection.GetTrackings(param2);
            Assert.True(totalOutDelivery.Count > 10);
            Assert.True(totalOutDelivery.Count <= 50);

        }

        [Fact]
        public void TestGetTrackings_D()
        {
            var param3 = new ParametersTracking
            {
                Limit = 50
            };
            List<Tracking> totalOutDelivery1 = connection.GetTrackings(param3);
            Assert.True(totalOutDelivery1.Count > 10);
            Assert.True(totalOutDelivery1.Count <= 50);
        }

        [Fact]
        public void TestGetTrackings_E()
        {

            var param4 = new ParametersTracking
            {
                Keyword = "title"
            };
            param4.AddField(FieldTracking.title);
            param4.Limit = 50;

            List<Tracking> totalOutDelivery2 = connection.GetTrackings(param4);
            //  Assert.AreEqual( 2, totalOutDelivery2.Count);
            Assert.Equal("this title", totalOutDelivery2[0].Title);
        }

        [Fact]
        public void TestGetTrackings_F()
        {

            var param5 = new ParametersTracking();
            param5.AddField(FieldTracking.tracking_number);
            //param5.setLimit(50);

            List<Tracking> totalOutDelivery3 = connection.GetTrackings(param5);
            Assert.Null(totalOutDelivery3[0].Title);
        }
        [Fact]
        public void TestGetTrackings_G()
        {

            var param6 = new ParametersTracking();
            param6.AddField(FieldTracking.tracking_number);
            param6.AddField(FieldTracking.title);
            param6.AddField(FieldTracking.checkpoints);
            param6.AddField(FieldTracking.order_id);
            param6.AddField(FieldTracking.tag);
            param6.AddField(FieldTracking.order_id);
            //param6.setLimit(50);

            List<Tracking> totalOutDelivery4 = connection.GetTrackings(param6);
            Assert.Null(totalOutDelivery4[0].Slug);
        }
        [Fact]
        public void TestGetTrackings_H()
        {

            var param7 = new ParametersTracking();
            param7.AddOrigin(ISO3Country.ESP);
            // param7.setLimit(50);

            List<Tracking> totalOutDelivery5 = connection.GetTrackings(param7);
            Assert.Single(totalOutDelivery5);
        }

        [Fact]
        public void TestRetrack()
        {

            var tracking = new Tracking("00340433836621378669")
            {
                Slug = "dhl-germany"
            };
            try
            {
                connection.Retrack(tracking);
                Assert.True(false);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Assert.Contains("4016", e.Message);
                Assert.Contains("Retrack is not allowed. You can only retrack each shipment once.", e.Message);

            }
        }
    }
}
