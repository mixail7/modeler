﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagramDesigner {
  public class SelectionService {
    private readonly DesignerCanvas designerCanvas;

    private List<ISelectable> currentSelection;
    internal List<ISelectable> CurrentSelection {
      get {        
        if (currentSelection == null)
          currentSelection = new List<ISelectable>();

        return currentSelection;
      }
    }

    public event EventHandler SelectedChanged;

    public SelectionService(DesignerCanvas canvas) {
      this.designerCanvas = canvas;
    }

    internal void SelectItem(ISelectable item) {
      this.ClearSelection();
      this.AddToSelection(item);

      if (SelectedChanged != null) {
        SelectedChanged(designerCanvas, new EventArgs());
      }
    }

    internal void AddToSelection(ISelectable item) {
      if (item is IGroupable) {
        List<IGroupable> groupItems = GetGroupMembers(item as IGroupable);
          
        foreach (ISelectable groupItem in groupItems) {
          CurrentSelection.Add(groupItem);
          groupItem.IsSelected = true;          
        }
      } else {
        CurrentSelection.Add(item);
        item.IsSelected = true;        
      }

      if (SelectedChanged != null) {
        SelectedChanged(designerCanvas, new EventArgs());
      }
    }

    internal void RemoveFromSelection(ISelectable item) {
      if (item is IGroupable) {
        List<IGroupable> groupItems = GetGroupMembers(item as IGroupable);

        foreach (ISelectable groupItem in groupItems) {
          groupItem.IsSelected = false;
          CurrentSelection.Remove(groupItem);
        }
      } else {
        item.IsSelected = false;
        CurrentSelection.Remove(item);
      }
    }

    internal void ClearSelection() {
      CurrentSelection.ForEach(item => item.IsSelected = false);
      CurrentSelection.Clear();
      if (SelectedChanged != null) {
        SelectedChanged(designerCanvas, new EventArgs());
      }
    }

    internal void SelectAll() {
      ClearSelection();
      CurrentSelection.AddRange(designerCanvas.Children.OfType<ISelectable>());
      CurrentSelection.ForEach(item => item.IsSelected = true);
      if (SelectedChanged != null) {
        SelectedChanged(designerCanvas, new EventArgs());
      }
    }

    internal List<IGroupable> GetGroupMembers(IGroupable item) {
      IEnumerable<IGroupable> list = designerCanvas.Children.OfType<IGroupable>();
      IGroupable rootItem = GetRoot(list, item);
      return GetGroupMembers(list, rootItem);
    }

    internal IGroupable GetGroupRoot(IGroupable item) {
      IEnumerable<IGroupable> list = designerCanvas.Children.OfType<IGroupable>();
      return GetRoot(list, item);
    }

    private static IGroupable GetRoot(IEnumerable<IGroupable> list, IGroupable node) {
      if (node == null || node.ParentID == Guid.Empty) {
        return node;
      }
      return (from item in list where item.ID == node.ParentID select GetRoot(list, item)).FirstOrDefault();
    }

    private static List<IGroupable> GetGroupMembers(IEnumerable<IGroupable> list, IGroupable parent) {
      var groupMembers = new List<IGroupable> {parent};

      var children = list.Where(node => node.ParentID == parent.ID);

      foreach (IGroupable child in children) {
        groupMembers.AddRange(GetGroupMembers(list, child));
      }

      return groupMembers;
    }
  }
}
