using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
namespace Algoo
{
    class Program
    {
        public class movie
        {
            public string Mname;
            public List<string> actors = new List<string>();
            int Id;
        }
        class queries
        {
            public string actor1;
            public string actor2;
        }
        //class adjacentList
        //{
        //    LinkedList<Tuple<int, int>>[] adjList;
        //    public adjacentList(int no_of_actors)
        //    {
        //        adjList = new LinkedList<Tuple<int, int>>[no_of_actors];
        //        for(int i = 0; i < adjList.Length; i++)
        //        {
        //            adjList[i] = new LinkedList<Tuple<int, int>>();
        //        }
        //    }
        //    public void addEdgeAtEnd (int startV , int endV , int w )
        //    {
        //        adjList[startV].AddLast(new Tuple<int, int>(endV, w));

        //    }
        //    public int getnoOFactors() 
        //    {
        //        return adjList.Length;
        //    }
        //    public bool removeEdge(int startV, int endV, int w)
        //    {
        //        Tuple<int, int> edge = new Tuple<int, int>(endV, we);

        //        return adjList[startV].Remove(edge);
        //    }
        //    public void printAdjList() 
        //    {
        //        int index = 0; 
        //        foreach(LinkedList<Tuple<int,int>>  list in adjList)
        //        {
        //            Console.Write("adjList" + index + "->");
        //            foreach (Tuple<int,int> edge in  list){
        //                Console.Write(edge.Item1 + "(" + edge.Item2 + ")");
        //            }
        //            ++index;
        //            Console.WriteLine();
        //        } 
        //    }


        //}

        public static Dictionary<string, List<string>> actors = new Dictionary<string, List<string>>();

        public static class graph {
        public static void add(string actorname, string value)
        {
            if (actors.ContainsKey(actorname)) 
            {
                    List<string> list = actors[actorname];
                    if (list.Contains(value) == false)
                    {
                        list.Add(value);
                    }
                    //else
                    //{
                    //    break;
                    //}
            }
            else
                {
                    List<string> list = new List<string>();
                    list.Add(value);
                    actors.Add(actorname,list);
                    //this.Add(key, list);
                }
                
        }

            public static void print()
            {
                Console.WriteLine("actor is " + actors);
            }
        }
        public static List<movie> movies_list = new List<movie>();
        static void Main(string[] args)
        {


            FileStream fs = new FileStream("movies1.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string line = null;
            movie m = new movie();
            // my_movie.movie_name = line_splited[0];
            //foreach (var word in splitted_words)
            //{
            //graph.actors.Add(splitted_words[]);
            //    //System.Console.WriteLine("word");

            //}
            // [0] movie name , [1] actor name , [2] , actor name 2 
            while (sr.Peek() != -1)
            {                
                line = sr.ReadLine();

                string[] splitted_words = line.Split('/');
                m.Mname = splitted_words[0];
                for (int i = 1; i < splitted_words.Count(); i++)
                {
                //string v = splitted_words[i + 1];
                //graph.actors.Add(splitted_words[i],v);
                    graph.add(splitted_words[i], splitted_words[i + 1]);
                    m.actors.Add(splitted_words[i]);

                }
            movies_list.Add(m);

            //Console.WriteLine("Hello World!");
            }

            //for (int i = 0; i < actors.Count(); i++)
            //{
            //    Console.WriteLine("actor is " + graph.);
            //}

        }
        }
    }