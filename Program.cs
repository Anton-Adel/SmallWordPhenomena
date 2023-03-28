using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.IO;
namespace SmallWordPhenomena_project
{
    internal class Program
    {


        public static bool exist(Dictionary<string, List<string>>  Actors_graph, string actor)
        {
            if (Actors_graph.ContainsKey(actor))
                return true;
            else
                return false;
        }

        public static bool in_the_list(List<string> Actors_list, string actor)
        {
            foreach (string  key in Actors_list)
            {
                if (key == actor)
                    return true;

            }
            return false;
        }

        public static void create_graph(ref Dictionary<string, List<string>> Actors_graph, string actor1, string actor2, string movie, ref Dictionary<Tuple<string, string>, List<string>> num_of_movies)
        {

            if (exist(Actors_graph, actor1) && exist(Actors_graph, actor2))//Actor 1 & Actor 2 //o[v]
            {
                if (in_the_list(Actors_graph[actor1], actor2)) //o[E]
                {
                    Tuple<string, string> act1 = new Tuple<string, string>(actor1, actor2);
                    Tuple<string, string> act2 = new Tuple<string, string>(actor2, actor1);
                    if (num_of_movies.ContainsKey(act1)) //o[v]
                    {
                        num_of_movies[act1].Add(movie); //o[1]
                        
                    }
                    else
                    {
                        num_of_movies[act2].Add(movie);
                    }
                }
                else
                {
                    string add_actor2 = actor2;
                    Actors_graph[actor1].Add(add_actor2);
                    string add_actor1 =actor1;
                    Actors_graph[actor2].Add(add_actor1);
                    Tuple<string, string> act1 = new Tuple<string, string>(actor1, actor2);
                    List<string> MovieList = new List<string> { };
                    MovieList.Add(movie);
                    num_of_movies.Add(act1, MovieList);
                }
            }

            else if (exist(Actors_graph, actor1) && !exist(Actors_graph, actor2)) // Actor exist &Actor2 not
            {
                List<string> actors = new List<string>();
               string add_actor1 = actor1;
                actors.Add(add_actor1);
                Actors_graph.Add(actor2, actors);
                string add_actor2 = actor2;
                Actors_graph[actor1].Add(add_actor2);
                Tuple<string, string> act1 = new Tuple<string, string>(actor1, actor2);
                List<string> MovieList = new List<string> { };
                MovieList.Add(movie);
                num_of_movies.Add(act1, MovieList);
            }
            else if (!exist(Actors_graph, actor1) && exist(Actors_graph, actor2)) // Actor1 notexist &Actor2 Exist
            {
                List<string> actors = new List<string>();
                string add_actor2 = actor2;
                actors.Add(add_actor2);
                Actors_graph.Add(actor1, actors);
                string add_actor1 = actor1;
                Actors_graph[actor2].Add(add_actor1);
                Tuple<string, string> act1 = new Tuple<string, string>(actor1, actor2);
                List<string> MovieList = new List<string> { };
                MovieList.Add(movie);
                num_of_movies.Add(act1, MovieList);
            }
            else //Actor & Actor2 not Exist
            {
                List<string> actors = new List<string>();
                string add_actor2 =actor2;
                actors.Add(add_actor2);
                Actors_graph.Add(actor1, actors);
                actors = new List<string>();
                string add_actor1 = actor1;
                actors.Add(add_actor1);
                Actors_graph.Add(actor2, actors);
                Tuple<string, string> act1 = new Tuple<string, string>(actor1, actor2);
                List<string> MovieList = new List<string> { };
                MovieList.Add(movie);
                num_of_movies.Add(act1, MovieList);
            }
        }

        public static void init(Dictionary<string, List<string>> Actors_graph, ref Dictionary<string, string> color, ref Dictionary<string, int> distance, ref Dictionary<string, string> parent)
        {
            foreach (string key in Actors_graph.Keys)  //o[v]
            {  
                    color.Add(key, "white");
                    distance.Add(key, -1);
                    parent.Add(key, null);
            }
        }
        public static void Reinit(List<string> actors,ref Dictionary<string, string> color, ref Dictionary<string, int> distance, ref Dictionary<string, string> parent)
        {

            foreach (string actor in actors)
            {
                color[actor] = "white";
                distance[actor] = -1;
                parent[actor] = null;
;            }

        }

        //create bfs create fun elly hntl3 beha el output
        // static List<string> actors = new List<string>();

