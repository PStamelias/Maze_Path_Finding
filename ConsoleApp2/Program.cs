using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleApp2
{
    
    class Pair
    {
        public int fir, sec;/*Coordinates of Position*/
        public Pair(int fir,int sec)
        {
            this.fir = fir;
            this.sec = sec;
        }

        public void print()/*Printing the cooordinates of Position */
        {
            Console.Write("({0}:{1}),", this.fir+1, this.sec+1);
        }
    }
    
    class Program
    {

        static void Main(string[] args)
        {
            List<Pair> list = new List<Pair>();/*empty list which using in DFS Algorithm*/
        Stopwatch sw = Stopwatch.StartNew();/*starting counting time*/
            int Found = 0;/* variable which help understand if path has been found*/
            Pair beg = new Pair(-1, -1);/*start pair-S*/
            Pair end = new Pair(-1, -1);/*end pair  -G*/
            int N = 0;/*Vertical Input File Size*/
            string path = "text.txt";/*name of input file*/
            StreamReader file = new StreamReader(path);/*Creating Input File*/
            string[] matrix = new string[Int32.MaxValue/5];/*Maze Table-The problem puts the maximun size at Int32.Maxvalue but it crashes at this size*/
            while (file.EndOfStream == false)/*Reading the input File */
                matrix[N++] = file.ReadLine();/*Save data of input file on matrix table*/

            int M = matrix[0].Length;/*Ηorizontal Input File Size*/
            bool[,] state = new bool[N, M];/*True if the position has block , False if hasnt*/
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    if (matrix[i][j] == 'S')/*Position of Start*/
                    {
                        beg.fir = i;
                        beg.sec = j;
                    }

                    else if (matrix[i][j] == 'G')/*Position of End*/
                    {
                        end.fir = i;
                        end.sec = j;
                    }

                    else if (matrix[i][j] == 'X')/*Block Poistion-*/
                        state[i, j] = true;

                    else if (matrix[i][j] == '_')
                        state[i, j] = false;

                }
            }

            if (beg.fir == -1 || beg.sec == -1 || end.fir == -1 || end.sec == -1)/*Out of limits*/
                return ;

            bool[, ]  visited = new bool[N, M];/*Which positions i have visited in the past*/        
            initialize_visited(ref visited, beg);/*making all positions false except for the beginning*/ 
            list.Add(beg);/*Add to the solution list the beginnig pair*/
            Console.WriteLine("DFS Algorithm");
            DFS(beg, end, ref Found, ref state, ref visited, list);/*Apply DFS Algorithm*/
            Found = 0;
            initialize_visited(ref visited, beg);/*the last call of funtion took the visited by ref so i need to <<restart>> the table again*/
            BFS(beg, end, ref visited, ref state, ref Found);/*Apply BFS Algorithm*/




            if (Found == 0)
                Console.WriteLine("Path does not Exist");

            file.Close();/*Closing Input File*/
            sw.Stop();/*Stop Counting time*/

            Console.WriteLine("Time taken: {0}ms", sw.Elapsed.TotalMilliseconds);
            Console.ReadLine();
        }
        public static void initialize_visited(ref bool[, ] visited,Pair beg)/*Function initialize all cells */
        {
            int N = visited.GetLength(0);/*visited has the size of matrix table*/
            int M = visited.GetLength(1);
            for (int i = 0; i < N; i++)
                for (int j = 0; j < M; j++)
                    visited[i, j] = false;


            visited[beg.fir, beg.sec] = true;/*Start Node visiting anyway*/
        }
        public static void print_result( List<Pair>  my_List)/*This Function prints the List of Pairs-List Result*/
        {
            for (int i = 0; i < my_List.Count; i++)
            {

                if (i == 0)
                    Console.Write("({0}:{1}(S)),", my_List[i].fir + 1, my_List[i].sec + 1);
                else if (i == my_List.Count - 1)
                    Console.Write("({0}:{1}(G))", my_List[i].fir + 1, my_List[i].sec + 1);
                else
                    my_List[i].print();

            }
            Console.WriteLine();

        }
        
        
        public static bool Is_valid(int Fir, int Sec, ref bool[,] state, int i)/*Function check if the position is valid*/ 
        {

            int N = state.GetLength(0);/*Vertical Input File Size=     0  -  N-1*/
            int M = state.GetLength(1);/*Ηorizontal Input File Size=   0-   M-1*/
            if (i == 1)//up
            {
                if (Fir == -1)/*out of range*/
            return false;
            }
            else if (i == 2)//down
            {
                if (Fir == N)/*out of range*/
                    return false;
            }
            else if (i == 3)//east
            {
                if (Sec == M)/*out of range*/
                    return false;
            }
            else//west
            {
                if (Sec == -1)/*out of range*/
                    return false;
            }
            if (state[Fir, Sec] == true)/*Block Position*/
                return false;
            return true;
        }
        public static bool is_equal(Pair a, Pair b)/*Function Compares if Pair a is_equal with Pair b*/
        {
            if (a.fir == b.fir && a.sec == b.sec)
                return true;
            else
                return false;
        }
        public static void DFS(Pair node, Pair end, ref int succeed, ref bool[,] state, ref bool[,] visited, List<Pair> my_list)
        {


            if (is_equal(node, end) == true)/*If the node is equal with end , it means that i found the path*/
            {
                print_result( my_list);/*Print the List-Result*/
                Console.WriteLine();
                succeed = 1;
                return;
            }


            for (int i = 1; i <= 4; i++)//SIMULATION
            {

                if (i == 1)//up
                {

                    if (Is_valid(node.fir - 1, node.sec, ref state, i) == true && visited[node.fir - 1, node.sec] == false)/*if i can go up then create a node with it Fir Reduced by one(1)*/
                    {
                        visited[node.fir - 1, node.sec] = true;/*Making the coordinates of Node Visited*/
                        Pair conn_node = new Pair(node.fir - 1, node.sec);/*Creating the New Node */
                        my_list.Add(conn_node);/*Add the Node to the Result List*/
                        DFS(conn_node, end, ref succeed, ref state, ref visited, my_list); /*Recall DFS Function , now the argument is the new Node*/
                    }
                }
                else if (i == 2)//down
                {
                    if (Is_valid(node.fir + 1, node.sec, ref state, i) == true && visited[node.fir + 1, node.sec] == false)/*if i can go down then create a node with it Fir Increase by one(1)*/
                    {
                        visited[node.fir + 1, node.sec] = true;/*Making the coordinates of Node Visited*/
                        Pair conn_node = new Pair(node.fir + 1, node.sec);/*Creating the New Node */
                        my_list.Add(conn_node);/*Add the Node to the Result List*/
                        DFS(conn_node, end, ref succeed, ref state, ref visited, my_list);/*Recall DFS Function , now the argument is the new Node*/
                    }
                }
                else if (i == 3)//east
                {
                    if (Is_valid(node.fir, node.sec + 1, ref state, i) == true && visited[node.fir, node.sec + 1] == false)/*if i can go right then create a node with it Sec Increase by one(1)*/
                    {
                        visited[node.fir, node.sec + 1] = true;/*Making the coordinates of Node Visited*/
                        Pair conn_node = new Pair(node.fir, node.sec + 1);/*Creating the New Node */
                        my_list.Add(conn_node);/*Add the Node to the Result List*/
                        DFS(conn_node, end, ref succeed, ref state, ref visited, my_list);/*Recall DFS Function , now the argument is the new Node*/
                    }
                }
                else //west
                {
                    if (Is_valid(node.fir, node.sec - 1, ref state, i) == true && visited[node.fir, node.sec - 1] == false)/*if i can go left then create a node with it Sec Reduce by one(1)*/
                    {
                        visited[node.fir, node.sec - 1] = true;/*Making the coordinates of Node Visited*/
                        Pair conn_node = new Pair(node.fir, node.sec - 1);/*Creating the New Node */
                        my_list.Add(conn_node);/*Add the Node to the Result List*/
                        DFS(conn_node, end, ref succeed, ref state, ref visited, my_list);/*Recall DFS Function , now the argument is the new Node*/
                    }
                }

            }
            my_list.RemoveAt(my_list.Count - 1);/*In this case i can  not go anywhere,so i will turn back to the previous position*/
            /*Delete the Node with these coordinates from Result List*/
            if (my_list.Count == 0)/*I took all paths , no one lead me to the end position-Path does not exist*/
                return;/*E-N-D*/
        }
        public static void BFS(Pair beg,Pair end,ref bool [,]visited,ref bool [,] state,ref int Found)
        {
            Console.WriteLine("BFS Algorithm");
            int N = state.GetLength(0);
            int M = state.GetLength(1);
            int a, b;
            
            List<Pair> MyList = new List<Pair>();/*Queue list*/
            Pair[,] Father = new Pair[N, M];/*Each node has a <<Father>>-it help me print the Path-coordinates*/

            MyList.Insert(0, beg);/*Insert the start to Queue*/
            Father[beg.fir, beg.sec] = beg;/*the start has no Father, i will put the same Node as its value*/
            while (MyList.Count() != 0 &&Found==0)//SIMULATION
            {
                
                Pair Parent = new Pair(MyList.First().fir, MyList.First().sec);//Creating the Parent Node*/
                
                MyList.RemoveAt(0);/*Removing the Parent Node from Queue*/
                for (int i = 0; i < 4; i++)
                {

                    a = Parent.fir;/*Parent Coordinate*/
                    b = Parent.sec;/*Parent Coordinate*/
                    
                    if (i == 0)//up
                        a--;
                    else if (i == 1)//down
                        a++;
                    else if (i == 2)//east
                        b++;
                    else//west
                        b--;
                  
                    if (Is_valid(a, b, ref state, i + 1) == true && visited[a, b] == false)
                    {
                        
                        Pair Node = new Pair(a, b);/*Create the New Node*/
                       
                        visited[a, b] = true;/*i visit the New Node*/
                        Father[a, b] = Parent;/*Parent of the Node is the Node which i came from*/

                        if (is_equal(Node, end) == true)
                        {
                            Found = 1;
                            break;
                        }
                      
                        MyList.Insert(MyList.Count, Node);/*Insert the New Node to the Queue*/
                    }
                }
   
            }

            MyList.Clear();
            if (Found == 0)
                return;
            bool  timi = false;

            while(true)
            {
                if (timi == false)
                    MyList.Insert(0, end);//I insert to the Result List the End Pair*/
                else
                    MyList.Insert(MyList.Count(), Father[MyList[MyList.Count() - 1].fir, MyList[MyList.Count() - 1].sec]);
                    /* I insert the Previous Node coordinates  find the next Node in path */
                timi = true;

                
                if (is_equal(MyList[MyList.Count() - 1], beg)==true)/*i found the path*/
                    break;
            }
            MyList.Reverse();/*Reverse the List for correct sequence of Pairs*/
            print_result( MyList);/*Print the Path*/
            MyList.Clear();/*E-N-D Clear the List*/
        }
    }
}
/*ΕΓΓΡΑΦΕΣ
 /*Input:Maze Problem Solution\ConsoleApp2\bin\Debug text.txt 
/* Αναφορές
/*For  information about maze solving , i searched on wikipedia for special algorithms
/*So, i took a look at  "https://en.wikipedia.org/wiki/Maze_solving_algorithm" ,specially on paragraph 
/* Shortest path algorithm.Depth-First search algorithm and Breadth-first search algorithm are used because I taught them at my university 
/* Also i could use Dijkstra Algorithm and Bellman-Ford Algorithm but they used for negative costs*/
