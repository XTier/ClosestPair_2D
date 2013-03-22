using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;

namespace ClosestPair_2D
{
    public class Dot
    {
        public int X, Y;
        public Dot(string str)
        {
            var coords = str.Split('\t').Select(int.Parse).ToArray();
            X = coords[0];
            Y = coords[1];
        }
    }

    public class Segment
    {
        public readonly Dot P1;
        public readonly Dot P2;
        public Segment(Dot p1, Dot p2)
        {
            P1 = p1;
            P2 = p2;
        }
        public long LengthSquared()
        {
            return (long)(Math.Pow(P1.X - P2.X, 2) + Math.Pow((P1.Y - P2.Y),2));
        }
        public double Length()
        {
            return (double)Math.Sqrt(LengthSquared());
        }
    }


    public static class Perform
    {
        public static void XMerge(Dot[] arr, int iiMin, int iiMax, int jjMax)
        {
            Dot[] _temp = new Dot[jjMax - iiMin + 1];
            int _i = iiMin, _j = iiMax + 1, _k = 0;
            while ((_i <= iiMax) && (_j <= jjMax))
            {
                if (arr[_i].X < arr[_j].X)
                {
                    _temp[_k] = arr[_i];
                    _i++;
                }
                else
                {
                    _temp[_k] = arr[_j];
                    _j++;
                }
                _k++;
            }
            while (_i <= iiMax)
            {
                _temp[_k] = arr[_i];
                _i++;
                _k++;
            }
            while (_j <= jjMax)
            {
                _temp[_k] = arr[_j];
                _j++;
                _k++;
            }
            _temp.CopyTo(arr, iiMin);
        }

        public static void YMerge(Dot[] arr, int iiMin, int iiMax, int jjMax)
        {
            Dot[] _temp = new Dot[jjMax - iiMin + 1];
            int _i = iiMin, _j = iiMax + 1, _k = 0;
            while ((_i <= iiMax) && (_j <= jjMax))
            {
                if (arr[_i].Y < arr[_j].Y)
                {
                    _temp[_k] = arr[_i];
                    _i++;
                }
                else
                {
                    _temp[_k] = arr[_j];
                    _j++;
                }
                _k++;
            }
            while (_i <= iiMax)
            {
                _temp[_k] = arr[_i];
                _i++;
                _k++;
            }
            while (_j <= jjMax)
            {
                _temp[_k] = arr[_j];
                _j++;
                _k++;
            }
            _temp.CopyTo(arr, iiMin);
        }

        public static void Print(Dot[] arr)
        {
            int _i = 0;
            foreach (var dot in arr)
            {
                if (dot != null) Console.WriteLine("Point " + _i + " \t" + dot.X + "\t" + dot.Y);
                _i++;
            }
        }

        public static Segment ClosestPairBruteForce(Dot[] arr, int iiMin, int iiMax, long dSqMin)
        {
            long _dmin = dSqMin;
            Segment result = null;
            for (int ii = iiMin; ii <= iiMax - 1; ii++)
                for (int jj = ii + 1; jj <= iiMax; jj++)
                {
                    Segment seg = new Segment(arr[ii], arr[jj]);
                    if (seg.LengthSquared() < _dmin)
                    {
                        result = seg;
                        _dmin = result.LengthSquared();
                    }
                }
            return result;
        }

        public static Segment ClosestPair(Dot[] arrX, Dot[] arrY, int iiMin, int iiMax, long dSqMin)
        {
            long _dSqMin = dSqMin;
            Segment result = null;
            if ((iiMax - iiMin) < 4) result = ClosestPairBruteForce(arrX, iiMin, iiMax, _dSqMin);
            else
            {
                int _k = (iiMax + iiMin) / 2;
                Segment seg1 = null;
                if (result != null) _dSqMin = result.LengthSquared();
                seg1 = ClosestPair(arrX, arrY, iiMin, _k, _dSqMin);
                Segment seg2 = null;
                if (seg1 != null) _dSqMin = seg1.LengthSquared();
                seg2 = ClosestPair(arrX, arrY, _k + 1, iiMax, _dSqMin);
                if (seg2 != null) result = seg2;
                else result = seg1;
                if (result != null) _dSqMin = result.LengthSquared();
                seg1 = ClosestSplitPair(arrY, arrX[_k], _dSqMin);
                if (seg1 != null) result = seg1;
            }
            return result;
        }

