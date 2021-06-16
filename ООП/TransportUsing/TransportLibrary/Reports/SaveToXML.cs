using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TransportLibrary.Reports
{
    public class SaveToXML
    {
        public static void SaveDriverReport(DriverReport driverReport)
        {
            List<DriverReport> reports;

            XmlSerializer formatter = new XmlSerializer(typeof(List<DriverReport>));

            using (FileStream fs = new FileStream(@"report\driverReports.xml", FileMode.OpenOrCreate))
            {
                try
                {
                    reports = (List<DriverReport>)formatter.Deserialize(fs);
                }
                catch
                {
                    reports = new List<DriverReport>();
                }
                reports.Add(driverReport);
            }

            using (FileStream fs = new FileStream(@"report\driverReports.xml", FileMode.Create))
            {
                formatter.Serialize(fs, reports);
            }
        }
        public static void SaveTransportReport(TransportReport transportReport)
        {
            List<TransportReport> reports;

            XmlSerializer formatter = new XmlSerializer(typeof(List<TransportReport>));

            using (FileStream fs = new FileStream(@"report\transportReports.xml", FileMode.OpenOrCreate))
            {
                try
                {
                    reports = (List<TransportReport>)formatter.Deserialize(fs);
                }
                catch
                {
                    reports = new List<TransportReport>();
                }
                reports.Add(transportReport);
            }

            using (FileStream fs = new FileStream(@"report\transportReports.xml", FileMode.Create))
            {
                formatter.Serialize(fs, reports);
            }
        }
        public static void SaveMarkReport(MarkReport markReport)
        {
            List<MarkReport> reports;

            XmlSerializer formatter = new XmlSerializer(typeof(List<MarkReport>));

            using (FileStream fs = new FileStream(@"report\markReports.xml", FileMode.OpenOrCreate))
            {
                try
                {
                    reports = (List<MarkReport>)formatter.Deserialize(fs);
                }
                catch
                {
                    reports = new List<MarkReport>();
                }
                reports.Add(markReport);
            }

            using (FileStream fs = new FileStream(@"report\markReports.xml", FileMode.Create))
            {
                formatter.Serialize(fs, reports);
            }
        }
        public static void SaveTypeReport(TypeReport typeReport)
        {
            List<TypeReport> reports;

            XmlSerializer formatter = new XmlSerializer(typeof(List<TypeReport>));

            using (FileStream fs = new FileStream(@"report\typeReports.xml", FileMode.OpenOrCreate))
            {
                try
                {
                    reports = (List<TypeReport>)formatter.Deserialize(fs);
                }
                catch
                {
                    reports = new List<TypeReport>();
                }
                reports.Add(typeReport);
            }

            using (FileStream fs = new FileStream(@"report\typeReports.xml", FileMode.Create))
            {
                formatter.Serialize(fs, reports);
            }
        }
    }
}
