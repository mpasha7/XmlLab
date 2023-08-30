using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;
using System.Xml.XPath;
using System.Xml;

namespace XmlTasks
{
    internal static class Tasks
    {
        private static readonly string textPath = @"C:\Users\Паша\source\repos\XmlLab\CreateXmlTasks\DBMS.txt";
        private static readonly string xmlPath = @"C:\Users\Паша\source\repos\XmlLab\CreateXmlTasks\newXML.xml";
        private static readonly string labTextPath = @"C:\Users\Паша\source\repos\XmlLab\CreateXmlTasks\Laboratory.txt";
        private static readonly string labXmlPath = @"C:\Users\Паша\source\repos\XmlLab\CreateXmlTasks\Laboratory.xml";

        public static void Task1()
        {
            XDocument xml = new XDocument(new XDeclaration("1.0", "utf-8", null));
            XElement? root = new XElement("root");
            xml.Add(root);
            using (StreamReader reader = new StreamReader(textPath, Encoding.UTF8))
            {
                string? line;
                int count = 1;
                while ((line = reader.ReadLine()) is not null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        string[] words = line.Split();
                        Array.Sort(words);
                        XElement lineElem = new XElement("line",
                            new XAttribute("num", count));
                        count++;
                        foreach (var word in words)
                        {
                            if (!string.IsNullOrWhiteSpace(word))
                            {
                                if (word.StartsWith("data:"))
                                {
                                    lineElem.Add(new XElement("instr", word.Substring(5)));
                                    continue;
                                }
                                lineElem.Add(new XElement("word", word));
                            }
                        }
                        root?.Add(lineElem);
                    }
                }
                xml.Save(xmlPath);
            }
            Console.WriteLine(xml);
        }

        public static void TaskToCreate()
        {
            string[] lines = File.ReadAllLines(textPath);

            XDocument xml = new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                new XElement("root", lines.Where(l => !string.IsNullOrWhiteSpace(l)).Select((l, i) =>
                    new XElement("line",
                        new XAttribute("num", i + 1),
                        l.Split().Where(w => !string.IsNullOrWhiteSpace(w)).Select(w =>
                        w.StartsWith("data:") ? new XElement("instr", w.Substring(5)) : new XElement("word", w))))));