        public static Segment ClosestSplitPair(Dot[] arrY, Dot dot, long dSqMin)
        {
            long _dSqMin = dSqMin;
            double _dmin = Math.Sqrt(dSqMin);
            Segment result = null;
            var _query = arrY.Where(d => (Math.Abs(dot.X - d.X) <= _dmin)).ToArray();
            int _n = _query.Length;
            for (int ii = 0; ii < _n; ii++)
            {
                for (int jj = ii + 1; jj < Math.Min(ii + 7, _n); jj++)
                {
                    Segment seg = new Segment(_query[ii], _query[jj]);
                    long _dSq = seg.LengthSquared();
                    if (_dSq < _dSqMin)
                    {
                        result = seg;
                        _dSqMin = _dSq;
                    }
                }
                if ((_n - ii) < 7) break;
            }
            return result;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {

            var sw = new Stopwatch();
            sw.Start();

            string path = @"d:\100000dots.dat";
            int n = 0, index = 0;
            var dataFile = File.ReadAllLines(path);
            foreach (string line in dataFile) n++;
            Console.WriteLine(n);

            Dot[] dots = new Dot[n];

            //dataFile1 = File.ReadLines(path);
            foreach (string line in dataFile)
            {
                dots[index] = new Dot(line);
                index++;
            }

            sw.Stop();
            Console.WriteLine("File reading comleted: {0}", sw.Elapsed.TotalSeconds);
            
            //var dataFile = File.OpenText(path);
            //for (int ii = 0; !dataFile.EndOfStream; ii++)
            //{
            //    dots[ii] = new Dot(dataFile.ReadLine());
            //}

            //Perform.Print(dots);
            //Console.WriteLine("========================================");

            sw.Start();

            Dot[] dotsXSorted = new Dot[n];
            dots.CopyTo(dotsXSorted, 0);
            for (int ii = 1; ii < n; ii *= 2)
                for (int jj = 0; jj < n; jj += 2 * ii)
                    Perform.XMerge(dotsXSorted, jj, Math.Min(jj + ii - 1, n - 1), Math.Min(jj + 2 * ii - 1, n - 1));
            //Perform.Print(dotsXSorted);
            
            Dot[] dotsYSorted = new Dot[n];
            dots.CopyTo(dotsYSorted, 0);
            for (int ii = 1; ii < n; ii *= 2)
                for (int jj = 0; jj < n; jj += 2 * ii)
                    Perform.YMerge(dotsYSorted, jj, Math.Min(jj + ii - 1, n - 1), Math.Min(jj + 2 * ii - 1, n - 1));
            //Perform.Print(dotsYSorted);

            sw.Stop();
            Console.WriteLine("Presorting comleted: {0}", sw.Elapsed.TotalSeconds);

            sw.Start();

            Segment segm_min = new Segment(dotsXSorted[1], dotsXSorted[2]);
            long dSqMin = segm_min.LengthSquared();

            segm_min = Perform.ClosestPair(dotsXSorted, dotsYSorted, 0, n - 1, dSqMin);

            Console.WriteLine("The two closest points (Divide&Conquer) of " + n + " items:");
            Console.WriteLine("1: " + segm_min.P1.X + "\t" + segm_min.P1.Y);
            Console.WriteLine("2: " + segm_min.P2.X + "\t" + segm_min.P2.Y);
            Console.WriteLine("The distance: " + segm_min.Length());
            Console.WriteLine("========================================");

            sw.Stop();
            Console.WriteLine("Divide&Conquer task comleted: {0}", sw.Elapsed.TotalSeconds);

            sw.Start();

            Perform.ClosestPairBruteForce(dots, 0, n - 1, dSqMin);

            Console.WriteLine("The two closest points (Brute-Force) of " + n + " items:");
            Console.WriteLine("1: " + segm_min.P1.X + "\t" + segm_min.P1.Y);
            Console.WriteLine("2: " + segm_min.P2.X + "\t" + segm_min.P2.Y);
            Console.WriteLine("The distance: " + segm_min.Length());

            sw.Stop();
            Console.WriteLine("Brute-force task comleted: {0}", sw.Elapsed.TotalSeconds);

        }
    }
}
