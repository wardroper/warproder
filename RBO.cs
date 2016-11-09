using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBO2
{
    class RBO
    {
        static void Main(string[] args)
        {
            //test1
            string[] test1a = { "a", "b", "c", "d", "e", "f", "g", "h" };
            string[] test1b = { "h", "g", "f", "e", "d", "c", "b", "a" };
            double p1 = 0.95;
            double rboExpected1 = 0.771924;
            double rboActual1 = 0.0;

            rboActual1 = RBO(test1a, test1b, p1);
            Console.WriteLine(String.Format("Test 1: RBO extected is {0} and RBO Actual is {1}", rboExpected1, rboActual1));

            //test2
            string[] test2a = { "g", "a", "f", "c", "z" };
            string[] test2b = { "a", "b", "c", "d" };
            double p2 = 0.8;
            double rboExpected2 = 0.3786667;
            double rboActual2 = 0.0;

            rboActual2 = RBO(test2a, test2b, p2);
            Console.WriteLine(String.Format("Test 2: RBO extected is {0} and RBO Actual is {1}", rboExpected2, rboActual2));


            //test3
            string[] test3a = { "a", "b", "c", "d", "e", "f", "g", "h" };
            string[] test3b = { "b", "a", "d", "f", "c", "h" };
            double p3 = 0.8;
            double rboExpected3 = 0.6869504;
            double rboActual3 = 0.0;

            rboActual3 = RBO(test3a, test3b, p3);
            Console.WriteLine(String.Format("Test 3: RBO extected is {0} and RBO Actual is {1}", rboExpected3, rboActual3));


            //test4
            string[] test4a = { "a", "b", "c", "d", "e" };
            string[] test4b = { "b", "a", "g", "h", "e", "k", "l", "c" };
            double p4 = 0.9;
            double rboExpected4 = 0.6338971;
            double rboActual4 = 0.0;

            rboActual4 = RBO(test4a, test4b, p4);
            Console.WriteLine(String.Format("Test 4: RBO extected is {0} and RBO Actual is {1}", rboExpected4, rboActual4));

            Console.Read();
        }

        static double RBO(string[] s, string[] t, double pgiven = 0.98)
        {
            /** 
            * This calculates the Rank Biased Overlap(RBO) for two sorted lists. 
            *   * Based on "A Similarity Measure for Indefinite Rankings" William Webber, Alistair Moffat, 
            * and Justin Zobel (Nov 2010). 
            * 
            * For more information, read 
            * 	http://www.williamwebber.com/research/papers/wmz10_tois.pdf 
            * 
            * Based on the reference by Damian Gryski in Golang available from  
            *	https://github.com/dgryski 
            * 
            * Licensed under the MIT license. 
            * 
            */        
            
            
            //parameters 
            double p;
            double rbo = 0.0;
            double depth = 0.0;
            double overlap = 0.0;
            double shortDepth = -1.0;
            Dictionary<string, bool> seen = new Dictionary<string, bool> { };
            double wgt;
            double shortOverlap = -1.0;

            p = pgiven;

            wgt = (1.0 - p) / p;

            if (t.Length < s.Length)
            {
                var _t = s;
                s = t;
                t = _t;
            }

            for (int i = 0, l = s.Length; i < l; i++)
            {                
                var e1 = s[i];
                var e2 = t[i];

                bool value;

                if (shortDepth != -1.0)
                {
                    Console.WriteLine("RBO: update() called after EndShort()");
                } 

                if (e1 == e2)
                {
                    overlap++;
                }
                else
                {
                    if (seen == null || seen.TryGetValue(e1, out value))
                    {
                        seen[e1] = false;
                        overlap++;
                    }
                    else
                    {
                        seen.Add(e1, true);
                    }

                    if (seen.TryGetValue(e2, out value))
                    {
                        seen[e2] = false;
                        overlap++;
                    }
                    else
                    {
                        seen.Add(e2, true);
                    }
                }

                depth++;
                wgt *= p;
                rbo += (overlap / depth) * wgt;
            }


            //this.endShort();
            shortDepth = depth;
            shortOverlap = overlap;



            if (t.Length > s.Length)
            {
                for (int n = s.Length, le = t.Length; n < le; n++)
                {
                    //this.updateUneven(t[n]);
                    var e = t[n];

                    if (shortDepth == -1.0)
                    {
                        Console.WriteLine("RBO: UpdateUneven() called without EndShort()");
                    }

                    bool value;
            
                    if (seen.TryGetValue(e, out value))
                    {
                        overlap++;
                        seen[e] = false;
                    }


                    depth++;
                    wgt *= p;


                    rbo += (overlap / depth) * wgt;
                    rbo += ((shortOverlap * (depth - shortDepth)) / (depth * shortDepth)) * wgt;
                }
            }

            var pl = Math.Pow(p, depth);

            if (shortDepth == -1.0)
            {
                shortDepth = depth;
                shortOverlap = overlap;
            }

            return rbo + ((overlap - shortOverlap) / (depth) + ((shortOverlap) / (shortDepth))) * pl;
        }
    }
}
