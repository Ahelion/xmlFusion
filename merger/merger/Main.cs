using System;
using System.Xml;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace merger
{
	class MainClass
	{
		static string xml1="/home/mirciu/Documents/xml/base.xml";
		static string xml2="/home/mirciu/Documents/xml/source.xml";
		static int noTests=0;
		static string output="/home/mirciu/Documents/xml/output.xml";

		public static void Main (string[] args)
		{
			XElement baseXml = XElement.Load (xml1);
			XElement sourceXml = XElement.Load (xml2);

			/*create or open output file*/
			FileStream fs=new FileStream(output,FileMode.OpenOrCreate,FileAccess.ReadWrite,FileShare.None);

			/*add test nodes from source to base*/
			baseXml.Add(sourceXml.Descendants("test"));

			/*print and count*/
		    foreach (var child in baseXml.Elements("test")) 
			{
				/*count these motherfuckers*/
				noTests++;
				Console.WriteLine (child.Name);
			}

			/*Add new number of tests to*/
			var nrOfTests=baseXml.Element("details").Element("nrOfTests");

			nrOfTests.SetValue(noTests.ToString());

			/*Update failed tests*/

			var srcFailed=sourceXml.Element("details").Element("failed");
			var baseFailed=baseXml.Element("details").Element("failed");

			baseFailed.SetValue(int.Parse(baseFailed.Value)+int.Parse(srcFailed.Value));

			/*process pairs kei value*/
			var stats = baseXml.Elements("stats").Elements("pair");

			/*update relevant fields*/
			foreach (var child in stats.Descendants()) 
			{
				XElement nextNode=null;
				if(child.Name=="key" && child.Value=="User")
				{
					nextNode=(XElement)child.NextNode;
					nextNode.SetValue("bub");
				}
				if(child.Name=="key" && child.Value=="File")
				{
					nextNode=(XElement)child.NextNode;
					nextNode.SetValue(xml1);
				}
				if(child.Name=="key" && child.Value=="Date")
				{
					nextNode=(XElement)child.NextNode;
					nextNode.SetValue(DateTime.Now.ToShortDateString());
				}
			}

			baseXml.Save(fs);


		}
	}
}
