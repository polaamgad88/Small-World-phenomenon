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






        public static List<movie> all_movies = new List<movie>(); 
        public static HashSet<string> all_actors = new HashSet<string>();
        public static Dictionary<string,List<string>> adj = new Dictionary<string, List<string>>();

        public static Dictionary<string, List<string>> Actors = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> Movies = new Dictionary<string, List<string>>();
        public static void graph ()
        {
            //for (int i = 0; i < all_actors.Count(); i++)
            //{
            //    adj.Add(new Dictionary<string, string>()); //adding a map in each index in the adjacency list

            //}
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

       

        static void Main(string[] args)
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


        }

    }
}
