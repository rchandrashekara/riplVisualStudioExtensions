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

using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace riplVisualStudioExtensions {
  [Export(typeof(IWpfTextViewCreationListener))]
  [ContentType("text")]
  [TextViewRole(PredefinedTextViewRoles.Document)]
  public sealed class Connector : IWpfTextViewCreationListener {

    public void TextViewCreated(IWpfTextView textView) {
      CommentAdornmentManager.Create(textView);
    }

    static public void Execute(IWpfTextViewHost host) {
      IWpfTextView view = host.TextView;
      //Add a comment on the selected text.   
      if (!view.Selection.IsEmpty) {
        //Get the provider for the comment adornments in the property bag of the view.  
        CommentAdornmentProvider provider = view.Properties.GetProperty<CommentAdornmentProvider>(typeof(CommentAdornmentProvider));

        //Add some arbitrary author and comment text.   
        string author = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        string comment = "Four score....";

        //Add the comment adornment using the provider.  
        provider.Add(view.Selection.SelectedSpans[0], author, comment);
      }
    }

    [Export(typeof(AdornmentLayerDefinition))]
    [Name("CommentAdornmentLayer")]
    [Order(After = PredefinedAdornmentLayers.Selection, Before = PredefinedAdornmentLayers.Text)]
    public AdornmentLayerDefinition commentLayerDefinition;
  }
}
