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


        class Non_optimized
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
            { public string actor1, actor2; }
            public static List<movie> all_movies = new List<movie>();
            public static HashSet<string> all_actors = new HashSet<string>();
            public static Dictionary<string, List<string>> adj = new Dictionary<string, List<string>>();
            public static Dictionary<string, List<string>> Actors = new Dictionary<string, List<string>>();
            public static Dictionary<string, List<string>> Movies = new Dictionary<string, List<string>>();
            public static void graph()
            {
                DateTime A = new DateTime();
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
                DateTime B = new DateTime();
                //ExcutionTime(A, B);
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
                FileStream queries_file = new FileStream("queries600.txt", FileMode.Open, FileAccess.Read);
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

                FileStream fs = new FileStream("Movies14129.txt", FileMode.Open, FileAccess.Read);
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
                public string Mname;   // O(1)
                public List<int> actors;  //O(1)
                public movie()
                {
                    actors = new List<int>();
                }
            }
            public class queries
            {
                public int actor1, actor2; //O(1)
            }
            static int index = 0; // the index for giving every actor a number   O(1)
            public static List<movie> all_movies = new List<movie>(); //list of all movies   O(1)
            public static HashSet<int> all_actors = new HashSet<int>(); // hashset of all actors    O(1)
            public static Dictionary<string, int> act_id = new Dictionary<string, int>(); // giving every actor a id   O(1)     
            public static Dictionary<int, string> get_act = new Dictionary<int, string>();// dictionary to return the actor by gibimg it to id  O(1)
            public static Dictionary<int, List<int>> adj = new Dictionary<int, List<int>>();// the adjecent list for every actor   O(1)
            public static Dictionary<string, List<int>> Actors = new Dictionary<string, List<int>>(); // the actors which appeared in specific movie O(1)
            public static Dictionary<int, List<string>> Movies = new Dictionary<int, List<string>>(); // dictionary for storing the list of movies of every actor  O(1)

            public static Stopwatch totaltime;
            public static string MoviePath;
            public static string QueriePath;

            public static void load_movies()//O(N^2) 
            {


                var fileName = MoviePath; //O(1)
                var lines = File.ReadLines(fileName);  //O(1)
                var line_split = lines.Select(line => line.Split('/')).ToArray(); //O(1)



                foreach (string[] line_splited in line_split) //O(N^2)
                {
                    movie new_movie = new movie(); //O(1)


                    new_movie.Mname = line_splited[0];//O(1)
                    Actors[new_movie.Mname] = new List<int>();//O(1)


                    for (int i = 1; i < line_splited.Count(); i++)//O(N)
                    {

                        if (!act_id.ContainsKey(line_splited[i]))//O(N)
                        {
                            act_id.Add(line_splited[i], index);//O(1)

                            get_act.Add(index, line_splited[i]);//O(1)
                        }



                        Actors[new_movie.Mname].Add(act_id[line_splited[i]]);// O(1)

                        if (!Movies.ContainsKey(act_id[line_splited[i]]))// O(N)
                            Movies[act_id[line_splited[i]]] = new List<string>();// O(1)

                        Movies[act_id[line_splited[i]]].Add(new_movie.Mname);// O(1)
                        all_actors.Add(act_id[line_splited[i]]);// O(1)

                        new_movie.actors.Add(act_id[line_splited[i]]);// O(1)

                        index++;// O(1)

                    }


                    all_movies.Add(new_movie);// O(1)


                }


                graph();
            }
            public static void graph()//O(N^3)
            {


                foreach (int actor in all_actors)// O(N^3)
                {

                    HashSet<int> actors = new HashSet<int>();// O(1)
                    foreach (string movie in Movies[actor])// O(N^2)
                    {

                        foreach (int ac in Actors[movie])// O(N^2)
                            if (ac != actor)
                                actors.Add(ac);//O(1)
                    }
                    adj[actor] = actors.ToList();//O(1)


                    actors = new HashSet<int>();//O(1)
                }


            }
            static void BFS(int actor1_str, int actor2_str)// O(V+e)
            {

                List<int> path = new List<int>();//O(1)

                bool[] visited = new bool[adj.Count() * 1000];//O(1)
                int[] parent = new int[adj.Count() * 1000];//O(1)
                Queue<int> q = new Queue<int>();//O(1)
                int[] strngth = new int[adj.Count() * 1000];//O(1)
                q.Enqueue(actor1_str);//O(1)
                visited[actor1_str] = true;//O(1)
                parent[actor1_str] = -1;//O(1)
                strngth[actor1_str] = 0;//O(1)
                int[] distance = new int[adj.Count * 1000];//O(1)
                distance[actor1_str] = 0;//O(1)
                bool found = false;//O(1)
                bool foo = false;//O(1)
                while (q.Count() != 0)//O(V+e)
                {
                    int node = q.Dequeue();//O(1)

                    foreach (int v in adj[node])//O(e)
                    {
                        if (visited[v] == false)//O(N^2)
                        {
                            visited[v] = false;//O(1)

                            int x = strength(v, node);//O(N^2)
                            strngth[v] = x + strngth[node];//O(1)
                            distance[v] = distance[node] + 1;//O(1)

                        }
                        if (visited[v])//O(N^2)
                        {
                            if (distance[v] > distance[node])//O(N^2)
                            {
                                int x = strength(v, node);//O(N^2)
                                x += strngth[node];//O(1)
                                if (strngth[v] < x)//O(1)
                                {
                                    strngth[v] = x;//O(1)
                                }
                            }
                            continue;


                        }
                        visited[v] = true;//O(1)
                        parent[v] = node;//O(1)
                        q.Enqueue(v);//O(1)

                        if (v == actor2_str)//O(1)
                        {
                            found = true;//O(1)
                            break;//O(1)

                        }
                        if (found == true && distance[v] > distance[actor2_str])//O(1)
                        {
                            foo = true;//O(1)
                        }


                    }

                    if (foo == true)//O(1)
                    {
                        break;
                    }

                }


                int cur = actor2_str;//O(1)
                while (cur != -1)//O(N)
                {
                    path.Add(cur);//O(1)
                    cur = parent[cur];//O(1)
                }
                path.Reverse();
                degre_of_seprations(path);
                Console.WriteLine(" RoS = " + strngth[actor2_str]);//O(1)
                Console.Write("CHAIN OF ACTORS:  =>   ");//O(1)
                foreach (var actr in path)//O(N)  // for printing the chain of actors
                {
                    Console.Write(get_act[actr] + "->");
                }

                Console.WriteLine("");
                Console.Write("CHAIN OF MOVIES:  =>   ");// for printing the chain of movies
                chain_of_movies(path);


            }
            public static int strength(int s1, int s2)//O(N^2) // for calculating the relation strength between 2 actors
            {
                List<string> s = new List<string>();//O(1)
                s = Movies[s1].Intersect(Movies[s2]).ToList();//(N^2)

                return s.Count();
            }

            public static void degre_of_seprations(List<int> s)//O(1)  // for calculating the degree of sepration of the path
            {
                int x = s.Count();//O(1)
                x = x - 1;//O(1)
                Console.Write("DoS =" + x + ", ");//O(1)
            }

            public static void chain_of_movies(List<int> path)//O(N^2) // function for finding the chain of movies
            {

                Dictionary<int, List<string>> chain = new Dictionary<int, List<string>>(); // dictionary for saving the common movies between 2 actors in it 
                for (int i = 0; i < path.Count() - 1; i++)
                {
                    chain[i] = (Movies[path[i]].Intersect(Movies[path[i + 1]]).ToList());//O(N^2)
                }
                for (int i = 0; i < chain.Count(); i++) // check if there is more than 1 coomon film between 2 acors so printing Or between the 2 movie 
                {
                    int s = chain[i].Count();
                    foreach (string v in chain[i])
                    {

                        if (s > 1)
                        {

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

            public static void load_queries()//O(N)  // the following part is for loading queries
            {

                var fileName = QueriePath;
                using FileStream fs = File.OpenRead(fileName);
                using var sr = new StreamReader(fs);

                string line;

                while ((line = sr.ReadLine()) != null)
                {

                    queries my_querie = new queries();

                    string[] actors_splited = line.Split('/');
                    my_querie.actor1 = act_id[actors_splited[0]];
                    my_querie.actor2 = act_id[actors_splited[1]];

                    Console.WriteLine(get_act[my_querie.actor1] + "/" + get_act[my_querie.actor2]);
                    BFS(my_querie.actor1, my_querie.actor2);



                }
            }

            //var lines = File.ReadLines(path0);
            //foreach (var line in lines)
            //{
            //    queries my_querie = new queries();
            //string[] actors_splited = line.Split('/');
            //my_querie.actor1 = act_id[actors_splited[0]];
            //    my_querie.actor2 = act_id[actors_splited[1]];
            //    Console.WriteLine(get_act[my_querie.actor1] + "/" + get_act[my_querie.actor2]);
            //    BFS(my_querie.actor1, my_querie.actor2);

            //}
            // Process line
            public static void ExcutionTime(DateTime s, DateTime e) //////////  O(1)
            {
                TimeSpan Tss = (e - s);
                Console.WriteLine("Excutuion time in Seconds =  " + Tss.TotalSeconds);
                Console.WriteLine("Excutuion time in Milliseconds =  " + Tss.TotalMilliseconds);
                Console.WriteLine("Excutuion time in Minutes =       " + Tss.TotalMinutes);
            }
            static void Main(string[] args)
            {
                int testcase;
                Console.WriteLine("Enter the test you need:\n[1]Sample\n[2]Complete");
                testcase = int.Parse(Console.ReadLine());

                switch (testcase)
                {
                    #region Sample
                    case 1:
                        MoviePath = "movies1.txt";
                        QueriePath = "queries1.txt";


                        totaltime = Stopwatch.StartNew();
                        load_movies();
                        load_queries();

                        totaltime.Stop();
                        Console.WriteLine("Excutuion time in Seconds =  " + totaltime.Elapsed.TotalSeconds);
                        Console.WriteLine("Excutuion time in Milliseconds =  " + totaltime.Elapsed.TotalMilliseconds);
                        Console.WriteLine("Excutuion time in Minutes =       " + totaltime.Elapsed.TotalMinutes);

                        break;
                    #endregion
                    #region Complete
                    case 2:
                        Console.WriteLine("Enter the test you need:\n[1]Small\n[2]Medium\n[3]large\n[4]Extreme");
                        int test = int.Parse(Console.ReadLine());
                        switch (test)
                        {
                            #region Small
                            case 1:
                                Console.WriteLine("Enter the case you need:\n[1]Case 1\n[2]Case 2");
                                int test_small = int.Parse(Console.ReadLine());
                                switch (test_small)
                                {
                                    case 1:
                                        MoviePath = "Movies193.txt";
                                        QueriePath = "queries110.txt";





                                        totaltime = Stopwatch.StartNew();
                                        load_movies();
                                        load_queries();
                                        totaltime.Stop();
                                        Console.WriteLine("Excutuion time in Seconds =  " + totaltime.Elapsed.TotalSeconds);
                                        Console.WriteLine("Excutuion time in Milliseconds =  " + totaltime.Elapsed.TotalMilliseconds);
                                        Console.WriteLine("Excutuion time in Minutes =       " + totaltime.Elapsed.TotalMinutes);
                                        break;
                                    case 2:
                                        MoviePath = "Movies187.txt";
                                        QueriePath = "queries50.txt";

                                        totaltime = Stopwatch.StartNew();
                                        load_movies();
                                        load_queries();
                                        totaltime.Stop();
                                        Console.WriteLine("Excutuion time in Seconds =  " + totaltime.Elapsed.TotalSeconds);
                                        Console.WriteLine("Excutuion time in Milliseconds =  " + totaltime.Elapsed.TotalMilliseconds);
                                        Console.WriteLine("Excutuion time in Minutes =       " + totaltime.Elapsed.TotalMinutes);
                                        break;
                                }


                                break;
                            #endregion
                            #region Medium
                            case 2:
                                Console.WriteLine("Enter the Query you need:\n[1]Case 1 (Queries85)\n[2]Case 1 (Queries4000)\n[3]case 2 (Queries110)\n[4]case 2 (Queries2000)");
                                int test_medium = int.Parse(Console.ReadLine());
                                switch (test_medium)
                                {
                                    case 1:
                                        MoviePath = "Movies967.txt";
                                        QueriePath = "queries85.txt";

                                        totaltime = Stopwatch.StartNew();
                                        load_movies();
                                        load_queries();
                                        totaltime.Stop();
                                        Console.WriteLine("Excutuion time in Seconds =  " + totaltime.Elapsed.TotalSeconds);
                                        Console.WriteLine("Excutuion time in Milliseconds =  " + totaltime.Elapsed.TotalMilliseconds);
                                        Console.WriteLine("Excutuion time in Minutes =       " + totaltime.Elapsed.TotalMinutes);
                                        break;
                                    case 2:
                                        MoviePath = "Movies967.txt";
                                        QueriePath = "queries4000.txt";


                                        totaltime = Stopwatch.StartNew();
                                        load_movies();
                                        load_queries();
                                        totaltime.Stop();
                                        Console.WriteLine("Excutuion time in Seconds =  " + totaltime.Elapsed.TotalSeconds);
                                        Console.WriteLine("Excutuion time in Milliseconds =  " + totaltime.Elapsed.TotalMilliseconds);
                                        Console.WriteLine("Excutuion time in Minutes =       " + totaltime.Elapsed.TotalMinutes);
                                        break;
                                    case 3:
                                        MoviePath = "Movies4736.txt";
                                        QueriePath = "queries110.txt";

                                        totaltime = Stopwatch.StartNew();
                                        load_movies();
                                        load_queries();
                                        totaltime.Stop();
                                        Console.WriteLine("Excutuion time in Seconds =  " + totaltime.Elapsed.TotalSeconds);
                                        Console.WriteLine("Excutuion time in Milliseconds =  " + totaltime.Elapsed.TotalMilliseconds);
                                        Console.WriteLine("Excutuion time in Minutes =       " + totaltime.Elapsed.TotalMinutes);
                                        break;
                                    case 4:
                                        MoviePath = "Movies4736.txt";
                                        QueriePath = "queries2000.txt";

                                        totaltime = Stopwatch.StartNew();
                                        load_movies();
                                        load_queries();

                                        Console.WriteLine("Excutuion time in Seconds =  " + totaltime.Elapsed.TotalSeconds);
                                        Console.WriteLine("Excutuion time in Milliseconds =  " + totaltime.Elapsed.TotalMilliseconds);
                                        Console.WriteLine("Excutuion time in Minutes =       " + totaltime.Elapsed.TotalMinutes);
                                        break;
                                }

                                break;
                            #endregion
                            #region Large
                            case 3:
                                Console.WriteLine("Enter the Query you need:\n[1]Query26\n[2]Query600");
                                int test_large = int.Parse(Console.ReadLine());
                                switch (test_large)
                                {
                                    case 1:
                                        MoviePath = "Movies14129.txt";
                                        QueriePath = "queries26.txt";

                                        totaltime = Stopwatch.StartNew();
                                        load_movies();
                                        load_queries();
                                        Thread.Sleep(5000);
                                        totaltime.Stop();
                                        Console.WriteLine("Excutuion time in Seconds =  " + totaltime.Elapsed.TotalSeconds);
                                        Console.WriteLine("Excutuion time in Milliseconds =  " + totaltime.Elapsed.TotalMilliseconds);
                                        Console.WriteLine("Excutuion time in Minutes =       " + totaltime.Elapsed.TotalMinutes);
                                        break;
                                    case 2:
                                        MoviePath = "Movies14129.txt";
                                        QueriePath = "queries600.txt";

                                        totaltime = Stopwatch.StartNew();
                                        load_movies();
                                        load_queries();
                                        Thread.Sleep(5000);
                                        totaltime.Stop();
                                        Console.WriteLine("Excutuion time in Seconds =  " + totaltime.Elapsed.TotalSeconds);
                                        Console.WriteLine("Excutuion time in Milliseconds =  " + totaltime.Elapsed.TotalMilliseconds);
                                        Console.WriteLine("Excutuion time in Minutes =       " + totaltime.Elapsed.TotalMinutes);
                                        break;
                                }

                                break;
                            #endregion
                            #region Extreme
                            case 4:
                                Console.WriteLine("Enter the Query you need:\n[1]Query22\n[2]Query200");
                                int test_ex = int.Parse(Console.ReadLine());
                                switch (test_ex)
                                {
                                    case 1:
                                        MoviePath = "Movies122806.txt";
                                        QueriePath = "queries22.txt";


                                        totaltime = Stopwatch.StartNew();
                                        load_movies();
                                        load_queries();
                                        totaltime.Stop();
                                        Console.WriteLine("Excutuion time in Seconds =  " + totaltime.Elapsed.TotalSeconds);
                                        Console.WriteLine("Excutuion time in Milliseconds =  " + totaltime.Elapsed.TotalMilliseconds);
                                        Console.WriteLine("Excutuion time in Minutes =       " + totaltime.Elapsed.TotalMinutes);
                                        break;
                                    case 2:
                                        MoviePath = "Movies122806.txt";
                                        QueriePath = "queries200.txt";
                                        totaltime = Stopwatch.StartNew();
                                        load_movies();
                                        load_queries();
                                        totaltime.Stop();
                                        Console.WriteLine("Excutuion time in Seconds =  " + totaltime.Elapsed.TotalSeconds);
                                        Console.WriteLine("Excutuion time in Milliseconds =  " + totaltime.Elapsed.TotalMilliseconds);
                                        Console.WriteLine("Excutuion time in Minutes =       " + totaltime.Elapsed.TotalMinutes);
                                        break;
                                }
                                break;
                                #endregion

                        }

                        break;
                        #endregion

                }





            }

        }
    }
}