        static int stregnth_ = 0;
        static List<string> actors = new List<string>();
        //static string Actor="";
        public static bool BFS(string actor1, string actor2, Dictionary<string, List<string>> Actors_graph, ref Dictionary<string, string> color, ref Dictionary<string, int> distance, ref Dictionary<string, string> parent,  Dictionary<Tuple<string, string>, List<string>> num_of_movies)
        {
            stregnth_ = 0;
            // init(actor1, Actors_graph, ref color, ref distance, ref parent);
            color[actor1] = "grey";
            distance[actor1] = 0;
            // parent[actor1] = null;
            actors = new List<string>();
            actors.Add(actor1);
            Queue<string> queue = new Queue<string>();
            Dictionary<string ,int> stregnth=new Dictionary<string ,int>();
            bool found_actor = false;
            bool found_actor2 = false;
            queue.Enqueue(actor1);
            stregnth.Add(actor1, 0);    
            while (queue.Count != 0)
            {
                string actor_ = queue.Dequeue();
                if (found_actor == true && actor_ == actor2)
                {
                    //Console.WriteLine("Antttttttton");
                    return true;
                }
                if ( found_actor == true&&distance[actor_] == distance[actor2] )
                {
                    // Console.WriteLine("hiiii");  
                    return true;
                }

                foreach (string key in Actors_graph[actor_])  // keypair is Actor & movie
                {  
                    if (key == actor2)
                    { 
                        if (found_actor2 == false)
                        {
                            found_actor = true;
                            found_actor2 = true;
                            //List<string> list = new List<string>();
                            //list.Add(actor_);

                            color[key] = "grey";
                            actors.Add(key);
                            distance[key] = distance[actor_] + 1;
                            parent[key] = actor_;
                            queue.Enqueue(key);
                            Tuple<string, string> act1 = new Tuple<string, string>(actor_, key);
                            Tuple<string, string> act2 = new Tuple<string, string>(key, actor_);
                            if (num_of_movies.ContainsKey(act1))
                            {
                                int x = num_of_movies[act1].Count + stregnth[actor_];
                                stregnth_ = x;
                            }
                            else
                            {
                                int x = num_of_movies[act2].Count + stregnth[actor_];
                                stregnth_ = x;
                            }
                         
                        }
                        else if (distance[actor_] < distance[key])
                        {  
                            Tuple<string, string> act1 = new Tuple<string, string>(actor_, key);
                            Tuple<string, string> act2 = new Tuple<string, string>(key, actor_);
                            if (num_of_movies.ContainsKey(act1))
                            {
                                int x = num_of_movies[act1].Count + stregnth[actor_];
                                if (x > stregnth_)
                                {
                                    stregnth_ = x;

                                    parent[key] = actor_;
                                }//bounse
                            }
                            else
                            {
                                int x = num_of_movies[act2].Count + stregnth[actor_];
                                if (x > stregnth_)
                                {
                                    stregnth_ = x;
                                    parent[key] = actor_;
                                }//bounse
                            }
                           
                        }
                        break;
                    }


                     if (color[key] == "white")
                    {
                        //List<string> list_of_children = new List<string>(); // a d c      a b c
                        //list_of_children.Add(actor_);
                      
                        color[key] = "grey";
                        actors.Add(key);
                        distance[key] = distance[actor_] + 1;
                        parent[key] = actor_;
                        queue.Enqueue(key);
                        Tuple<string, string> act1 = new Tuple<string, string>(actor_, key);
                        Tuple<string, string> act2 = new Tuple<string, string>(key, actor_);
                        if (num_of_movies.ContainsKey(act1))
                        {
                            int x = num_of_movies[act1].Count + stregnth[actor_];
                            stregnth.Add(key, x);
                        }
                        else
                        {
                            int x = num_of_movies[act2].Count + stregnth[actor_];
                            stregnth.Add(key, x);
                        }


                    }
                    else if (key != actor1&& distance[actor_] < distance[key])
                    {
                        
                           Tuple<string, string> act1 = new Tuple<string, string>(actor_, key);
                            Tuple<string, string> act2 = new Tuple<string, string>(key, actor_);
                            if (num_of_movies.ContainsKey(act1))
                            {
                                int x = num_of_movies[act1].Count + stregnth[actor_];
                                if (x > stregnth[key])
                                {  
                                   stregnth[key] = x;
                            
                                  parent[key] = actor_;
                                }//bounse
                           
                            }
                            else
                            
                            {
                                int x = num_of_movies[act2].Count + stregnth[actor_];
                                if (x > stregnth[key])
                                {
                                   stregnth[key] = x;
                                    parent[key] = actor_;
                                }//bounse
                            
                            }   
                    }
                }
                color[actor_] = "black";
            }
            return found_actor;
        }

