﻿using System;
using Newtonsoft.Json;
using AftershipAPI.Enums;
using System.Collections.Generic;
using System.Threading;


namespace AftershipAPI
{

	public class Tests{



		static void Main(string[] args)
		{

			//Create an instance of ConnectionAPI using the token of the user
            // String key = System.IO.File.ReadAllText(@"\\psf\Home\Documents\aftership-key.txt");
            // ConnectionAPI connection_api = new ConnectionAPI(key, null);
            // ConnectionAPI connection_api_backup= new ConnectionAPI(key, "https://api-backup.aftership.com/");

			// string dhl_example = "1234567890";
			// Tracking tracking1 = new Tracking(dhl_example);
			// tracking1.slug = "dhl";
			// while (true) {
			// 	try
			// 	{
			// 		Tracking trackingPosted = connection_api.createTracking(tracking1);
			// 		connection_api.deleteTracking(tracking1);
			// 	}
			// 	catch (Exception e)
			// 	{
			// 		Console.WriteLine("failing in the api.aftershi.com");
			// 		Console.WriteLine(e);
			// 	}

			// 	try
			// 	{
			// 		Tracking trackingPosted = connection_api_backup.createTracking(tracking1);
			// 		connection_api_backup.deleteTracking(tracking1);
			// 	}
			// 	catch (Exception e)
			// 	{
			// 		Console.WriteLine("failing in the api.aftershi.com");
			// 		Console.WriteLine(e);
			// 	}
			// 	Thread.Sleep(1000);
			// }

			// Console.WriteLine("execution finished!!!");
            // Console.ReadLine();

            //
            //			//create a new tracking to add to our account
            //			Tracking newTracking = new Tracking ("7126900292");
            //			newTracking.slug = "dhl";
            //			newTracking.title = "this is a test";
            //			newTracking.addEmails ("emailprueba@gmail.com");
            //			newTracking.addEmails ("another@gmail.com");
            //			newTracking.addSmses("+85295340110");
            //			newTracking.addSmses("+85295349999");
            //			newTracking.customerName = "Mr Smith";
            //			newTracking.destinationCountryISO3 = ISO3Country.HKG;
            //			newTracking.orderID = "10000";
            //			newTracking.orderIDPath = "wwww.aaaaa.com";
            //			newTracking.trackingAccountNumber = "1234567";
            //			newTracking.trackingPostalCode = "28046";
            //			newTracking.trackingShipDate = "today";
            //			newTracking.addCustomFields("price","1000");
            //			newTracking.addCustomFields("product","iphone 5");
            //
            //			//before adding it, lets try to delete (otherwise maybe it would fail if already exist)
            //			try{
            //				//is important to catch the exceptions and read the messages,
            //				//it will give you information why the transactions went wrong
            //				if(connection.deleteTracking(newTracking))
            //					Console.Write("Tracking deleted!!");
            //			}catch(Exception e){
            //				Console.Write (e.Message);
            //			}
            //
            //			//now lets add the tracking
            //			Tracking trackingAdded = connection.createTracking(newTracking);
            //
            //			//the tracking is added, it return a Tracking with the information
            //			//that the system have of the tracking (probably it won't have the
            //			//Checkpoints, cause the system didn't have time to retrieve them from
            //			//the couriers, so we can retrieve the Tracking
            //			Tracking trackingGet = connection.getTrackingByNumber (trackingAdded);
            //
            //			//trackingGet will have all the information we want (including checkpoints)
            //			// there is 2 ways of get the tracking:
            //			//	* by id
            //			//	* by slug, tracking number and optinally required params
            //
            //			//example id:
            ////			Tracking trackingGet1 = new Tracking("");//you dont care about tracking number
            ////			trackingGet1.id = "53be255bfdacaaae7b17834b";
            ////			Tracking tracking3 = connection.getTrackingByNumber(trackingGet1);
            ////
            //			//example slug, tracking:
            //			Tracking trackingGet2 = new Tracking("RC328021065CN");
            //			trackingGet2.slug = "canada-post";
            //			connection.getTrackingByNumber (trackingGet2);
            //
            //			//example slug,tracking + required fields
            //			Tracking trackingGet3 = new Tracking("RT406182863DE");
            //			trackingGet3.slug = "deutsch-post";
            //			trackingGet3.trackingShipDate = "20140627";
            //			connection.getTrackingByNumber (trackingGet3);
            //
            //            Tracking trackingGet4 = new Tracking ("9405510897700003230737");
            //            trackingGet4.slug = "usps";
            ////            Tracking trackingAdded1 = connection.createTracking(trackingGet4);
            ////            Console.WriteLine (">>>>"+trackingAdded1.ToString ());
            //
            //            Tracking test_usps = connection.getTrackingByNumber (trackingGet4);
            //            Console.WriteLine ("");

            //            int i;
            //            List<Tracking> listTrackings = connection.getTrackings (1);
            //            Console.WriteLine ("Number of trackings-> "+listTrackings.Count);
            //
            //            for (i = 0; i < listTrackings.Count; i++) {
            //                Console.WriteLine (listTrackings [i].ToString ());
            //                ;
            //            }
            //            DateTime newDateTime = DateMethods.getDate ("2014-10-20T03:24:18-00:00");
            //
            //            Console.WriteLine (DateMethods.ToString(newDateTime));

            //            List<FieldCheckpoint> fields = new List<FieldCheckpoint>();
            //            fields.Add(FieldCheckpoint.message);
            //            Tracking trackingGet1 = new Tracking("whatever");
            //            trackingGet1.id = "53d1e35405e166704ea8adb9";
            //
            //          
            //            fields.Add(FieldCheckpoint.created_at);
            //            //            System.out.println("list:"+fields.toString());
            //            Checkpoint newCheckpoint2 = connection.getLastCheckpoint(trackingGet1,fields,"");
            ////            Assert.AreEqual( "Network movement commenced", newCheckpoint2.message);
            //            Console.Write (DateMethods.ToString(newCheckpoint2.createdAt));
            //          
            //
            //            ParametersTracking parameters = new ParametersTracking();
            //            parameters.addSlug("dhl");
            //            DateTime date = DateTime.Today.AddMonths(-1);

            //            parameters.createdAtMin = date;
            //            try{
            //            List<Tracking> totalDHL = connection.getTrackings(parameters);
            //            }catch(Exception e){
            //                Console.WriteLine (e);
            //            }
            //            ParametersTracking param1 = new ParametersTracking();
            //            param1.addDestination(ISO3Country.ESP);
            //            param1.setLimit(20);
            //            List<Tracking> totalSpain =connection.getTrackings(param1);
            //           
            //            List<Tracking> totalSpain2 =connection.getTrackingsNext(param1);
            //
            //            connection.getTrackings (1);
            //

            // ParametersTracking param3 = new ParametersTracking();
            // param3.limit = 50;
            // List<Tracking> totalOutDelivery1 = connection.getTrackings(param3);
            // Console.WriteLine(totalOutDelivery1);


        }

	}
}
;
