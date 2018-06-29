using AftershipAPI;
using AftershipAPI.Enums;
using System;
using System.Collections.Generic;
using Xunit;

namespace Test
{
        public class TestConnectionAPI
    {

        ConnectionAPI connection; 
        //static int TOTAL_COURIERS_API = 225;

        String[] couriersDetected = { "dpd", "fedex" };

        //post tracking number
        String trackingNumberPost = "05167019264110";
        String slugPost = "dpd";
        String orderIDPathPost = "www.whatever.com";
        String orderIDPost = "ID 1234";
        String customerNamePost = "Mr Smith";
        String titlePost = "this title";
        ISO3Country countryDestinationPost = ISO3Country.USA;
        String email1Post = "email@yourdomain.com";
        String email2Post = "another_email@yourdomain.com";
        String sms1Post = "+85292345678";
        String sms2Post = "+85292345679";
        String customProductNamePost = "iPhone Case";
        String customProductPricePost = "USD19.99";

        //tracking numbers to detect
        String trackingNumberToDetect = "09445246482536";
        String trackingNumberToDetectError = "asdq";

        //Tracking to Delete
        String trackingNumberDelete = "596454081704";
        String slugDelete = "fedex";


        //tracking to DeleteBad
        String trackingNumberDelete2 = "798865638020";

        static bool firstTime = true;

        Dictionary<string, string> firstCourier = new Dictionary<string, string>();
        Dictionary<string, string> firstCourierAccount = new Dictionary<string, string>();