        public static void get_chain(string actor1, string actor2, ref Dictionary<string, string> color, ref Dictionary<string, string> parent, Dictionary<Tuple<string, string>, List<string>> num_of_movies,ref Dictionary<string, int> distance)
        {
           
            List<string> actual_actors_list1 = new List<string>();
            List<string> actual_actors_list = new List<string>();
            List<string> MovieList = new List<string>();
            string act = actor2;

            Tuple<string, string> act1;
            Tuple<string, string> act2;
            while (parent[act] != null)
            {
                actual_actors_list.Add(parent[act]);// f h  h e   h e f
                act = parent[act];

            }
            actual_actors_list1 = actual_actors_list;

            actual_actors_list = new List<string>();
            for (int i = actual_actors_list1.Count - 1; i >= 0; i--)
            {
                actual_actors_list.Add(actual_actors_list1[i]);
                //Console.WriteLine(actual_actors_list1[i]);

            }


            int degree_of_separation = distance[actor2];
            actual_actors_list.Add(actor2);
            if (degree_of_separation > 1)
            { 
                for (int i = 0; i < actual_actors_list.Count - 1; i++)
                {
                    act1 = new Tuple<string, string>(actual_actors_list[i], actual_actors_list[i + 1]);
                    act2 = new Tuple<string, string>(actual_actors_list[i + 1], actual_actors_list[i]);
                    if (num_of_movies.ContainsKey(act1))
                    {


                        //int counter_ = 0;
                        //foreach (string key in num_of_movies[act1])
                        //{
                        //    if (counter_ < num_of_movies[act1].Count - 1)
                        //        MovieList.Add(key + " or ");
                        //    else
                        //        MovieList.Add(key + " => ");
                        //    counter_++;
                        //}
                        MovieList.Add(num_of_movies[act1][0]);
                        //  relation_strength += num_of_movies[act1].Count;
                    }
                    else
                    {

                        //int counter_ = 0;
                        //foreach (string key in num_of_movies[act2])
                        //{
                        //    if (counter_ < num_of_movies[act2].Count - 1)
                        //        MovieList.Add(key + " or ");
                        //    else
                        //        MovieList.Add(key + " => ");
                        //    counter_++;
                        //}
                        MovieList.Add(num_of_movies[act2][0]);
                        //relation_strength += num_of_movies[act2].Count;
                    }
                }
            }
            else
            {
                
                act1 = new Tuple<string, string>(actor1, actor2);
                act2 = new Tuple<string, string>(actor2, actor1);
                if (num_of_movies.ContainsKey(act1))
                {

                    //int counter_ = 0;
                    //foreach (string key in num_of_movies[act1])
                    //{
                    //    if (counter_ < num_of_movies[act1].Count - 1)
                    //        MovieList.Add(key + " or ");
                    //    else
                    //        MovieList.Add(key + " => ");
                    //    counter_++;
                    //}
                    MovieList.Add(num_of_movies[act1][0]);
                    //relation_strength += num_of_movies[act1].Count;
                }
                else
                {

                    //int counter_ = 0;
                    //foreach (string key in num_of_movies[act2])
                    //{
                    //    if (counter_ < num_of_movies[act2].Count - 1)
                    //        MovieList.Add(key + " or ");
                    //    else
                    //        MovieList.Add(key + " => ");
                    //    counter_++;
                    //}
                    MovieList.Add(num_of_movies[act2][0]);
                    // relation_strength += num_of_movies[act2].Count;
                }
            }
            Console.Write(actor1 + '/');
            Console.WriteLine(actor2 );
            Console.Write("DS = "+degree_of_separation.ToString() + "\t\t");
            Console.WriteLine("RS = "+stregnth_.ToString());
            int x = actual_actors_list.Count-1;
            int counter = 0;
            Console.Write("Actors = ");
            foreach (string key in actual_actors_list)
            {
                if (counter < x)
                    Console.Write(key + " -> ");
                else
                    Console.Write(key);
                counter++;
            }
            Console.WriteLine();
            x = MovieList.Count-1;
             counter = 0;
            Console.Write("Movies = ");
            foreach (string movie in MovieList)
            {
                //Console.WriteLine("Adel");

                if (counter < x)
                    Console.Write(movie+" => " );
                else
                    Console.Write(movie);
                counter++;
            }
            Console.WriteLine();
            Reinit(actors, ref color, ref distance, ref parent);
        }


        //public static void RUN(string movies_path, string quere_path, ref Dictionary<string, List<KeyValuePair<string, string>>> Actors_graph, ref Dictionary<Tuple<string, string>, List<string>> num_of_movies
        //    ,ref Dictionary<string, List<string>> parentList,ref Dictionary<string, string> parent,ref Dictionary<string, string> color , ref Dictionary<string, int> distance )

