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
            var key = System.IO.File.ReadAllText(@"\\psf\Home\Documents\aftership-key.txt");
            connection = new ConnectionAPI(key);

            if (firstTime)
            {

                Console.WriteLine("****************SET-UP BEGIN**************");
                firstTime = false;
                //delete the tracking we are going to post (in case it exist)
                var tracking = new Tracking("05167019264110")
                {
                    slug = "dpd"
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

                try { connection.deleteTracking(tracking); }
                catch (Exception e)
                {
                    Console.WriteLine("**1" + e.Message);
                }
                var tracking1 = new Tracking(_trackingNumberToDetect)
                {
                    slug = "dpd"
                };
                try { connection.deleteTracking(tracking1); }
                catch (Exception e)
                {
                    Console.WriteLine("**2" + e.Message);
                }
                try
                {
                    var newTracking = new Tracking(_trackingNumberDelete)
                    {
                        slug = _slugDelete
                    };
                    connection.createTracking(newTracking);
                }
                catch (Exception e)
                {
                    Console.WriteLine("**3" + e.Message);
                }
                try
                {
                    var newTracking1 = new Tracking("9400110897700003231250")
                    {
                        slug = "usps"
                    };
                    connection.createTracking(newTracking1);
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
            List<Courier> couriers = connection.detectCouriers(_trackingNumberToDetect);
            Assert.Equal(3, couriers.Count);
            //the couriers should be dpd or fedex
            Console.WriteLine("**0" + couriers[0].slug);
            Console.WriteLine("**1" + couriers[1].slug);

            Assert.True(Equals(couriers[0].slug, _couriersDetected[0])
                || Equals(couriers[1].slug, _couriersDetected[0]));
            Assert.True(Equals(couriers[0].slug, _couriersDetected[1])
                || Equals(_couriersDetected[1], couriers[1].slug));

            //if the trackingNumber doesn't match any courier defined, should give an error.

            try
            {
                List<Courier> couriers1 = connection.detectCouriers(_trackingNumberToDetectError);
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
            List<Courier> couriers2 = connection.detectCouriers(_trackingNumberToDetect, "28046", "", null, slugs);
            Assert.Single(couriers2);
        }
        [Fact]
        public void TestCreateTracking()
        {
            var tracking1 = new Tracking(_trackingNumberPost)
            {
                slug = _slugPost,
                orderIDPath = _orderIDPathPost,
                customerName = _customerNamePost,
                orderID = _orderIDPost,
                title = _titlePost,
                destinationCountryISO3 = _countryDestinationPost
            };
            tracking1.addEmails(_email1Post);
            tracking1.addEmails(_email2Post);
            tracking1.addCustomFields("product_name", _customProductNamePost);
            tracking1.addCustomFields("product_price", _customProductPricePost);
            tracking1.addSmses(_sms1Post);
            tracking1.addSmses(_sms2Post);
            Tracking trackingPosted = connection.createTracking(tracking1);

            Assert.Equal(_trackingNumberPost, trackingPosted.trackingNumber);
            Assert.Equal(_slugPost, trackingPosted.slug);
            Assert.Equal(_orderIDPathPost, trackingPosted.orderIDPath);
            Assert.Equal(_orderIDPost, trackingPosted.orderID);
            Assert.Equal(_countryDestinationPost,
                trackingPosted.destinationCountryISO3);

            Assert.Contains(_email1Post, trackingPosted.emails);
            Assert.Contains(_email2Post, trackingPosted.emails);
            Assert.Equal(2, trackingPosted.emails.Count);

            Assert.Contains(_sms1Post, trackingPosted.smses);
            Assert.Contains(_sms2Post, trackingPosted.smses);
            Assert.Equal(2, trackingPosted.smses.Count);

            Assert.Equal(_customProductNamePost,
                trackingPosted.customFields["product_name"]);
            Assert.Equal(_customProductPricePost,
                trackingPosted.customFields["product_price"]);
        }

        [Fact]
        public void TestCreateTrackingEmptySlug()
        {
            //test post only informing trackingNumber (the slug can be dpd and fedex)
            var tracking2 = new Tracking(_trackingNumberToDetect);
            Tracking trackingPosted2 = connection.createTracking(tracking2);
            Assert.Equal(_trackingNumberToDetect, trackingPosted2.trackingNumber);
            Assert.Equal("dpd", trackingPosted2.slug);//the system assign dpd (it exist)


        }

        //test post tracking number doesn't exist
        [Fact]
        public void TestCreateTrackingError()
        {
            var tracking3 = new Tracking(_trackingNumberToDetectError);

            try
            {
                connection.createTracking(tracking3);
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
                slug = _slugDelete
            };
            Assert.True(connection.deleteTracking(deleteTracking));

        }

        [Fact]
        public void TestDeleteTracking1()
        {
            //if the slug is bad informed
            try
            {
                var deleteTracking2 = new Tracking(_trackingNumberDelete2);
                Assert.True(connection.deleteTracking(deleteTracking2));
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
                    slug = "fedex"
                };
                Assert.True(connection.deleteTracking(deleteTracking3));
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
                slug = slug
            };

            Tracking tracking = connection.getTrackingByNumber(trackingGet1);
            Assert.Equal(trackingNumber, tracking.trackingNumber);
            Assert.Equal(slug, tracking.slug);
            Assert.Null(tracking.shipmentType);

            List<Checkpoint> checkpoints = tracking.checkpoints;
            Checkpoint lastCheckpoint = checkpoints[checkpoints.Count - 1];
            Assert.NotNull(checkpoints);
            Assert.True(checkpoints.Count > 1);

            Assert.False(string.IsNullOrEmpty(lastCheckpoint.message));
            Assert.False(string.IsNullOrEmpty(lastCheckpoint.countryName));

        }

        [Fact]
        public void TestGetTrackingByNumber2()
        {

            //slug is bad informed
            try
            {
                var trackingGet2 = new Tracking("RC328021065CN");

                connection.getTrackingByNumber(trackingGet2);
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
                    slug = "fedex"
                };
                connection.getTrackingByNumber(trackingGet3);
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
                id = "5550529d74346ecd5099ab47"
            };
            Checkpoint newCheckpoint = connection.getLastCheckpoint(trackingGet1);
            Assert.False(string.IsNullOrEmpty(newCheckpoint.message));
            Assert.Null(newCheckpoint.countryName);
            Assert.Equal("Delivered", newCheckpoint.tag);
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
                id = "555035fe74346ecd50998680"
            };

            Checkpoint newCheckpoint1 = connection.getLastCheckpoint(trackingGet1, fields, "");
            Assert.False(string.IsNullOrEmpty(newCheckpoint1.message));
            Assert.Equal("0001-01-01T00:00:00+08:00", DateMethods.ToString(newCheckpoint1.createdAt));

            fields.Add(FieldCheckpoint.created_at);
            Checkpoint newCheckpoint2 = connection.getLastCheckpoint(trackingGet1, fields, "");
            Assert.False(string.IsNullOrEmpty(newCheckpoint2.message));
            Assert.NotEqual("0001-01-01T00:00:00+00:00", DateMethods.ToString(newCheckpoint2.createdAt));
            Assert.False(string.IsNullOrEmpty(DateMethods.ToString(newCheckpoint2.createdAt)));

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
                id = "5550361716f0d77344cb80ad"
            };


            Checkpoint newCheckpoint1 = connection.getLastCheckpoint(trackingGet1, fields, "");
            Assert.False(string.IsNullOrEmpty(newCheckpoint1.message));

        }

        [Fact]
        public void TestGetTrackings()
        {


            //get the first 100 Trackings
            List<Tracking> listTrackings100 = connection.getTrackings(1);
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
                slug = "dhl-germany",
                title = "another title"
            };

            Tracking tracking2 = connection.putTracking(tracking);
            Assert.Equal("another title", tracking2.title);

            //test post tracking number doesn't exist
            var tracking3 = new Tracking(_trackingNumberToDetectError)
            {
                title = "another title"
            };

            try
            {
                connection.putTracking(tracking3);
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

            List<Courier> couriers = connection.getAllCouriers();

            //check first courier
            Assert.False(string.IsNullOrEmpty(couriers[0].slug));
            Assert.False(string.IsNullOrEmpty(couriers[0].name));
            Assert.False(string.IsNullOrEmpty(couriers[0].phone));
            Assert.False(string.IsNullOrEmpty(couriers[0].other_name));
            Assert.False(string.IsNullOrEmpty(couriers[0].web_url));

            //total Couriers returned
            Assert.True(couriers.Count > 200);
            //try to acces with a bad API Key
            var connectionBadKey = new ConnectionAPI("badKey");

            try
            {
                connectionBadKey.getCouriers();
            }
            catch (Exception e)
            {
                Assert.Equal("{\"meta\":{\"code\":401,\"message\":\"Invalid API key.\",\"type\":\"Unauthorized\"},\"data\":{}}", e.Message);
            }
        }

        [Fact]
        public void TestGetCouriers()
        {

            List<Courier> couriers = connection.getCouriers();
            //total Couriers returned
            Assert.True(couriers.Count > 30);
            //check first courier

            Assert.False(string.IsNullOrEmpty(couriers[0].slug));
            Assert.False(string.IsNullOrEmpty(couriers[0].name));
            Assert.False(string.IsNullOrEmpty(couriers[0].phone));
            Assert.False(string.IsNullOrEmpty(couriers[0].other_name));
            Assert.False(string.IsNullOrEmpty(couriers[0].web_url));

            //try to acces with a bad API Key
            var connectionBadKey = new ConnectionAPI("badKey");

            try
            {
                connectionBadKey.getCouriers();
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
            parameters.addSlug("dhl");
            DateTime date = DateTime.Today.AddMonths(-1);


            parameters.createdAtMin = date;
            parameters.limit = 50;

            List<Tracking> totalDHL = connection.getTrackings(parameters);
            Assert.True(totalDHL.Count >= 1);
        }

        [Fact]
        public void TestGetTrackings_B()
        {

            var param1 = new ParametersTracking();
            param1.addDestination(ISO3Country.DEU);
            param1.limit = 20;
            List<Tracking> totalSpain = connection.getTrackings(param1);
            Assert.True(totalSpain.Count >= 1);
        }

        [Fact]
        public void TestGetTrackings_C()
        {
            var param2 = new ParametersTracking();
            param2.addTag(StatusTag.Delivered);
            param2.limit = 50;

            List<Tracking> totalOutDelivery = connection.getTrackings(param2);
            Assert.True(totalOutDelivery.Count > 10);
            Assert.True(totalOutDelivery.Count <= 50);

        }

        [Fact]
        public void TestGetTrackings_D()
        {
            var param3 = new ParametersTracking
            {
                limit = 50
            };
            List<Tracking> totalOutDelivery1 = connection.getTrackings(param3);
            Assert.True(totalOutDelivery1.Count > 10);
            Assert.True(totalOutDelivery1.Count <= 50);
        }

        [Fact]
        public void TestGetTrackings_E()
        {

            var param4 = new ParametersTracking
            {
                keyword = "title"
            };
            param4.addField(FieldTracking.title);
            param4.limit = 50;

            List<Tracking> totalOutDelivery2 = connection.getTrackings(param4);
            //  Assert.AreEqual( 2, totalOutDelivery2.Count);
            Assert.Equal("this title", totalOutDelivery2[0].title);
        }

        [Fact]
        public void TestGetTrackings_F()
        {

            var param5 = new ParametersTracking();
            param5.addField(FieldTracking.tracking_number);
            //param5.setLimit(50);

            List<Tracking> totalOutDelivery3 = connection.getTrackings(param5);
            Assert.Null(totalOutDelivery3[0].title);
        }
        [Fact]
        public void TestGetTrackings_G()
        {

            var param6 = new ParametersTracking();
            param6.addField(FieldTracking.tracking_number);
            param6.addField(FieldTracking.title);
            param6.addField(FieldTracking.checkpoints);
            param6.addField(FieldTracking.order_id);
            param6.addField(FieldTracking.tag);
            param6.addField(FieldTracking.order_id);
            //param6.setLimit(50);

            List<Tracking> totalOutDelivery4 = connection.getTrackings(param6);
            Assert.Null(totalOutDelivery4[0].slug);
        }
        [Fact]
        public void TestGetTrackings_H()
        {

            var param7 = new ParametersTracking();
            param7.addOrigin(ISO3Country.ESP);
            // param7.setLimit(50);

            List<Tracking> totalOutDelivery5 = connection.getTrackings(param7);
            Assert.Single(totalOutDelivery5);
        }

        [Fact]
        public void TestRetrack()
        {

            var tracking = new Tracking("00340433836621378669")
            {
                slug = "dhl-germany"
            };
            try
            {
                connection.retrack(tracking);
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
