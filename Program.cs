using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        continue;
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


            foreach (string v in path)
            {
                Console.Write(v + " ");

            }

            //Console.WriteLine();
            deg(path);



        }
        public static void deg(List<string> s)
        {
            int x = s.Count();
            x = x - 1;
            Console.WriteLine("Degree of sepration -> " + x);
        }

        public static void load_queries()
        {
            // the following part is for loading queries

            FileStream queries_file = new FileStream("queries1.txt", FileMode.Open, FileAccess.Read);

            StreamReader queries_SR = new StreamReader(queries_file);

            string actors = null;

            while (queries_SR.Peek() != -1)
            {

                queries my_querie = new queries();

                actors = queries_SR.ReadLine();

                string[] actors_splited = actors.Split('/');

                my_querie.actor1 = actors_splited[0];

                my_querie.actor2 = actors_splited[1];


                BFS(my_querie.actor1, my_querie.actor2);

            }

            queries_SR.Close();

        }
        public static void load_movies()
        {
            FileStream fs = new FileStream("movies1.txt", FileMode.Open, FileAccess.Read);
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
        }

    }
}
