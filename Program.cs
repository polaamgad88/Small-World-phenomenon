using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq; 
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Data;
namespace Small_World_Phenomenon
{
    class Program
    {
       
        
        class Non_optimized {
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
            { public string actor1, actor2; }
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
                    //foreach (string ac in acs)
                    //    if (ac != actor)
                    //        actors.Add(ac);
                    foreach (string ac in acs.Where(ac => ac != actor))
                    {
                        actors.Add(ac);
                    }
                }

                adj[actor] = actors.ToList();
                actors = new HashSet<string>();
            }
        }
        static void BFS(string actor1_str, string actor2_str)
        {
            List<string> path = new List<string>();

            Dictionary<string, bool> visited = new Dictionary<string, bool>();
            Dictionary<string, string> parent = new Dictionary<string, string>();
            Queue<string> q = new Queue<string>();
            Dictionary<string, int> strngth = new Dictionary<string, int>();
            q.Enqueue(actor1_str);
            visited[actor1_str] = true;
            parent[actor1_str] = null;
            strngth.Add(actor1_str, 0);
            Dictionary<string, int> distance = new Dictionary<string, int>();
            distance.Add(actor1_str, 0);
            while (q.Count() != 0)
            {
                string node = q.Dequeue();
                foreach (string v in adj[node])
                {
                    if (!visited.ContainsKey(v))
                    {
                        visited[v] = false;

                        int x = strength(v, node);
                        strngth.Add(v, x + strngth[node]);
                        distance.Add(v, distance[node] + 1);
                    }
                    if (visited[v])
                    {
                        if (distance[v] > distance[node])
                        {
                            int x = strength(v, node);
                            x += strngth[node];
                            if (strngth[v] < x)
                            {
                                strngth[v] = x;
                            }
                        }
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
            degre_of_seprations(path);
            Console.WriteLine(" RoS = " + strngth[actor2_str]);
            Console.Write("CHAIN OF ACTORS:  =>   ");
            foreach (var vv in path)
            {
                Console.Write(vv + "->");
            }
            Console.WriteLine("");
            Console.Write("CHAIN OF MOVIES:  =>   ");
            chain_of_movies(path);


        }
        public static int strength(string s1, string s2)
        {
            List<string> s = new List<string>();
            s = Movies[s1].Intersect(Movies[s2]).ToList();

            return s.Count();
        }

        public static void degre_of_seprations(List<string> s)
        {
            int x = s.Count();
            x = x - 1;
            Console.Write("DoS =" + x + ", ");
        }
        //unused
        //public static void Relation_strenght(List<string> path)
        //{
        //    int sum = 0;
        //    List<string> ac = new List<string>();

        //    for (int i = 0; i < path.Count() - 1; i++)
        //    {
        //        ac = Movies[path[i]].Intersect(Movies[path[i + 1]]).ToList();
        //        sum += ac.Count();
        //    }  
        //    Console.Write(sum+ "\t");
        //}
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
                int s = chain[i].Count();
                foreach (string v in chain[i])
                {

                    if (s > 1)
                    {

                        //Console.Write(v+" or  " );
                        Console.Write(v + " or  ");

                    }
                    else if (i < chain.Count() - 1)
                    {
                        Console.Write(v + " =>" + "  ");

                    }
                    else
                    {
                        Console.Write(" " + v);
                    }
                    s--;
                }
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
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
            FileStream queries_file = new FileStream("queries2000.txt", FileMode.Open, FileAccess.Read);
            StreamReader queries_SR = new StreamReader(queries_file);
            string actors = null;
            //Console.WriteLine("Query" + "\t" + "Deg." + "\t" + "Rel." + "\t" + "Chain");
            while (queries_SR.Peek() != -1)
            {
                queries my_querie = new queries();
                actors = queries_SR.ReadLine();
                string[] actors_splited = actors.Split('/');
                my_querie.actor1 = actors_splited[0];
                my_querie.actor2 = actors_splited[1];
                //if sample test
                //Console.Write(my_querie.actor1 + "/" + my_querie.actor2);
                Console.WriteLine(my_querie.actor1 + "/" + my_querie.actor2);
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

            FileStream fs = new FileStream("Movies4736.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string line = null;
            // movie m = new movie();
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
    
    }
        class optimized
        {
            public class movie
            {
                public string Mname;
                public List<int> actors;
                public movie()
                {
                    actors = new List<int>();
                }
            }
            public class queries
            {
                public int actor1, actor2;
            }
            static int indexing = 0;
            public static List<movie> all_movies = new List<movie>();
            public static HashSet<int> all_actors = new HashSet<int>();
            public static Dictionary<string, int> actors_incoding = new Dictionary<string, int>();
            public static Dictionary<int, string> reversed_incoding = new Dictionary<int, string>();
            public static Dictionary<int, List<int>> adj = new Dictionary<int, List<int>>();
            public static Dictionary<string, List<int>> Actors = new Dictionary<string, List<int>>();
            public static Dictionary<int, List<string>> Movies = new Dictionary<int, List<string>>();


            public static void load_movies()
            {


                const Int32 BufferSize = 128;
                //            FileStream queries_file = new FileStream("queries26.txt", FileMode.Open, FileAccess.Read);

                using (var fileStream = File.OpenRead("D:/3rd year second term/Algo/Algoo/Algoo/bin/Debug/netcoreapp3.1/Movies4736.txt"))
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    String line;
                    while ((line = streamReader.ReadLine()) != null)

                    {
                        movie new_movie = new movie();



                        string[] line_splited = line.Split('/');

                        new_movie.Mname = line_splited[0];
                        Actors[new_movie.Mname] = new List<int>();


                        for (int i = 1; i < line_splited.Count(); i++)
                        {

                            if (!actors_incoding.ContainsKey(line_splited[i]))
                            {
                                actors_incoding.Add(line_splited[i], indexing);

                                reversed_incoding.Add(indexing, line_splited[i]);
                            }



                            Actors[new_movie.Mname].Add(actors_incoding[line_splited[i]]);

                            if (!Movies.ContainsKey(actors_incoding[line_splited[i]]))
                                Movies[actors_incoding[line_splited[i]]] = new List<string>();

                            Movies[actors_incoding[line_splited[i]]].Add(new_movie.Mname);
                            all_actors.Add(actors_incoding[line_splited[i]]);

                            new_movie.actors.Add(actors_incoding[line_splited[i]]);

                            indexing++;

                        }


                        all_movies.Add(new_movie);


                    }
                    streamReader.Close();
                    graph();

                }



            }
            public static void graph()
            {


                foreach (int actor in all_actors)
                {

                    HashSet<int> actors = new HashSet<int>();
                    foreach (string movie in Movies[actor])
                    {

                        foreach (int ac in Actors[movie])
                            if (ac != actor)
                                actors.Add(ac);
                    }
                    adj[actor] = actors.ToList();


                    actors = new HashSet<int>();
                }


            }
            static void BFS(int actor1_str, int actor2_str)
            {

                List<int> path = new List<int>();

                Dictionary<int, bool> visited = new Dictionary<int, bool>();
                Dictionary<int, int> parent = new Dictionary<int, int>();
                Queue<int> q = new Queue<int>();
                Dictionary<int, int> strngth = new Dictionary<int, int>();
                q.Enqueue(actor1_str);
                visited[actor1_str] = true;
                parent[actor1_str] = -1;
                strngth.Add(actor1_str, 0);
                Dictionary<int, int> distance = new Dictionary<int, int>();
                distance.Add(actor1_str, 0);
                while (q.Count() != 0)
                {
                    int node = q.Dequeue();

                    foreach (int v in adj[node])
                    {
                        if (!visited.ContainsKey(v))
                        {
                            visited[v] = false;

                            int x = strength(v, node);
                            strngth.Add(v, x + strngth[node]);
                            distance.Add(v, distance[node] + 1);
                        }
                        if (visited[v])
                        {
                            if (distance[v] > distance[node])
                            {
                                int x = strength(v, node);
                                x += strngth[node];
                                if (strngth[v] < x)
                                {
                                    strngth[v] = x;
                                }
                            }
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


                int cur = actor2_str;
                while (cur != -1)
                {
                    path.Add(cur);
                    cur = parent[cur];
                }
                path.Reverse();
                degre_of_seprations(path);
                Console.WriteLine(" RoS = " + strngth[actor2_str]);
                Console.Write("CHAIN OF ACTORS:  =>   ");
                foreach (var vv in path)
                {
                    Console.Write(reversed_incoding[vv] + "->");
                }

                Console.WriteLine("");
                Console.Write("CHAIN OF MOVIES:  =>   ");
                chain_of_movies(path);


            }
            public static int strength(int s1, int s2)
            {
                List<string> s = new List<string>();
                s = Movies[s1].Intersect(Movies[s2]).ToList();

                return s.Count();
            }

            public static void degre_of_seprations(List<int> s)
            {
                int x = s.Count();
                x = x - 1;
                Console.Write("DoS =" + x + ", ");
            }
            public static void Relation_strenght(List<int> path)
            {
                int sum = 0;
                List<string> ac = new List<string>();

                for (int i = 0; i < path.Count() - 1; i++)
                {
                    ac = Movies[path[i]].Intersect(Movies[path[i + 1]]).ToList();
                    sum += ac.Count();
                }
                Console.Write(sum + "\t");
            }
            public static void chain_of_movies(List<int> path)
            {
                //string[] mov = new string[path.Count()];
                Dictionary<int, List<string>> chain = new Dictionary<int, List<string>>();
                for (int i = 0; i < path.Count() - 1; i++)
                {
                    chain[i] = (Movies[path[i]].Intersect(Movies[path[i + 1]]).ToList());
                }
                for (int i = 0; i < chain.Count(); i++)
                {
                    int s = chain[i].Count();
                    foreach (string v in chain[i])
                    {

                        if (s > 1)
                        {

                            //Console.Write(v+" or  " );
                            Console.Write(v + " or  ");

                        }
                        else if (i < chain.Count() - 1)
                        {
                            Console.Write(v + " =>" + "  ");

                        }
                        else
                        {


                            Console.Write(" " + v);
                        }
                        s--;
                    }
                }
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }

            public static void load_queries()
            {
                string actors = null;
                // the following part is for loading queries
                const Int32 BufferSize = 128;
                using (var fileStream = File.OpenRead("D:/3rd year second term/Algo/Algoo/Algoo/bin/Debug/netcoreapp3.1/queries2000.txt"))
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    String line;
                    while ((line = streamReader.ReadLine()) != null)

                    {
                        queries my_querie = new queries();
                        actors = streamReader.ReadLine();
                        string[] actors_splited = actors.Split('/');
                        my_querie.actor1 = actors_incoding[actors_splited[0]];
                        my_querie.actor2 = actors_incoding[actors_splited[1]];
                        
                        Console.WriteLine(reversed_incoding[my_querie.actor1] + "/" + reversed_incoding[my_querie.actor2]);
                        BFS(my_querie.actor1, my_querie.actor2);
                    }
                    streamReader.Close();
                }
            }
        }
        public static void ExcutionTime(DateTime s , DateTime e) {
            TimeSpan Tss = (e - s);
            Console.WriteLine("Excutuion time in Seconds =  " + Tss.TotalSeconds);
            Console.WriteLine("Excutuion time in Milliseconds =  " + Tss.TotalMilliseconds);
            Console.WriteLine("Excutuion time in Minutes =       " + Tss.TotalMinutes);
        }
        static void Main(string[] args)
        {
            Console.WriteLine("'O' for optimized soultion , 'N' for non optimized:- ");
            string ans = Console.ReadLine();
            if (ans == "O")
            {
                DateTime start = DateTime.Now;
                optimized.load_movies();
                optimized.load_queries();
                Thread.Sleep(5000);
                DateTime end = DateTime.Now;
                ExcutionTime(start, end);
              
            }
            else
            {
                DateTime start = DateTime.Now;
                Non_optimized.load_movies();
                Non_optimized.load_queries();
                Thread.Sleep(5000);
                DateTime end = DateTime.Now;
                ExcutionTime(start, end);


            }
        }

    }
}