            xml.Save(@"C:\Users\Паша\source\repos\XmlLab\CreateXmlTasks\newXML2.xml");
        }

        public static void Task2()
        {
            XDocument xml = XDocument.Load(@"C:\Users\Паша\source\repos\XmlLab\CreateXmlTasks\newXML.xml");
            XElement? root = xml.Element("root");
            if (root is not null)
            {
                foreach (XElement line in root.Elements("line"))
                {
                    XAttribute? num = line.Attribute("num");
                    Console.WriteLine($"num - {num?.Value}");
                    foreach (var instr in line.Elements("instr"))
                    {
                        Console.WriteLine($"\tinstr - {instr.Value}");
                    }
                    foreach (var word in line.Elements("word"))
                    {
                        Console.WriteLine($"\tword - {word.Value}");
                    }
                    Console.WriteLine();
                }
            }
        }
                
        public static void Task3()
        {
            XDocument xml = XDocument.Load(@"C:\Users\Паша\source\repos\XmlLab\CreateXmlTasks\newXML.xml");
            XElement? root = xml.Element("root");
            if (root is not null)
            {
                foreach (XElement line in root.Elements("line"))
                {
                    XAttribute? num = line.Attribute("num");
                    Console.WriteLine($"num - {num?.Value}");
                    foreach (var instr in line.Elements("instr"))
                    {
                        Console.WriteLine($"\tinstr - {instr.Value}");
                    }
                    foreach (var word in line.Elements("word"))
                    {
                        Console.WriteLine($"\tword - {word.Value}");
                    }
                    Console.WriteLine();
                }
            }
        }

        public static void TaskToAnalyze()
        {
            XDocument xml = XDocument.Load(@"C:\Users\Паша\source\repos\XmlLab\CreateXmlTasks\newXML.xml");
            xml?.Element("root")?.Elements("line").
                Where(l => l.Element("instr")?.Value == "SQL").
                Select(l => new
                {
                    num = l.Attribute("num")?.Value,
                    instr = l.Element("instr")?.Value,
                    words = l.Elements("word").Select(w => w.Value)
                }).ToList().ForEach(l =>
                {
                    Console.WriteLine($"num - {l.num}");
                    Console.WriteLine($"\tinstr - {l.instr}");
                    l.words.ToList().ForEach(w => Console.WriteLine($"\tword - {w}"));
                });


            //if (result is not null)
            //{
            //    foreach (var line in result)
            //    {
            //        Console.WriteLine($"num - {line.num}");
            //        Console.WriteLine($"\tinstr - {line.instr}");
            //        foreach (var word in line.words)
            //        {
            //            Console.WriteLine($"\tword - {word}");
            //        }
            //    }
            //}
        }

        public static void LabCreate()
        {
            string[] lines = File.ReadAllLines(labTextPath);
            XDocument xml = new XDocument(
                new XDeclaration("1.0", "windows-1251", null),
                new XElement("lab",
                    //new XAttribute("xmlns", @"http://tempuri.org/LabXML.xsd"),
                    lines.Where(line => !string.IsNullOrWhiteSpace(line)).Select((line, i) =>
                        new XElement("room",
                            new XAttribute("num", i + 1),
                            new XAttribute("title", line.Split()[0]),
                            new XAttribute("phone", line.Split()[1][4..]),
                            line.Split().Where(word => !string.IsNullOrWhiteSpace(word) && word.StartsWith("шкаф:")).Select(word =>
                                new XElement("case", 
                                    new XAttribute("title", word[5..word.IndexOf('^')]),
                                    word[(word.IndexOf('^')+1)..].Split('/').Where(item => !string.IsNullOrWhiteSpace(item)).Select(item => 
                                        new XElement("reagent", item[..item.IndexOf('&')],
                                            item[(item.IndexOf('&')+1)..].Split('~').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x =>
                                                x[..x.IndexOf(':')] switch
                                                {
                                                    "Q" => new XAttribute("quality", x[(x.IndexOf(':') + 1)..]),
                                                    "CAS" => new XAttribute("CAS", x[(x.IndexOf(':') + 1)..]),
                                                    "C" => new XAttribute("content", x[(x.IndexOf(':') + 1)..]),
                                                    _ => throw new Exception("REAGENT_SWITCH_EXCEPTION!")}))))),
                            line.Split().Where(word => !string.IsNullOrWhiteSpace(word) && word.StartsWith("прибор:")).Select(word =>
                                new XElement("device", word[(word.IndexOf(':')+1)..word.IndexOf('/')],
                                    new XAttribute("verificationDate", word[(word.IndexOf('/')+1)..])))))));
            Console.WriteLine(xml);
            xml.Save(labXmlPath);
        }
        
        public static void LabAnalyze()
        {
            XDocument xml = XDocument.Load(labXmlPath);
            Console.WriteLine("Лаборатории с приборами прошлогодней поверки:");
            xml?.Element("lab")?.Elements("room").
                Where(r => DateTime.Parse(r.Element("device")?.Attribute("verificationDate")?.Value!).Year < DateTime.Now.Year).
                //Where(r => r.Element("device").Value.StartsWith("Р")).
                Select(r => new
                {
                    num = r.Attribute("num")?.Value,
                    title = r.Attribute("title")?.Value,
                    devices = r.Elements("device").Select(d => new
                    {
                        title = d.Value,
                        verificationDate = d.Attribute("verificationDate")?.Value
                    })

                }).ToList().ForEach(r =>
                {
                    Console.WriteLine($"Lab №{r.num} - {r.title}");
                    r.devices.ToList().ForEach(d => Console.WriteLine($"\t{d.title} - {DateTime.Parse(d.verificationDate!).Year}"));
                });

            Console.WriteLine();
            Console.WriteLine("Комнаты и шкафы со списком реактивов без CAS-номера:");
            xml?.Element("lab")?.Elements("room").Select(r => new
            {
                title = r.Attribute("title")?.Value,
                cases = r.Elements("case").Select(c => new
                {
                    title = c.Attribute("title")?.Value,
                    reagents = c.Elements("reagent").Where(r => r.Attribute("CAS") is null).Select(r => new
                    {
                        name = r.Value,
                        quality = r.Attribute("quality")?.Value,
                        content = r.Attribute("content")?.Value
                    })
                })
            }).ToList().ForEach(r =>
            {
                Console.WriteLine(r.title);
                r.cases.ToList().ForEach(c =>
                {
                    Console.WriteLine($"\tШкаф: {c.title}");
                    c.reagents.ToList().ForEach(r =>
                    {
                        Console.WriteLine($"\t\t{r.name} ({r?.quality}, C(ОВ) = {r?.content}%)");
                    });
                });
            });
        }

        public static void LabEdit()
        {
            XDocument xml = XDocument.Load(labXmlPath);
            xml?.Root?.SetAttributeValue("child-count", xml?.Root?.Elements().Count());

            xml?.Root?.Elements().
                Where(e => e.Elements().Count() > 0).
                Select(e => e).ToList().ForEach(e => e.SetAttributeValue("child-count", e.Elements().Count()));


            xml?.Save(labXmlPath);
            Console.WriteLine(xml);
        }
    }
}
