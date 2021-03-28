/*
ISC License

Copyright(c) 2021, Raghavendra Chandrashekara

Permission to use, copy, modify, and/or distribute this software for any
purpose with or without fee is hereby granted, provided that the above
copyright notice and this permission notice appear in all copies.

THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
*/

// Code is derived from: https://docs.microsoft.com/en-gb/previous-versions/visualstudio/visual-studio-2015/extensibility/walkthrough-using-a-shell-command-with-an-editor-extension?view=vs-2015&redirectedfrom=MSDN

using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace riplVisualStudioExtensions {
  internal class CommentAdornmentManager {
    private readonly IWpfTextView view;
    private readonly IAdornmentLayer layer;
    private readonly CommentAdornmentProvider provider;

    private CommentAdornmentManager(IWpfTextView view) {
      this.view = view;
      this.view.LayoutChanged += OnLayoutChanged;
      this.view.Closed += OnClosed;

      this.layer = view.GetAdornmentLayer("CommentAdornmentLayer");

      this.provider = CommentAdornmentProvider.Create(view);
      this.provider.CommentsChanged += OnCommentsChanged;
    }

    public static CommentAdornmentManager Create(IWpfTextView view) {
      return view.Properties.GetOrCreateSingletonProperty<CommentAdornmentManager>(delegate { return new CommentAdornmentManager(view); });
    }

    private void OnCommentsChanged(object sender, CommentsChangedEventArgs e) {
      //Remove the comment (when the adornment was added, the comment adornment was used as the tag).   
      if (e.CommentRemoved != null)
        this.layer.RemoveAdornmentsByTag(e.CommentRemoved);

      //Draw the newly added comment (this will appear immediately: the view does not need to do a layout).   
      if (e.CommentAdded != null)
        this.DrawComment(e.CommentAdded);
    }

    private void OnClosed(object sender, EventArgs e) {
      this.provider.Detach();
      this.view.LayoutChanged -= OnLayoutChanged;
      this.view.Closed -= OnClosed;
    }

    private void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e) {
      //Get all of the comments that intersect any of the new or reformatted lines of text.  
      List<CommentAdornment> newComments = new List<CommentAdornment>();

      //The event args contain a list of modified lines and a NormalizedSpanCollection of the spans of the modified lines.    
      //Use the latter to find the comments that intersect the new or reformatted lines of text.   
      foreach (Span span in e.NewOrReformattedSpans) {
        newComments.AddRange(this.provider.GetComments(new SnapshotSpan(this.view.TextSnapshot, span)));
      }

      //It is possible to get duplicates in this list if a comment spanned 3 lines, and the first and last lines were modified but the middle line was not.   
      //Sort the list and skip duplicates.  
      newComments.Sort(delegate (CommentAdornment a, CommentAdornment b) { return a.GetHashCode().CompareTo(b.GetHashCode()); });

      CommentAdornment lastComment = null;
      foreach (CommentAdornment comment in newComments) {
        if (comment != lastComment) {
          lastComment = comment;
          this.DrawComment(comment);
        }
      }
    }

    private void DrawComment(CommentAdornment comment) {
      SnapshotSpan span = comment.Span.GetSpan(this.view.TextSnapshot);
      Geometry g = this.view.TextViewLines.GetMarkerGeometry(span);

      if (g != null) {
        //Find the rightmost coordinate of all the lines that intersect the adornment.
        double maxRight = 0.0;
        foreach (ITextViewLine line in this.view.TextViewLines.GetTextViewLinesIntersectingSpan(span))
          maxRight = Math.Max(maxRight, line.Right);

        //Create the visualization.
        CommentBlock block = new CommentBlock(maxRight, this.view.ViewportRight, g, comment.Author, comment.Text);

        //Add it to the layer.
        this.layer.AddAdornment(span, comment, block);
      }
    }
  }
}
