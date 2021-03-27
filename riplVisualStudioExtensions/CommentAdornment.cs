// Code is derived from: https://docs.microsoft.com/en-gb/previous-versions/visualstudio/visual-studio-2015/extensibility/walkthrough-using-a-shell-command-with-an-editor-extension?view=vs-2015&redirectedfrom=MSDN

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;

namespace riplVisualStudioExtensions {
  class CommentAdornment {
    public readonly ITrackingSpan Span;
    public readonly string Author;
    public readonly string Text;

    public CommentAdornment(SnapshotSpan span, string author, string text) {
      this.Span = span.Snapshot.CreateTrackingSpan(span, SpanTrackingMode.EdgeExclusive);
      this.Author = author;
      this.Text = text;
    }
  }
}
