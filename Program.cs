using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Threading;

using System.IO;
using System.Data;


namespace Small_World_Phenomenon
{
    class Program
    {

        public class movie
        {

            public string Mname;

            public List<string> actors;
            public movie()
            {

                actors = new List<string>();

            }


        }
        public class queries
        {

            public string actor1, actor2;

        }
        public static List<movie> all_movies = new List<movie>();
        public static HashSet<string> all_actors = new HashSet<string>();
        public static Dictionary<string, List<string>> adj = new Dictionary<string, List<string>>();

        public static Dictionary<string, List<string>> Actors = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> Movies = new Dictionary<string, List<string>>();
        public static void graph()
        {


            foreach (string actor in all_actors)
            {
                List<string> movies = Movies[actor];
                HashSet<string> actors = new HashSet<string>();
                foreach (string movie in movies)
                {
                    List<string> acs = Actors[movie];
                    foreach (string ac in acs)
                        if (ac != actor)
                            actors.Add(ac);
                }
                adj[actor] = actors.ToList();
                actors = new HashSet<string>();
            }



        }
        static void BFS(string actor1_str, string actor2_str)
        {
            List<string> path = new List<string>();

            Dictionary<string, bool> visited = new Dictionary<string, bool>();
            int size = all_actors.Count();
            Dictionary<string, string> parent = new Dictionary<string, string>();
            Queue<string> q = new Queue<string>();
            q.Enqueue(actor1_str);
            visited[actor1_str] = true;
            parent[actor1_str] = null;
            while (q.Count() != 0)
            {
                string node = q.Dequeue();
                foreach (string v in adj[node])
                {
                    if (!visited.ContainsKey(v))
                    {
                        visited[v] = false;
                    }
                    if (visited[v])
                    {
                        continue;
                    }
                    visited[v] = true;
                    parent[v] = node;
                    q.Enqueue(v);
                    if (v == actor2_str)
                    {
                        break;
                    }
                }
            }
            string cur = actor2_str;
            while (cur != null)
            {
                path.Add(cur);
                cur = parent[cur];
            }
            path.Reverse();
            //foreach (string v in path)
            //{
            //    Console.Write(v + "\t");
            //}
            //Console.WriteLine();
            degre_of_seprations(path);
            Relation_strenght(path);
            chain_of_movies(path);
            
        }
        public static void degre_of_seprations(List<string> s)
        {
            int x = s.Count();
            x = x - 1;
            Console.Write("\t" + x+ "\t");
        }
        public static void Relation_strenght(List<string> path)

        {
            int sum = 0;
            List<string> ac = new List<string>();

            for (int i = 0; i < path.Count() - 1; i++)
            {
                ac = Movies[path[i]].Intersect(Movies[path[i + 1]]).ToList();
                sum += ac.Count();
            }
            Console.Write(sum+ "\t");
        }
        public static void chain_of_movies(List<string> path)
        {
            //string[] mov = new string[path.Count()];
            Dictionary<int, List<string>> chain = new Dictionary<int, List<string>>();
            for (int i = 0; i < path.Count() - 1; i++)
            {

                chain[i] = (Movies[path[i]].Intersect(Movies[path[i + 1]]).ToList());


            }
            for (int i = 0; i < chain.Count(); i++)
            {
                foreach (string v in chain[i])
                    Console.Write(v + " ");
            }
            Console.WriteLine();

        }

        //public static void chain_of_movies (List<string> s)
        //{
        // could be optimized chain of movie ;;;;;;;;;;;
        //    int[] chain_index_of_movies = new int[s.Count()];
        //    for (int j = 0; j <Movies.Count ; j++) {
        //        for (int i = 0; i < s.Count(); i++)
        //        {
        //            if (Movies[j] == s[i])
        //            {
        //                chain_index_of_movies[i] = Movies[j];
        //            }
        //        }
        //    }
        //} 
        public static void load_queries()
        {
            // the following part is for loading queries

            FileStream queries_file = new FileStream("queries200.txt", FileMode.Open, FileAccess.Read);

            StreamReader queries_SR = new StreamReader(queries_file);

            string actors = null;
            Console.WriteLine("Query" + "\t" + "Deg." + "\t" + "Rel." + "\t" + "Chain");

            while (queries_SR.Peek() != -1)
            {

                queries my_querie = new queries();

                actors = queries_SR.ReadLine();

                string[] actors_splited = actors.Split('/');

                my_querie.actor1 = actors_splited[0];

                my_querie.actor2 = actors_splited[1];
                Console.Write(my_querie.actor1 + "/" + my_querie.actor2);
                BFS(my_querie.actor1, my_querie.actor2);
                
            }
            
            queries_SR.Close();

        }
        //public static void Relation_strenght(queries my, string a, string b)
        //{
        //    for (int i = 0; i < my.actor1.Count(); i++)
        //    {
        //        string[] common = Movies[a].Intersect(Movies[b]).ToArray();
        //        if (common[i] == null )
        //        {
        //            Relation_strenght();

        //        }
        //        Console.WriteLine("Relation strenght ---->> " + common.Count());
        //        //List<string> common = Movies[a].Intersect(Movies[b]).ToList();
        //        // Console.WriteLine(common.Count);
        //    }
          
            //for (int i = 0; i < Movies[a].Count(); i++)
            //{
            //    for(int j = 0; j < Movies[b].Count(); i++)
            //    {
            //        if (Movies[a][i] == Movies[b][j])
            //        {
            //            counter++;
            //            common_films.Add(a);
            //        }
            //        else
            //        {
            //            continue;
            //        }
            //    }
            //}

            //for (int i = 0; i < all_movies.Count(); i++)
            //{

            // common_films = (List<string>)adj[a].Intersect(adj[b]) ;

            //}
            // Console.WriteLine("Relation strenght ---->> " + common_films.Count());
       // }
        public static void load_movies()
        {
            FileStream fs = new FileStream("Movies122806.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string line = null;
            movie m = new movie();

            while (sr.Peek() != -1)
            {

                movie new_movie = new movie();

                line = sr.ReadLine();

                string[] line_splited = line.Split('/');

                new_movie.Mname = line_splited[0];
                Actors[new_movie.Mname] = new List<string>();

                for (int i = 1; i < line_splited.Count(); i++)
                {

                    Actors[new_movie.Mname].Add(line_splited[i]);
                    if (!Movies.ContainsKey(line_splited[i]))
                        Movies[line_splited[i]] = new List<string>();

                    Movies[line_splited[i]].Add(new_movie.Mname);
                    all_actors.Add(line_splited[i]);

                    new_movie.actors.Add(line_splited[i]);
                }


                all_movies.Add(new_movie);

            }
            sr.Close();
            graph();
        }
        static void Main(string[] args)
        {

            load_movies();
            load_queries();
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            Thread.Sleep(5000);
            stopwatch.Stop();

            TimeSpan ts = stopwatch.Elapsed;

            Console.WriteLine("Elapsed Time is {0:00}:{1:00}:{2:00}.{3}",
                            ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
        }

    }
}
