using System.Collections.Concurrent;

namespace Kunai.InvertedIndex
{
    public class ConcurrentInvertedIndex<Tkey,TValue>
    {
        private ConcurrentDictionary<Tkey, ConcurrentBag<TValue>> _dictionary = new ConcurrentDictionary<Tkey, ConcurrentBag<TValue>>();


        //// add in inverted dico
        //current = 0;
        //firstThread = 0;
        //var max1 = max;
        //Parallel.ForEach(matrix.ToEnumerable(), cell =>
        //{
        //    cell.BestScore.RefreshCellInSquare();
        //    foreach (var position in cell.BestScore.CellInSquare.Select(c => c.Position))
        //    {

        //        // ADD PROGRESS BAR !!!
        //        InvertedScorings.AddOrUpdate(position,
        //             point =>
        //             {
        //                 var list = new ConcurrentBag<CellScoring> {cell.BestScore};
        //                 return list;
        //             }, (point, list) =>
        //             {
        //                 list.Add(cell.BestScore);
        //                 return list;
        //             });
        //    }
        //    current++;

        //    if (firstThread == 0)
        //    {
        //        firstThread = Thread.CurrentThread.ManagedThreadId;
        //    }
        //    if (firstThread == Thread.CurrentThread.ManagedThreadId)
        //        Helpers.ConsoleProgressBar(current, max1, 70);

    }
}
