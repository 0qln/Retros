using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;

//https://codereview.stackexchange.com/q/205407
namespace Retros;
public static class SpanExtensions {
    public static void InsertionSort<T>(this Span<T> values, int startIndex, int endIndex, IComparer<T> comparer) {
        var left = startIndex;
        var right = 0;
        var temp = default(T);

        while (left < endIndex) {
            right = left;
            temp = values[++left];

            while ((right >= startIndex) && (0 < comparer.Compare(values[right], temp))) {
                values[(right + 1)] = values[right--];
            }

            values[(right + 1)] = temp;
        }
    }
    public static void InsertionSort<T>(this Span<T> values, IComparer<T> comparer) {
        InsertionSort(values, 0, (values.Length - 1), comparer);
    }
    public static void InsertionSort<T>(this Span<T> values) {
        InsertionSort(values, Comparer<T>.Default);
    }

    public static void QuickSort<T>(this Span<T> values, int startIndex, int endIndex, IComparer<T> comparer) {
        var range = (startIndex, endIndex);
        var stack = new Stack<(int, int)>();

        do {
            startIndex = range.startIndex;
            endIndex = range.endIndex;

            if ((endIndex - startIndex + 1) < 31) {
                values.InsertionSort(startIndex, endIndex, comparer);

                continue;
            }

            var pivot = values.SampleMedian(startIndex, endIndex, comparer);
            var left = startIndex;
            var right = endIndex;

            while (left <= right) {
                while (0 > comparer.Compare(values[left], pivot)) { left++; }
                while (0 > comparer.Compare(pivot, values[right])) { right--; }

                if (left <= right) {
                    values.Swap(left++, right--);
                }
            }

            if (startIndex < right) {
                stack.Push((startIndex, right));
            }

            if (left < endIndex) {
                stack.Push((left, endIndex));
            }
        }
        while (stack.TryPop(out range));
    }
    public static void QuickSort<T>(this Span<T> values, IComparer<T> comparer) {
        QuickSort(values, 0, (values.Length - 1), comparer);
    }
    public static void QuickSort<T>(this Span<T> values) {
        QuickSort(values, Comparer<T>.Default);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T SampleMedian<T>(this Span<T> values, int startIndex, int endIndex, IComparer<T> comparer) {
        var random = new System.Random();
        var left = random.Next(startIndex, endIndex);
        var middle = random.Next(startIndex, endIndex);
        var right = random.Next(startIndex, endIndex);

        if (0 > comparer.Compare(values[right], values[left])) {
            Swap(values, right, left);
        }

        if (0 > comparer.Compare(values[middle], values[left])) {
            Swap(values, middle, left);
        }

        if (0 > comparer.Compare(values[right], values[middle])) {
            Swap(values, right, middle);
        }

        return values[middle];
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Swap<T>(this Span<T> values, int xIndex, int yIndex) {
        var temp = values[xIndex];

        values[xIndex] = values[yIndex];
        values[yIndex] = temp;
    }
}

public static class ListExtensions
{
    public static void InsertionSort<T>(this IList<T> values, int startIndex, int endIndex, IComparer<T> comparer) {

        var left = startIndex;
        var right = 0;
        var temp = default(T);

        while (left < endIndex) {
            right = left;
            temp = values[++left];

            while ((right >= startIndex) && (0 < comparer.Compare(values[right], temp))) {
                values[(right + 1)] = values[right--];
            }

            values[(right + 1)] = temp;
        }
    }
    public static void InsertionSort<T>(this IList<T> values, IComparer<T> comparer) {
        InsertionSort(values, 0, (values.Count - 1), comparer);
    }
    public static void InsertionSort<T>(this IList<T> values) {
        InsertionSort(values, Comparer<T>.Default);
    }
    public static void QuickSort<T>(this IList<T> values, int startIndex, int endIndex, IComparer<T> comparer) {
        var range = (startIndex, endIndex);
        var stack = new Stack<(int, int)>();

        do {
            startIndex = range.startIndex;
            endIndex = range.endIndex;

            if ((endIndex - startIndex + 1) < 31) {
                values.InsertionSort(startIndex, endIndex, comparer);

                continue;
            }

            var pivot = values.SampleMedian(startIndex, endIndex, comparer);
            var left = startIndex;
            var right = endIndex;

            while (left <= right) {
                while (0 > comparer.Compare(values[left], pivot)) { left++; }
                while (0 > comparer.Compare(pivot, values[right])) { right--; }

                if (left <= right) {
                    values.Swap(left++, right--);
                }
            }

            if (startIndex < right) {
                stack.Push((startIndex, right));
            }

            if (left < endIndex) {
                stack.Push((left, endIndex));
            }
        }
        while (stack.TryPop(out range));
    }
    public static void QuickSort<T>(this IList<T> values, IComparer<T> comparer) {
        QuickSort(values, 0, (values.Count - 1), comparer);
    }
    public static void QuickSort<T>(this IList<T> values) {
        QuickSort(values, Comparer<T>.Default);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T SampleMedian<T>(this IList<T> values, int startIndex, int endIndex, IComparer<T> comparer) {
        var random = new System.Random();
        var left = random.Next(startIndex, endIndex);
        var middle = random.Next(startIndex, endIndex);
        var right = random.Next(startIndex, endIndex);

        if (0 > comparer.Compare(values[right], values[left])) {
            Swap(values, right, left);
        }

        if (0 > comparer.Compare(values[middle], values[left])) {
            Swap(values, middle, left);
        }

        if (0 > comparer.Compare(values[right], values[middle])) {
            Swap(values, right, middle);
        }

        return values[middle];
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Swap<T>(this IList<T> values, int xIndex, int yIndex) {
        var temp = values[xIndex];

        values[xIndex] = values[yIndex];
        values[yIndex] = temp;
    }
}

