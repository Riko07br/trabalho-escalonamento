using System.Xml.Linq;

namespace escalonamento {
    internal class Program {

        static List<ProcessSim> processList = new List<ProcessSim>();

        static bool CanStartNewTask { 
            get {
                // caps max list size
                if (processList.Count > 7)
                    return false;

                Random rand = new Random();
                //10% chance of starting new task
                return rand.Next(0, 5) == 0;
            } 
        }

        static int RandomTaskLenght { 
            get {
                Random rand = new Random();               
                return rand.Next(2, 5);
            } 
        }

        static void Main(string[] args) {

            Console.WriteLine("Criando processos...");
            processList = new List<ProcessSim> {
                new ProcessSim("roberval", 4),
                new ProcessSim("teobaldo", 5),
                new ProcessSim("godofredo", 3),
                new ProcessSim("birosvaldo", 4),                
            };
            Console.WriteLine("");
            Console.WriteLine("Processos criados\n");
            string selectedAlgorithm = "";
            
            while (selectedAlgorithm != "p" && selectedAlgorithm != "t" && selectedAlgorithm != "e") {
                Console.WriteLine("Selecione o algoritimo de escalonamento: \n");
                Console.WriteLine("[P]rimeiro a chegar, primeiro a ser servido");
                Console.WriteLine("[T]arefa mais curta primeiro");
                Console.WriteLine("T[e]mpo restante mais curto em seguida");

                selectedAlgorithm = Console.ReadLine();

                if (selectedAlgorithm != null)
                    selectedAlgorithm = selectedAlgorithm.ToLower();
            }
            Console.WriteLine("");

            bool newTaskAdded = true;
            int newTaskNameIndex = 0;

            if(selectedAlgorithm == "p") {
                while (newTaskAdded) {

                    newTaskAdded = false;

                    //fisrt come, first serve
                    foreach (ProcessSim p in processList) {
                        // avoid running finished tasks
                        if (p.IsFinished)
                            continue;

                        p.TellProcessIsRunning();

                        while (p.Lenght > 0) {
                            p.TellRemainingTime();
                            Thread.Sleep(500); //espera .5s
                            p.Lenght--;

                            //simulate new task starting while other is running
                            if (CanStartNewTask) {
                                processList.Add(new ProcessSim("Nova tarefa " + (newTaskNameIndex++), RandomTaskLenght));
                                newTaskAdded = true;
                                Thread.Sleep(500); //espera .5s
                            }
                        }

                        //Exits foreach loop
                        if (newTaskAdded)
                            break;
                    }
                }
            }else if (selectedAlgorithm == "t") {
                while (newTaskAdded) {

                    //sort tasks if new task is added
                    if (newTaskAdded) {
                        List<ProcessSim> tmpProcessList = processList.OrderBy(a => a.Lenght).ToList();
                        processList = tmpProcessList;
                        newTaskAdded = false;
                    }

                    foreach (ProcessSim p in processList) {

                        // avoid running finished tasks
                        if (p.IsFinished)
                            continue;
                        Console.WriteLine("Tarefa mais curta escolhida para execução...");
                        p.TellProcessIsRunning();

                        while (p.Lenght > 0) {
                            p.TellRemainingTime();
                            Thread.Sleep(500); //espera .5s
                            p.Lenght--;

                            //simulate new task starting while other is running
                            if (CanStartNewTask) {
                                processList.Add(new ProcessSim("Nova tarefa " + (newTaskNameIndex++), RandomTaskLenght));
                                newTaskAdded = true;
                            }
                        }

                        //Exits foreach loop
                        if (newTaskAdded)
                            break;
                    }
                }
            }
            else if (selectedAlgorithm == "e") {                
                
                while (newTaskAdded) {

                    //sort tasks if new task is added
                    if (newTaskAdded) {
                        List<ProcessSim> tmpProcessList = processList.OrderBy(a => a.Lenght).ToList();
                        processList = tmpProcessList;
                        newTaskAdded = false;     
                        
                    }

                    foreach (ProcessSim p in processList) {
                        // avoid running finished tasks
                        if (p.IsFinished)
                            continue;
                        Console.WriteLine("Tarefa mais curta escolhida para execução...");
                        p.TellProcessIsRunning();

                        while (p.Lenght > 0) {
                            p.TellRemainingTime();
                            Thread.Sleep(500); //espera .5s
                            p.Lenght--;

                            //simulate new task starting while other is running
                            if (CanStartNewTask) {
                                ProcessSim tmpProcess = new ProcessSim("Nova tarefa " + (newTaskNameIndex++), RandomTaskLenght);
                                processList.Add(tmpProcess);
                                newTaskAdded = true;
                                Console.WriteLine("====== Verificando se a nova tarefa é mais curta ======");
                                Thread.Sleep(1000); //espera 1s

                                if(tmpProcess.Lenght < p.Lenght) {
                                    Console.WriteLine("Parando execução atual...\n");
                                    Thread.Sleep(1000); //espera 1s
                                    break;
                                }

                                Console.WriteLine("Nova tarefa é mais LONGA, resumindo execução atual...");
                                Thread.Sleep(500); //espera .5s
                            }
                        }

                        //Exits foreach loop
                        if (newTaskAdded)
                            break;
                    }
                }
                
            }

            Console.WriteLine("Tarefas finalizadas");
            Console.WriteLine("Aperte Enter para finalizar");
            Console.ReadLine();
        }
    }

    public class ProcessSim {
        string name = "";
        int lenght = 0;

        public bool IsFinished { get => lenght <= 0; }

        public int Lenght {
            get => lenght;
            set { 
                if (value <= 0)
                    Console.WriteLine("Processo " + name + " finalizado...\n");

                lenght = value; 
            }
        }

        public ProcessSim(string name, int lenght) {
            this.name = name;
            this.lenght = lenght;

            Console.WriteLine("Processo " + name + " criado com duração de " + lenght + " segundos");
        }

        public void TellProcessIsRunning() {
            Console.WriteLine("Processo " + name + " em execução na CPU...");
        }

        public void TellRemainingTime() {
            Console.WriteLine("Tempo para terminar: " + lenght + "s");
        }       

    }
}