        //{ 
        //var watch = new System.Diagnostics.Stopwatch();

        //    watch.Start();
        //    string[] lines = File.ReadAllLines(@movies_path);
        //    string[] Actors;
        //    foreach (string author in lines)
        //    {
        //        // Actors = author.Split(' ');
        //        Actors = author.Split('/');
        //        for (int i = 1; i < Actors.Length - 1; i++)
        //        {
        //            for (int j = i + 1; j < Actors.Length; j++)
        //            {

        //                create_graph(ref Actors_graph, Actors[i], Actors[j], Actors[0], ref num_of_movies);
        //            }
        //        }


        //    }
        //    if (BFS("C", "F", Actors_graph, ref color, ref distance, ref parent, ref parentList))
        //        get_chain("C", "F", parent, num_of_movies, parentList);
        //    //string[] queres = File.ReadAllLines(@quere_path);
        //    //Console.Write("queres" + "\t\t");
        //    //Console.Write("Degree" + "\t\t");
        //    //Console.Write("relation" + "\t");
        //    //Console.WriteLine("chain");


        //    //foreach (string quere in queres)
        //    //{
        //    //    Actors = quere.Split('/');

        //    //    color = new Dictionary<string, string>();
        //    //    distance = new Dictionary<string, int>();
        //    //    parent = new Dictionary<string, string>();
        //    //    parentList = new Dictionary<string, List<string>>();

        //    //    if (BFS(Actors[0], Actors[1], Actors_graph, ref color, ref distance, ref parent, ref parentList))
        //    //    {

        //    //        // Console.WriteLine("Anton");
        //    //        get_chain(Actors[0], Actors[1], parent, num_of_movies, parentList);

        //    //    }

        //    //    else
        //    //    {
        //    //        Console.Write(Actors[0] + '/');
        //    //        Console.Write(Actors[1] + "\t\t");
        //    //        Console.Write("0" + "\t\t");
        //    //        Console.Write("0" + "\t\t");
        //    //        Console.Write("there is no relation");
        //    //    }

        //    //}
        //    watch.Stop();
        //    var time = watch.Elapsed;

        //    Console.WriteLine($"Execution Time: {time} ms");


        //}



        static void Main(string[] args)
        {
            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();
            Dictionary<string, List<string>> Actors_graph = new Dictionary<string, List<string>>();

            Dictionary<Tuple<string, string>, List<string>> num_of_movies = new Dictionary<Tuple<string, string>, List<string>>();

            Dictionary<string, string> parent = new Dictionary<string, string>();

            Dictionary<string, string> color = new Dictionary<string, string>();

            Dictionary<string, int> distance = new Dictionary<string, int>();
       

          //  string[] lines = File.ReadAllLines(@"movies14129.txt");
            //var reader=new StreamReader(@"movies193.txt");
            string[] reader = File.ReadAllLines(@"movies122806.txt");
            string[] Actors;
           foreach(string line in reader)
            { 
               // var nextlint=reader.ReadLine();
                Actors = line.Split('/');
                for (int i = 1; i < Actors.Length - 1; i++)
                {
                    for (int j = i + 1; j < Actors.Length; j++)
                    {
                        create_graph(ref Actors_graph, Actors[i], Actors[j], Actors[0], ref num_of_movies);
                    }
                }

            }
            init( Actors_graph, ref color, ref distance, ref parent);

            string[] queres = File.ReadAllLines(@"queries22.txt");
            //Console.Write("queres" + "\t\t");
            //Console.Write("Degree" + "\t\t");
            //Console.Write("relation" + "\t");
            //Console.WriteLine("chain");
            foreach (string quere in queres)
            {
                Actors = quere.Split('/');

                //color = new Dictionary<string, string>();
                //distance = new Dictionary<string, int>();
                //parent = new Dictionary<string, string>();
               
                if (BFS(Actors[0], Actors[1], Actors_graph, ref color, ref distance, ref parent,num_of_movies))
                {

                    // Console.WriteLine("Anton");
                    get_chain(Actors[0], Actors[1], ref color,ref  parent, num_of_movies,ref distance);

                }

                else
                {
                    Console.Write(Actors[0] + '/');
                    Console.Write(Actors[1] + "\t\t");
                    Console.Write("0" + "\t\t");
                    Console.Write("0" + "\t\t");
                    Console.Write("there is no relation");
                }

            }
            watch.Stop();
            var time = watch.Elapsed;

            Console.WriteLine($"Execution Time: {time} ms");

        }
    }
}