        [TestInitialize()]
        public void setUp()
        { 
            String key = System.IO.File.ReadAllText(@"\\psf\Home\Documents\aftership-key.txt");
            connection = new ConnectionAPI(key);

            if (firstTime)
            {

                Console.WriteLine("****************SET-UP BEGIN**************");
                firstTime = false;
                //delete the tracking we are going to post (in case it exist)
                Tracking tracking = new Tracking("05167019264110");
                tracking.slug = "dpd";

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
                Tracking tracking1 = new Tracking(trackingNumberToDetect);
                tracking1.slug = "dpd";
                try { connection.deleteTracking(tracking1); }
                catch (Exception e)
                {
                    Console.WriteLine("**2" + e.Message);
                }
                try
                {
                    Tracking newTracking = new Tracking(trackingNumberDelete);
                    newTracking.slug = slugDelete;
                    connection.createTracking(newTracking);
                }
                catch (Exception e)
                {
                    Console.WriteLine("**3" + e.Message);
                }
                try
                {
                    Tracking newTracking1 = new Tracking("9400110897700003231250");
                    newTracking1.slug = "usps";
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
        public void testDetectCouriers()
        {

            //get trackings of this number.
            List<Courier> couriers = connection.detectCouriers(trackingNumberToDetect);
            Assert.Equal(3, couriers.Count);
            //the couriers should be dpd or fedex
            Console.WriteLine("**0" + couriers[0].slug);
            Console.WriteLine("**1" + couriers[1].slug);

            Assert.True(Equals(couriers[0].slug, couriersDetected[0])
                || Equals(couriers[1].slug, couriersDetected[0]));
            Assert.True(Equals(couriers[0].slug, couriersDetected[1])
                || Equals(couriersDetected[1], couriers[1].slug));

            //if the trackingNumber doesn't match any courier defined, should give an error.

            try
            {
                List<Courier> couriers1 = connection.detectCouriers(trackingNumberToDetectError);
                Assert.Equal(0, couriers1.Count);
            }
            catch (Exception e)
            {
                Assert.Equal("{\"meta\":{\"code\":4005,\"message\":\"The value of `tracking_number` is invalid.\",\"type\":\"BadRequest\"},\"data\":{\"tracking\":{\"tracking_number\":\"asdq\"}}}", e.Message);
            }

            List<String> slugs = new List<String>();
            slugs.Add("dtdc");
            slugs.Add("ukrposhta");
            slugs.Add("usps");
            //   slugs.add("asdfasdfasdfasd");
            slugs.Add("dpd");
            List<Courier> couriers2 = connection.detectCouriers(trackingNumberToDetect, "28046", "", null, slugs);
            Assert.Equal(1, couriers2.Count);
        }
        [Fact]
        public void TestCreateTracking()
        {
            Tracking tracking1 = new Tracking(trackingNumberPost);
            tracking1.slug = slugPost;
            tracking1.orderIDPath = orderIDPathPost;
            tracking1.customerName = customerNamePost;
            tracking1.orderID = orderIDPost;
            tracking1.title = titlePost;
            tracking1.destinationCountryISO3 = countryDestinationPost;
            tracking1.addEmails(email1Post);
            tracking1.addEmails(email2Post);
            tracking1.addCustomFields("product_name", customProductNamePost);
            tracking1.addCustomFields("product_price", customProductPricePost);
            tracking1.addSmses(sms1Post);
            tracking1.addSmses(sms2Post);
            Tracking trackingPosted = connection.createTracking(tracking1);

            Assert.Equal(trackingNumberPost, trackingPosted.trackingNumber, "#A01");
            Assert.Equal(slugPost, trackingPosted.slug, "#A02");
            Assert.Equal(orderIDPathPost, trackingPosted.orderIDPath, "#A03");
            Assert.Equal(orderIDPost, trackingPosted.orderID, "#A04");
            Assert.Equal(countryDestinationPost,
                trackingPosted.destinationCountryISO3, "#A05");

            Assert.True(trackingPosted.emails.Contains(email1Post), "#A06");
            Assert.True(trackingPosted.emails.Contains(email2Post), "#A07");
            Assert.Equal(2, trackingPosted.emails.Count, "#A08");

            Assert.True(trackingPosted.smses.Contains(sms1Post), "#A09");
            Assert.True(trackingPosted.smses.Contains(sms2Post), "#A10");
            Assert.Equal(2, trackingPosted.smses.Count, "#A11");

            Assert.Equal(customProductNamePost,
                trackingPosted.customFields["product_name"], "#A12");
            Assert.Equal(customProductPricePost,
                trackingPosted.customFields["product_price"], "#A13");
        }

        [Fact]
        public void TestCreateTrackingEmptySlug()
        {
            //test post only informing trackingNumber (the slug can be dpd and fedex)
            Tracking tracking2 = new Tracking(trackingNumberToDetect);
            Tracking trackingPosted2 = connection.createTracking(tracking2);
            Assert.Equal(trackingNumberToDetect, trackingPosted2.trackingNumber, "#A14");
            Assert.Equal("dpd", trackingPosted2.slug, "#A15");//the system assign dpd (it exist)


        }

        //test post tracking number doesn't exist
        [Fact]
        public void TestCreateTrackingError()
        {
            Tracking tracking3 = new Tracking(trackingNumberToDetectError);

            try
            {
                connection.createTracking(tracking3);
                //always should give an exception before this
                Assert.True(false, "#A16");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Assert.Equal("{\"meta\":{\"code\":4005,\"message\":\"The value of `tracking_number` is invalid.\",\"type\":\"BadRequest\"},\"data\":{\"tracking\":{\"tracking_number\":\"asdq\",\"title\":\"asdq\"}}}",
                    e.Message, "#A17");
            }
        }


        [Fact]
        public void testDeleteTracking()
        {

            //delete a tracking number (posted in the setup)
            Tracking deleteTracking = new Tracking(trackingNumberDelete);
            deleteTracking.slug = slugDelete;
            Assert.True(connection.deleteTracking(deleteTracking), "#A18");

        }

        [Fact]
        public void testDeleteTracking1()
        {
            //if the slug is bad informed
            try
            {
                Tracking deleteTracking2 = new Tracking(trackingNumberDelete2);
                Assert.True(connection.deleteTracking(deleteTracking2), "#A19");
                //always should give an exception before this
                Assert.True(false);
            }
            catch (Exception e)
            {
                Assert.Equal("{\"meta\":{\"code\":404,\"message\":\"The URI requested is invalid or the resource requested does not exist.\",\"type\":\"NotFound\"},\"data\":{\"resource\":\"/v4/trackings//798865638020\"}}",
                    e.Message, "#A20");
            }

        }

        [Fact]
        public void testDeleteTracking2()
        {

            //if the trackingNumber is bad informed
            try
            {
                Tracking deleteTracking3 = new Tracking("adfa");
                deleteTracking3.slug = "fedex";
                Assert.True(connection.deleteTracking(deleteTracking3), "#A20");
                //always should give an exception before this
                Assert.True(false, "#A21");
            }
            catch (Exception e)
            {
                Assert.Equal("{\"meta\":{\"code\":4005,\"message\":\"The value of `tracking_number` is invalid.\",\"type\":\"BadRequest\"},\"data\":{}}",
                    e.Message, "#A22");
            }
        }

        [Fact]
        public void testGetTrackingByNumber()
        {
            String trackingNumber = "3799517046";
            String slug = "dhl";

            Tracking trackingGet1 = new Tracking(trackingNumber);
            trackingGet1.slug = slug;

            Tracking tracking = connection.getTrackingByNumber(trackingGet1);
            Assert.Equal(trackingNumber, tracking.trackingNumber, "#A23");
            Assert.Equal(slug, tracking.slug, "#A24");
            Assert.Equal(null, tracking.shipmentType, "#A25");

            List<Checkpoint> checkpoints = tracking.checkpoints;
            Checkpoint lastCheckpoint = checkpoints[checkpoints.Count - 1];
            Assert.NotNull(checkpoints);
            Assert.True(checkpoints.Count > 1, "A25-2");

            Assert.False(string.IsNullOrEmpty(lastCheckpoint.message));
            Assert.False(string.IsNullOrEmpty(lastCheckpoint.countryName));

        }

        [Fact]
        public void testGetTrackingByNumber2()
        {

            //slug is bad informed
            try
            {
                Tracking trackingGet2 = new Tracking("RC328021065CN");

                connection.getTrackingByNumber(trackingGet2);
                //always should give an exception before this
                Assert.True(false, "#A26");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Assert.Equal("{\"meta\":{\"code\":404,\"message\":\"The URI requested is invalid or the resource requested does not exist.\",\"type\":\"NotFound\"},\"data\":{\"resource\":\"/v4/trackings//RC328021065CN\"}}",
                    e.Message, "#A27");
            }
        }

        [Fact]
        public void testGetTrackingByNumber3()
        {

            //if the trackingNumber is bad informed
            try
            {
                Tracking trackingGet3 = new Tracking("adf");
                trackingGet3.slug = "fedex";
                connection.getTrackingByNumber(trackingGet3);
                //always should give an exception before this
                Assert.True(false, "#A28");
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                Assert.Equal("{\"meta\":{\"code\":4005,\"message\":\"The value of `tracking_number` is invalid.\",\"type\":\"BadRequest\"},\"data\":{}}",
                    e.Message, "#A29");
            }
        }

        [Fact]
        public void testGetLastCheckpointID()
        {
            Tracking trackingGet1 = new Tracking("whatever");
            trackingGet1.id = "5550529d74346ecd5099ab47";
            Checkpoint newCheckpoint = connection.getLastCheckpoint(trackingGet1);
            Assert.False(string.IsNullOrEmpty(newCheckpoint.message));
            Assert.Equal(null, newCheckpoint.countryName);
            Assert.Equal("Delivered", newCheckpoint.tag);
        }

        [Fact]
        public void testGetLastCheckpoint2ID()
        {
            List<FieldCheckpoint> fields = new List<FieldCheckpoint>();
            fields.Add(FieldCheckpoint.message);
            Tracking trackingGet1 = new Tracking("whatever");
            trackingGet1.id = "555035fe74346ecd50998680";

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
        public void testGetLastCheckpoint3ID()
        {
            List<FieldCheckpoint> fields = new List<FieldCheckpoint>();
            fields.Add(FieldCheckpoint.message);
            Tracking trackingGet1 = new Tracking("whatever");
            trackingGet1.id = "5550361716f0d77344cb80ad";


            Checkpoint newCheckpoint1 = connection.getLastCheckpoint(trackingGet1, fields, "");
            Assert.False(string.IsNullOrEmpty(newCheckpoint1.message));

        }

        [Fact]
        public void testGetTrackings()
        {


            //get the first 100 Trackings
            List<Tracking> listTrackings100 = connection.getTrackings(1);
            // Assert.AreEqual(10, listTrackings100.Count);
            //at least we have 10 elements
            Assert.NotNull(listTrackings100[0].ToString());
            Assert.NotNull(listTrackings100[10].ToString());
        }

        [Fact]
        public void testPutTracking()
        {
            Tracking tracking = new Tracking("00340433836621378669");
            tracking.slug = "dhl-germany";
            tracking.title = "another title";

            Tracking tracking2 = connection.putTracking(tracking);
            Assert.Equal("another title", tracking2.title);

            //test post tracking number doesn't exist
            Tracking tracking3 = new Tracking(trackingNumberToDetectError);
            tracking3.title = "another title";

            try
            {
                connection.putTracking(tracking3);
                //always should give an exception before this
                Assert.Equal("This never should be executed", false);
            }
            catch (Exception e)
            {
                Assert.Equal("{\"meta\":{\"code\":404,\"message\":\"The URI requested is invalid or the resource requested does not exist.\",\"type\":\"NotFound\"},\"data\":{\"resource\":\"/v4/trackings//asdq\"}}", e.Message);
            }
        }

        [Fact]
        public void testGetAllCouriers()
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
            ConnectionAPI connectionBadKey = new ConnectionAPI("badKey");

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
        public void testGetCouriers()
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
            ConnectionAPI connectionBadKey = new ConnectionAPI("badKey");

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
        public void testGetTrackings_A()
        {

            ParametersTracking parameters = new ParametersTracking();
            parameters.addSlug("dhl");
            DateTime date = DateTime.Today.AddMonths(-1);


            parameters.createdAtMin = date;
            parameters.limit = 50;

            List<Tracking> totalDHL = connection.getTrackings(parameters);
            Assert.True(totalDHL.Count >= 1);
        }

        [Fact]
        public void testGetTrackings_B()
        {

            ParametersTracking param1 = new ParametersTracking();
            param1.addDestination(ISO3Country.DEU);
            param1.limit = 20;
            List<Tracking> totalSpain = connection.getTrackings(param1);
            Assert.True(totalSpain.Count >=1);
        }

        [Fact]
        public void testGetTrackings_C()
        {
            ParametersTracking param2 = new ParametersTracking();
            param2.addTag(StatusTag.Delivered);
            param2.limit = 50;

            List<Tracking> totalOutDelivery = connection.getTrackings(param2);
            Assert.True(totalOutDelivery.Count > 10);
            Assert.True(totalOutDelivery.Count <= 50);

        }

        [Fact]
        public void testGetTrackings_D()
        {
            ParametersTracking param3 = new ParametersTracking();
            param3.limit = 50;
            List<Tracking> totalOutDelivery1 = connection.getTrackings(param3);
            Assert.True(totalOutDelivery1.Count > 10);
            Assert.True(totalOutDelivery1.Count <= 50);
        }

        [Fact]
        public void testGetTrackings_E()
        {

            ParametersTracking param4 = new ParametersTracking();
            param4.keyword = "title";
            param4.addField(FieldTracking.title);
            param4.limit = 50;

            List<Tracking> totalOutDelivery2 = connection.getTrackings(param4);
            //  Assert.AreEqual( 2, totalOutDelivery2.Count);
            Assert.Equal("this title", totalOutDelivery2[0].title);
        }

        [Fact]
        public void testGetTrackings_F()
        {

            ParametersTracking param5 = new ParametersTracking();
            param5.addField(FieldTracking.tracking_number);
            //param5.setLimit(50);

            List<Tracking> totalOutDelivery3 = connection.getTrackings(param5);
            Assert.Equal(null, totalOutDelivery3[0].title);
        }
        [Fact]
        public void testGetTrackings_G()
        {

            ParametersTracking param6 = new ParametersTracking();
            param6.addField(FieldTracking.tracking_number);
            param6.addField(FieldTracking.title);
            param6.addField(FieldTracking.checkpoints);
            param6.addField(FieldTracking.order_id);
            param6.addField(FieldTracking.tag);
            param6.addField(FieldTracking.order_id);
            //param6.setLimit(50);

            List<Tracking> totalOutDelivery4 = connection.getTrackings(param6);
            Assert.Equal(null, totalOutDelivery4[0].slug);
        }
        [Fact]
        public void testGetTrackings_H()
        {

            ParametersTracking param7 = new ParametersTracking();
            param7.addOrigin(ISO3Country.ESP);
            // param7.setLimit(50);

            List<Tracking> totalOutDelivery5 = connection.getTrackings(param7);
            Assert.Equal(1, totalOutDelivery5.Count);
        }

        [Fact]
        public void testRetrack()
        {

            Tracking tracking = new Tracking("00340433836621378669");
            tracking.slug = "dhl-germany";
            try
            {
                connection.retrack(tracking);
                Assert.True(false);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Assert.True(e.Message.Contains("4016"));
                Assert.True(e.Message.Contains("Retrack is not allowed. You can only retrack each shipment once."));

            }
        }
    }
}
