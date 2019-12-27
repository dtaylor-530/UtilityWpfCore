﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace UtilityWpf.View
{
    public class ListBoxCriteria : ListBox
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            var criteriaItem = new CriteriaItem();
            criteriaItem.CriteriaChanged += CriteriaItem_CriteriaChanged;
            criteriaItem.DataContextChanged += CriteriaItem_DataContextChanged; ;
            return criteriaItem;
        }

        private void CriteriaItem_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            object myDataObject = e.NewValue;
            Binding myBinding = new Binding(PropertyName);
            myBinding.Source = myDataObject;
            (sender as CriteriaItem).SetBinding(CriteriaItem.MeetsCriteriaProperty, myBinding);

        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            if (item is CriteriaItem criteriaItem)
            {
                return true;
            }
            return false;

        }


        public ListBoxCriteria() : base()
        {

        }


        public string  PropertyName
        {
            get { return (string )GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register("PropertyName", typeof(string ), typeof(ListBoxCriteria), new PropertyMetadata(null, PropertyNameChanged));

        private static void PropertyNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        public bool IsCriteriaMet
        {
            get { return (bool)GetValue(IsCriteriaMetProperty); }
            set { SetValue(IsCriteriaMetProperty, value); }
        }

        public static readonly DependencyProperty IsCriteriaMetProperty = DependencyProperty.Register("IsCriteriaMet", typeof(bool), typeof(ListBoxCriteria), new PropertyMetadata(false, MetChanged));

        private static void MetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           
        }

        private void CriteriaItem_CriteriaChanged(object sender, RoutedEventArgs e)
        {
            List<int> indices = new List<int>();
            for (int i = this.Items.Count - 1; i >= 0; i--)
            {
                if (this.Items[i] is CriteriaItem criteriaItem)
                {
                    if (criteriaItem.MeetsCriteria == true)
                    {
                        indices.Add(i);
                    }
                }
                else
                {
                    criteriaItem = this.ItemContainerGenerator.ContainerFromIndex(i) as CriteriaItem;
                    if (criteriaItem?.MeetsCriteria == true)
                    {
                        indices.Add(i);
                    }
                }
            }
            if (indices.Count > 0)
            {
                this.Dispatcher.InvokeAsync(() => IsCriteriaMet = true, DispatcherPriority.Background);
            }

            RaiseEvent(new CriteriaMetEventArgs(ListBoxCriteria.CriteriaMetEvent) { Indices = indices.ToArray() });
        }

        public static readonly RoutedEvent CriteriaMetEvent = EventManager.RegisterRoutedEvent("CriteriaMet", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CriteriaItem));

        public event RoutedEventHandler CriteriaMet
        {
            add { AddHandler(CriteriaMetEvent, value); }
            remove { RemoveHandler(CriteriaMetEvent, value); }
        }

        public class CriteriaMetEventArgs : RoutedEventArgs
        {
            public int[] Indices;

            public CriteriaMetEventArgs(RoutedEvent @event) : base(@event)
            { }
        }
    }




    public class CriteriaItem : ListBoxItem
    {
        static CriteriaItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CriteriaItem), new FrameworkPropertyMetadata(typeof(CriteriaItem)));
        }

        public CriteriaItem()
        {
            this.Loaded += CriteriaItem_Loaded;
        }

        private void CriteriaItem_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public bool MeetsCriteria
        {
            get { return (bool)GetValue(MeetsCriteriaProperty); }
            set { SetValue(MeetsCriteriaProperty, value); }
        }

        public static readonly DependencyProperty MeetsCriteriaProperty =
            DependencyProperty.Register("MeetsCriteria", typeof(bool), typeof(CriteriaItem), new FrameworkPropertyMetadata(false, MeetsCriteriaChanged));

        private static void MeetsCriteriaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CriteriaItem).RaiseEvent(new CriteriaChangedEventArgs(CriteriaItem.CriteriaChangedEvent) { CriteriaIsMet = (bool)e.NewValue });
        }

        public static readonly RoutedEvent CriteriaChangedEvent = EventManager.RegisterRoutedEvent("CriteriaChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CriteriaItem));

        public event RoutedEventHandler CriteriaChanged
        {
            add { AddHandler(CriteriaChangedEvent, value); }
            remove { RemoveHandler(CriteriaChangedEvent, value); }
        }

        public class CriteriaChangedEventArgs : RoutedEventArgs
        {
            public bool CriteriaIsMet;

            public CriteriaChangedEventArgs(RoutedEvent @event) : base(@event)
            { }
        }
    }
}