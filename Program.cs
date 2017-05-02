using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;
namespace MostFrequentlyOrderedPizzaTopping
{
    class Program
    {
        static void Main(string[] args)
        {
            // get the topings 
            Dictionary<string, int> counts = new Dictionary<string, int>();
            
            StreamReader stream = new StreamReader(@"C:\users\john\downloads\pizzas.json");
            string text = stream.ReadToEnd();
            stream.Close();

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<RootObject>));
            MemoryStream stream2 = new MemoryStream(Encoding.UTF8.GetBytes(text));
            var obj = (List<RootObject>)ser.ReadObject(stream2);

             // make a count of each combination
            foreach(RootObject ro in obj)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string s in ro.toppings)
                {
                    if (sb.Length > 0)
                        sb.Append("," + s);
                    else
                        sb.Append(s);
                }

                if (counts.ContainsKey(sb.ToString()))
                {
                    int i = (int)counts[sb.ToString()];
                    counts[sb.ToString()] = ++i;
                }
                else
                {
                    counts[sb.ToString()] = 1;
                }
            }

            // sort by count
            var sortedDict = from entry in counts orderby entry.Value descending select entry;
             // get top 20
            var top20 =  sortedDict.Take(20);

            // write them out
            int rank=1;
            foreach (KeyValuePair<string, int> kp in top20)
            {
                string item = string.Format("{0}, {1}, {2}", rank++, kp.Value, kp.Key);
                Console.WriteLine(item);
            }

        }


    }


    [DataContract]
    public class RootObject
    {
        [DataMember]
        public List<string> toppings { get; set; }
    }


}
