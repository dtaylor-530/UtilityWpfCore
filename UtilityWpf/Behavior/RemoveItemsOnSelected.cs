﻿using Microsoft.Xaml.Behaviors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Controls.Primitives;

namespace UtilityWpf.Behavior
{
    public class RemoveItemsOnSelected : Behavior<Selector>
    {
        protected override void OnAttached()
        {
            RemoveItemsOnSelectedAddItemsOnDeselected(AssociatedObject);
            base.OnAttached();
        }

        private static void RemoveItemsOnSelectedAddItemsOnDeselected(Selector selector)
        {
            //var collection = new ObservableCollection<object>(selector.ItemsSource.Cast<object>());
            Stack<(int, object)> removedObjects = new Stack<(int, object)>();
            IEnumerable itemsSource = null;
            selector.SelectAddChanges().Select(adds => adds.Cast<object>().SingleOrDefault()).Where(a => a != null).Subscribe(a =>
            {
                itemsSource = selector.ItemsSource;
                var itemsSourceCollection = selector.ItemsSource.Cast<object>().ToList();
                var index = selector.Items.IndexOf(a);
                for (int i = selector.Items.Count - 1; i > -1; i--)
                {
                    if (index != i)
                    {
                        removedObjects.Push((i, itemsSourceCollection[i]));
                        itemsSourceCollection.RemoveAt(i);
                    }
                }
                selector.ItemsSource = itemsSourceCollection;
            });

            selector.SelectRemoveChanges().Where(rem => rem.Cast<object>().Any()).Subscribe(a =>
            {
                selector.ItemsSource = itemsSource;
            });
        }
    }
}