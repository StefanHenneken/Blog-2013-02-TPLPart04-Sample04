using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sample04
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Run();
        }
        public void Run()
        {
            Console.WriteLine("Start Run");
            CancellationTokenSource cts = new CancellationTokenSource();
            ParallelOptions po = new ParallelOptions() { CancellationToken = cts.Token };
            Task t = Task.Run(() => Work(po), cts.Token);  // new method in .NET 4.5
            Console.WriteLine("Cancel in 3sec");
            cts.CancelAfter(3000);  // new method in .NET 4.5
            t.Wait();
            Console.WriteLine("End Run");
            Console.ReadLine();
        }
        private void Work(ParallelOptions po)
        {
            Console.WriteLine("Start Work");
            ParallelLoopResult loopResult = new ParallelLoopResult();
            try
            {
                loopResult = Parallel.For(0, 50000, po, (index, loopState) =>
                {
                    DoSomeWork(index, loopState);
                });
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("End OperationCanceledException");
            }
            Console.WriteLine("End Work: {0}", loopResult.IsCompleted);
        }
        private void DoSomeWork(int index, ParallelLoopState loopState)
        {
            double temp = 1.1;
            for (int i = 0; i < 5000; i++)
            {
                temp = Math.Sin(index) + Math.Sqrt(index) * Math.Pow(index, 3.1415) + temp;
                if (loopState.ShouldExitCurrentIteration)
                {
                    Console.WriteLine("Return DoSomeWork");
                    return;
                }
            }
        }
    }
}
