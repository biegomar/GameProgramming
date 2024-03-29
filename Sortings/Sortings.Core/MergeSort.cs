﻿using System.Diagnostics;

namespace Sortings.Core;

public class MergeSort : ISort
{
    /// <inheritdoc/>
    public ObservableArray<int> Sort(ObservableArray<int> input)
    {
        var mergeArray = input.Clone();
        
        var result = MergeSortRecursive(mergeArray);
        
        return result;
    }

    /// <summary>
    /// The recursive part of merge sort. 
    /// </summary>
    /// <param name="input">The array.</param>
    /// <returns>The resulting array.</returns>
    private ObservableArray<int>  MergeSortRecursive(ObservableArray<int>  input)
    {
        if (input.Length == 1)
        {
            return input;
        }
        
        int middle = input.Length / 2;
        ObservableArray<int>  left = new ObservableArray<int>(middle);
        ObservableArray<int>  right = new ObservableArray<int>(input.Length - middle);
        left = input[..middle];
        right = input[middle..];
        
        left = MergeSortRecursive(left);
        right = MergeSortRecursive(right);
        
        return Merge(left, right);
    }

    /// <summary>
    /// The merge part.
    /// </summary>
    /// <param name="left">The left part to merge.</param>
    /// <param name="right">The right part to merge.</param>
    /// <returns>The result of the merge.</returns>
    private ObservableArray<int> Merge(ObservableArray<int> left, ObservableArray<int> right)
    {
        var merged = new ObservableArray<int>(left.Length + right.Length);
        int i = 0, j = 0, k = 0;
        while (i < left.Length && j < right.Length)
        {
            if (left[i] <= right[j])
            {
                merged[k] = left[i];
                i++;
            }
            else
            {
                merged[k] = right[j];
                j++;
            }
            k++;
        }
        while (i < left.Length)
        {
            merged[k] = left[i];
            i++;
            k++;
        }
        while (j < right.Length)
        {
            merged[k] = right[j];
            j++;
            k++;
        }
        return merged;
    }
}